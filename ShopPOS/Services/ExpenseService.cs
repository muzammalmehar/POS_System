using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ShopPOS.Data;
using ShopPOS.Models;

namespace ShopPOS.Services
{
    public class ExpenseService
    {
        public List<LookupOption> GetExpenseTypes()
        {
            return LoadLookupOptions("SELECT expense_type_id, expense_type_name FROM expense_types WHERE is_active = 1 ORDER BY expense_type_name ASC;");
        }

        public List<ExpenseRecord> GetRecentExpenses()
        {
            List<ExpenseRecord> items = new List<ExpenseRecord>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT
                        e.expense_id,
                        et.expense_type_name,
                        e.expense_date,
                        e.amount,
                        wa.account_name,
                        e.description,
                        u.full_name
                    FROM expenses e
                    INNER JOIN expense_types et ON et.expense_type_id = e.expense_type_id
                    INNER JOIN wallet_accounts wa ON wa.wallet_account_id = e.payment_wallet_account_id
                    INNER JOIN users u ON u.user_id = e.created_by
                    ORDER BY e.expense_date DESC
                    LIMIT 30;";

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ExpenseRecord item = new ExpenseRecord();
                        item.ExpenseId = Convert.ToInt64(reader["expense_id"]);
                        item.ExpenseTypeName = Convert.ToString(reader["expense_type_name"]);
                        item.ExpenseDate = Convert.ToDateTime(reader["expense_date"]);
                        item.Amount = Convert.ToDecimal(reader["amount"]);
                        item.WalletName = Convert.ToString(reader["account_name"]);
                        item.Description = Convert.ToString(reader["description"]);
                        item.CreatedByName = Convert.ToString(reader["full_name"]);
                        items.Add(item);
                    }
                }
            }

            return items;
        }

        public void SaveExpense(ExpenseSaveRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.ExpenseTypeId <= 0)
            {
                throw new InvalidOperationException("Select an expense type.");
            }

            if (request.WalletAccountId <= 0)
            {
                throw new InvalidOperationException("Select a payment wallet.");
            }

            if (request.Amount <= 0)
            {
                throw new InvalidOperationException("Amount must be greater than zero.");
            }

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    long expenseId;
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = @"
                            INSERT INTO expenses (
                                expense_type_id, expense_date, amount, payment_wallet_account_id,
                                description, shift_id, created_by
                            )
                            VALUES (
                                @expenseTypeId, @expenseDate, @amount, @walletId,
                                @description, NULL, @createdBy
                            );
                            SELECT LAST_INSERT_ID();";
                        command.Parameters.AddWithValue("@expenseTypeId", request.ExpenseTypeId);
                        command.Parameters.AddWithValue("@expenseDate", request.ExpenseDate);
                        command.Parameters.AddWithValue("@amount", request.Amount);
                        command.Parameters.AddWithValue("@walletId", request.WalletAccountId);
                        command.Parameters.AddWithValue("@description", string.IsNullOrWhiteSpace(request.Description) ? (object)DBNull.Value : request.Description.Trim());
                        command.Parameters.AddWithValue("@createdBy", request.UserId);
                        expenseId = Convert.ToInt64(command.ExecuteScalar());
                    }

                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = @"
                            UPDATE wallet_accounts
                            SET current_balance = current_balance - @amount
                            WHERE wallet_account_id = @walletId;";
                        command.Parameters.AddWithValue("@amount", request.Amount);
                        command.Parameters.AddWithValue("@walletId", request.WalletAccountId);
                        command.ExecuteNonQuery();
                    }

                    AccountingService.PostExpenseEntry(
                        connection,
                        transaction,
                        expenseId,
                        request.WalletAccountId,
                        request.Amount,
                        request.Description,
                        request.UserId);

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private static List<LookupOption> LoadLookupOptions(string sql)
        {
            List<LookupOption> items = new List<LookupOption>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = sql;

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        LookupOption item = new LookupOption();
                        item.Id = Convert.ToInt32(reader.GetValue(0));
                        item.Name = Convert.ToString(reader.GetValue(1));
                        items.Add(item);
                    }
                }
            }

            return items;
        }
    }
}
