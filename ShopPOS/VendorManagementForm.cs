using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using ShopPOS.Models;
using ShopPOS.Services;

namespace ShopPOS
{
    public partial class VendorManagementForm : Form
    {
        private readonly UserSession _session;
        private readonly VendorService _vendorService;
        private readonly VendorPaymentService _vendorPaymentService;
        private readonly SalesService _salesService;
        private List<VendorRecord> _allVendors;

        private TextBox txtSearch;
        private DataGridView dgvVendors;
        private DataGridView dgvPurchaseHistory;
        private DataGridView dgvLedger;
        private DataGridView dgvVendorProducts;
        private Label lblSelectedVendor;
        private Label lblNetBalance;
        private Label lblPurchaseDue;
        private Label lblExpiryPending;
        private ComboBox cboWallet;
        private NumericUpDown nudPaymentAmount;
        private DateTimePicker dtpPaymentDate;
        private TextBox txtPaymentRemarks;
        private int? _selectedSupplierId;
        private Button btnPrintStatement;
        private DataGridView dgvExpiredReturns;

        public VendorManagementForm(UserSession session)
        {
            if (session == null) throw new ArgumentNullException("session");

            _session = session;
            _vendorService = new VendorService();
            _vendorPaymentService = new VendorPaymentService();
            _salesService = new SalesService();
            _allVendors = new List<VendorRecord>();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1440, 840);
            MinimumSize = new Size(1458, 887);
            StartPosition = FormStartPosition.CenterParent;
            WindowState = FormWindowState.Maximized;
            Text = "Vendor Management";

            Panel header = new Panel();
            header.BackColor = Color.FromArgb(121, 84, 46);
            header.Dock = DockStyle.Top;
            header.Height = 90;
            Controls.Add(header);

            Label lblTitle = new Label();
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(24, 18);
            lblTitle.Text = "Vendor Management";
            header.Controls.Add(lblTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.WhiteSmoke;
            lblSubtitle.Location = new Point(28, 57);
            lblSubtitle.Text = "Track vendor payables, purchases, ledger history, and linked products.";
            header.Controls.Add(lblSubtitle);

            SplitContainer splitMain = new SplitContainer();
            splitMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitMain.Location = new Point(20, 108);
            splitMain.Size = new Size(1400, 710);
            splitMain.SplitterDistance = 430;
            Controls.Add(splitMain);

            Panel leftPanel = new Panel();
            leftPanel.Dock = DockStyle.Fill;
            leftPanel.BackColor = Color.White;
            leftPanel.Padding = new Padding(14);
            splitMain.Panel1.Controls.Add(leftPanel);

            Label lblSearch = new Label();
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblSearch.Location = new Point(14, 14);
            lblSearch.Text = "Search Vendor";
            leftPanel.Controls.Add(lblSearch);

            txtSearch = new TextBox();
            txtSearch.Font = new Font("Segoe UI", 11F);
            txtSearch.Location = new Point(18, 42);
            txtSearch.Size = new Size(380, 32);
            txtSearch.TextChanged += txtSearch_TextChanged;
            leftPanel.Controls.Add(txtSearch);

            Button btnNewVendor = new Button();
            btnNewVendor.BackColor = Color.White;
            btnNewVendor.FlatStyle = FlatStyle.Flat;
            btnNewVendor.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            btnNewVendor.Location = new Point(18, 82);
            btnNewVendor.Size = new Size(120, 34);
            btnNewVendor.Text = "New Vendor";
            btnNewVendor.Click += btnNewVendor_Click;
            leftPanel.Controls.Add(btnNewVendor);

            Button btnEditVendor = new Button();
            btnEditVendor.BackColor = Color.White;
            btnEditVendor.FlatStyle = FlatStyle.Flat;
            btnEditVendor.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            btnEditVendor.Location = new Point(146, 82);
            btnEditVendor.Size = new Size(120, 34);
            btnEditVendor.Text = "Edit Vendor";
            btnEditVendor.Click += btnEditVendor_Click;
            leftPanel.Controls.Add(btnEditVendor);

            Button btnVendorPayments = new Button();
            btnVendorPayments.BackColor = Color.White;
            btnVendorPayments.FlatStyle = FlatStyle.Flat;
            btnVendorPayments.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            btnVendorPayments.Location = new Point(274, 82);
            btnVendorPayments.Size = new Size(124, 34);
            btnVendorPayments.Text = "All Payments";
            btnVendorPayments.Click += btnVendorPayments_Click;
            leftPanel.Controls.Add(btnVendorPayments);

            btnPrintStatement = new Button();
            btnPrintStatement.BackColor = Color.White;
            btnPrintStatement.FlatStyle = FlatStyle.Flat;
            btnPrintStatement.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            btnPrintStatement.Location = new Point(18, 670);
            btnPrintStatement.Size = new Size(380, 34);
            btnPrintStatement.Text = "Print Selected Vendor Statement";
            btnPrintStatement.Enabled = false;
            btnPrintStatement.Click += btnPrintStatement_Click;
            leftPanel.Controls.Add(btnPrintStatement);

            dgvVendors = CreateGrid(new Point(18, 128), new Size(380, 535), true);
            dgvVendors.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvVendors.SelectionChanged += dgvVendors_SelectionChanged;
            leftPanel.Controls.Add(dgvVendors);
            ConfigureVendorGrid();

            Panel rightPanel = new Panel();
            rightPanel.Dock = DockStyle.Fill;
            rightPanel.BackColor = Color.White;
            rightPanel.Padding = new Padding(14);
            rightPanel.AutoScroll = true;
            splitMain.Panel2.Controls.Add(rightPanel);

            lblSelectedVendor = new Label();
            lblSelectedVendor.AutoSize = true;
            lblSelectedVendor.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
            lblSelectedVendor.Location = new Point(14, 14);
            lblSelectedVendor.Text = "Select a vendor";
            rightPanel.Controls.Add(lblSelectedVendor);

            TableLayoutPanel rightLayout = new TableLayoutPanel();
            rightLayout.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            rightLayout.ColumnCount = 1;
            rightLayout.RowCount = 4;
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 180F));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 220F));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 260F));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 280F));
            rightLayout.Location = new Point(18, 56);
            rightLayout.Size = new Size(920, 960);
            rightPanel.Controls.Add(rightLayout);

            Panel summaryPanel = new Panel();
            summaryPanel.Dock = DockStyle.Fill;
            rightLayout.Controls.Add(summaryPanel, 0, 0);

            GroupBox summary = new GroupBox();
            summary.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            summary.Text = "Payable Summary";
            summary.Location = new Point(0, 0);
            summary.Size = new Size(560, 88);
            summaryPanel.Controls.Add(summary);

            Label lblNet = new Label();
            lblNet.AutoSize = true;
            lblNet.Font = new Font("Segoe UI", 10F);
            lblNet.Location = new Point(16, 30);
            lblNet.Text = "Net Balance";
            summary.Controls.Add(lblNet);

            lblNetBalance = new Label();
            lblNetBalance.AutoSize = true;
            lblNetBalance.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblNetBalance.ForeColor = Color.FromArgb(121, 84, 46);
            lblNetBalance.Location = new Point(16, 50);
            lblNetBalance.Text = "Rs. 0.00";
            summary.Controls.Add(lblNetBalance);

            Label lblDue = new Label();
            lblDue.AutoSize = true;
            lblDue.Font = new Font("Segoe UI", 10F);
            lblDue.Location = new Point(260, 30);
            lblDue.Text = "Purchase Due";
            summary.Controls.Add(lblDue);

            lblPurchaseDue = new Label();
            lblPurchaseDue.AutoSize = true;
            lblPurchaseDue.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblPurchaseDue.ForeColor = Color.Firebrick;
            lblPurchaseDue.Location = new Point(260, 50);
            lblPurchaseDue.Text = "Rs. 0.00";
            summary.Controls.Add(lblPurchaseDue);

            Label lblExpiry = new Label();
            lblExpiry.AutoSize = true;
            lblExpiry.Font = new Font("Segoe UI", 10F);
            lblExpiry.Location = new Point(420, 30);
            lblExpiry.Text = "Expiry Pending";
            summary.Controls.Add(lblExpiry);

            lblExpiryPending = new Label();
            lblExpiryPending.AutoSize = true;
            lblExpiryPending.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblExpiryPending.ForeColor = Color.DarkOrange;
            lblExpiryPending.Location = new Point(420, 50);
            lblExpiryPending.Text = "0";
            summary.Controls.Add(lblExpiryPending);

            GroupBox payment = new GroupBox();
            payment.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            payment.Text = "Make Payment";
            payment.Location = new Point(0, 96);
            payment.Size = new Size(700, 80);
            summaryPanel.Controls.Add(payment);

            AddFieldLabel(payment, "Wallet", 16, 24);
            cboWallet = CreateComboBox(16, 46, 220);
            payment.Controls.Add(cboWallet);

            AddFieldLabel(payment, "Amount", 256, 24);
            nudPaymentAmount = CreateMoneyNumeric(256, 46, 120);
            payment.Controls.Add(nudPaymentAmount);

            AddFieldLabel(payment, "Payment Date", 396, 24);
            dtpPaymentDate = new DateTimePicker();
            dtpPaymentDate.Font = new Font("Segoe UI", 10F);
            dtpPaymentDate.CustomFormat = "dd MMM yyyy hh:mm tt";
            dtpPaymentDate.Format = DateTimePickerFormat.Custom;
            dtpPaymentDate.Location = new Point(396, 46);
            dtpPaymentDate.Size = new Size(220, 30);
            payment.Controls.Add(dtpPaymentDate);

            GroupBox remarksGroup = new GroupBox();
            remarksGroup.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            remarksGroup.Text = "Payment Notes";
            remarksGroup.Dock = DockStyle.Fill;
            rightLayout.Controls.Add(remarksGroup, 0, 1);

            txtPaymentRemarks = new TextBox();
            txtPaymentRemarks.Font = new Font("Segoe UI", 10F);
            txtPaymentRemarks.Location = new Point(14, 30);
            txtPaymentRemarks.Multiline = true;
            txtPaymentRemarks.Size = new Size(720, 70);
            remarksGroup.Controls.Add(txtPaymentRemarks);

            Button btnPayNow = new Button();
            btnPayNow.BackColor = Color.FromArgb(39, 110, 241);
            btnPayNow.FlatAppearance.BorderSize = 0;
            btnPayNow.FlatStyle = FlatStyle.Flat;
            btnPayNow.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnPayNow.ForeColor = Color.White;
            btnPayNow.Location = new Point(14, 120);
            btnPayNow.Size = new Size(150, 36);
            btnPayNow.Text = "Save Payment";
            btnPayNow.Click += btnPayNow_Click;
            remarksGroup.Controls.Add(btnPayNow);

            GroupBox purchaseHistory = new GroupBox();
            purchaseHistory.Dock = DockStyle.Fill;
            purchaseHistory.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            purchaseHistory.Text = "Purchase History";
            rightLayout.Controls.Add(purchaseHistory, 0, 2);

            dgvPurchaseHistory = CreateGrid(new Point(0, 0), new Size(0, 0), true);
            dgvPurchaseHistory.Dock = DockStyle.Fill;
            purchaseHistory.Controls.Add(dgvPurchaseHistory);
            ConfigurePurchaseHistoryGrid();

            GroupBox ledger = new GroupBox();
            ledger.Dock = DockStyle.Fill;
            ledger.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            ledger.Text = "Linked Products";
            rightLayout.Controls.Add(ledger, 0, 3);

            dgvVendorProducts = CreateGrid(new Point(0, 0), new Size(0, 0), true);
            dgvVendorProducts.Dock = DockStyle.Fill;
            ledger.Controls.Add(dgvVendorProducts);
            ConfigureVendorProductsGrid();

            GroupBox ledgerHistory = new GroupBox();
            ledgerHistory.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ledgerHistory.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            ledgerHistory.Text = "Payment + Ledger History";
            ledgerHistory.Location = new Point(18, 1030);
            ledgerHistory.Size = new Size(920, 260);
            rightPanel.Controls.Add(ledgerHistory);

            dgvLedger = CreateGrid(new Point(14, 28), new Size(890, 214), true);
            ledgerHistory.Controls.Add(dgvLedger);
            ConfigureLedgerGrid();

            GroupBox expiredReturns = new GroupBox();
            expiredReturns.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            expiredReturns.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            expiredReturns.Text = "Expired / Returned Products";
            expiredReturns.Location = new Point(18, 1304);
            expiredReturns.Size = new Size(920, 240);
            rightPanel.Controls.Add(expiredReturns);

            dgvExpiredReturns = CreateGrid(new Point(14, 28), new Size(890, 194), true);
            expiredReturns.Controls.Add(dgvExpiredReturns);
            ConfigureExpiredReturnsGrid();

            Load += VendorManagementForm_Load;
            ResumeLayout(false);
        }

        private void VendorManagementForm_Load(object sender, EventArgs e)
        {
            cboWallet.DataSource = _salesService.GetWalletAccounts();
            cboWallet.DisplayMember = "Name";
            cboWallet.ValueMember = "Id";
            LoadVendors();
            ResetSelection();
        }

        private void LoadVendors()
        {
            _allVendors = _vendorService.GetVendors();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            string search = txtSearch == null ? string.Empty : txtSearch.Text.Trim().ToLowerInvariant();
            List<VendorRecord> filtered = new List<VendorRecord>();
            for (int i = 0; i < _allVendors.Count; i++)
            {
                VendorRecord item = _allVendors[i];
                string haystack = string.Format("{0} {1} {2}", item.SupplierName, item.Phone, item.PreferredVisitDay).ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(search) || haystack.Contains(search))
                {
                    filtered.Add(item);
                }
            }

            dgvVendors.DataSource = null;
            dgvVendors.DataSource = filtered;
            dgvVendors.ClearSelection();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void dgvVendors_SelectionChanged(object sender, EventArgs e)
        {
            VendorRecord item = GetSelectedVendor();
            if (item == null)
            {
                return;
            }

            LoadVendorDashboard(item);
        }

        private void btnNewVendor_Click(object sender, EventArgs e)
        {
            using (VendorEntryForm form = new VendorEntryForm())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    LoadVendors();
                    SelectVendorInGrid(form.SavedSupplierId);
                }
            }
        }

        private void btnEditVendor_Click(object sender, EventArgs e)
        {
            if (!_selectedSupplierId.HasValue)
            {
                MessageBox.Show("Select a vendor first.", "Vendor Management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (VendorEntryForm form = new VendorEntryForm(_selectedSupplierId))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    LoadVendors();
                    SelectVendorInGrid(form.SavedSupplierId);
                }
            }
        }

        private void btnVendorPayments_Click(object sender, EventArgs e)
        {
            using (VendorPaymentForm paymentForm = new VendorPaymentForm(_session))
            {
                paymentForm.ShowDialog(this);
            }

            LoadVendors();
            if (_selectedSupplierId.HasValue)
            {
                SelectVendorInGrid(_selectedSupplierId.Value);
            }
        }

        private void btnPayNow_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_selectedSupplierId.HasValue)
                {
                    throw new InvalidOperationException("Select a vendor first.");
                }

                LookupOption wallet = cboWallet.SelectedItem as LookupOption;
                VendorPaymentRequest request = new VendorPaymentRequest();
                request.SupplierId = _selectedSupplierId.Value;
                request.WalletAccountId = wallet == null ? 0 : wallet.Id;
                request.Amount = nudPaymentAmount.Value;
                request.PaymentDate = dtpPaymentDate.Value;
                request.Notes = txtPaymentRemarks.Text;
                request.UserId = _session.UserId;

                _vendorPaymentService.SaveVendorPayment(request);
                nudPaymentAmount.Value = 0;
                txtPaymentRemarks.Clear();
                LoadVendors();
                SelectVendorInGrid(request.SupplierId);
                MessageBox.Show("Vendor payment saved successfully.", "Vendor Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Vendor Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadVendorDashboard(VendorRecord item)
        {
            _selectedSupplierId = item.SupplierId;
            btnPrintStatement.Enabled = true;
            lblSelectedVendor.Text = string.Format("Vendor Overview: {0}", item.SupplierName);
            lblNetBalance.Text = string.Format("Rs. {0:N2} ({1})", Math.Abs(item.NetBalance), item.BalanceStatus);
            lblPurchaseDue.Text = string.Format("Rs. {0:N2}", item.PurchaseDueAmount);
            lblExpiryPending.Text = string.Format("{0} pending | {1} returned | {2} burnt", item.ExpiryPendingCount, item.ExpiryReturnedCount, item.ExpiryBurntCount);
            dgvPurchaseHistory.DataSource = _vendorService.GetVendorPurchaseHistory(item.SupplierId);
            dgvLedger.DataSource = _vendorService.GetVendorLedger(item.SupplierId);
            dgvVendorProducts.DataSource = _vendorService.GetLinkedVendorProducts(item.SupplierId);
            dgvExpiredReturns.DataSource = _vendorService.GetVendorExpiredReturns(item.SupplierId);
        }

        private VendorRecord GetSelectedVendor()
        {
            if (dgvVendors.CurrentRow == null) return null;
            return dgvVendors.CurrentRow.DataBoundItem as VendorRecord;
        }

        private void SelectVendorInGrid(int supplierId)
        {
            for (int i = 0; i < dgvVendors.Rows.Count; i++)
            {
                VendorRecord item = dgvVendors.Rows[i].DataBoundItem as VendorRecord;
                if (item == null || item.SupplierId != supplierId)
                {
                    continue;
                }

                dgvVendors.ClearSelection();
                dgvVendors.Rows[i].Selected = true;
                dgvVendors.CurrentCell = dgvVendors.Rows[i].Cells[0];
                break;
            }
        }

        private void ResetSelection()
        {
            _selectedSupplierId = null;
            lblSelectedVendor.Text = "Select a vendor";
            lblNetBalance.Text = "Rs. 0.00";
            lblPurchaseDue.Text = "Rs. 0.00";
            lblExpiryPending.Text = "0";
            nudPaymentAmount.Value = 0;
            txtPaymentRemarks.Clear();
            dtpPaymentDate.Value = DateTime.Now;
            dgvPurchaseHistory.DataSource = null;
            dgvLedger.DataSource = null;
            dgvVendorProducts.DataSource = null;
            dgvExpiredReturns.DataSource = null;
            dgvVendors.ClearSelection();
            btnPrintStatement.Enabled = false;
            if (cboWallet.Items.Count > 0) cboWallet.SelectedIndex = 0;
        }

        private void ConfigureVendorGrid()
        {
            ApplyGridStyle(dgvVendors);
            dgvVendors.Columns.Add(CreateTextColumn("SupplierName", "Vendor", 140F, null));
            dgvVendors.Columns.Add(CreateTextColumn("Phone", "Phone", 90F, null));
            dgvVendors.Columns.Add(CreateTextColumn("PurchaseDueAmount", "Due", 75F, "N2"));
            dgvVendors.Columns.Add(CreateTextColumn("NetBalance", "Net", 75F, "N2"));
            dgvVendors.Columns.Add(CreateTextColumn("BalanceStatus", "Status", 70F, null));
        }

        private void ConfigurePurchaseHistoryGrid()
        {
            ApplyGridStyle(dgvPurchaseHistory);
            dgvPurchaseHistory.Columns.Add(CreateTextColumn("PurchaseNo", "Purchase No", 85F, null));
            dgvPurchaseHistory.Columns.Add(CreateTextColumn("PurchaseDate", "Date", 100F, "dd MMM yyyy hh:mm tt"));
            dgvPurchaseHistory.Columns.Add(CreateTextColumn("InvoiceNo", "Invoice", 80F, null));
            dgvPurchaseHistory.Columns.Add(CreateTextColumn("GrandTotal", "Grand", 75F, "N2"));
            dgvPurchaseHistory.Columns.Add(CreateTextColumn("PaidAmount", "Paid", 70F, "N2"));
            dgvPurchaseHistory.Columns.Add(CreateTextColumn("DueAmount", "Due", 70F, "N2"));
            dgvPurchaseHistory.Columns.Add(CreateTextColumn("Remarks", "Remarks", 120F, null));
        }

        private void ConfigureVendorProductsGrid()
        {
            ApplyGridStyle(dgvVendorProducts);
            dgvVendorProducts.Columns.Add(CreateTextColumn("ProductCode", "Code", 70F, null));
            dgvVendorProducts.Columns.Add(CreateTextColumn("ProductName", "Product", 180F, null));
            dgvVendorProducts.Columns.Add(CreateTextColumn("SalePrice", "Sale", 65F, "N2"));
            dgvVendorProducts.Columns.Add(CreateTextColumn("LastPurchasePrice", "Last Cost", 80F, "N2"));
            dgvVendorProducts.Columns.Add(CreateTextColumn("IsPreferred", "Preferred", 60F, null));
        }

        private void ConfigureLedgerGrid()
        {
            ApplyGridStyle(dgvLedger);
            dgvLedger.Columns.Add(CreateTextColumn("EntryDate", "Date", 100F, "dd MMM yyyy hh:mm tt"));
            dgvLedger.Columns.Add(CreateTextColumn("EntryType", "Type", 70F, null));
            dgvLedger.Columns.Add(CreateTextColumn("ReferenceNo", "Reference", 90F, null));
            dgvLedger.Columns.Add(CreateTextColumn("Debit", "Debit", 70F, "N2"));
            dgvLedger.Columns.Add(CreateTextColumn("Credit", "Credit", 70F, "N2"));
            dgvLedger.Columns.Add(CreateTextColumn("Remarks", "Remarks", 140F, null));
        }

        private void ConfigureExpiredReturnsGrid()
        {
            ApplyGridStyle(dgvExpiredReturns);
            dgvExpiredReturns.Columns.Add(CreateTextColumn("ProductCode", "Code", 65F, null));
            dgvExpiredReturns.Columns.Add(CreateTextColumn("ProductName", "Product", 140F, null));
            dgvExpiredReturns.Columns.Add(CreateTextColumn("BatchNo", "Batch", 70F, null));
            dgvExpiredReturns.Columns.Add(CreateTextColumn("ExpiryDate", "Expiry", 75F, "dd MMM yyyy"));
            dgvExpiredReturns.Columns.Add(CreateTextColumn("Quantity", "Qty", 55F, "N2"));
            dgvExpiredReturns.Columns.Add(CreateTextColumn("ResolutionStatus", "Status", 85F, null));
            dgvExpiredReturns.Columns.Add(CreateTextColumn("ProcessedAt", "Moved On", 85F, "dd MMM yyyy"));
            dgvExpiredReturns.Columns.Add(CreateTextColumn("Remarks", "Remarks", 120F, null));
        }

        private static DataGridView CreateGrid(Point p, Size s, bool readOnly)
        {
            return new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                GridColor = Color.Gainsboro,
                Location = p,
                MultiSelect = false,
                ReadOnly = readOnly,
                RowHeadersVisible = false,
                RowTemplate = { Height = 30 },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Size = s
            };
        }

        private static void ApplyGridStyle(DataGridView grid)
        {
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = Color.FromArgb(243, 246, 251);
            headerStyle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            headerStyle.SelectionBackColor = Color.FromArgb(243, 246, 251);
            headerStyle.SelectionForeColor = Color.Black;
            grid.ColumnHeadersDefaultCellStyle = headerStyle;
            grid.ColumnHeadersHeight = 36;

            DataGridViewCellStyle rowStyle = new DataGridViewCellStyle();
            rowStyle.BackColor = Color.White;
            rowStyle.Font = new Font("Segoe UI", 9F);
            rowStyle.SelectionBackColor = Color.FromArgb(233, 240, 255);
            rowStyle.SelectionForeColor = Color.Black;
            grid.DefaultCellStyle = rowStyle;
        }

        private static DataGridViewTextBoxColumn CreateTextColumn(string propertyName, string headerText, float fillWeight, string format)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = propertyName;
            column.HeaderText = headerText;
            column.FillWeight = fillWeight;
            if (!string.IsNullOrWhiteSpace(format))
            {
                column.DefaultCellStyle.Format = format;
            }

            return column;
        }

        private static ComboBox CreateComboBox(int left, int top, int width)
        {
            ComboBox comboBox = new ComboBox();
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.Font = new Font("Segoe UI", 10F);
            comboBox.Location = new Point(left, top);
            comboBox.Size = new Size(width, 31);
            return comboBox;
        }

        private static NumericUpDown CreateMoneyNumeric(int left, int top, int width)
        {
            NumericUpDown numeric = new NumericUpDown();
            numeric.DecimalPlaces = 2;
            numeric.Maximum = 100000000;
            numeric.ThousandsSeparator = true;
            numeric.Font = new Font("Segoe UI", 10F);
            numeric.Location = new Point(left, top);
            numeric.Size = new Size(width, 30);
            return numeric;
        }

        private static void AddFieldLabel(Control parent, string text, int left, int top)
        {
            Label label = new Label();
            label.AutoSize = true;
            label.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            label.Location = new Point(left, top);
            label.Text = text;
            parent.Controls.Add(label);
        }

        private void btnPrintStatement_Click(object sender, EventArgs e)
        {
            VendorRecord vendor = GetSelectedVendor();
            if (vendor == null)
            {
                MessageBox.Show("Select a vendor first.", "Vendor Statement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PrintPreviewDialog dialog = new PrintPreviewDialog();
            dialog.Document = CreateVendorStatementDocument(vendor);
            dialog.Width = 1100;
            dialog.Height = 780;
            dialog.ShowDialog(this);
        }

        private PrintDocument CreateVendorStatementDocument(VendorRecord vendor)
        {
            PrintDocument document = new PrintDocument();
            document.DefaultPageSettings.Margins = new Margins(45, 45, 45, 45);
            document.PrintPage += delegate(object sender, PrintPageEventArgs e)
            {
                DrawVendorStatement(e, vendor);
            };
            return document;
        }

        private void DrawVendorStatement(PrintPageEventArgs e, VendorRecord vendor)
        {
            Graphics g = e.Graphics;
            Rectangle pageBounds = e.MarginBounds;
            Color brand = Color.FromArgb(121, 84, 46);
            Color muted = Color.FromArgb(110, 110, 110);
            Color soft = Color.FromArgb(244, 238, 231);
            Font shopFont = new Font("Segoe UI Semibold", 24F, FontStyle.Bold);
            Font titleFont = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            Font sectionFont = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            Font bodyFont = new Font("Segoe UI", 9.5F);
            Font smallFont = new Font("Segoe UI", 8.5F);
            int x = pageBounds.Left;
            int y = pageBounds.Top;
            int width = pageBounds.Width;

            using (SolidBrush brandBrush = new SolidBrush(brand))
            using (SolidBrush mutedBrush = new SolidBrush(muted))
            using (SolidBrush softBrush = new SolidBrush(soft))
            using (Pen borderPen = new Pen(Color.FromArgb(210, 210, 210), 1.0F))
            {
                Rectangle headerRect = new Rectangle(x, y, width, 102);
                g.FillRectangle(brandBrush, headerRect);
                g.DrawString(ShopBranding.ShopName, shopFont, Brushes.White, x + 24, y + 16);
                g.DrawString("Vendor Statement", titleFont, Brushes.White, x + 28, y + 60);
                g.DrawString(DateTime.Now.ToString("dd MMM yyyy hh:mm tt"), bodyFont, Brushes.WhiteSmoke, x + width - 180, y + 66);
                y += 120;

                Rectangle infoRect = new Rectangle(x, y, width, 94);
                g.FillRectangle(softBrush, infoRect);
                g.DrawRectangle(borderPen, infoRect);
                g.DrawString("Vendor: " + vendor.SupplierName, sectionFont, Brushes.Black, x + 18, y + 18);
                g.DrawString("Phone: " + (string.IsNullOrWhiteSpace(vendor.Phone) ? "-" : vendor.Phone), bodyFont, Brushes.Black, x + 18, y + 46);
                g.DrawString("Visit Day: " + (string.IsNullOrWhiteSpace(vendor.PreferredVisitDay) ? "-" : vendor.PreferredVisitDay), bodyFont, Brushes.Black, x + 250, y + 46);
                g.DrawString("Cycle: " + (string.IsNullOrWhiteSpace(vendor.PaymentCycle) ? "-" : vendor.PaymentCycle), bodyFont, Brushes.Black, x + 420, y + 46);
                g.DrawString("Net Balance: Rs. " + Math.Abs(vendor.NetBalance).ToString("N2") + " (" + vendor.BalanceStatus + ")", bodyFont, Brushes.Black, x + 18, y + 68);
                g.DrawString("Purchase Due: Rs. " + vendor.PurchaseDueAmount.ToString("N2"), bodyFont, Brushes.Black, x + 320, y + 68);
                y += 116;

                g.DrawString("Recent Purchase History", sectionFont, brandBrush, x, y);
                y += 26;
                y = DrawTableHeader(g, x, y, width, new[] { "Purchase No", "Date", "Grand", "Paid", "Due" });
                List<VendorPurchaseHistoryItem> purchases = _vendorService.GetVendorPurchaseHistory(vendor.SupplierId);
                for (int i = 0; i < purchases.Count && i < 8; i++)
                {
                    VendorPurchaseHistoryItem item = purchases[i];
                    y = DrawTableRow(g, x, y, width, new[]
                    {
                        item.PurchaseNo,
                        item.PurchaseDate.ToString("dd MMM yyyy"),
                        item.GrandTotal.ToString("N2"),
                        item.PaidAmount.ToString("N2"),
                        item.DueAmount.ToString("N2")
                    }, bodyFont, borderPen);
                }

                y += 24;
                g.DrawString("Ledger Summary", sectionFont, brandBrush, x, y);
                y += 26;
                y = DrawTableHeader(g, x, y, width, new[] { "Date", "Type", "Reference", "Debit", "Credit", "Remarks" });
                List<VendorLedgerItem> ledger = _vendorService.GetVendorLedger(vendor.SupplierId);
                for (int i = 0; i < ledger.Count && i < 10; i++)
                {
                    VendorLedgerItem item = ledger[i];
                    y = DrawTableRow(g, x, y, width, new[]
                    {
                        item.EntryDate.ToString("dd MMM yyyy"),
                        item.EntryType,
                        item.ReferenceNo,
                        item.Debit.ToString("N2"),
                        item.Credit.ToString("N2"),
                        item.Remarks
                    }, bodyFont, borderPen);
                }

                string footer = "Generated from " + ShopBranding.ShopName + " vendor payable system";
                SizeF footerSize = g.MeasureString(footer, smallFont);
                g.DrawString(footer, smallFont, mutedBrush, x + width - footerSize.Width, pageBounds.Bottom - 18);
            }
        }

        private static int DrawTableHeader(Graphics g, int x, int y, int width, string[] columns)
        {
            int height = 28;
            int colWidth = width / columns.Length;
            using (SolidBrush headerBrush = new SolidBrush(Color.FromArgb(243, 246, 251)))
            using (Pen borderPen = new Pen(Color.FromArgb(210, 210, 210), 1.0F))
            using (Font font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold))
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    Rectangle rect = new Rectangle(x + (i * colWidth), y, colWidth, height);
                    g.FillRectangle(headerBrush, rect);
                    g.DrawRectangle(borderPen, rect);
                    g.DrawString(columns[i], font, Brushes.Black, new RectangleF(rect.X + 4, rect.Y + 6, rect.Width - 8, rect.Height - 8));
                }
            }

            return y + height;
        }

        private static int DrawTableRow(Graphics g, int x, int y, int width, string[] values, Font font, Pen borderPen)
        {
            int height = 26;
            int colWidth = width / values.Length;
            for (int i = 0; i < values.Length; i++)
            {
                Rectangle rect = new Rectangle(x + (i * colWidth), y, colWidth, height);
                g.DrawRectangle(borderPen, rect);
                g.DrawString(string.IsNullOrWhiteSpace(values[i]) ? "-" : values[i], font, Brushes.Black, new RectangleF(rect.X + 4, rect.Y + 5, rect.Width - 8, rect.Height - 8));
            }

            return y + height;
        }
    }
}
