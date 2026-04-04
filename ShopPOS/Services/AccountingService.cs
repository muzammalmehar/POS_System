using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ShopPOS.Data;
using ShopPOS.Models;

namespace ShopPOS.Services
{
    public class AccountingService
    {
        public BusinessSummaryMetrics GetBusinessSummary(DateTime fromDate, DateTime toDate)
        {
            BusinessSummaryMetrics summary = new BusinessSummaryMetrics();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            IFNULL(SUM(sale_summary.grand_total), 0.00) AS grocery_sales,
                            IFNULL(SUM(sale_summary.grand_total - sale_summary.total_cost), 0.00) AS grocery_profit,
                            COUNT(*) AS total_orders
                        FROM
                        (
                            SELECT
                                sh.sale_id,
                                sh.grand_total,
                                IFNULL(SUM(sd.cost_rate * sd.quantity), 0.00) AS total_cost
                            FROM sale_header sh
                            LEFT JOIN sale_detail sd ON sd.sale_id = sh.sale_id
                            WHERE sh.sale_date >= @fromDate
                              AND sh.sale_date < @toDate
                              AND IFNULL(sh.is_refunded, 0) = 0
                            GROUP BY sh.sale_id, sh.grand_total
                        ) AS sale_summary;";
                    command.Parameters.AddWithValue("@fromDate", fromDate.Date);
                    command.Parameters.AddWithValue("@toDate", toDate.Date.AddDays(1));

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            summary.GrocerySalesAmount = Convert.ToDecimal(reader["grocery_sales"]);
                            summary.GroceryProfitAmount = Convert.ToDecimal(reader["grocery_profit"]);
                            summary.GroceryOrderCount = Convert.ToInt32(reader["total_orders"]);
                        }
                    }
                }

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            IFNULL(SUM(amount), 0.00) AS service_sales,
                            IFNULL(SUM(commission_earned), 0.00) AS service_profit
                        FROM service_transaction_header
                        WHERE txn_date >= @fromDate
                          AND txn_date < @toDate
                          AND status = 'Completed'
                          AND IFNULL(is_refunded, 0) = 0;";
                    command.Parameters.AddWithValue("@fromDate", fromDate.Date);
                    command.Parameters.AddWithValue("@toDate", toDate.Date.AddDays(1));

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            summary.ServiceSalesAmount = Convert.ToDecimal(reader["service_sales"]);
                            summary.ServiceProfitAmount = Convert.ToDecimal(reader["service_profit"]);
                        }
                    }
                }
            }

            return summary;
        }

        public List<AccountBalanceItem> GetAccountBalances()
        {
            List<AccountBalanceItem> items = new List<AccountBalanceItem>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureSystemAccounts(connection, null);

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            a.account_id,
                            a.account_name,
                            a.account_type,
                            IFNULL(SUM(l.debit), 0.00) AS debit_total,
                            IFNULL(SUM(l.credit), 0.00) AS credit_total
                        FROM accounts a
                        LEFT JOIN ledger_transaction_lines l ON l.account_id = a.account_id
                        WHERE a.is_active = 1
                        GROUP BY a.account_id, a.account_name, a.account_type
                        ORDER BY FIELD(a.account_type, 'Asset','Liability','Income','Expense','Equity'), a.account_name;";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AccountBalanceItem item = new AccountBalanceItem();
                            item.AccountId = Convert.ToInt32(reader["account_id"]);
                            item.AccountName = Convert.ToString(reader["account_name"]);
                            item.AccountType = Convert.ToString(reader["account_type"]);
                            item.PeriodDebit = Convert.ToDecimal(reader["debit_total"]);
                            item.PeriodCredit = Convert.ToDecimal(reader["credit_total"]);
                            item.DebitTotal = item.PeriodDebit;
                            item.CreditTotal = item.PeriodCredit;
                            item.OpeningBalance = 0;
                            item.ClosingBalance = GetSignedBalance(item.AccountType, item.PeriodDebit, item.PeriodCredit);
                            item.Balance = item.ClosingBalance;
                            items.Add(item);
                        }
                    }
                }
            }

            return items;
        }

        public List<AccountBalanceItem> GetAccountBalances(DateTime fromDate, DateTime toDate)
        {
            List<AccountBalanceItem> items = new List<AccountBalanceItem>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureSystemAccounts(connection, null);

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            a.account_id,
                            a.account_name,
                            a.account_type,
                            IFNULL(SUM(CASE
                                WHEN t.txn_date < @fromDate THEN l.debit
                                ELSE 0.00
                            END), 0.00) AS opening_debit,
                            IFNULL(SUM(CASE
                                WHEN t.txn_date < @fromDate THEN l.credit
                                ELSE 0.00
                            END), 0.00) AS opening_credit,
                            IFNULL(SUM(CASE
                                WHEN t.txn_date >= @fromDate AND t.txn_date < @toDate THEN l.debit
                                ELSE 0.00
                            END), 0.00) AS period_debit,
                            IFNULL(SUM(CASE
                                WHEN t.txn_date >= @fromDate AND t.txn_date < @toDate THEN l.credit
                                ELSE 0.00
                            END), 0.00) AS period_credit
                        FROM accounts a
                        LEFT JOIN ledger_transaction_lines l ON l.account_id = a.account_id
                        LEFT JOIN ledger_transactions t ON t.ledger_txn_id = l.ledger_txn_id
                        WHERE a.is_active = 1
                        GROUP BY a.account_id, a.account_name, a.account_type
                        ORDER BY FIELD(a.account_type, 'Asset','Liability','Income','Expense','Equity'), a.account_name;";
                    command.Parameters.AddWithValue("@fromDate", fromDate.Date);
                    command.Parameters.AddWithValue("@toDate", toDate.Date.AddDays(1));

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AccountBalanceItem item = new AccountBalanceItem();
                            item.AccountId = Convert.ToInt32(reader["account_id"]);
                            item.AccountName = Convert.ToString(reader["account_name"]);
                            item.AccountType = Convert.ToString(reader["account_type"]);
                            decimal openingDebit = Convert.ToDecimal(reader["opening_debit"]);
                            decimal openingCredit = Convert.ToDecimal(reader["opening_credit"]);
                            item.OpeningBalance = GetSignedBalance(item.AccountType, openingDebit, openingCredit);
                            item.PeriodDebit = Convert.ToDecimal(reader["period_debit"]);
                            item.PeriodCredit = Convert.ToDecimal(reader["period_credit"]);
                            item.DebitTotal = item.PeriodDebit;
                            item.CreditTotal = item.PeriodCredit;
                            item.ClosingBalance = item.OpeningBalance + GetSignedBalance(item.AccountType, item.PeriodDebit, item.PeriodCredit);
                            item.Balance = item.ClosingBalance;
                            items.Add(item);
                        }
                    }
                }
            }

            return items;
        }

        public List<ProfitLossLineItem> GetProfitLoss(DateTime fromDate, DateTime toDate)
        {
            List<ProfitLossLineItem> items = new List<ProfitLossLineItem>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureSystemAccounts(connection, null);

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            a.account_name,
                            a.account_type,
                            IFNULL(SUM(l.debit), 0.00) AS debit_total,
                            IFNULL(SUM(l.credit), 0.00) AS credit_total
                        FROM ledger_transactions t
                        INNER JOIN ledger_transaction_lines l ON l.ledger_txn_id = t.ledger_txn_id
                        INNER JOIN accounts a ON a.account_id = l.account_id
                        WHERE t.txn_date >= @fromDate
                          AND t.txn_date < @toDate
                          AND a.account_type IN ('Income','Expense')
                        GROUP BY a.account_name, a.account_type
                        ORDER BY a.account_type, a.account_name;";
                    command.Parameters.AddWithValue("@fromDate", fromDate.Date);
                    command.Parameters.AddWithValue("@toDate", toDate.Date.AddDays(1));

                    decimal totalIncome = 0;
                    decimal totalExpense = 0;
                    decimal salesIncome = 0;
                    decimal costOfGoodsSold = 0;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string accountType = Convert.ToString(reader["account_type"]);
                            decimal debit = Convert.ToDecimal(reader["debit_total"]);
                            decimal credit = Convert.ToDecimal(reader["credit_total"]);
                            decimal amount = accountType == "Income" ? credit - debit : debit - credit;

                            ProfitLossLineItem item = new ProfitLossLineItem();
                            item.Section = accountType;
                            item.AccountName = Convert.ToString(reader["account_name"]);
                            item.Amount = amount;
                            items.Add(item);

                            if (accountType == "Income") totalIncome += amount;
                            if (accountType == "Expense") totalExpense += amount;
                            if (item.AccountName == "Sales Income") salesIncome += amount;
                            if (item.AccountName == "Purchase Account") costOfGoodsSold += amount;
                        }
                    }

                    items.Add(new ProfitLossLineItem { Section = "Summary", AccountName = "Gross Profit on Sales", Amount = salesIncome - costOfGoodsSold });
                    items.Add(new ProfitLossLineItem { Section = "Summary", AccountName = "Total Income", Amount = totalIncome });
                    items.Add(new ProfitLossLineItem { Section = "Summary", AccountName = "Total Expense", Amount = totalExpense });
                    items.Add(new ProfitLossLineItem { Section = "Summary", AccountName = "Net Profit / Loss", Amount = totalIncome - totalExpense });
                }
            }

            return items;
        }

        public List<LedgerVoucherItem> GetRecentVouchers()
        {
            List<LedgerVoucherItem> items = new List<LedgerVoucherItem>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT
                        t.txn_date,
                        t.voucher_type,
                        t.reference_table,
                        t.reference_id,
                        t.remarks,
                        IFNULL(SUM(l.debit), 0.00) AS debit_total,
                        IFNULL(SUM(l.credit), 0.00) AS credit_total
                    FROM ledger_transactions t
                    LEFT JOIN ledger_transaction_lines l ON l.ledger_txn_id = t.ledger_txn_id
                    GROUP BY t.ledger_txn_id, t.txn_date, t.voucher_type, t.reference_table, t.reference_id, t.remarks
                    ORDER BY t.txn_date DESC, t.ledger_txn_id DESC
                    LIMIT 200;";

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        LedgerVoucherItem item = new LedgerVoucherItem();
                        item.TransactionDate = Convert.ToDateTime(reader["txn_date"]);
                        item.VoucherType = Convert.ToString(reader["voucher_type"]);
                        item.ReferenceTable = Convert.ToString(reader["reference_table"]);
                        item.ReferenceId = reader["reference_id"] == DBNull.Value ? (long?)null : Convert.ToInt64(reader["reference_id"]);
                        item.ReferenceLabel = FormatReferenceLabel(item.ReferenceTable, item.ReferenceId);
                        item.TotalAmount = Math.Max(Convert.ToDecimal(reader["debit_total"]), Convert.ToDecimal(reader["credit_total"]));
                        item.Remarks = Convert.ToString(reader["remarks"]);
                        items.Add(item);
                    }
                }
            }

            return items;
        }

        public List<LedgerVoucherItem> GetRecentVouchers(DateTime fromDate, DateTime toDate)
        {
            List<LedgerVoucherItem> items = new List<LedgerVoucherItem>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT
                        t.txn_date,
                        t.voucher_type,
                        t.reference_table,
                        t.reference_id,
                        t.remarks,
                        IFNULL(SUM(l.debit), 0.00) AS debit_total,
                        IFNULL(SUM(l.credit), 0.00) AS credit_total
                    FROM ledger_transactions t
                    LEFT JOIN ledger_transaction_lines l ON l.ledger_txn_id = t.ledger_txn_id
                    WHERE t.txn_date >= @fromDate
                      AND t.txn_date < @toDate
                    GROUP BY t.ledger_txn_id, t.txn_date, t.voucher_type, t.reference_table, t.reference_id, t.remarks
                    ORDER BY t.txn_date DESC, t.ledger_txn_id DESC
                    LIMIT 200;";
                command.Parameters.AddWithValue("@fromDate", fromDate.Date);
                command.Parameters.AddWithValue("@toDate", toDate.Date.AddDays(1));

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        LedgerVoucherItem item = new LedgerVoucherItem();
                        item.TransactionDate = Convert.ToDateTime(reader["txn_date"]);
                        item.VoucherType = Convert.ToString(reader["voucher_type"]);
                        item.ReferenceTable = Convert.ToString(reader["reference_table"]);
                        item.ReferenceId = reader["reference_id"] == DBNull.Value ? (long?)null : Convert.ToInt64(reader["reference_id"]);
                        item.ReferenceLabel = FormatReferenceLabel(item.ReferenceTable, item.ReferenceId);
                        item.TotalAmount = Math.Max(Convert.ToDecimal(reader["debit_total"]), Convert.ToDecimal(reader["credit_total"]));
                        item.Remarks = Convert.ToString(reader["remarks"]);
                        items.Add(item);
                    }
                }
            }

            return items;
        }

        public static void EnsureSystemAccounts(MySqlConnection connection, MySqlTransaction transaction)
        {
            EnsureNamedAccount(connection, transaction, "Sales Income", "Income", "None", null);
            EnsureNamedAccount(connection, transaction, "Service Income", "Income", "None", null);
            EnsureNamedAccount(connection, transaction, "Service Pending Liability", "Liability", "None", null);
            EnsureNamedAccount(connection, transaction, "Inventory Asset", "Asset", "None", null);
            EnsureNamedAccount(connection, transaction, "Purchase Account", "Expense", "None", null);
            EnsureNamedAccount(connection, transaction, "Expense Account", "Expense", "None", null);
            EnsureNamedAccount(connection, transaction, "Customer Receivable", "Asset", "None", null);
            EnsureNamedAccount(connection, transaction, "Supplier Payable", "Liability", "None", null);
            EnsureWalletLinkedAccounts(connection, transaction);
        }

        public static void PostSaleEntry(MySqlConnection connection, MySqlTransaction transaction, long saleId, decimal grandTotal, decimal totalCost, decimal paidAmount, int? walletAccountId, string remarks, int userId)
        {
            EnsureSystemAccounts(connection, transaction);
            int salesIncomeId = GetAccountId(connection, transaction, "Sales Income");
            int customerReceivableId = GetAccountId(connection, transaction, "Customer Receivable");
            int inventoryAssetId = GetAccountId(connection, transaction, "Inventory Asset");
            int purchaseAccountId = GetAccountId(connection, transaction, "Purchase Account");

            List<LedgerLine> lines = new List<LedgerLine>();
            lines.Add(new LedgerLine { AccountId = salesIncomeId, Credit = grandTotal, Description = "Sale income" });

            if (paidAmount > 0 && walletAccountId.HasValue)
            {
                lines.Add(new LedgerLine
                {
                    AccountId = GetWalletLinkedAccountId(connection, transaction, walletAccountId.Value),
                    Debit = paidAmount,
                    Description = "Sale payment received"
                });
            }

            decimal dueAmount = grandTotal - paidAmount;
            if (dueAmount > 0)
            {
                lines.Add(new LedgerLine { AccountId = customerReceivableId, Debit = dueAmount, Description = "Customer credit sale" });
            }

            if (totalCost > 0)
            {
                lines.Add(new LedgerLine { AccountId = purchaseAccountId, Debit = totalCost, Description = "Cost of goods sold" });
                lines.Add(new LedgerLine { AccountId = inventoryAssetId, Credit = totalCost, Description = "Inventory issued on sale" });
            }

            InsertVoucher(connection, transaction, DateTime.Now, "Sale", "sale_header", saleId, remarks, userId, lines);
        }

        public static void PostSaleRefundEntry(MySqlConnection connection, MySqlTransaction transaction, long saleId, decimal grandTotal, decimal totalCost, decimal paidAmount, int? walletAccountId, string remarks, int userId)
        {
            EnsureSystemAccounts(connection, transaction);
            int salesIncomeId = GetAccountId(connection, transaction, "Sales Income");
            int customerReceivableId = GetAccountId(connection, transaction, "Customer Receivable");
            int inventoryAssetId = GetAccountId(connection, transaction, "Inventory Asset");
            int purchaseAccountId = GetAccountId(connection, transaction, "Purchase Account");

            List<LedgerLine> lines = new List<LedgerLine>();
            lines.Add(new LedgerLine { AccountId = salesIncomeId, Debit = grandTotal, Description = "Sale refund reversal" });

            if (paidAmount > 0 && walletAccountId.HasValue)
            {
                lines.Add(new LedgerLine
                {
                    AccountId = GetWalletLinkedAccountId(connection, transaction, walletAccountId.Value),
                    Credit = paidAmount,
                    Description = "Refund paid back from wallet"
                });
            }

            decimal dueAmount = grandTotal - paidAmount;
            if (dueAmount > 0)
            {
                lines.Add(new LedgerLine { AccountId = customerReceivableId, Credit = dueAmount, Description = "Customer credit sale reversed" });
            }

            if (totalCost > 0)
            {
                lines.Add(new LedgerLine { AccountId = purchaseAccountId, Credit = totalCost, Description = "Cost of goods sold reversed" });
                lines.Add(new LedgerLine { AccountId = inventoryAssetId, Debit = totalCost, Description = "Inventory restored from refund" });
            }

            InsertVoucher(connection, transaction, DateTime.Now, "Sale Refund", "sale_header", saleId, remarks, userId, lines);
        }

        public static void PostExpenseEntry(MySqlConnection connection, MySqlTransaction transaction, long expenseId, int walletAccountId, decimal amount, string remarks, int userId)
        {
            EnsureSystemAccounts(connection, transaction);
            int expenseAccountId = GetAccountId(connection, transaction, "Expense Account");
            int walletLedgerAccountId = GetWalletLinkedAccountId(connection, transaction, walletAccountId);

            List<LedgerLine> lines = new List<LedgerLine>();
            lines.Add(new LedgerLine { AccountId = expenseAccountId, Debit = amount, Description = "Expense booked" });
            lines.Add(new LedgerLine { AccountId = walletLedgerAccountId, Credit = amount, Description = "Paid from wallet" });
            InsertVoucher(connection, transaction, DateTime.Now, "Expense", "expenses", expenseId, remarks, userId, lines);
        }

        public static void PostServiceEntry(MySqlConnection connection, MySqlTransaction transaction, long serviceTxnId, int walletAccountId, decimal grossAmount, decimal commissionAmount, string remarks, int userId)
        {
            EnsureSystemAccounts(connection, transaction);
            int cashAccountId = GetCashAccountId(connection, transaction);
            int walletLedgerAccountId = GetWalletLinkedAccountId(connection, transaction, walletAccountId);
            int serviceIncomeId = GetAccountId(connection, transaction, "Service Income");

            List<LedgerLine> lines = new List<LedgerLine>();
            lines.Add(new LedgerLine { AccountId = cashAccountId, Debit = grossAmount + commissionAmount, Description = "Cash received from service customer" });
            lines.Add(new LedgerLine { AccountId = walletLedgerAccountId, Credit = grossAmount, Description = "Wallet used for service" });
            lines.Add(new LedgerLine { AccountId = serviceIncomeId, Credit = commissionAmount, Description = "Service commission income" });
            InsertVoucher(connection, transaction, DateTime.Now, "Service", "service_transaction_header", serviceTxnId, remarks, userId, lines);
        }

        public static void PostWithdrawalServiceEntry(MySqlConnection connection, MySqlTransaction transaction, long serviceTxnId, int walletAccountId, decimal grossAmount, decimal commissionAmount, string remarks, int userId)
        {
            EnsureSystemAccounts(connection, transaction);
            int cashAccountId = GetCashAccountId(connection, transaction);
            int walletLedgerAccountId = GetWalletLinkedAccountId(connection, transaction, walletAccountId);
            int serviceIncomeId = GetAccountId(connection, transaction, "Service Income");
            decimal receivedAmount = grossAmount + commissionAmount;

            List<LedgerLine> lines = new List<LedgerLine>();
            lines.Add(new LedgerLine { AccountId = walletLedgerAccountId, Debit = receivedAmount, Description = "Incoming transfer received for withdrawal" });
            lines.Add(new LedgerLine { AccountId = cashAccountId, Credit = grossAmount, Description = "Cash paid out to withdrawal customer" });
            lines.Add(new LedgerLine { AccountId = serviceIncomeId, Credit = commissionAmount, Description = "Withdrawal commission income" });
            InsertVoucher(connection, transaction, DateTime.Now, "Withdrawal", "service_transaction_header", serviceTxnId, remarks, userId, lines);
        }

        public static void PostPendingServiceEntry(MySqlConnection connection, MySqlTransaction transaction, long serviceTxnId, decimal grossAmount, decimal commissionAmount, string remarks, int userId)
        {
            EnsureSystemAccounts(connection, transaction);
            int cashAccountId = GetCashAccountId(connection, transaction);
            int serviceIncomeId = GetAccountId(connection, transaction, "Service Income");
            int pendingLiabilityId = GetAccountId(connection, transaction, "Service Pending Liability");

            List<LedgerLine> lines = new List<LedgerLine>();
            lines.Add(new LedgerLine { AccountId = cashAccountId, Debit = grossAmount + commissionAmount, Description = "Cash received for pending service" });
            lines.Add(new LedgerLine { AccountId = pendingLiabilityId, Credit = grossAmount, Description = "Pending bill liability" });
            lines.Add(new LedgerLine { AccountId = serviceIncomeId, Credit = commissionAmount, Description = "Commission received on pending service" });
            InsertVoucher(connection, transaction, DateTime.Now, "Service Pending", "service_transaction_header", serviceTxnId, remarks, userId, lines);
        }

        public static void PostPendingWithdrawalEntry(MySqlConnection connection, MySqlTransaction transaction, long serviceTxnId, int walletAccountId, decimal grossAmount, decimal commissionAmount, string remarks, int userId)
        {
            EnsureSystemAccounts(connection, transaction);
            int walletLedgerAccountId = GetWalletLinkedAccountId(connection, transaction, walletAccountId);
            int serviceIncomeId = GetAccountId(connection, transaction, "Service Income");
            int pendingLiabilityId = GetAccountId(connection, transaction, "Service Pending Liability");
            decimal receivedAmount = grossAmount + commissionAmount;

            List<LedgerLine> lines = new List<LedgerLine>();
            lines.Add(new LedgerLine { AccountId = walletLedgerAccountId, Debit = receivedAmount, Description = "Incoming transfer received for pending withdrawal" });
            lines.Add(new LedgerLine { AccountId = pendingLiabilityId, Credit = grossAmount, Description = "Pending withdrawal payable" });
            lines.Add(new LedgerLine { AccountId = serviceIncomeId, Credit = commissionAmount, Description = "Withdrawal commission income" });
            InsertVoucher(connection, transaction, DateTime.Now, "Withdrawal Pending", "service_transaction_header", serviceTxnId, remarks, userId, lines);
        }

        public static void PostPendingServiceSettlementEntry(MySqlConnection connection, MySqlTransaction transaction, long serviceTxnId, int walletAccountId, decimal grossAmount, string remarks, int userId)
        {
            EnsureSystemAccounts(connection, transaction);
            int walletLedgerAccountId = GetWalletLinkedAccountId(connection, transaction, walletAccountId);
            int pendingLiabilityId = GetAccountId(connection, transaction, "Service Pending Liability");

            List<LedgerLine> lines = new List<LedgerLine>();
            lines.Add(new LedgerLine { AccountId = pendingLiabilityId, Debit = grossAmount, Description = "Pending bill settled" });
            lines.Add(new LedgerLine { AccountId = walletLedgerAccountId, Credit = grossAmount, Description = "Wallet used to settle pending bill" });
            InsertVoucher(connection, transaction, DateTime.Now, "Service Settlement", "service_transaction_header", serviceTxnId, remarks, userId, lines);
        }

        public static void PostPendingWithdrawalSettlementEntry(MySqlConnection connection, MySqlTransaction transaction, long serviceTxnId, decimal grossAmount, string remarks, int userId)
        {
            EnsureSystemAccounts(connection, transaction);
            int cashAccountId = GetCashAccountId(connection, transaction);
            int pendingLiabilityId = GetAccountId(connection, transaction, "Service Pending Liability");

            List<LedgerLine> lines = new List<LedgerLine>();
            lines.Add(new LedgerLine { AccountId = pendingLiabilityId, Debit = grossAmount, Description = "Pending withdrawal settled" });
            lines.Add(new LedgerLine { AccountId = cashAccountId, Credit = grossAmount, Description = "Cash paid out for settled withdrawal" });
            InsertVoucher(connection, transaction, DateTime.Now, "Withdrawal Settlement", "service_transaction_header", serviceTxnId, remarks, userId, lines);
        }

        public static void PostServiceRefundEntry(MySqlConnection connection, MySqlTransaction transaction, long serviceTxnId, int walletAccountId, decimal grossAmount, decimal commissionAmount, string remarks, int userId)
        {
            EnsureSystemAccounts(connection, transaction);
            int cashAccountId = GetCashAccountId(connection, transaction);
            int walletLedgerAccountId = GetWalletLinkedAccountId(connection, transaction, walletAccountId);
            int serviceIncomeId = GetAccountId(connection, transaction, "Service Income");

            List<LedgerLine> lines = new List<LedgerLine>();
            lines.Add(new LedgerLine { AccountId = cashAccountId, Credit = grossAmount + commissionAmount, Description = "Cash returned on service refund" });
            lines.Add(new LedgerLine { AccountId = walletLedgerAccountId, Debit = grossAmount, Description = "Wallet restored from service refund" });
            lines.Add(new LedgerLine { AccountId = serviceIncomeId, Debit = commissionAmount, Description = "Service income reversed" });
            InsertVoucher(connection, transaction, DateTime.Now, "Service Refund", "service_transaction_header", serviceTxnId, remarks, userId, lines);
        }

        public static void PostWithdrawalServiceRefundEntry(MySqlConnection connection, MySqlTransaction transaction, long serviceTxnId, int walletAccountId, decimal grossAmount, decimal commissionAmount, string remarks, int userId)
        {
            EnsureSystemAccounts(connection, transaction);
            int cashAccountId = GetCashAccountId(connection, transaction);
            int walletLedgerAccountId = GetWalletLinkedAccountId(connection, transaction, walletAccountId);
            int serviceIncomeId = GetAccountId(connection, transaction, "Service Income");
            decimal receivedAmount = grossAmount + commissionAmount;

            List<LedgerLine> lines = new List<LedgerLine>();
            lines.Add(new LedgerLine { AccountId = cashAccountId, Debit = grossAmount, Description = "Cash recovered from withdrawal customer" });
            lines.Add(new LedgerLine { AccountId = serviceIncomeId, Debit = commissionAmount, Description = "Withdrawal commission reversed" });
            lines.Add(new LedgerLine { AccountId = walletLedgerAccountId, Credit = receivedAmount, Description = "Incoming withdrawal transfer returned" });
            InsertVoucher(connection, transaction, DateTime.Now, "Withdrawal Refund", "service_transaction_header", serviceTxnId, remarks, userId, lines);
        }

        public static void PostCustomerReceipt(MySqlConnection connection, MySqlTransaction transaction, long paymentId, int walletAccountId, decimal amount, string remarks, int userId)
        {
            EnsureSystemAccounts(connection, transaction);
            int walletLedgerAccountId = GetWalletLinkedAccountId(connection, transaction, walletAccountId);
            int customerReceivableId = GetAccountId(connection, transaction, "Customer Receivable");

            List<LedgerLine> lines = new List<LedgerLine>();
            lines.Add(new LedgerLine { AccountId = walletLedgerAccountId, Debit = amount, Description = "Customer payment received" });
            lines.Add(new LedgerLine { AccountId = customerReceivableId, Credit = amount, Description = "Customer receivable settled" });
            InsertVoucher(connection, transaction, DateTime.Now, "Receipt", "customer_payments", paymentId, remarks, userId, lines);
        }

        public static void PostVendorPayment(MySqlConnection connection, MySqlTransaction transaction, long paymentId, int walletAccountId, decimal amount, string remarks, int userId)
        {
            EnsureSystemAccounts(connection, transaction);
            int walletLedgerAccountId = GetWalletLinkedAccountId(connection, transaction, walletAccountId);
            int supplierPayableId = GetAccountId(connection, transaction, "Supplier Payable");

            List<LedgerLine> lines = new List<LedgerLine>();
            lines.Add(new LedgerLine { AccountId = supplierPayableId, Debit = amount, Description = "Supplier payment" });
            lines.Add(new LedgerLine { AccountId = walletLedgerAccountId, Credit = amount, Description = "Paid from wallet" });
            InsertVoucher(connection, transaction, DateTime.Now, "Payment", "supplier_payments", paymentId, remarks, userId, lines);
        }

        public static void PostPurchaseEntry(MySqlConnection connection, MySqlTransaction transaction, long purchaseId, decimal grandTotal, decimal paidAmount, int? walletAccountId, string remarks, int userId)
        {
            EnsureSystemAccounts(connection, transaction);
            int inventoryAssetId = GetAccountId(connection, transaction, "Inventory Asset");
            int supplierPayableId = GetAccountId(connection, transaction, "Supplier Payable");

            List<LedgerLine> lines = new List<LedgerLine>();
            lines.Add(new LedgerLine { AccountId = inventoryAssetId, Debit = grandTotal, Description = "Inventory received from purchase" });

            if (paidAmount > 0 && walletAccountId.HasValue)
            {
                lines.Add(new LedgerLine
                {
                    AccountId = GetWalletLinkedAccountId(connection, transaction, walletAccountId.Value),
                    Credit = paidAmount,
                    Description = "Purchase paid from wallet"
                });
            }

            decimal dueAmount = grandTotal - paidAmount;
            if (dueAmount > 0)
            {
                lines.Add(new LedgerLine { AccountId = supplierPayableId, Credit = dueAmount, Description = "Vendor credit purchase" });
            }

            InsertVoucher(connection, transaction, DateTime.Now, "Purchase", "purchase_header", purchaseId, remarks, userId, lines);
        }

        public static void DeleteVouchers(MySqlConnection connection, MySqlTransaction transaction, string referenceTable, long referenceId)
        {
            List<long> voucherIds = new List<long>();

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT ledger_txn_id
                    FROM ledger_transactions
                    WHERE reference_table = @referenceTable
                      AND reference_id = @referenceId;";
                command.Parameters.AddWithValue("@referenceTable", referenceTable);
                command.Parameters.AddWithValue("@referenceId", referenceId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        voucherIds.Add(Convert.ToInt64(reader["ledger_txn_id"]));
                    }
                }
            }

            for (int i = 0; i < voucherIds.Count; i++)
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = "DELETE FROM ledger_transaction_lines WHERE ledger_txn_id = @ledgerTxnId;";
                    command.Parameters.AddWithValue("@ledgerTxnId", voucherIds[i]);
                    command.ExecuteNonQuery();
                }

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = "DELETE FROM ledger_transactions WHERE ledger_txn_id = @ledgerTxnId;";
                    command.Parameters.AddWithValue("@ledgerTxnId", voucherIds[i]);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteVouchersByType(MySqlConnection connection, MySqlTransaction transaction, string voucherType, string referenceTable, long referenceId)
        {
            List<long> voucherIds = new List<long>();

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT ledger_txn_id
                    FROM ledger_transactions
                    WHERE voucher_type = @voucherType
                      AND reference_table = @referenceTable
                      AND reference_id = @referenceId;";
                command.Parameters.AddWithValue("@voucherType", voucherType);
                command.Parameters.AddWithValue("@referenceTable", referenceTable);
                command.Parameters.AddWithValue("@referenceId", referenceId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        voucherIds.Add(Convert.ToInt64(reader["ledger_txn_id"]));
                    }
                }
            }

            for (int i = 0; i < voucherIds.Count; i++)
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = "DELETE FROM ledger_transaction_lines WHERE ledger_txn_id = @ledgerTxnId;";
                    command.Parameters.AddWithValue("@ledgerTxnId", voucherIds[i]);
                    command.ExecuteNonQuery();
                }

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = "DELETE FROM ledger_transactions WHERE ledger_txn_id = @ledgerTxnId;";
                    command.Parameters.AddWithValue("@ledgerTxnId", voucherIds[i]);
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void InsertVoucher(MySqlConnection connection, MySqlTransaction transaction, DateTime txnDate, string voucherType, string referenceTable, long referenceId, string remarks, int userId, List<LedgerLine> lines)
        {
            long ledgerTxnId;
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    INSERT INTO ledger_transactions
                    (txn_date, voucher_type, reference_table, reference_id, remarks, created_by)
                    VALUES
                    (@txnDate, @voucherType, @referenceTable, @referenceId, @remarks, @createdBy);
                    SELECT LAST_INSERT_ID();";
                command.Parameters.AddWithValue("@txnDate", txnDate);
                command.Parameters.AddWithValue("@voucherType", voucherType);
                command.Parameters.AddWithValue("@referenceTable", referenceTable);
                command.Parameters.AddWithValue("@referenceId", referenceId);
                command.Parameters.AddWithValue("@remarks", string.IsNullOrWhiteSpace(remarks) ? (object)DBNull.Value : remarks.Trim());
                command.Parameters.AddWithValue("@createdBy", userId);
                ledgerTxnId = Convert.ToInt64(command.ExecuteScalar());
            }

            for (int i = 0; i < lines.Count; i++)
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = @"
                        INSERT INTO ledger_transaction_lines
                        (ledger_txn_id, account_id, debit, credit, description)
                        VALUES
                        (@ledgerTxnId, @accountId, @debit, @credit, @description);";
                    command.Parameters.AddWithValue("@ledgerTxnId", ledgerTxnId);
                    command.Parameters.AddWithValue("@accountId", lines[i].AccountId);
                    command.Parameters.AddWithValue("@debit", lines[i].Debit);
                    command.Parameters.AddWithValue("@credit", lines[i].Credit);
                    command.Parameters.AddWithValue("@description", string.IsNullOrWhiteSpace(lines[i].Description) ? (object)DBNull.Value : lines[i].Description);
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void EnsureWalletLinkedAccounts(MySqlConnection connection, MySqlTransaction transaction)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT wallet_account_id, account_name
                    FROM wallet_accounts
                    WHERE is_active = 1;";

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    List<LookupOption> wallets = new List<LookupOption>();
                    while (reader.Read())
                    {
                        wallets.Add(new LookupOption
                        {
                            Id = Convert.ToInt32(reader["wallet_account_id"]),
                            Name = Convert.ToString(reader["account_name"])
                        });
                    }
                    reader.Close();

                    for (int i = 0; i < wallets.Count; i++)
                    {
                        EnsureNamedAccount(connection, transaction, wallets[i].Name, "Asset", "Wallet", wallets[i].Id);
                    }
                }
            }
        }

        private static int GetCashAccountId(MySqlConnection connection, MySqlTransaction transaction)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT a.account_id
                    FROM accounts a
                    WHERE a.linked_entity_type = 'Wallet'
                      AND a.account_name LIKE '%Cash%'
                    LIMIT 1;";
                object value = command.ExecuteScalar();
                if (value != null && value != DBNull.Value)
                {
                    return Convert.ToInt32(value);
                }
            }

            return GetAccountId(connection, transaction, "Cash in Hand");
        }

        private static int GetWalletLinkedAccountId(MySqlConnection connection, MySqlTransaction transaction, int walletAccountId)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT account_id
                    FROM accounts
                    WHERE linked_entity_type = 'Wallet'
                      AND linked_entity_id = @walletAccountId
                    LIMIT 1;";
                command.Parameters.AddWithValue("@walletAccountId", walletAccountId);
                object value = command.ExecuteScalar();
                if (value == null || value == DBNull.Value)
                {
                    throw new InvalidOperationException("Wallet linked account not found.");
                }

                return Convert.ToInt32(value);
            }
        }

        private static int GetAccountId(MySqlConnection connection, MySqlTransaction transaction, string accountName)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT account_id
                    FROM accounts
                    WHERE account_name = @accountName
                    LIMIT 1;";
                command.Parameters.AddWithValue("@accountName", accountName);
                object value = command.ExecuteScalar();
                if (value == null || value == DBNull.Value)
                {
                    throw new InvalidOperationException(string.Format("Required account not found: {0}", accountName));
                }

                return Convert.ToInt32(value);
            }
        }

        private static void EnsureNamedAccount(MySqlConnection connection, MySqlTransaction transaction, string accountName, string accountType, string linkedEntityType, int? linkedEntityId)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT account_id
                    FROM accounts
                    WHERE account_name = @accountName
                    LIMIT 1;";
                command.Parameters.AddWithValue("@accountName", accountName);
                object existing = command.ExecuteScalar();
                if (existing != null && existing != DBNull.Value)
                {
                    return;
                }
            }

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    INSERT INTO accounts
                    (account_name, account_type, parent_account_id, linked_entity_type, linked_entity_id, is_active)
                    VALUES
                    (@accountName, @accountType, NULL, @linkedEntityType, @linkedEntityId, 1);";
                command.Parameters.AddWithValue("@accountName", accountName);
                command.Parameters.AddWithValue("@accountType", accountType);
                command.Parameters.AddWithValue("@linkedEntityType", linkedEntityType);
                command.Parameters.AddWithValue("@linkedEntityId", (object)linkedEntityId ?? DBNull.Value);
                command.ExecuteNonQuery();
            }
        }

        private static bool IsDebitNature(string accountType)
        {
            return accountType == "Asset" || accountType == "Expense";
        }

        private static decimal GetSignedBalance(string accountType, decimal debit, decimal credit)
        {
            return IsDebitNature(accountType) ? debit - credit : credit - debit;
        }

        private static string FormatReferenceLabel(string referenceTable, long? referenceId)
        {
            string friendlyName;
            switch ((referenceTable ?? string.Empty).ToLowerInvariant())
            {
                case "sale_header":
                    friendlyName = "Grocery Sale";
                    break;
                case "service_transaction_header":
                    friendlyName = "Service Transaction";
                    break;
                case "customer_payments":
                    friendlyName = "Customer Receipt";
                    break;
                case "supplier_payments":
                    friendlyName = "Vendor Payment";
                    break;
                case "purchase_header":
                    friendlyName = "Purchase";
                    break;
                case "expenses":
                    friendlyName = "Expense";
                    break;
                default:
                    friendlyName = string.IsNullOrWhiteSpace(referenceTable)
                        ? "Ledger Entry"
                        : referenceTable.Replace("_", " ");
                    break;
            }

            return referenceId.HasValue
                ? string.Format("{0} #{1}", friendlyName, referenceId.Value)
                : friendlyName;
        }

        private class LedgerLine
        {
            public int AccountId { get; set; }
            public decimal Debit { get; set; }
            public decimal Credit { get; set; }
            public string Description { get; set; }
        }
    }
}
