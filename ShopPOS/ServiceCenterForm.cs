using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ShopPOS.Models;
using ShopPOS.Services;

namespace ShopPOS
{
    public partial class ServiceCenterForm : Form
    {
        private readonly UserSession _session;
        private readonly ServiceCenterService _serviceService;
        private readonly SalesService _salesService;
        private readonly long? _editingTransactionId;
        private List<ServiceCustomerProfileRecord> _profiles;
        private int? _profileId;
        private bool _isCommissionAutoUpdating;
        private bool _manualCommissionOverride;

        public ServiceCenterForm(UserSession session)
            : this(session, null)
        {
        }

        public ServiceCenterForm()
            : this(new UserSession
            {
                UserId = 0,
                FullName = "Designer",
                Username = "designer",
                RoleName = "Designer"
            }, null)
        {
        }

        public ServiceCenterForm(UserSession session, long? editingTransactionId)
        {
            if (session == null && !IsInDesignMode())
            {
                throw new ArgumentNullException("session");
            }

            _session = session;
            _serviceService = new ServiceCenterService();
            _salesService = new SalesService();
            _editingTransactionId = editingTransactionId;
            _profiles = new List<ServiceCustomerProfileRecord>();
            InitializeComponent();
            EnsureProfilesGridConfigured();
        }

        private void ServiceCenterForm_Load(object sender, EventArgs e)
        {
            if (IsInDesignMode())
            {
                return;
            }

            ReloadServiceTypes(null);
            cboWallet.DataSource = _salesService.GetWalletAccounts();
            cboWallet.ValueMember = "Id";
            cboWallet.DisplayMember = "Name";
            LoadData();
            ResetForm();
            if (_editingTransactionId.HasValue)
            {
                LoadExistingTransaction(_editingTransactionId.Value);
            }
        }

        private void LoadData()
        {
            _profiles = _serviceService.GetCustomerProfiles();
            ApplyProfileFilter();
        }

        private void ApplyProfileFilter()
        {
            List<ServiceCustomerProfileRecord> filtered = new List<ServiceCustomerProfileRecord>();
            string search = txtSearch.Text.Trim().ToLowerInvariant();
            for (int i = 0; i < _profiles.Count; i++)
            {
                ServiceCustomerProfileRecord item = _profiles[i];
                string text = string.Format("{0} {1} {2} {3} {4}", item.CustomerName, item.CustomerMobile, item.ReferenceNumber, item.ServiceTypeName, item.BillCategory).ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(search) || text.Contains(search)) filtered.Add(item);
            }
            dgvProfiles.DataSource = null;
            dgvProfiles.DataSource = filtered;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) { ApplyProfileFilter(); }

        private void dgvProfiles_SelectionChanged(object sender, EventArgs e)
        {
            ServiceCustomerProfileRecord p = dgvProfiles.CurrentRow == null ? null : dgvProfiles.CurrentRow.DataBoundItem as ServiceCustomerProfileRecord;
            if (p == null) return;
            _profileId = p.ServiceCustomerProfileId;
            lblMode.Text = string.Format("Service Entry for {0}", p.CustomerName);
            lblProfileSummary.Text = string.Format(
                "Loaded profile: {0} | Service: {1}{2}",
                p.CustomerName,
                string.IsNullOrWhiteSpace(p.ServiceTypeName) ? "-" : p.ServiceTypeName,
                string.IsNullOrWhiteSpace(p.BillCategory) ? string.Empty : " | Bill: " + p.BillCategory);
            txtCustomer.Text = p.CustomerName; txtMobile.Text = p.CustomerMobile; txtReference.Text = p.ReferenceNumber;
            nudAmount.Value = p.DefaultAmount; nudCommission.Value = p.DefaultServiceCharge; txtRemarks.Text = p.Notes;
            SelectServiceType(p.ServiceTypeId); SelectWallet(p.WalletAccountId);
            SelectBillCategory(p.BillCategory);
            cboRecurrence.SelectedItem = string.IsNullOrWhiteSpace(p.RecurrenceType) ? "OnDemand" : p.RecurrenceType;
            nudDueDay.Value = p.ExpectedDayOfMonth.HasValue && p.ExpectedDayOfMonth.Value >= 1 ? p.ExpectedDayOfMonth.Value : 1;
            chkNextDue.Checked = p.NextDueDate.HasValue; dtpNextDue.Value = p.NextDueDate ?? DateTime.Today;
            chkWalkInCustomer.Checked = false;
            chkSaveProfile.Checked = true;
            _manualCommissionOverride = true;
            UpdateCommission();
        }

        private void btnNew_Click(object sender, EventArgs e) { ResetForm(); }
        private void btnClearProfile_Click(object sender, EventArgs e) { ResetForm(); }

        private void btnSaveProfile_Click(object sender, EventArgs e)
        {
            try
            {
                _serviceService.SaveCustomerProfile(BuildProfile());
                LoadData();
                MessageBox.Show("Service customer profile saved successfully.", "Service Center", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Profile Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnSaveService_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceTypeRecord type = cboServiceType.SelectedItem as ServiceTypeRecord;
                LookupOption wallet = cboWallet.SelectedItem as LookupOption;
                if (type == null || wallet == null) throw new InvalidOperationException("Select service type and wallet.");
                ServiceTransactionSaveRequest request = new ServiceTransactionSaveRequest();
                request.ServiceTypeId = type.ServiceTypeId;
                request.TransactionDate = dtpTxnDate.Value;
                request.IsWalkInCustomer = chkWalkInCustomer.Checked;
                request.CustomerName = chkWalkInCustomer.Checked ? "Walk-in Customer" : txtCustomer.Text;
                request.CustomerMobile = txtMobile.Text;
                request.ReferenceNumber = txtReference.Text;
                request.BillCategory = GetSelectedBillCategory();
                request.WalletAccountId = wallet.Id;
                request.PaymentMethod = Convert.ToString(cboPaymentMethod.SelectedItem);
                request.CustomerAccountNumber = txtCustomerAccountNo.Text;
                request.ExternalTransactionId = txtTransactionId.Text;
                request.IsExternalTransactionIdNotApplicable = chkTransactionIdNotApplicable.Checked;
                request.Amount = nudAmount.Value;
                request.ServiceCharge = nudCommission.Value;
                request.Status = Convert.ToString(cboStatus.SelectedItem);
                request.Remarks = txtRemarks.Text;
                request.UserId = _session.UserId;
                request.ProfileId = _profileId;
                request.SaveProfile = chkSaveProfile.Checked && !chkWalkInCustomer.Checked;
                request.RecurrenceType = Convert.ToString(cboRecurrence.SelectedItem);
                request.ExpectedDayOfMonth = request.RecurrenceType == "Monthly" ? (int?)Convert.ToInt32(nudDueDay.Value) : null;
                request.NextDueDate = chkNextDue.Checked ? (DateTime?)dtpNextDue.Value.Date : null;
                string txnNo;
                if (_editingTransactionId.HasValue)
                {
                    request.ServiceTransactionId = _editingTransactionId.Value;
                    _serviceService.UpdateServiceTransaction(request);
                    txnNo = "Updated";
                }
                else
                {
                    txnNo = _serviceService.SaveServiceTransaction(request);
                }

                LoadData();
                if (_editingTransactionId.HasValue)
                {
                    MessageBox.Show("Service updated successfully.", "Service Center", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }

                ResetForm();
                MessageBox.Show(string.Format("Service saved successfully. Transaction No: {0}", txnNo), "Service Center", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Service Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnOpenTransactions_Click(object sender, EventArgs e)
        {
            using (ServiceTransactionsForm form = new ServiceTransactionsForm(_session))
            {
                form.ShowDialog(this);
            }

            LoadData();
        }

        private void btnNewServiceType_Click(object sender, EventArgs e)
        {
            using (ServiceTypeEntryForm form = new ServiceTypeEntryForm())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    ReloadServiceTypes(form.SavedServiceTypeId);
                }
            }
        }

        private void btnEditServiceType_Click(object sender, EventArgs e)
        {
            ServiceTypeRecord selected = cboServiceType.SelectedItem as ServiceTypeRecord;
            if (selected == null)
            {
                MessageBox.Show("Select a service first.", "Service Center", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (ServiceTypeEntryForm form = new ServiceTypeEntryForm(selected.ServiceTypeId))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    ReloadServiceTypes(form.SavedServiceTypeId);
                }
            }
        }

        private void cboServiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceTypeRecord type = cboServiceType.SelectedItem as ServiceTypeRecord;
            _manualCommissionOverride = false;
            ApplySuggestedCommission(true);
            SuggestPaymentMethod(type);
            UpdateBillCategoryState();
            UpdateServiceModeLabels();
            UpdateServiceSummary();
            UpdateCommission();
        }

        private void AnyAmountChanged(object sender, EventArgs e)
        {
            ApplySuggestedCommission(ServiceCenterService.IsWithdrawalService(cboServiceType.SelectedItem as ServiceTypeRecord));
            UpdateCommission();
        }

        private void nudCommission_ValueChanged(object sender, EventArgs e)
        {
            if (!_isCommissionAutoUpdating)
            {
                _manualCommissionOverride = true;
            }

            lblCommission.Text = string.Format("Rs. {0:N2}", nudCommission.Value);
        }

        private void cboRecurrence_SelectedIndexChanged(object sender, EventArgs e) { ApplyWalkInMode(); }
        private void chkNextDue_CheckedChanged(object sender, EventArgs e) { ApplyWalkInMode(); }
        private void cboPaymentMethod_SelectedIndexChanged(object sender, EventArgs e) { UpdateServiceModeLabels(); }
        private void cboBillCategory_SelectedIndexChanged(object sender, EventArgs e) { UpdateServiceSummary(); }
        private void chkTransactionIdNotApplicable_CheckedChanged(object sender, EventArgs e) { UpdateTransactionIdState(true); }

        private void UpdateCommission()
        {
            lblCommission.Text = string.Format("Rs. {0:N2}", nudCommission.Value);
        }

        private void ApplySuggestedCommission(bool force)
        {
            ServiceTypeRecord type = cboServiceType.SelectedItem as ServiceTypeRecord;
            if (type == null)
            {
                return;
            }

            if (!force && _manualCommissionOverride && !ServiceCenterService.IsWithdrawalService(type) &&
                !string.Equals(ServiceCenterService.NormalizeCommissionType(type.CommissionType), "Percent", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            decimal suggested = ServiceCenterService.CalculateCommission(type, nudAmount.Value);
            _isCommissionAutoUpdating = true;
            try
            {
                nudCommission.Value = suggested;
            }
            finally
            {
                _isCommissionAutoUpdating = false;
            }
        }

        private void UpdateServiceSummary()
        {
            ServiceTypeRecord type = cboServiceType.SelectedItem as ServiceTypeRecord;
            if (type == null)
            {
                lblServiceSummary.Text = "Service details will appear here after you select a service.";
                return;
            }

            string provider = string.IsNullOrWhiteSpace(type.ProviderName) ? "General service" : type.ProviderName;
            string commissionType = ServiceCenterService.NormalizeCommissionType(type.CommissionType);
            string rule;
            if (ServiceCenterService.IsWithdrawalService(type))
            {
                decimal ratePerThousand = type.CommissionValue > 0 ? type.CommissionValue : (type.DefaultCharge > 0 ? type.DefaultCharge : 20M);
                rule = string.Format("Rs. {0:N2} per Rs. 1000 withdrawal", ratePerThousand);
            }
            else if (string.Equals(commissionType, "Percent", StringComparison.OrdinalIgnoreCase))
            {
                rule = string.Format("{0:N2}% commission", type.CommissionValue);
            }
            else
            {
                rule = string.Format("Fixed commission: Rs. {0:N2}", type.CommissionValue > 0 ? type.CommissionValue : type.DefaultCharge);
            }

            string billNote = ServiceCenterService.IsBillService(type)
                ? string.Format(" | Bill type: {0}", string.IsNullOrWhiteSpace(GetSelectedBillCategory()) ? "Select category" : GetSelectedBillCategory())
                : string.Empty;
            string flowNote = ServiceCenterService.IsWithdrawalService(type)
                ? " | Flow: customer sends digitally, selected store wallet receives funds"
                : string.Empty;
            lblServiceSummary.Text = string.Format("{0} | Provider: {1}\r\nCommission rule: {2}{3}{4}", type.ServiceName, provider, rule, billNote, flowNote);
        }

        private void ReloadServiceTypes(int? selectedServiceTypeId)
        {
            List<ServiceTypeRecord> items = _serviceService.GetServiceTypes();
            cboServiceType.DataSource = null;
            cboServiceType.DataSource = items;
            cboServiceType.DisplayMember = "ServiceName";

            if (selectedServiceTypeId.HasValue)
            {
                SelectServiceType(selectedServiceTypeId);
            }
            else if (items.Count > 0)
            {
                cboServiceType.SelectedIndex = 0;
            }

            UpdateServiceSummary();
        }

        private void SuggestPaymentMethod(ServiceTypeRecord type)
        {
            if (cboPaymentMethod == null || cboPaymentMethod.Items.Count == 0 || type == null)
            {
                return;
            }

            string serviceName = type.ServiceName ?? string.Empty;
            string provider = type.ProviderName ?? string.Empty;
            string targetMethod = "Cash";

            if (serviceName.IndexOf("jazzcash", StringComparison.OrdinalIgnoreCase) >= 0 ||
                provider.IndexOf("jazzcash", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                targetMethod = "JazzCash";
            }
            else if (serviceName.IndexOf("easypaisa", StringComparison.OrdinalIgnoreCase) >= 0 ||
                     provider.IndexOf("easypaisa", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                targetMethod = "EasyPaisa";
            }
            else if (serviceName.IndexOf("bank", StringComparison.OrdinalIgnoreCase) >= 0 ||
                     provider.IndexOf("bank", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                targetMethod = "Bank";
            }

            cboPaymentMethod.SelectedItem = targetMethod;
        }

        private void UpdatePaymentAccountState()
        {
            ServiceTypeRecord type = cboServiceType.SelectedItem as ServiceTypeRecord;
            bool isWithdrawal = ServiceCenterService.IsWithdrawalService(type);
            string paymentMethod = Convert.ToString(cboPaymentMethod == null ? null : cboPaymentMethod.SelectedItem);
            bool needsAccount = isWithdrawal || !string.Equals(paymentMethod, "Cash", StringComparison.OrdinalIgnoreCase);

            if (lblPaymentAccount != null)
            {
                if (isWithdrawal)
                {
                    if (string.Equals(paymentMethod, "Bank", StringComparison.OrdinalIgnoreCase))
                    {
                        lblPaymentAccount.Text = "Sender Bank Account No";
                    }
                    else if (string.Equals(paymentMethod, "JazzCash", StringComparison.OrdinalIgnoreCase) ||
                             string.Equals(paymentMethod, "EasyPaisa", StringComparison.OrdinalIgnoreCase))
                    {
                        lblPaymentAccount.Text = "Sender Wallet / Mobile No";
                    }
                    else
                    {
                        lblPaymentAccount.Text = "Sender Account / Mobile No";
                    }
                }
                else if (string.Equals(paymentMethod, "Bank", StringComparison.OrdinalIgnoreCase))
                {
                    lblPaymentAccount.Text = "Customer Bank Account No";
                }
                else if (string.Equals(paymentMethod, "JazzCash", StringComparison.OrdinalIgnoreCase) ||
                         string.Equals(paymentMethod, "EasyPaisa", StringComparison.OrdinalIgnoreCase))
                {
                    lblPaymentAccount.Text = "Customer Wallet / Mobile No";
                }
                else
                {
                    lblPaymentAccount.Text = "Account / Mobile No";
                }
            }

            if (txtCustomerAccountNo != null)
            {
                txtCustomerAccountNo.Enabled = needsAccount;
                if (!needsAccount)
                {
                    txtCustomerAccountNo.Clear();
                }
            }
        }

        private void UpdateTransactionIdState(bool clearOnDisable)
        {
            if (txtTransactionId == null || chkTransactionIdNotApplicable == null)
            {
                return;
            }

            bool isNotApplicable = chkTransactionIdNotApplicable.Checked;
            txtTransactionId.Enabled = !isNotApplicable;

            if (isNotApplicable && clearOnDisable)
            {
                txtTransactionId.Clear();
            }
        }

        private ServiceCustomerProfileRecord BuildProfile()
        {
            ServiceTypeRecord type = cboServiceType.SelectedItem as ServiceTypeRecord;
            LookupOption wallet = cboWallet.SelectedItem as LookupOption;
            ServiceCustomerProfileRecord p = new ServiceCustomerProfileRecord();
            p.ServiceCustomerProfileId = _profileId.GetValueOrDefault();
            p.CustomerName = chkWalkInCustomer.Checked ? "Walk-in Customer" : txtCustomer.Text;
            p.CustomerMobile = txtMobile.Text;
            p.ReferenceNumber = txtReference.Text;
            p.BillCategory = GetSelectedBillCategory();
            p.ServiceTypeId = type == null ? (int?)null : type.ServiceTypeId;
            p.WalletAccountId = wallet == null ? (int?)null : wallet.Id;
            p.DefaultAmount = nudAmount.Value;
            p.DefaultServiceCharge = nudCommission.Value;
            p.RecurrenceType = Convert.ToString(cboRecurrence.SelectedItem);
            p.ExpectedDayOfMonth = p.RecurrenceType == "Monthly" ? (int?)Convert.ToInt32(nudDueDay.Value) : null;
            p.NextDueDate = chkNextDue.Checked ? (DateTime?)dtpNextDue.Value.Date : null;
            p.Notes = txtRemarks.Text;
            p.IsActive = true;
            return p;
        }

        private void ResetForm()
        {
            _profileId = null;
            lblMode.Text = _editingTransactionId.HasValue ? "Edit Service Entry" : "New Service Entry";
            lblProfileSummary.Text = "No repeat-customer profile selected";
            txtCustomer.Clear(); txtMobile.Clear(); txtReference.Clear(); txtRemarks.Clear();
            txtCustomerAccountNo.Clear();
            txtTransactionId.Clear();
            nudAmount.Value = 0; nudCommission.Value = 0;
            dtpTxnDate.Value = DateTime.Now;
            cboStatus.SelectedItem = "Completed";
            cboRecurrence.SelectedItem = "OnDemand";
            nudDueDay.Value = 1;
            chkNextDue.Checked = false;
            dtpNextDue.Value = DateTime.Today;
            dtpNextDue.Enabled = false;
            chkWalkInCustomer.Checked = true;
            chkSaveProfile.Checked = true;
            chkTransactionIdNotApplicable.Checked = true;
            _manualCommissionOverride = false;
            SelectBillCategory(null);
            if (cboPaymentMethod.Items.Count > 0) cboPaymentMethod.SelectedItem = "Cash";
            if (cboServiceType.Items.Count > 0) cboServiceType.SelectedIndex = 0;
            if (cboWallet.Items.Count > 0) cboWallet.SelectedIndex = 0;
            ApplyWalkInMode();
            UpdateServiceModeLabels();
            UpdateBillCategoryState();
            ApplySuggestedCommission(true);
            UpdateCommission();
            UpdateServiceSummary();
        }

        private void LoadExistingTransaction(long serviceTransactionId)
        {
            ServiceTransactionSaveRequest request = _serviceService.GetTransactionForEdit(serviceTransactionId);
            txtCustomer.Text = request.CustomerName;
            txtMobile.Text = request.CustomerMobile;
            txtReference.Text = request.ReferenceNumber;
            txtCustomerAccountNo.Text = request.CustomerAccountNumber;
            txtTransactionId.Text = request.ExternalTransactionId;
            chkTransactionIdNotApplicable.Checked = request.IsExternalTransactionIdNotApplicable;
            dtpTxnDate.Value = request.TransactionDate;
            nudAmount.Value = request.Amount;
            _isCommissionAutoUpdating = true;
            try
            {
                nudCommission.Value = request.ServiceCharge;
            }
            finally
            {
                _isCommissionAutoUpdating = false;
            }
            txtRemarks.Text = request.Remarks;
            cboStatus.SelectedItem = request.Status;
            SelectServiceType(request.ServiceTypeId);
            SelectBillCategory(request.BillCategory);
            SelectWallet(request.WalletAccountId);
            cboPaymentMethod.SelectedItem = ServiceCenterService.NormalizePaymentMethod(request.PaymentMethod);
            chkSaveProfile.Checked = false;
            chkWalkInCustomer.Checked = string.Equals(request.CustomerName, "Walk-in Customer", StringComparison.OrdinalIgnoreCase);
            _manualCommissionOverride = true;
            ApplyWalkInMode();
            UpdateServiceModeLabels();
            UpdateBillCategoryState();
            UpdateTransactionIdState(false);
            lblProfileSummary.Text = chkWalkInCustomer.Checked ? "Editing a walk-in service entry" : "Editing a named customer service entry";
            lblMode.Text = string.Format("Edit Service: {0}", request.CustomerName);
            UpdateServiceSummary();
        }

        private void chkWalkInCustomer_CheckedChanged(object sender, EventArgs e)
        {
            ApplyWalkInMode();
        }

        private void ApplyWalkInMode()
        {
            bool isWalkIn = chkWalkInCustomer != null && chkWalkInCustomer.Checked;
            txtCustomer.Enabled = !isWalkIn;
            chkSaveProfile.Enabled = !isWalkIn;
            cboRecurrence.Enabled = !isWalkIn;
            nudDueDay.Enabled = !isWalkIn && Convert.ToString(cboRecurrence.SelectedItem) == "Monthly";
            chkNextDue.Enabled = !isWalkIn;
            dtpNextDue.Enabled = !isWalkIn && chkNextDue.Checked;

            if (isWalkIn)
            {
                _profileId = null;
                txtCustomer.Text = "Walk-in Customer";
                chkSaveProfile.Checked = false;
                cboRecurrence.SelectedItem = "OnDemand";
                chkNextDue.Checked = false;
                lblProfileSummary.Text = "Walk-in mode: quick entry without saving customer profile";
                lblEntryModeHint.Text = "Walk-in mode is active. Use it for balance, package, withdrawal, or bill-payment customers who may not return.";
            }
            else if (string.Equals(txtCustomer.Text, "Walk-in Customer", StringComparison.OrdinalIgnoreCase))
            {
                txtCustomer.Clear();
                lblProfileSummary.Text = _profileId.HasValue ? lblProfileSummary.Text : "Named customer mode: you can save this customer for future service history";
                lblEntryModeHint.Text = "Named customer mode is active. Save profile only for customers whose monthly bills or repeat services you want to track.";
            }
        }

        private void SelectServiceType(int? id)
        {
            for (int i = 0; i < cboServiceType.Items.Count; i++)
            {
                ServiceTypeRecord item = cboServiceType.Items[i] as ServiceTypeRecord;
                if (item != null && id.HasValue && item.ServiceTypeId == id.Value) { cboServiceType.SelectedIndex = i; return; }
            }
        }

        private void SelectWallet(int? id)
        {
            for (int i = 0; i < cboWallet.Items.Count; i++)
            {
                LookupOption item = cboWallet.Items[i] as LookupOption;
                if (item != null && id.HasValue && item.Id == id.Value) { cboWallet.SelectedIndex = i; return; }
            }
        }

        private void ConfigureProfilesGrid()
        {
            StyleGrid(dgvProfiles);
            dgvProfiles.Columns.Clear();
            dgvProfiles.AutoGenerateColumns = false;
            dgvProfiles.Columns.Add(MakeColumn("CustomerName", "Customer", 115F, null));
            dgvProfiles.Columns.Add(MakeColumn("ServiceTypeName", "Service", 100F, null));
            dgvProfiles.Columns.Add(MakeColumn("BillCategory", "Bill Type", 80F, null));
            dgvProfiles.Columns.Add(MakeColumn("ReferenceNumber", "Reference", 95F, null));
            dgvProfiles.Columns.Add(MakeColumn("NextDueDate", "Next Due", 78F, "dd MMM yyyy"));
            dgvProfiles.Columns.Add(MakeColumn("DueStatus", "Status", 72F, null));
        }

        private void EnsureProfilesGridConfigured()
        {
            if (dgvProfiles == null || dgvProfiles.Columns.Count > 0)
            {
                return;
            }

            ConfigureProfilesGrid();
        }

        private static DataGridView CreateGrid(Point p, Size s)
        {
            return new DataGridView { AllowUserToAddRows = false, AllowUserToDeleteRows = false, AutoGenerateColumns = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, EnableHeadersVisualStyles = false, GridColor = Color.Gainsboro, Location = p, MultiSelect = false, ReadOnly = true, RowHeadersVisible = false, RowTemplate = { Height = 30 }, SelectionMode = DataGridViewSelectionMode.FullRowSelect, Size = s };
        }

        private static Panel CreateSectionPanel(int x, int y, int width, int height)
        {
            return new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(x, y),
                Size = new Size(width, height)
            };
        }

        private bool IsInDesignMode()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime ||
                   (Site != null && Site.DesignMode);
        }

        private void UpdateBillCategoryState()
        {
            if (cboBillCategory == null)
            {
                return;
            }

            ServiceTypeRecord type = cboServiceType.SelectedItem as ServiceTypeRecord;
            bool isBillService = ServiceCenterService.IsBillService(type);
            cboBillCategory.Enabled = isBillService;

            if (!isBillService)
            {
                SelectBillCategory(null);
                return;
            }

            if (string.IsNullOrWhiteSpace(GetSelectedBillCategory()))
            {
                SelectBillCategory(ServiceCenterService.SuggestBillCategory(type));
            }
        }

        private void UpdateServiceModeLabels()
        {
            ServiceTypeRecord type = cboServiceType.SelectedItem as ServiceTypeRecord;
            bool isWithdrawal = ServiceCenterService.IsWithdrawalService(type);

            if (lblWallet != null)
            {
                lblWallet.Text = isWithdrawal ? "Store Wallet Receiving Funds" : "Store Wallet / Settlement Source";
            }

            if (lblPaymentMethod != null)
            {
                lblPaymentMethod.Text = isWithdrawal ? "Customer Sent Via" : "Payment Method";
            }

            if (lblTransactionIdTitle != null)
            {
                lblTransactionIdTitle.Text = isWithdrawal ? "Incoming Transaction ID" : "Transaction ID";
            }

            if (chkTransactionIdNotApplicable != null)
            {
                chkTransactionIdNotApplicable.Text = "Not Applicable";
            }

            if (lblCommissionTitle != null)
            {
                lblCommissionTitle.Text = isWithdrawal ? "Withdrawal Charge" : "Commission";
            }

            if (lblExpectedCommissionTitle != null)
            {
                lblExpectedCommissionTitle.Text = isWithdrawal ? "Expected Withdrawal Charge" : "Expected Commission";
            }

            UpdatePaymentAccountState();
            UpdateTransactionIdState(true);
        }

        private string GetSelectedBillCategory()
        {
            return ServiceCenterService.NormalizeBillCategory(Convert.ToString(cboBillCategory == null ? null : cboBillCategory.SelectedItem));
        }

        private void SelectBillCategory(string billCategory)
        {
            if (cboBillCategory == null || cboBillCategory.Items.Count == 0)
            {
                return;
            }

            string normalized = ServiceCenterService.NormalizeBillCategory(billCategory);
            if (string.IsNullOrWhiteSpace(normalized))
            {
                cboBillCategory.SelectedItem = "Not Applicable";
                return;
            }

            for (int i = 0; i < cboBillCategory.Items.Count; i++)
            {
                if (string.Equals(Convert.ToString(cboBillCategory.Items[i]), normalized, StringComparison.OrdinalIgnoreCase))
                {
                    cboBillCategory.SelectedIndex = i;
                    return;
                }
            }

            cboBillCategory.SelectedItem = normalized;
        }

        private static void StyleGrid(DataGridView g)
        {
            g.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(243, 246, 251), Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), SelectionBackColor = Color.FromArgb(243, 246, 251), SelectionForeColor = Color.Black };
            g.ColumnHeadersHeight = 36;
            g.DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.White, Font = new Font("Segoe UI", 9F), SelectionBackColor = Color.FromArgb(233, 240, 255), SelectionForeColor = Color.Black };
        }

        private static DataGridViewTextBoxColumn MakeColumn(string prop, string text, float weight, string format)
        {
            DataGridViewTextBoxColumn c = new DataGridViewTextBoxColumn { DataPropertyName = prop, HeaderText = text, FillWeight = weight };
            if (!string.IsNullOrWhiteSpace(format)) c.DefaultCellStyle.Format = format;
            return c;
        }

        private static Label MakeLabel(string text, int x, int y) { return new Label { AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold), Location = new Point(x, y), Text = text }; }
        private static TextBox MakeText(int x, int y, int w) { return new TextBox { Font = new Font("Segoe UI", 10F), Location = new Point(x, y), Size = new Size(w, 30) }; }
        private static ComboBox MakeCombo(int x, int y, int w) { return new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10F), Location = new Point(x, y), Size = new Size(w, 31) }; }
        private static NumericUpDown MakeMoney(int x, int y, int w) { return new NumericUpDown { DecimalPlaces = 2, Maximum = 100000000, ThousandsSeparator = true, Font = new Font("Segoe UI", 10F), Location = new Point(x, y), Size = new Size(w, 30) }; }
        private static Button MakeButton(string text, Color back, Color fore, int x, int y, int w) { return new Button { BackColor = back, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), ForeColor = fore, Location = new Point(x, y), Size = new Size(w, 36), Text = text }; }
    }
}
