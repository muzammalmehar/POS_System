using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ShopPOS.Data;
using ShopPOS.Models;

namespace ShopPOS.Services
{
    public class ServiceCenterService
    {
        public ServiceTransactionSaveRequest GetTransactionForEdit(long serviceTransactionId)
        {
            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlCommand command = connection.CreateCommand())
            {
                EnsureServiceRefundColumns(connection);

                command.CommandText = @"
                    SELECT
                        service_txn_id,
                        service_type_id,
                        txn_date,
                        customer_name,
                        customer_mobile,
                        reference_number,
                        bill_category,
                        wallet_account_id,
                        payment_method,
                        customer_account_no,
                        external_transaction_id,
                        amount,
                        commission_earned,
                        status,
                        remarks,
                        IFNULL(is_refunded, 0) AS is_refunded
                    FROM service_transaction_header
                    WHERE service_txn_id = @serviceTransactionId;";
                command.Parameters.AddWithValue("@serviceTransactionId", serviceTransactionId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        throw new InvalidOperationException("Selected service sale was not found.");
                    }

                    if (Convert.ToBoolean(reader["is_refunded"]) || Convert.ToString(reader["status"]) == "Refunded")
                    {
                        throw new InvalidOperationException("Refunded service sales cannot be edited.");
                    }

                    ServiceTransactionSaveRequest request = new ServiceTransactionSaveRequest();
                    request.ServiceTransactionId = Convert.ToInt64(reader["service_txn_id"]);
                    request.ServiceTypeId = Convert.ToInt32(reader["service_type_id"]);
                    request.TransactionDate = Convert.ToDateTime(reader["txn_date"]);
                    request.CustomerName = Convert.ToString(reader["customer_name"]);
                    request.CustomerMobile = Convert.ToString(reader["customer_mobile"]);
                    request.ReferenceNumber = Convert.ToString(reader["reference_number"]);
                    request.BillCategory = Convert.ToString(reader["bill_category"]);
                    request.WalletAccountId = Convert.ToInt32(reader["wallet_account_id"]);
                    request.PaymentMethod = Convert.ToString(reader["payment_method"]);
                    request.CustomerAccountNumber = Convert.ToString(reader["customer_account_no"]);
                    request.ExternalTransactionId = Convert.ToString(reader["external_transaction_id"]);
                    request.IsExternalTransactionIdNotApplicable = string.IsNullOrWhiteSpace(request.ExternalTransactionId);
                    request.Amount = Convert.ToDecimal(reader["amount"]);
                    request.ServiceCharge = Convert.ToDecimal(reader["commission_earned"]);
                    request.Status = Convert.ToString(reader["status"]);
                    request.Remarks = Convert.ToString(reader["remarks"]);
                    return request;
                }
            }
        }

        public ServiceTransactionRecord GetTransactionRecord(long serviceTransactionId)
        {
            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlCommand command = connection.CreateCommand())
            {
                EnsureServiceRefundColumns(connection);

                command.CommandText = @"
                    SELECT
                        sth.service_txn_id,
                        sth.txn_no,
                        sth.txn_date,
                        st.service_name,
                        st.provider_name,
                        sth.customer_name,
                        sth.customer_mobile,
                        sth.reference_number,
                        sth.bill_category,
                        wa.account_name,
                        sth.payment_method,
                        sth.customer_account_no,
                        sth.external_transaction_id,
                        sth.amount,
                        sth.service_charge,
                        sth.commission_earned,
                        sth.net_effect_amount,
                        CASE
                            WHEN IFNULL(sth.is_refunded, 0) = 1 OR sth.status = 'Refunded' THEN 'Refunded'
                            ELSE sth.status
                        END AS status,
                        sth.remarks,
                        u.full_name
                    FROM service_transaction_header sth
                    INNER JOIN service_types st ON st.service_type_id = sth.service_type_id
                    INNER JOIN wallet_accounts wa ON wa.wallet_account_id = sth.wallet_account_id
                    INNER JOIN users u ON u.user_id = sth.created_by
                    WHERE sth.service_txn_id = @serviceTransactionId;";
                command.Parameters.AddWithValue("@serviceTransactionId", serviceTransactionId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        throw new InvalidOperationException("Selected service sale was not found.");
                    }

                    ServiceTransactionRecord item = new ServiceTransactionRecord();
                    item.ServiceTransactionId = Convert.ToInt64(reader["service_txn_id"]);
                    item.TransactionNo = Convert.ToString(reader["txn_no"]);
                    item.TransactionDate = Convert.ToDateTime(reader["txn_date"]);
                    item.ServiceName = Convert.ToString(reader["service_name"]);
                    item.ProviderName = Convert.ToString(reader["provider_name"]);
                    item.CustomerName = Convert.ToString(reader["customer_name"]);
                    item.CustomerMobile = Convert.ToString(reader["customer_mobile"]);
                    item.ReferenceNumber = Convert.ToString(reader["reference_number"]);
                    item.BillCategory = Convert.ToString(reader["bill_category"]);
                    item.WalletName = Convert.ToString(reader["account_name"]);
                    item.PaymentMethod = Convert.ToString(reader["payment_method"]);
                    item.CustomerAccountNumber = Convert.ToString(reader["customer_account_no"]);
                    item.ExternalTransactionId = Convert.ToString(reader["external_transaction_id"]);
                    item.Amount = Convert.ToDecimal(reader["amount"]);
                    item.ServiceCharge = Convert.ToDecimal(reader["service_charge"]);
                    item.CommissionEarned = Convert.ToDecimal(reader["commission_earned"]);
                    item.NetEffectAmount = Convert.ToDecimal(reader["net_effect_amount"]);
                    item.Status = Convert.ToString(reader["status"]);
                    item.Remarks = Convert.ToString(reader["remarks"]);
                    item.CreatedByName = Convert.ToString(reader["full_name"]);
                    return item;
                }
            }
        }

        public List<ServiceTypeRecord> GetServiceTypes()
        {
            List<ServiceTypeRecord> items = new List<ServiceTypeRecord>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT
                        service_type_id,
                        service_name,
                        provider_name,
                        default_charge,
                        commission_type,
                        commission_value,
                        is_active
                    FROM service_types
                    WHERE is_active = 1
                    ORDER BY service_name ASC;";

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ServiceTypeRecord item = new ServiceTypeRecord();
                        item.ServiceTypeId = Convert.ToInt32(reader["service_type_id"]);
                        item.ServiceName = Convert.ToString(reader["service_name"]);
                        item.ProviderName = Convert.ToString(reader["provider_name"]);
                        item.DefaultCharge = Convert.ToDecimal(reader["default_charge"]);
                        item.CommissionType = NormalizeCommissionType(Convert.ToString(reader["commission_type"]));
                        item.CommissionValue = Convert.ToDecimal(reader["commission_value"]);
                        item.IsActive = Convert.ToBoolean(reader["is_active"]);
                        items.Add(item);
                    }
                }
            }

            return items;
        }

        public ServiceTypeRecord GetServiceType(int serviceTypeId)
        {
            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT
                        service_type_id,
                        service_name,
                        provider_name,
                        default_charge,
                        commission_type,
                        commission_value,
                        is_active
                    FROM service_types
                    WHERE service_type_id = @serviceTypeId;";
                command.Parameters.AddWithValue("@serviceTypeId", serviceTypeId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        throw new InvalidOperationException("Selected service type was not found.");
                    }

                    ServiceTypeRecord item = new ServiceTypeRecord();
                    item.ServiceTypeId = Convert.ToInt32(reader["service_type_id"]);
                    item.ServiceName = Convert.ToString(reader["service_name"]);
                    item.ProviderName = Convert.ToString(reader["provider_name"]);
                    item.DefaultCharge = Convert.ToDecimal(reader["default_charge"]);
                    item.CommissionType = NormalizeCommissionType(Convert.ToString(reader["commission_type"]));
                    item.CommissionValue = Convert.ToDecimal(reader["commission_value"]);
                    item.IsActive = Convert.ToBoolean(reader["is_active"]);
                    return item;
                }
            }
        }

        public int SaveServiceType(ServiceTypeRecord serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            if (string.IsNullOrWhiteSpace(serviceType.ServiceName))
            {
                throw new InvalidOperationException("Service name is required.");
            }

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlCommand command = connection.CreateCommand())
            {
                if (serviceType.ServiceTypeId > 0)
                {
                    command.CommandText = @"
                        UPDATE service_types
                        SET
                            service_name = @serviceName,
                            provider_name = @providerName,
                            default_charge = @defaultCharge,
                            commission_type = @commissionType,
                            commission_value = @commissionValue,
                            is_active = @isActive
                        WHERE service_type_id = @serviceTypeId;";
                    command.Parameters.AddWithValue("@serviceTypeId", serviceType.ServiceTypeId);
                }
                else
                {
                    command.CommandText = @"
                        INSERT INTO service_types
                        (
                            service_name,
                            provider_name,
                            default_charge,
                            commission_type,
                            commission_value,
                            is_active
                        )
                        VALUES
                        (
                            @serviceName,
                            @providerName,
                            @defaultCharge,
                            @commissionType,
                            @commissionValue,
                            @isActive
                        );";
                }

                command.Parameters.AddWithValue("@serviceName", serviceType.ServiceName.Trim());
                command.Parameters.AddWithValue("@providerName", string.IsNullOrWhiteSpace(serviceType.ProviderName) ? (object)DBNull.Value : serviceType.ProviderName.Trim());
                command.Parameters.AddWithValue("@defaultCharge", serviceType.DefaultCharge);
                command.Parameters.AddWithValue("@commissionType", NormalizeCommissionType(serviceType.CommissionType));
                command.Parameters.AddWithValue("@commissionValue", serviceType.CommissionValue);
                command.Parameters.AddWithValue("@isActive", serviceType.IsActive);
                command.ExecuteNonQuery();

                return serviceType.ServiceTypeId > 0 ? serviceType.ServiceTypeId : Convert.ToInt32(command.LastInsertedId);
            }
        }

        public List<ServiceCustomerProfileRecord> GetCustomerProfiles()
        {
            List<ServiceCustomerProfileRecord> items = new List<ServiceCustomerProfileRecord>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureServiceCustomerProfileTable(connection);

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            scp.service_customer_profile_id,
                            scp.customer_name,
                            scp.customer_mobile,
                            scp.reference_number,
                            scp.bill_category,
                            scp.service_type_id,
                            st.service_name,
                            scp.preferred_wallet_account_id,
                            wa.account_name,
                            scp.default_amount,
                            scp.default_service_charge,
                            scp.recurrence_type,
                            scp.expected_day_of_month,
                            scp.next_due_date,
                            scp.last_service_date,
                            scp.notes,
                            scp.is_active
                        FROM service_customer_profiles scp
                        LEFT JOIN service_types st ON st.service_type_id = scp.service_type_id
                        LEFT JOIN wallet_accounts wa ON wa.wallet_account_id = scp.preferred_wallet_account_id
                        WHERE scp.is_active = 1
                        ORDER BY
                            CASE WHEN scp.next_due_date IS NULL THEN 1 ELSE 0 END,
                            scp.next_due_date ASC,
                            scp.customer_name ASC;";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ServiceCustomerProfileRecord item = new ServiceCustomerProfileRecord();
                            item.ServiceCustomerProfileId = Convert.ToInt32(reader["service_customer_profile_id"]);
                            item.CustomerName = Convert.ToString(reader["customer_name"]);
                            item.CustomerMobile = Convert.ToString(reader["customer_mobile"]);
                            item.ReferenceNumber = Convert.ToString(reader["reference_number"]);
                            item.BillCategory = Convert.ToString(reader["bill_category"]);
                            item.ServiceTypeId = reader["service_type_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["service_type_id"]);
                            item.ServiceTypeName = Convert.ToString(reader["service_name"]);
                            item.WalletAccountId = reader["preferred_wallet_account_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["preferred_wallet_account_id"]);
                            item.WalletName = Convert.ToString(reader["account_name"]);
                            item.DefaultAmount = Convert.ToDecimal(reader["default_amount"]);
                            item.DefaultServiceCharge = Convert.ToDecimal(reader["default_service_charge"]);
                            item.RecurrenceType = Convert.ToString(reader["recurrence_type"]);
                            item.ExpectedDayOfMonth = reader["expected_day_of_month"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["expected_day_of_month"]);
                            item.NextDueDate = reader["next_due_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["next_due_date"]);
                            item.LastServiceDate = reader["last_service_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["last_service_date"]);
                            item.Notes = Convert.ToString(reader["notes"]);
                            item.IsActive = Convert.ToBoolean(reader["is_active"]);
                            items.Add(item);
                        }
                    }
                }
            }

            return items;
        }

        public List<ServiceTransactionRecord> GetRecentTransactions(string customerFilter = null, string statusFilter = null, string billCategoryFilter = null)
        {
            List<ServiceTransactionRecord> items = new List<ServiceTransactionRecord>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlCommand command = connection.CreateCommand())
            {
                EnsureServiceRefundColumns(connection);

                command.CommandText = @"
                    SELECT
                        sth.service_txn_id,
                        sth.txn_no,
                        sth.txn_date,
                        st.service_name,
                        st.provider_name,
                        sth.customer_name,
                        sth.customer_mobile,
                        sth.reference_number,
                        sth.bill_category,
                        wa.account_name,
                        sth.payment_method,
                        sth.customer_account_no,
                        sth.external_transaction_id,
                        sth.amount,
                        sth.service_charge,
                        sth.commission_earned,
                        sth.net_effect_amount,
                        sth.status,
                        IFNULL(sth.is_refunded, 0) AS is_refunded,
                        sth.remarks,
                        u.full_name
                    FROM service_transaction_header sth
                    INNER JOIN service_types st ON st.service_type_id = sth.service_type_id
                    INNER JOIN wallet_accounts wa ON wa.wallet_account_id = sth.wallet_account_id
                    INNER JOIN users u ON u.user_id = sth.created_by
                    WHERE
                        (
                            @customerFilter = '' OR
                            LOWER(IFNULL(sth.customer_name, '')) LIKE CONCAT('%', @customerFilter, '%') OR
                            LOWER(IFNULL(sth.customer_mobile, '')) LIKE CONCAT('%', @customerFilter, '%') OR
                            LOWER(IFNULL(sth.reference_number, '')) LIKE CONCAT('%', @customerFilter, '%') OR
                            LOWER(IFNULL(sth.bill_category, '')) LIKE CONCAT('%', @customerFilter, '%') OR
                            LOWER(IFNULL(sth.customer_account_no, '')) LIKE CONCAT('%', @customerFilter, '%') OR
                            LOWER(IFNULL(sth.external_transaction_id, '')) LIKE CONCAT('%', @customerFilter, '%') OR
                            LOWER(IFNULL(sth.payment_method, '')) LIKE CONCAT('%', @customerFilter, '%')
                        )
                      AND
                        (
                            @billCategoryFilter = 'All' OR
                            IFNULL(sth.bill_category, '') = @billCategoryFilter
                        )
                      AND
                        (
                            @statusFilter = 'All' OR
                            (CASE
                                WHEN IFNULL(sth.is_refunded, 0) = 1 THEN 'Refunded'
                                ELSE sth.status
                             END) = @statusFilter
                        )
                    ORDER BY sth.txn_date DESC, sth.service_txn_id DESC
                    LIMIT 200;";
                command.Parameters.AddWithValue("@customerFilter", string.IsNullOrWhiteSpace(customerFilter) ? string.Empty : customerFilter.Trim().ToLowerInvariant());
                command.Parameters.AddWithValue("@billCategoryFilter", NormalizeBillCategoryFilter(billCategoryFilter));
                command.Parameters.AddWithValue("@statusFilter", string.IsNullOrWhiteSpace(statusFilter) ? "All" : statusFilter.Trim());

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ServiceTransactionRecord item = new ServiceTransactionRecord();
                        item.ServiceTransactionId = Convert.ToInt64(reader["service_txn_id"]);
                        item.TransactionNo = Convert.ToString(reader["txn_no"]);
                        item.TransactionDate = Convert.ToDateTime(reader["txn_date"]);
                        item.ServiceName = Convert.ToString(reader["service_name"]);
                        item.ProviderName = Convert.ToString(reader["provider_name"]);
                        item.CustomerName = Convert.ToString(reader["customer_name"]);
                        item.CustomerMobile = Convert.ToString(reader["customer_mobile"]);
                        item.ReferenceNumber = Convert.ToString(reader["reference_number"]);
                        item.BillCategory = Convert.ToString(reader["bill_category"]);
                        item.WalletName = Convert.ToString(reader["account_name"]);
                        item.PaymentMethod = Convert.ToString(reader["payment_method"]);
                        item.CustomerAccountNumber = Convert.ToString(reader["customer_account_no"]);
                        item.ExternalTransactionId = Convert.ToString(reader["external_transaction_id"]);
                        item.Amount = Convert.ToDecimal(reader["amount"]);
                        item.ServiceCharge = Convert.ToDecimal(reader["service_charge"]);
                        item.CommissionEarned = Convert.ToDecimal(reader["commission_earned"]);
                        item.NetEffectAmount = Convert.ToDecimal(reader["net_effect_amount"]);
                        item.Status = Convert.ToBoolean(reader["is_refunded"]) ? "Refunded" : Convert.ToString(reader["status"]);
                        item.Remarks = Convert.ToString(reader["remarks"]);
                        item.CreatedByName = Convert.ToString(reader["full_name"]);
                        items.Add(item);
                    }
                }
            }

            return items;
        }

        public void UpdateTransactionStatus(long serviceTransactionId, string newStatus, string remarks, int userId)
        {
            if (string.IsNullOrWhiteSpace(newStatus))
            {
                throw new InvalidOperationException("Select a valid service status.");
            }

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureServiceCustomerProfileTable(connection);
                EnsureServiceRefundColumns(connection);

                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        ServiceTransactionSaveRequest existing = GetTransactionForEdit(connection, transaction, serviceTransactionId);
                        ServiceTypeRecord existingServiceType = GetServiceTypeById(connection, transaction, existing.ServiceTypeId);
                        string oldStatus = existing.Status ?? string.Empty;
                        bool isWithdrawalService = IsWithdrawalService(existingServiceType);

                        if (isWithdrawalService && string.Equals(newStatus, "Pending", StringComparison.OrdinalIgnoreCase))
                        {
                            throw new InvalidOperationException("Withdrawal services should be saved as Completed or Cancelled, not Pending.");
                        }

                        if (string.Equals(oldStatus, newStatus, StringComparison.OrdinalIgnoreCase))
                        {
                            using (MySqlCommand command = connection.CreateCommand())
                            {
                                command.Transaction = transaction;
                                command.CommandText = @"
                                    UPDATE service_transaction_header
                                    SET remarks = @remarks,
                                        edited_at = NOW(),
                                        edited_by = @editedBy
                                    WHERE service_txn_id = @serviceTxnId;";
                                command.Parameters.AddWithValue("@remarks", string.IsNullOrWhiteSpace(remarks) ? (object)DBNull.Value : remarks.Trim());
                                command.Parameters.AddWithValue("@editedBy", userId);
                                command.Parameters.AddWithValue("@serviceTxnId", serviceTransactionId);
                                command.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return;
                        }

                        if (string.Equals(oldStatus, "Completed", StringComparison.OrdinalIgnoreCase))
                        {
                            ReverseServiceOperationalEffect(connection, transaction, existing, existingServiceType);
                        }
                        else if (string.Equals(oldStatus, "Pending", StringComparison.OrdinalIgnoreCase) &&
                                 !string.Equals(newStatus, "Completed", StringComparison.OrdinalIgnoreCase))
                        {
                            AccountingService.DeleteVouchers(connection, transaction, "service_transaction_header", serviceTransactionId);
                        }

                        if (string.Equals(newStatus, "Completed", StringComparison.OrdinalIgnoreCase))
                        {
                            if (isWithdrawalService)
                            {
                                AdjustWalletBalance(connection, transaction, existing.WalletAccountId, existing.Amount + existing.ServiceCharge);
                                AccountingService.PostWithdrawalServiceEntry(connection, transaction, serviceTransactionId, existing.TransactionDate, existing.WalletAccountId, existing.Amount, existing.ServiceCharge, remarks, userId);
                            }
                            else if (string.Equals(oldStatus, "Pending", StringComparison.OrdinalIgnoreCase))
                            {
                                AdjustWalletBalance(connection, transaction, existing.WalletAccountId, -existing.Amount);
                                AccountingService.PostPendingServiceSettlementEntry(connection, transaction, serviceTransactionId, existing.WalletAccountId, existing.Amount, remarks, userId);
                            }
                            else
                            {
                                AdjustWalletBalance(connection, transaction, existing.WalletAccountId, -existing.Amount);
                                AccountingService.PostServiceEntry(connection, transaction, serviceTransactionId, existing.TransactionDate, existing.WalletAccountId, existing.Amount, existing.ServiceCharge, remarks, userId);
                            }
                        }
                        else if (!isWithdrawalService && string.Equals(newStatus, "Pending", StringComparison.OrdinalIgnoreCase))
                        {
                            AccountingService.PostPendingServiceEntry(connection, transaction, serviceTransactionId, existing.TransactionDate, existing.Amount, existing.ServiceCharge, remarks, userId);
                        }

                        using (MySqlCommand command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandText = @"
                                UPDATE service_transaction_header
                                SET status = @status,
                                    remarks = @remarks,
                                    edited_at = NOW(),
                                    edited_by = @editedBy
                                WHERE service_txn_id = @serviceTxnId;";
                            command.Parameters.AddWithValue("@status", newStatus);
                            command.Parameters.AddWithValue("@remarks", string.IsNullOrWhiteSpace(remarks) ? (object)DBNull.Value : remarks.Trim());
                            command.Parameters.AddWithValue("@editedBy", userId);
                            command.Parameters.AddWithValue("@serviceTxnId", serviceTransactionId);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void SaveCustomerProfile(ServiceCustomerProfileRecord profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }

            ValidateProfile(profile);

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureServiceCustomerProfileTable(connection);
                if (profile.ServiceTypeId.HasValue)
                {
                    ServiceTypeRecord serviceType = GetServiceTypeById(connection, null, profile.ServiceTypeId.Value);
                    profile.BillCategory = PrepareBillCategory(serviceType, profile.BillCategory);
                }
                else
                {
                    profile.BillCategory = null;
                }

                SaveOrUpdateProfile(connection, null, profile);
            }
        }

        public string SaveServiceTransaction(ServiceTransactionSaveRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            ValidateTransaction(request);

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureServiceCustomerProfileTable(connection);
                EnsureServiceRefundColumns(connection);

                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        ServiceTypeRecord serviceType = GetServiceTypeById(connection, transaction, request.ServiceTypeId);
                        string txnNo = GenerateNextTxnNo(connection, transaction);
                        decimal commission = request.ServiceCharge > 0
                            ? request.ServiceCharge
                            : CalculateCommission(serviceType, request.Amount);
                        ValidateTransactionForServiceType(request, serviceType);
                        string billCategory = PrepareBillCategory(serviceType, request.BillCategory);
                        decimal netEffectAmount = commission;
                        long serviceTxnId;

                        using (MySqlCommand command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandText = @"
                                INSERT INTO service_transaction_header
                                (
                                    txn_no, txn_date, service_type_id, customer_name, customer_mobile,
                                    reference_number, bill_category, wallet_account_id, payment_method, customer_account_no,
                                    external_transaction_id, amount, service_charge,
                                    commission_earned, net_effect_amount, status, remarks, shift_id, created_by
                                )
                                VALUES
                                (
                                    @txnNo, @txnDate, @serviceTypeId, @customerName, @customerMobile,
                                    @referenceNumber, @billCategory, @walletAccountId, @paymentMethod, @customerAccountNo,
                                    @externalTransactionId, @amount, @serviceCharge,
                                    @commissionEarned, @netEffectAmount, @status, @remarks, NULL, @createdBy
                                );
                                SELECT LAST_INSERT_ID();";

                            command.Parameters.AddWithValue("@txnNo", txnNo);
                            command.Parameters.AddWithValue("@txnDate", request.TransactionDate);
                            command.Parameters.AddWithValue("@serviceTypeId", request.ServiceTypeId);
                            command.Parameters.AddWithValue("@customerName", request.CustomerName.Trim());
                            command.Parameters.AddWithValue("@customerMobile", string.IsNullOrWhiteSpace(request.CustomerMobile) ? (object)DBNull.Value : request.CustomerMobile.Trim());
                            command.Parameters.AddWithValue("@referenceNumber", string.IsNullOrWhiteSpace(request.ReferenceNumber) ? (object)DBNull.Value : request.ReferenceNumber.Trim());
                            command.Parameters.AddWithValue("@billCategory", string.IsNullOrWhiteSpace(billCategory) ? (object)DBNull.Value : billCategory);
                            command.Parameters.AddWithValue("@walletAccountId", request.WalletAccountId);
                            command.Parameters.AddWithValue("@paymentMethod", NormalizePaymentMethod(request.PaymentMethod));
                            command.Parameters.AddWithValue("@customerAccountNo", string.IsNullOrWhiteSpace(request.CustomerAccountNumber) ? (object)DBNull.Value : request.CustomerAccountNumber.Trim());
                            command.Parameters.AddWithValue("@externalTransactionId", string.IsNullOrWhiteSpace(request.ExternalTransactionId) ? (object)DBNull.Value : request.ExternalTransactionId.Trim());
                            command.Parameters.AddWithValue("@amount", request.Amount);
                            command.Parameters.AddWithValue("@serviceCharge", 0M);
                            command.Parameters.AddWithValue("@commissionEarned", commission);
                            command.Parameters.AddWithValue("@netEffectAmount", netEffectAmount);
                            command.Parameters.AddWithValue("@status", request.Status);
                            command.Parameters.AddWithValue("@remarks", string.IsNullOrWhiteSpace(request.Remarks) ? (object)DBNull.Value : request.Remarks.Trim());
                            command.Parameters.AddWithValue("@createdBy", request.UserId);
                            serviceTxnId = Convert.ToInt64(command.ExecuteScalar());
                        }

                        if (request.Status == "Completed")
                        {
                            if (IsWithdrawalService(serviceType))
                            {
                                AdjustWalletBalance(connection, transaction, request.WalletAccountId, request.Amount + commission);
                                AccountingService.PostWithdrawalServiceEntry(connection, transaction, serviceTxnId, request.TransactionDate, request.WalletAccountId, request.Amount, commission, request.Remarks, request.UserId);
                            }
                            else
                            {
                                AdjustWalletBalance(connection, transaction, request.WalletAccountId, -request.Amount);
                                AccountingService.PostServiceEntry(
                                    connection,
                                    transaction,
                                    serviceTxnId,
                                    request.TransactionDate,
                                    request.WalletAccountId,
                                    request.Amount,
                                    commission,
                                    request.Remarks,
                                    request.UserId);
                            }
                        }
                        else if (!IsWithdrawalService(serviceType) && request.Status == "Pending")
                        {
                            AccountingService.PostPendingServiceEntry(
                                connection,
                                transaction,
                                serviceTxnId,
                                request.TransactionDate,
                                request.Amount,
                                commission,
                                request.Remarks,
                                request.UserId);
                        }

                        if (request.SaveProfile || request.ProfileId.HasValue)
                        {
                            ServiceCustomerProfileRecord profile = new ServiceCustomerProfileRecord();
                            profile.ServiceCustomerProfileId = request.ProfileId.GetValueOrDefault();
                            profile.CustomerName = request.CustomerName;
                            profile.CustomerMobile = request.CustomerMobile;
                            profile.ReferenceNumber = request.ReferenceNumber;
                            profile.BillCategory = billCategory;
                            profile.ServiceTypeId = request.ServiceTypeId;
                            profile.WalletAccountId = request.WalletAccountId;
                            profile.DefaultAmount = request.Amount;
                            profile.DefaultServiceCharge = commission;
                            profile.RecurrenceType = request.RecurrenceType;
                            profile.ExpectedDayOfMonth = request.ExpectedDayOfMonth;
                            profile.NextDueDate = request.NextDueDate;
                            profile.LastServiceDate = request.TransactionDate;
                            profile.Notes = request.Remarks;
                            profile.IsActive = true;

                            int profileId = SaveOrUpdateProfile(connection, transaction, profile);
                            UpdateProfileAfterTransaction(connection, transaction, profileId, request.TransactionDate, request.RecurrenceType, request.ExpectedDayOfMonth, request.NextDueDate);
                        }

                        transaction.Commit();
                        return txnNo;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void UpdateServiceTransaction(ServiceTransactionSaveRequest request)
        {
            if (request == null || !request.ServiceTransactionId.HasValue)
            {
                throw new InvalidOperationException("Select a service sale to edit.");
            }

            ValidateTransaction(request);

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureServiceCustomerProfileTable(connection);
                EnsureServiceRefundColumns(connection);

                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        ServiceTransactionSaveRequest existing = GetTransactionForEdit(connection, transaction, request.ServiceTransactionId.Value);
                        ServiceTypeRecord serviceType = GetServiceTypeById(connection, transaction, request.ServiceTypeId);
                        ValidateTransactionForServiceType(request, serviceType);
                        ServiceTypeRecord existingServiceType = GetServiceTypeById(connection, transaction, existing.ServiceTypeId);
                        if (string.Equals(existing.Status, "Pending", StringComparison.OrdinalIgnoreCase))
                        {
                            AccountingService.DeleteVouchers(connection, transaction, "service_transaction_header", request.ServiceTransactionId.Value);
                        }
                        else
                        {
                            ReverseServiceOperationalEffect(connection, transaction, existing, existingServiceType);
                        }
                        decimal commission = request.ServiceCharge > 0
                            ? request.ServiceCharge
                            : CalculateCommission(serviceType, request.Amount);
                        string billCategory = PrepareBillCategory(serviceType, request.BillCategory);

                        using (MySqlCommand command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandText = @"
                                UPDATE service_transaction_header
                                SET
                                    txn_date = @txnDate,
                                    service_type_id = @serviceTypeId,
                                    customer_name = @customerName,
                                    customer_mobile = @customerMobile,
                                    reference_number = @referenceNumber,
                                    bill_category = @billCategory,
                                    wallet_account_id = @walletAccountId,
                                    payment_method = @paymentMethod,
                                    customer_account_no = @customerAccountNo,
                                    external_transaction_id = @externalTransactionId,
                                    amount = @amount,
                                    service_charge = 0,
                                    commission_earned = @commissionEarned,
                                    net_effect_amount = @netEffectAmount,
                                    status = @status,
                                    remarks = @remarks,
                                    edited_at = NOW(),
                                    edited_by = @editedBy
                                WHERE service_txn_id = @serviceTxnId;";
                            command.Parameters.AddWithValue("@txnDate", request.TransactionDate);
                            command.Parameters.AddWithValue("@serviceTypeId", request.ServiceTypeId);
                            command.Parameters.AddWithValue("@customerName", request.CustomerName.Trim());
                            command.Parameters.AddWithValue("@customerMobile", string.IsNullOrWhiteSpace(request.CustomerMobile) ? (object)DBNull.Value : request.CustomerMobile.Trim());
                            command.Parameters.AddWithValue("@referenceNumber", string.IsNullOrWhiteSpace(request.ReferenceNumber) ? (object)DBNull.Value : request.ReferenceNumber.Trim());
                            command.Parameters.AddWithValue("@billCategory", string.IsNullOrWhiteSpace(billCategory) ? (object)DBNull.Value : billCategory);
                            command.Parameters.AddWithValue("@walletAccountId", request.WalletAccountId);
                            command.Parameters.AddWithValue("@paymentMethod", NormalizePaymentMethod(request.PaymentMethod));
                            command.Parameters.AddWithValue("@customerAccountNo", string.IsNullOrWhiteSpace(request.CustomerAccountNumber) ? (object)DBNull.Value : request.CustomerAccountNumber.Trim());
                            command.Parameters.AddWithValue("@externalTransactionId", string.IsNullOrWhiteSpace(request.ExternalTransactionId) ? (object)DBNull.Value : request.ExternalTransactionId.Trim());
                            command.Parameters.AddWithValue("@amount", request.Amount);
                            command.Parameters.AddWithValue("@commissionEarned", commission);
                            command.Parameters.AddWithValue("@netEffectAmount", commission);
                            command.Parameters.AddWithValue("@status", request.Status);
                            command.Parameters.AddWithValue("@remarks", string.IsNullOrWhiteSpace(request.Remarks) ? (object)DBNull.Value : request.Remarks.Trim());
                            command.Parameters.AddWithValue("@editedBy", request.UserId);
                            command.Parameters.AddWithValue("@serviceTxnId", request.ServiceTransactionId.Value);
                            command.ExecuteNonQuery();
                        }

                        if (request.Status == "Completed")
                        {
                            if (IsWithdrawalService(serviceType))
                            {
                                AdjustWalletBalance(connection, transaction, request.WalletAccountId, request.Amount + commission);
                                AccountingService.PostWithdrawalServiceEntry(connection, transaction, request.ServiceTransactionId.Value, request.TransactionDate, request.WalletAccountId, request.Amount, commission, request.Remarks, request.UserId);
                            }
                            else
                            {
                                AdjustWalletBalance(connection, transaction, request.WalletAccountId, -request.Amount);
                                AccountingService.PostServiceEntry(
                                    connection,
                                    transaction,
                                    request.ServiceTransactionId.Value,
                                    request.TransactionDate,
                                    request.WalletAccountId,
                                    request.Amount,
                                    commission,
                                    request.Remarks,
                                    request.UserId);
                            }
                        }
                        else if (!IsWithdrawalService(serviceType) && request.Status == "Pending")
                        {
                            AccountingService.PostPendingServiceEntry(
                                connection,
                                transaction,
                                request.ServiceTransactionId.Value,
                                request.TransactionDate,
                                request.Amount,
                                commission,
                                request.Remarks,
                                request.UserId);
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void RefundServiceTransaction(long serviceTransactionId, int userId, string remarks)
        {
            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureServiceRefundColumns(connection);

                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        ServiceTransactionSaveRequest existing = GetTransactionForEdit(connection, transaction, serviceTransactionId);
                        ServiceTypeRecord existingServiceType = GetServiceTypeById(connection, transaction, existing.ServiceTypeId);
                        bool isWithdrawalService = IsWithdrawalService(existingServiceType);

                        if (isWithdrawalService && !string.Equals(existing.Status, "Completed", StringComparison.OrdinalIgnoreCase))
                        {
                            throw new InvalidOperationException("Only completed withdrawal services can be refunded.");
                        }

                        if (string.Equals(existing.Status, "Pending", StringComparison.OrdinalIgnoreCase))
                        {
                            AccountingService.DeleteVouchers(connection, transaction, "service_transaction_header", serviceTransactionId);
                        }
                        else
                        {
                            ReverseServiceOperationalEffect(connection, transaction, existing, existingServiceType);
                        }

                        if (isWithdrawalService)
                        {
                            AccountingService.PostWithdrawalServiceRefundEntry(connection, transaction, serviceTransactionId, existing.WalletAccountId, existing.Amount, existing.ServiceCharge, remarks, userId);
                        }
                        else if (!string.Equals(existing.Status, "Pending", StringComparison.OrdinalIgnoreCase))
                        {
                            AccountingService.PostServiceRefundEntry(
                                connection,
                                transaction,
                                serviceTransactionId,
                                existing.WalletAccountId,
                                existing.Amount,
                                existing.ServiceCharge,
                                remarks,
                                userId);
                        }

                        using (MySqlCommand command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandText = @"
                                UPDATE service_transaction_header
                                SET
                                    status = 'Refunded',
                                    is_refunded = 1,
                                    refunded_at = NOW(),
                                    refunded_by = @userId,
                                    refund_remarks = @remarks
                                WHERE service_txn_id = @serviceTxnId;";
                            command.Parameters.AddWithValue("@userId", userId);
                            command.Parameters.AddWithValue("@remarks", string.IsNullOrWhiteSpace(remarks) ? (object)DBNull.Value : remarks.Trim());
                            command.Parameters.AddWithValue("@serviceTxnId", serviceTransactionId);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public static decimal CalculateCommission(ServiceTypeRecord serviceType, decimal amount)
        {
            if (serviceType == null)
            {
                return 0;
            }

            string serviceName = serviceType.ServiceName ?? string.Empty;
            if (serviceName.IndexOf("withdraw", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                decimal blocks = Math.Ceiling(amount / 1000M);
                if (blocks < 1)
                {
                    blocks = 1;
                }

                decimal ratePerThousand = serviceType.CommissionValue > 0 ? serviceType.CommissionValue : (serviceType.DefaultCharge > 0 ? serviceType.DefaultCharge : 20M);
                return blocks * ratePerThousand;
            }

            if (string.Equals(NormalizeCommissionType(serviceType.CommissionType), "Percent", StringComparison.OrdinalIgnoreCase))
            {
                return Math.Round(amount * serviceType.CommissionValue / 100M, 2);
            }

            if (serviceType.CommissionValue > 0)
            {
                return serviceType.CommissionValue;
            }

            return serviceType.DefaultCharge;
        }

        public static DateTime? ComputeNextDueDate(string recurrenceType, int? expectedDayOfMonth, DateTime baseDate, DateTime? explicitDate)
        {
            if (explicitDate.HasValue)
            {
                return explicitDate.Value.Date;
            }

            if (string.Equals(recurrenceType, "Monthly", StringComparison.OrdinalIgnoreCase))
            {
                int targetDay = expectedDayOfMonth.GetValueOrDefault(baseDate.Day);
                if (targetDay < 1)
                {
                    targetDay = 1;
                }

                DateTime candidate = new DateTime(
                    baseDate.Year,
                    baseDate.Month,
                    Math.Min(targetDay, DateTime.DaysInMonth(baseDate.Year, baseDate.Month)));

                if (candidate <= baseDate.Date)
                {
                    DateTime nextMonth = baseDate.Date.AddMonths(1);
                    candidate = new DateTime(
                        nextMonth.Year,
                        nextMonth.Month,
                        Math.Min(targetDay, DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month)));
                }

                return candidate;
            }

            if (string.Equals(recurrenceType, "Weekly", StringComparison.OrdinalIgnoreCase))
            {
                return baseDate.Date.AddDays(7);
            }

            return null;
        }

        private static void ValidateTransaction(ServiceTransactionSaveRequest request)
        {
            if (request.IsExternalTransactionIdNotApplicable)
            {
                request.ExternalTransactionId = null;
            }

            if (request.ServiceTypeId <= 0)
            {
                throw new InvalidOperationException("Select a service type.");
            }

            if (request.WalletAccountId <= 0)
            {
                throw new InvalidOperationException("Select a wallet account.");
            }

            if (!request.IsWalkInCustomer && string.IsNullOrWhiteSpace(request.CustomerName))
            {
                throw new InvalidOperationException("Enter customer name.");
            }

            if (request.IsWalkInCustomer && request.SaveProfile)
            {
                throw new InvalidOperationException("Walk-in customers cannot be saved as recurring profiles.");
            }

            if (request.Amount <= 0)
            {
                throw new InvalidOperationException("Amount must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(request.Status))
            {
                throw new InvalidOperationException("Select transaction status.");
            }

            string paymentMethod = NormalizePaymentMethod(request.PaymentMethod);
            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                throw new InvalidOperationException("Select payment method.");
            }

            if (!string.Equals(paymentMethod, "Cash", StringComparison.OrdinalIgnoreCase) &&
                string.IsNullOrWhiteSpace(request.CustomerAccountNumber))
            {
                throw new InvalidOperationException("Enter customer account or mobile number for non-cash service transactions.");
            }

            if (string.Equals(request.Status, "Completed", StringComparison.OrdinalIgnoreCase) &&
                !request.IsExternalTransactionIdNotApplicable &&
                string.IsNullOrWhiteSpace(request.ExternalTransactionId))
            {
                throw new InvalidOperationException("Enter transaction ID for completed service transactions.");
            }

            request.BillCategory = NormalizeBillCategory(request.BillCategory);
        }

        private static void ValidateTransactionForServiceType(ServiceTransactionSaveRequest request, ServiceTypeRecord serviceType)
        {
            if (request == null || serviceType == null)
            {
                return;
            }

            if (!IsWithdrawalService(serviceType))
            {
                return;
            }

            string paymentMethod = NormalizePaymentMethod(request.PaymentMethod);
            if (string.Equals(paymentMethod, "Cash", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Select how the customer sent funds for withdrawal services. Cash is not valid as the incoming method.");
            }

            if (string.Equals(request.Status, "Pending", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Withdrawal services should be saved as Completed or Cancelled, not Pending.");
            }

            if (string.IsNullOrWhiteSpace(request.CustomerAccountNumber))
            {
                throw new InvalidOperationException("Enter the customer sender account or mobile number for withdrawal services.");
            }

            if (!request.IsExternalTransactionIdNotApplicable &&
                string.IsNullOrWhiteSpace(request.ExternalTransactionId))
            {
                throw new InvalidOperationException("Enter the incoming transaction ID for withdrawal services.");
            }
        }

        private static void ValidateProfile(ServiceCustomerProfileRecord profile)
        {
            if (string.IsNullOrWhiteSpace(profile.CustomerName))
            {
                throw new InvalidOperationException("Enter customer name to save the recurring profile.");
            }

            profile.BillCategory = NormalizeBillCategory(profile.BillCategory);
        }

        private static ServiceTypeRecord GetServiceTypeById(MySqlConnection connection, MySqlTransaction transaction, int serviceTypeId)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT
                        service_type_id,
                        service_name,
                        provider_name,
                        default_charge,
                        commission_type,
                        commission_value,
                        is_active
                    FROM service_types
                    WHERE service_type_id = @serviceTypeId;";
                command.Parameters.AddWithValue("@serviceTypeId", serviceTypeId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        throw new InvalidOperationException("Selected service type was not found.");
                    }

                    ServiceTypeRecord item = new ServiceTypeRecord();
                    item.ServiceTypeId = Convert.ToInt32(reader["service_type_id"]);
                    item.ServiceName = Convert.ToString(reader["service_name"]);
                    item.ProviderName = Convert.ToString(reader["provider_name"]);
                    item.DefaultCharge = Convert.ToDecimal(reader["default_charge"]);
                    item.CommissionType = NormalizeCommissionType(Convert.ToString(reader["commission_type"]));
                    item.CommissionValue = Convert.ToDecimal(reader["commission_value"]);
                    item.IsActive = Convert.ToBoolean(reader["is_active"]);
                    return item;
                }
            }
        }

        private static string GenerateNextTxnNo(MySqlConnection connection, MySqlTransaction transaction)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT txn_no
                    FROM service_transaction_header
                    ORDER BY service_txn_id DESC
                    LIMIT 1;";

                object value = command.ExecuteScalar();
                int nextNumber = 1;

                if (value != null && value != DBNull.Value)
                {
                    string lastTxnNo = Convert.ToString(value);
                    int numericPart;
                    if (!string.IsNullOrWhiteSpace(lastTxnNo) &&
                        lastTxnNo.Length > 4 &&
                        int.TryParse(lastTxnNo.Substring(4), out numericPart))
                    {
                        nextNumber = numericPart + 1;
                    }
                }

                return string.Format("SRV-{0:D5}", nextNumber);
            }
        }

        private static void AdjustWalletBalance(MySqlConnection connection, MySqlTransaction transaction, int walletAccountId, decimal delta)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    UPDATE wallet_accounts
                    SET current_balance = current_balance + @delta
                    WHERE wallet_account_id = @walletAccountId;";
                command.Parameters.AddWithValue("@delta", delta);
                command.Parameters.AddWithValue("@walletAccountId", walletAccountId);
                command.ExecuteNonQuery();
            }
        }

        private static int SaveOrUpdateProfile(MySqlConnection connection, MySqlTransaction transaction, ServiceCustomerProfileRecord profile)
        {
            ValidateProfile(profile);

            bool hasProfileId = profile.ServiceCustomerProfileId > 0;
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                if (hasProfileId)
                {
                    command.CommandText = @"
                        UPDATE service_customer_profiles
                        SET
                            customer_name = @customerName,
                            customer_mobile = @customerMobile,
                            reference_number = @referenceNumber,
                            bill_category = @billCategory,
                            service_type_id = @serviceTypeId,
                            preferred_wallet_account_id = @walletAccountId,
                            default_amount = @defaultAmount,
                            default_service_charge = @defaultServiceCharge,
                            recurrence_type = @recurrenceType,
                            expected_day_of_month = @expectedDayOfMonth,
                            next_due_date = @nextDueDate,
                            last_service_date = @lastServiceDate,
                            notes = @notes,
                            is_active = @isActive
                        WHERE service_customer_profile_id = @profileId;";
                    command.Parameters.AddWithValue("@profileId", profile.ServiceCustomerProfileId);
                }
                else
                {
                    command.CommandText = @"
                        INSERT INTO service_customer_profiles
                        (
                            customer_name, customer_mobile, reference_number, service_type_id,
                            bill_category,
                            preferred_wallet_account_id, default_amount, default_service_charge,
                            recurrence_type, expected_day_of_month, next_due_date,
                            last_service_date, notes, is_active
                        )
                        VALUES
                        (
                            @customerName, @customerMobile, @referenceNumber, @serviceTypeId,
                            @billCategory,
                            @walletAccountId, @defaultAmount, @defaultServiceCharge,
                            @recurrenceType, @expectedDayOfMonth, @nextDueDate,
                            @lastServiceDate, @notes, @isActive
                        );";
                }

                command.Parameters.AddWithValue("@customerName", profile.CustomerName.Trim());
                command.Parameters.AddWithValue("@customerMobile", string.IsNullOrWhiteSpace(profile.CustomerMobile) ? (object)DBNull.Value : profile.CustomerMobile.Trim());
                command.Parameters.AddWithValue("@referenceNumber", string.IsNullOrWhiteSpace(profile.ReferenceNumber) ? (object)DBNull.Value : profile.ReferenceNumber.Trim());
                command.Parameters.AddWithValue("@billCategory", string.IsNullOrWhiteSpace(profile.BillCategory) ? (object)DBNull.Value : profile.BillCategory);
                command.Parameters.AddWithValue("@serviceTypeId", (object)profile.ServiceTypeId ?? DBNull.Value);
                command.Parameters.AddWithValue("@walletAccountId", (object)profile.WalletAccountId ?? DBNull.Value);
                command.Parameters.AddWithValue("@defaultAmount", profile.DefaultAmount);
                command.Parameters.AddWithValue("@defaultServiceCharge", profile.DefaultServiceCharge);
                command.Parameters.AddWithValue("@recurrenceType", string.IsNullOrWhiteSpace(profile.RecurrenceType) ? "OnDemand" : profile.RecurrenceType);
                command.Parameters.AddWithValue("@expectedDayOfMonth", (object)profile.ExpectedDayOfMonth ?? DBNull.Value);
                command.Parameters.AddWithValue("@nextDueDate", (object)profile.NextDueDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@lastServiceDate", (object)profile.LastServiceDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@notes", string.IsNullOrWhiteSpace(profile.Notes) ? (object)DBNull.Value : profile.Notes.Trim());
                command.Parameters.AddWithValue("@isActive", profile.IsActive);
                command.ExecuteNonQuery();

                if (hasProfileId)
                {
                    return profile.ServiceCustomerProfileId;
                }

                return Convert.ToInt32(command.LastInsertedId);
            }
        }

        private static void UpdateProfileAfterTransaction(
            MySqlConnection connection,
            MySqlTransaction transaction,
            int profileId,
            DateTime transactionDate,
            string recurrenceType,
            int? expectedDayOfMonth,
            DateTime? nextDueDate)
        {
            DateTime? computedDueDate = ComputeNextDueDate(recurrenceType, expectedDayOfMonth, transactionDate, nextDueDate);

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    UPDATE service_customer_profiles
                    SET
                        last_service_date = @lastServiceDate,
                        next_due_date = @nextDueDate
                    WHERE service_customer_profile_id = @profileId;";
                command.Parameters.AddWithValue("@lastServiceDate", transactionDate);
                command.Parameters.AddWithValue("@nextDueDate", (object)computedDueDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@profileId", profileId);
                command.ExecuteNonQuery();
            }
        }

        internal static void EnsureServiceRefundColumns(MySqlConnection connection)
        {
            EnsureColumn(connection, "service_transaction_header", "is_refunded", "ALTER TABLE service_transaction_header ADD COLUMN is_refunded TINYINT(1) NOT NULL DEFAULT 0 AFTER status;");
            EnsureColumn(connection, "service_transaction_header", "refunded_at", "ALTER TABLE service_transaction_header ADD COLUMN refunded_at DATETIME NULL AFTER is_refunded;");
            EnsureColumn(connection, "service_transaction_header", "refunded_by", "ALTER TABLE service_transaction_header ADD COLUMN refunded_by INT NULL AFTER refunded_at;");
            EnsureColumn(connection, "service_transaction_header", "refund_remarks", "ALTER TABLE service_transaction_header ADD COLUMN refund_remarks VARCHAR(255) NULL AFTER refunded_by;");
            EnsureColumn(connection, "service_transaction_header", "edited_at", "ALTER TABLE service_transaction_header ADD COLUMN edited_at DATETIME NULL AFTER refund_remarks;");
            EnsureColumn(connection, "service_transaction_header", "edited_by", "ALTER TABLE service_transaction_header ADD COLUMN edited_by INT NULL AFTER edited_at;");
            EnsureColumn(connection, "service_transaction_header", "payment_method", "ALTER TABLE service_transaction_header ADD COLUMN payment_method VARCHAR(30) NULL AFTER wallet_account_id;");
            EnsureColumn(connection, "service_transaction_header", "customer_account_no", "ALTER TABLE service_transaction_header ADD COLUMN customer_account_no VARCHAR(100) NULL AFTER payment_method;");
            EnsureColumn(connection, "service_transaction_header", "external_transaction_id", "ALTER TABLE service_transaction_header ADD COLUMN external_transaction_id VARCHAR(100) NULL AFTER customer_account_no;");
            EnsureColumn(connection, "service_transaction_header", "bill_category", "ALTER TABLE service_transaction_header ADD COLUMN bill_category VARCHAR(40) NULL AFTER reference_number;");
        }

        private static void EnsureColumn(MySqlConnection connection, string tableName, string columnName, string alterSql)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT COUNT(*)
                    FROM information_schema.COLUMNS
                    WHERE TABLE_SCHEMA = DATABASE()
                      AND TABLE_NAME = @tableName
                      AND COLUMN_NAME = @columnName;";
                command.Parameters.AddWithValue("@tableName", tableName);
                command.Parameters.AddWithValue("@columnName", columnName);
                bool exists = Convert.ToInt32(command.ExecuteScalar()) > 0;
                if (exists)
                {
                    return;
                }
            }

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = alterSql;
                command.ExecuteNonQuery();
            }
        }

        private static ServiceTransactionSaveRequest GetTransactionForEdit(MySqlConnection connection, MySqlTransaction transaction, long serviceTransactionId)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT
                        service_txn_id,
                        service_type_id,
                        txn_date,
                        customer_name,
                        customer_mobile,
                        reference_number,
                        bill_category,
                        wallet_account_id,
                        payment_method,
                        customer_account_no,
                        external_transaction_id,
                        amount,
                        commission_earned,
                        status,
                        remarks,
                        IFNULL(is_refunded, 0) AS is_refunded
                    FROM service_transaction_header
                    WHERE service_txn_id = @serviceTransactionId;";
                command.Parameters.AddWithValue("@serviceTransactionId", serviceTransactionId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        throw new InvalidOperationException("Selected service sale was not found.");
                    }

                    if (Convert.ToBoolean(reader["is_refunded"]) || Convert.ToString(reader["status"]) == "Refunded")
                    {
                        throw new InvalidOperationException("Refunded service sales cannot be edited.");
                    }

                    ServiceTransactionSaveRequest request = new ServiceTransactionSaveRequest();
                    request.ServiceTransactionId = Convert.ToInt64(reader["service_txn_id"]);
                    request.ServiceTypeId = Convert.ToInt32(reader["service_type_id"]);
                    request.TransactionDate = Convert.ToDateTime(reader["txn_date"]);
                    request.CustomerName = Convert.ToString(reader["customer_name"]);
                    request.CustomerMobile = Convert.ToString(reader["customer_mobile"]);
                    request.ReferenceNumber = Convert.ToString(reader["reference_number"]);
                    request.BillCategory = Convert.ToString(reader["bill_category"]);
                    request.WalletAccountId = Convert.ToInt32(reader["wallet_account_id"]);
                    request.PaymentMethod = Convert.ToString(reader["payment_method"]);
                    request.CustomerAccountNumber = Convert.ToString(reader["customer_account_no"]);
                    request.ExternalTransactionId = Convert.ToString(reader["external_transaction_id"]);
                    request.IsExternalTransactionIdNotApplicable = string.IsNullOrWhiteSpace(request.ExternalTransactionId);
                    request.Amount = Convert.ToDecimal(reader["amount"]);
                    request.ServiceCharge = Convert.ToDecimal(reader["commission_earned"]);
                    request.Status = Convert.ToString(reader["status"]);
                    request.Remarks = Convert.ToString(reader["remarks"]);
                    return request;
                }
            }
        }

        private static void ReverseServiceOperationalEffect(MySqlConnection connection, MySqlTransaction transaction, ServiceTransactionSaveRequest existing, ServiceTypeRecord serviceType)
        {
            if (existing.Status != "Completed")
            {
                return;
            }

            decimal delta = IsWithdrawalService(serviceType)
                ? -(existing.Amount + existing.ServiceCharge)
                : existing.Amount;
            AdjustWalletBalance(connection, transaction, existing.WalletAccountId, delta);

            AccountingService.DeleteVouchers(connection, transaction, "service_transaction_header", existing.ServiceTransactionId.Value);
        }

        public static string NormalizeCommissionType(string commissionType)
        {
            if (string.IsNullOrWhiteSpace(commissionType))
            {
                return "Fixed";
            }

            string value = commissionType.Trim();
            if (string.Equals(value, "Flat", StringComparison.OrdinalIgnoreCase))
            {
                return "Fixed";
            }

            return value;
        }

        public static string NormalizePaymentMethod(string paymentMethod)
        {
            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                return "Cash";
            }

            string value = paymentMethod.Trim();
            if (string.Equals(value, "Bank Account", StringComparison.OrdinalIgnoreCase))
            {
                return "Bank";
            }

            return value;
        }

        public static string NormalizeBillCategory(string billCategory)
        {
            if (string.IsNullOrWhiteSpace(billCategory))
            {
                return null;
            }

            string value = billCategory.Trim();
            if (string.Equals(value, "Not Applicable", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(value, "None", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(value, "All", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (string.Equals(value, "Wapda", StringComparison.OrdinalIgnoreCase))
            {
                return "WAPDA";
            }

            if (string.Equals(value, "Internet", StringComparison.OrdinalIgnoreCase))
            {
                return "Internet";
            }

            if (string.Equals(value, "Agriculture", StringComparison.OrdinalIgnoreCase))
            {
                return "Agriculture";
            }

            if (string.Equals(value, "Other", StringComparison.OrdinalIgnoreCase))
            {
                return "Other";
            }

            return value;
        }

        public static bool IsBillService(ServiceTypeRecord serviceType)
        {
            if (serviceType == null)
            {
                return false;
            }

            string lookup = string.Format("{0} {1}", serviceType.ServiceName ?? string.Empty, serviceType.ProviderName ?? string.Empty);
            return lookup.IndexOf("bill", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   lookup.IndexOf("wapda", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   lookup.IndexOf("internet", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   lookup.IndexOf("agriculture", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   lookup.IndexOf("utility", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static string SuggestBillCategory(ServiceTypeRecord serviceType)
        {
            if (serviceType == null)
            {
                return null;
            }

            string lookup = string.Format("{0} {1}", serviceType.ServiceName ?? string.Empty, serviceType.ProviderName ?? string.Empty);
            if (lookup.IndexOf("wapda", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "WAPDA";
            }

            if (lookup.IndexOf("agri", StringComparison.OrdinalIgnoreCase) >= 0 ||
                lookup.IndexOf("agriculture", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "Agriculture";
            }

            if (lookup.IndexOf("internet", StringComparison.OrdinalIgnoreCase) >= 0 ||
                lookup.IndexOf("fiber", StringComparison.OrdinalIgnoreCase) >= 0 ||
                lookup.IndexOf("broadband", StringComparison.OrdinalIgnoreCase) >= 0 ||
                lookup.IndexOf("wifi", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "Internet";
            }

            if (IsBillService(serviceType))
            {
                return "Other";
            }

            return null;
        }

        public static bool IsWithdrawalService(ServiceTypeRecord serviceType)
        {
            if (serviceType == null || string.IsNullOrWhiteSpace(serviceType.ServiceName))
            {
                return false;
            }

            return serviceType.ServiceName.IndexOf("withdraw", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static void EnsureServiceCustomerProfileTable(MySqlConnection connection)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS service_customer_profiles
                    (
                        service_customer_profile_id INT AUTO_INCREMENT PRIMARY KEY,
                        customer_name VARCHAR(150) NOT NULL,
                        customer_mobile VARCHAR(30) NULL,
                        reference_number VARCHAR(100) NULL,
                        bill_category VARCHAR(40) NULL,
                        service_type_id INT NULL,
                        preferred_wallet_account_id INT NULL,
                        default_amount DECIMAL(18,2) NOT NULL DEFAULT 0.00,
                        default_service_charge DECIMAL(18,2) NOT NULL DEFAULT 0.00,
                        recurrence_type VARCHAR(20) NOT NULL DEFAULT 'OnDemand',
                        expected_day_of_month INT NULL,
                        next_due_date DATE NULL,
                        last_service_date DATETIME NULL,
                        notes VARCHAR(255) NULL,
                        is_active TINYINT(1) NOT NULL DEFAULT 1,
                        created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                        CONSTRAINT fk_service_profile_type
                            FOREIGN KEY (service_type_id) REFERENCES service_types(service_type_id)
                            ON DELETE SET NULL ON UPDATE CASCADE,
                        CONSTRAINT fk_service_profile_wallet
                            FOREIGN KEY (preferred_wallet_account_id) REFERENCES wallet_accounts(wallet_account_id)
                            ON DELETE SET NULL ON UPDATE CASCADE
                    ) ENGINE=InnoDB;";
                command.ExecuteNonQuery();
            }

            EnsureColumn(connection, "service_customer_profiles", "bill_category", "ALTER TABLE service_customer_profiles ADD COLUMN bill_category VARCHAR(40) NULL AFTER reference_number;");
        }

        private static string PrepareBillCategory(ServiceTypeRecord serviceType, string billCategory)
        {
            if (!IsBillService(serviceType))
            {
                return null;
            }

            string normalized = NormalizeBillCategory(billCategory);
            if (!string.IsNullOrWhiteSpace(normalized))
            {
                return normalized;
            }

            string suggested = SuggestBillCategory(serviceType);
            if (!string.IsNullOrWhiteSpace(suggested))
            {
                return suggested;
            }

            throw new InvalidOperationException("Select a bill category for bill payment services.");
        }

        private static string NormalizeBillCategoryFilter(string billCategoryFilter)
        {
            string normalized = NormalizeBillCategory(billCategoryFilter);
            return string.IsNullOrWhiteSpace(normalized) ? "All" : normalized;
        }
    }
}
