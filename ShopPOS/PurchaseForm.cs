using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ShopPOS.Models;
using ShopPOS.Services;

namespace ShopPOS
{
    public partial class PurchaseForm : Form
    {
        private readonly UserSession _session;
        private readonly PurchaseService _purchaseService;
        private readonly BindingList<PurchaseCartItem> _cartItems;
        private List<PurchaseProductLookupItem> _allProducts;
        private List<PurchaseProductLookupItem> _filteredProducts;
        private TextBox txtSearch;
        private DataGridView dgvProducts;
        private DataGridView dgvCart;
        private ComboBox cboVendor;
        private ComboBox cboWallet;
        private DateTimePicker dtpPurchaseDate;
        private TextBox txtInvoiceNo;
        private NumericUpDown nudDiscount;
        private NumericUpDown nudOtherCharges;
        private NumericUpDown nudPaidAmount;
        private TextBox txtRemarks;
        private Label lblSubtotalValue;
        private Label lblGrandTotalValue;
        private Label lblDueValue;
        private Label lblPaymentStatus;
        private Label lblStatus;
        private DateTimePicker dtpCartExpiryEditor;
        private bool _isAutoUpdatingPaidAmount;
        private bool _isPaidAmountManuallyChanged;

        public PurchaseForm(UserSession session)
        {
            if (session == null) throw new ArgumentNullException("session");
            _session = session;
            _purchaseService = new PurchaseService();
            _cartItems = new BindingList<PurchaseCartItem>();
            _allProducts = new List<PurchaseProductLookupItem>();
            _filteredProducts = new List<PurchaseProductLookupItem>();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1360, 820);
            MinimumSize = new Size(1378, 867);
            StartPosition = FormStartPosition.CenterParent;
            WindowState = FormWindowState.Maximized;
            Text = "Purchase Entry";

            Panel header = new Panel();
            header.BackColor = Color.FromArgb(121, 84, 46);
            header.Dock = DockStyle.Top;
            header.Height = 92;
            Controls.Add(header);

            Label lblTitle = new Label();
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(24, 18);
            lblTitle.Text = "Purchase Entry";
            header.Controls.Add(lblTitle);

            Label lblUser = new Label();
            lblUser.AutoSize = true;
            lblUser.Font = new Font("Segoe UI", 10F);
            lblUser.ForeColor = Color.WhiteSmoke;
            lblUser.Location = new Point(28, 57);
            lblUser.Text = string.Format("Operator: {0} ({1})", _session.FullName, _session.RoleName);
            header.Controls.Add(lblUser);

            lblStatus = new Label();
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblStatus.ForeColor = Color.Gainsboro;
            lblStatus.Location = new Point(980, 34);
            lblStatus.Size = new Size(350, 24);
            lblStatus.TextAlign = ContentAlignment.MiddleRight;
            lblStatus.Text = "Ready to create purchase";
            header.Controls.Add(lblStatus);

            SplitContainer splitMain = new SplitContainer();
            splitMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitMain.Location = new Point(20, 110);
            splitMain.Size = new Size(1320, 690);
            splitMain.SplitterDistance = 455;
            Controls.Add(splitMain);

            Panel productPanel = new Panel();
            productPanel.Dock = DockStyle.Fill;
            productPanel.BackColor = Color.White;
            productPanel.Padding = new Padding(14);
            splitMain.Panel1.Controls.Add(productPanel);

            Label lblSearch = new Label();
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblSearch.Location = new Point(14, 16);
            lblSearch.Text = "Search Product";
            productPanel.Controls.Add(lblSearch);

            txtSearch = new TextBox();
            txtSearch.Font = new Font("Segoe UI", 11F);
            txtSearch.Location = new Point(18, 44);
            txtSearch.Size = new Size(408, 32);
            txtSearch.TextChanged += txtSearch_TextChanged;
            productPanel.Controls.Add(txtSearch);

            dgvProducts = new DataGridView();
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvProducts.AutoGenerateColumns = false;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.BackgroundColor = Color.White;
            dgvProducts.BorderStyle = BorderStyle.None;
            dgvProducts.EnableHeadersVisualStyles = false;
            dgvProducts.GridColor = Color.Gainsboro;
            dgvProducts.Location = new Point(18, 90);
            dgvProducts.MultiSelect = false;
            dgvProducts.ReadOnly = true;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.RowTemplate.Height = 30;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.Size = new Size(408, 520);
            dgvProducts.DoubleClick += btnAddSelected_Click;
            productPanel.Controls.Add(dgvProducts);
            ConfigureProductGrid();

            Button btnAddSelected = new Button();
            btnAddSelected.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnAddSelected.BackColor = Color.FromArgb(121, 84, 46);
            btnAddSelected.FlatAppearance.BorderSize = 0;
            btnAddSelected.FlatStyle = FlatStyle.Flat;
            btnAddSelected.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnAddSelected.ForeColor = Color.White;
            btnAddSelected.Location = new Point(18, 625);
            btnAddSelected.Size = new Size(408, 40);
            btnAddSelected.Text = "Add Selected Product";
            btnAddSelected.Click += btnAddSelected_Click;
            productPanel.Controls.Add(btnAddSelected);

            Panel purchasePanel = new Panel();
            purchasePanel.Dock = DockStyle.Fill;
            purchasePanel.BackColor = Color.White;
            purchasePanel.Padding = new Padding(14);
            splitMain.Panel2.Controls.Add(purchasePanel);

            TableLayoutPanel purchaseLayout = new TableLayoutPanel();
            purchaseLayout.Dock = DockStyle.Fill;
            purchaseLayout.ColumnCount = 1;
            purchaseLayout.RowCount = 3;
            purchaseLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F));
            purchaseLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            purchaseLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 180F));
            purchasePanel.Controls.Add(purchaseLayout);

            Panel infoPanel = new Panel();
            infoPanel.Dock = DockStyle.Fill;
            purchaseLayout.Controls.Add(infoPanel, 0, 0);

            AddInfoLabel(infoPanel, "Vendor", 0, 0);
            cboVendor = CreateComboBox();
            cboVendor.Location = new Point(16, 34);
            cboVendor.Size = new Size(250, 31);
            cboVendor.SelectedIndexChanged += cboVendor_SelectedIndexChanged;
            infoPanel.Controls.Add(cboVendor);

            AddInfoLabel(infoPanel, "Wallet", 280, 0);
            cboWallet = CreateComboBox();
            cboWallet.Location = new Point(280, 34);
            cboWallet.Size = new Size(190, 31);
            infoPanel.Controls.Add(cboWallet);

            AddInfoLabel(infoPanel, "Purchase Date", 484, 0);
            dtpPurchaseDate = new DateTimePicker();
            dtpPurchaseDate.Font = new Font("Segoe UI", 10F);
            dtpPurchaseDate.CustomFormat = "dd MMM yyyy hh:mm tt";
            dtpPurchaseDate.Format = DateTimePickerFormat.Custom;
            dtpPurchaseDate.Location = new Point(484, 34);
            dtpPurchaseDate.Size = new Size(220, 30);
            infoPanel.Controls.Add(dtpPurchaseDate);

            AddInfoLabel(infoPanel, "Invoice No", 16, 75);
            txtInvoiceNo = new TextBox();
            txtInvoiceNo.Font = new Font("Segoe UI", 10F);
            txtInvoiceNo.Location = new Point(16, 109);
            txtInvoiceNo.Size = new Size(160, 30);
            infoPanel.Controls.Add(txtInvoiceNo);

            AddInfoLabel(infoPanel, "Discount", 190, 75);
            nudDiscount = CreateMoneyNumeric();
            nudDiscount.Location = new Point(190, 109);
            nudDiscount.Size = new Size(110, 30);
            nudDiscount.ValueChanged += TotalsInputChanged;
            infoPanel.Controls.Add(nudDiscount);

            AddInfoLabel(infoPanel, "Other Charges", 314, 75);
            nudOtherCharges = CreateMoneyNumeric();
            nudOtherCharges.Location = new Point(314, 109);
            nudOtherCharges.Size = new Size(120, 30);
            nudOtherCharges.ValueChanged += TotalsInputChanged;
            infoPanel.Controls.Add(nudOtherCharges);

            AddInfoLabel(infoPanel, "Paid Amount", 448, 75);
            nudPaidAmount = CreateMoneyNumeric();
            nudPaidAmount.Location = new Point(448, 109);
            nudPaidAmount.Size = new Size(120, 30);
            nudPaidAmount.ValueChanged += TotalsInputChanged;
            infoPanel.Controls.Add(nudPaidAmount);

            AddInfoLabel(infoPanel, "Remarks", 582, 75);
            txtRemarks = new TextBox();
            txtRemarks.Font = new Font("Segoe UI", 10F);
            txtRemarks.Location = new Point(582, 109);
            txtRemarks.Size = new Size(122, 30);
            infoPanel.Controls.Add(txtRemarks);

            dgvCart = new DataGridView();
            dgvCart.AllowUserToAddRows = false;
            dgvCart.AllowUserToDeleteRows = false;
            dgvCart.AutoGenerateColumns = false;
            dgvCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCart.BackgroundColor = Color.White;
            dgvCart.BorderStyle = BorderStyle.None;
            dgvCart.Dock = DockStyle.Fill;
            dgvCart.EnableHeadersVisualStyles = false;
            dgvCart.GridColor = Color.Gainsboro;
            dgvCart.RowHeadersVisible = false;
            dgvCart.RowTemplate.Height = 30;
            dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCart.CellEndEdit += dgvCart_CellEndEdit;
            dgvCart.CellContentClick += dgvCart_CellContentClick;
            dgvCart.CellBeginEdit += dgvCart_CellBeginEdit;
            dgvCart.CellFormatting += dgvCart_CellFormatting;
            dgvCart.Scroll += dgvCart_Scroll;
            dgvCart.ColumnWidthChanged += dgvCart_LayoutChanged;
            dgvCart.RowHeightChanged += dgvCart_LayoutChanged;
            dgvCart.DataError += dgvCart_DataError;
            purchaseLayout.Controls.Add(dgvCart, 0, 1);
            ConfigureCartGrid();
            dgvCart.DataSource = _cartItems;

            dtpCartExpiryEditor = new DateTimePicker();
            dtpCartExpiryEditor.Visible = false;
            dtpCartExpiryEditor.Format = DateTimePickerFormat.Custom;
            dtpCartExpiryEditor.CustomFormat = "MM/dd/yyyy";
            dtpCartExpiryEditor.Font = new Font("Segoe UI", 9F);
            dtpCartExpiryEditor.CloseUp += dtpCartExpiryEditor_CloseUp;
            dtpCartExpiryEditor.ValueChanged += dtpCartExpiryEditor_ValueChanged;
            dgvCart.Controls.Add(dtpCartExpiryEditor);

            Panel totalsPanel = new Panel();
            totalsPanel.Dock = DockStyle.Fill;
            totalsPanel.BackColor = Color.FromArgb(248, 250, 253);
            purchaseLayout.Controls.Add(totalsPanel, 0, 2);

            lblSubtotalValue = AddSummaryRow(totalsPanel, "Subtotal", 22, 20);
            lblGrandTotalValue = AddSummaryRow(totalsPanel, "Grand Total", 22, 60);
            lblDueValue = AddSummaryRow(totalsPanel, "Vendor Due", 22, 100);
            lblPaymentStatus = new Label();
            lblPaymentStatus.AutoSize = true;
            lblPaymentStatus.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblPaymentStatus.ForeColor = Color.FromArgb(121, 84, 46);
            lblPaymentStatus.Location = new Point(460, 28);
            lblPaymentStatus.Text = "Status: Full Paid";
            totalsPanel.Controls.Add(lblPaymentStatus);

            Button btnClear = new Button();
            btnClear.BackColor = Color.White;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnClear.Location = new Point(460, 96);
            btnClear.Size = new Size(145, 42);
            btnClear.Text = "Clear Cart";
            btnClear.Click += btnClear_Click;
            totalsPanel.Controls.Add(btnClear);

            Button btnSave = new Button();
            btnSave.BackColor = Color.FromArgb(24, 125, 68);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(620, 96);
            btnSave.Size = new Size(190, 42);
            btnSave.Text = "Save Purchase";
            btnSave.Click += btnSave_Click;
            totalsPanel.Controls.Add(btnSave);

            Load += PurchaseForm_Load;
            ResumeLayout(false);
        }

        private void PurchaseForm_Load(object sender, EventArgs e)
        {
            LoadLookups();
            LoadProducts();
            RecalculateTotals();
        }

        private void LoadLookups()
        {
            List<LookupOption> vendors = _purchaseService.GetVendors();
            vendors.Insert(0, new LookupOption { Id = 0, Name = "Select Vendor" });
            cboVendor.DataSource = vendors;
            cboVendor.DisplayMember = "Name";
            cboVendor.ValueMember = "Id";

            List<LookupOption> wallets = _purchaseService.GetWalletAccounts();
            wallets.Insert(0, new LookupOption { Id = 0, Name = "Credit / No Wallet" });
            cboWallet.DataSource = wallets;
            cboWallet.DisplayMember = "Name";
            cboWallet.ValueMember = "Id";
        }

        private void LoadProducts()
        {
            _allProducts = _purchaseService.GetProducts(GetSelectedId(cboVendor));
            ApplyProductFilter();
        }

        private void ApplyProductFilter()
        {
            string search = txtSearch == null ? string.Empty : txtSearch.Text.Trim().ToLowerInvariant();
            _filteredProducts = new List<PurchaseProductLookupItem>();

            for (int i = 0; i < _allProducts.Count; i++)
            {
                PurchaseProductLookupItem item = _allProducts[i];
                string haystack = string.Format("{0} {1} {2} {3}", item.ProductCode, item.Barcode, item.ProductName, item.PreferredVendorName).ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(search) || haystack.Contains(search))
                {
                    _filteredProducts.Add(item);
                }
            }

            dgvProducts.DataSource = null;
            dgvProducts.DataSource = _filteredProducts;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) { ApplyProductFilter(); }
        private void cboVendor_SelectedIndexChanged(object sender, EventArgs e) { LoadProducts(); }

        private void btnAddSelected_Click(object sender, EventArgs e)
        {
            PurchaseProductLookupItem selected = GetSelectedProduct();
            if (selected == null) return;

            for (int i = 0; i < _cartItems.Count; i++)
            {
                if (_cartItems[i].ProductId == selected.ProductId && !selected.TrackExpiry)
                {
                    _cartItems[i].Quantity += 1;
                    RefreshCartGrid();
                    RecalculateTotals();
                    return;
                }
            }

            PurchaseCartItem item = new PurchaseCartItem();
            item.ProductId = selected.ProductId;
            item.UnitId = selected.UnitId;
            item.ProductCode = selected.ProductCode;
            item.ProductName = selected.ProductName;
            item.UnitName = selected.UnitName;
            item.Quantity = 1;
            item.Rate = selected.PurchasePrice > 0 ? selected.PurchasePrice : selected.SalePrice;
            item.SalePrice = selected.SalePrice;
            item.TrackExpiry = selected.TrackExpiry;
            item.BatchNo = string.Empty;
            item.ExpiryDate = selected.TrackExpiry ? selected.DefaultExpiryDate : null;
            _cartItems.Add(item);
            RefreshCartGrid();
            RecalculateTotals();

            if (selected.TrackExpiry)
            {
                FocusExpiryCellForRow(_cartItems.Count - 1);
            }
        }

        private PurchaseProductLookupItem GetSelectedProduct()
        {
            if (dgvProducts.CurrentRow == null) return null;
            return dgvProducts.CurrentRow.DataBoundItem as PurchaseProductLookupItem;
        }

        private void dgvCart_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _cartItems.Count) return;
            HideExpiryEditor();
            PurchaseCartItem item = _cartItems[e.RowIndex];
            if (item.Quantity <= 0) item.Quantity = 1;
            if (item.Rate <= 0) item.Rate = item.SalePrice > 0 ? item.SalePrice : 1;
            RefreshCartGrid();
            RecalculateTotals();
        }

        private void dgvCart_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
            MessageBox.Show("Select a valid expiry date in MM/dd/yyyy format.", "Purchase Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void dgvCart_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvCart.Columns[e.ColumnIndex].Name != "colRemove") return;
            HideExpiryEditor();
            _cartItems.RemoveAt(e.RowIndex);
            RecalculateTotals();
        }

        private void dgvCart_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _cartItems.Count)
            {
                HideExpiryEditor();
                return;
            }

            PurchaseCartItem item = _cartItems[e.RowIndex];
            string columnName = dgvCart.Columns[e.ColumnIndex].DataPropertyName;

            if (columnName == "ExpiryDate")
            {
                if (!item.TrackExpiry)
                {
                    e.Cancel = true;
                    return;
                }

                ShowExpiryEditor(e.RowIndex, e.ColumnIndex);
                e.Cancel = true;
                return;
            }

            HideExpiryEditor();
        }

        private void dgvCart_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _cartItems.Count)
            {
                return;
            }

            PurchaseCartItem item = _cartItems[e.RowIndex];
            DataGridViewRow row = dgvCart.Rows[e.RowIndex];

            if (item.TrackExpiry)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 249, 230);
                row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 236, 179);
                row.DefaultCellStyle.SelectionForeColor = Color.Black;
            }
            else
            {
                row.DefaultCellStyle.BackColor = Color.White;
                row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(233, 240, 255);
                row.DefaultCellStyle.SelectionForeColor = Color.Black;
            }
        }

        private void dgvCart_Scroll(object sender, ScrollEventArgs e)
        {
            HideExpiryEditor();
        }

        private void dgvCart_LayoutChanged(object sender, EventArgs e)
        {
            HideExpiryEditor();
        }

        private void dtpCartExpiryEditor_ValueChanged(object sender, EventArgs e)
        {
            if (dgvCart.CurrentCell == null || dgvCart.CurrentCell.RowIndex < 0 || dgvCart.CurrentCell.RowIndex >= _cartItems.Count)
            {
                return;
            }

            _cartItems[dgvCart.CurrentCell.RowIndex].ExpiryDate = dtpCartExpiryEditor.Value.Date;
            RefreshCartGrid();
        }

        private void dtpCartExpiryEditor_CloseUp(object sender, EventArgs e)
        {
            if (dgvCart.CurrentCell != null)
            {
                _cartItems[dgvCart.CurrentCell.RowIndex].ExpiryDate = dtpCartExpiryEditor.Value.Date;
            }

            HideExpiryEditor();
            RecalculateTotals();
            txtSearch.Focus();
            txtSearch.SelectAll();
        }

        private void TotalsInputChanged(object sender, EventArgs e)
        {
            if (sender == nudPaidAmount && !_isAutoUpdatingPaidAmount)
            {
                _isPaidAmountManuallyChanged = true;
            }

            RecalculateTotals();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _cartItems.Clear();
            txtInvoiceNo.Clear();
            nudDiscount.Value = 0;
            nudOtherCharges.Value = 0;
            _isPaidAmountManuallyChanged = false;
            SetPaidAmountProgrammatically(0);
            txtRemarks.Clear();
            if (cboWallet.Items.Count > 0) cboWallet.SelectedIndex = 0;
            RecalculateTotals();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                PurchaseSaveRequest request = new PurchaseSaveRequest();
                request.SupplierId = GetSelectedId(cboVendor).GetValueOrDefault();
                request.WalletAccountId = GetSelectedId(cboWallet);
                request.PurchaseDate = dtpPurchaseDate.Value;
                request.InvoiceNo = txtInvoiceNo.Text;
                request.Discount = nudDiscount.Value;
                request.OtherCharges = nudOtherCharges.Value;
                request.PaidAmount = nudPaidAmount.Value;
                request.Remarks = txtRemarks.Text;
                request.UserId = _session.UserId;

                for (int i = 0; i < _cartItems.Count; i++)
                {
                    request.Items.Add(_cartItems[i]);
                }

                PurchaseSaveResult result = _purchaseService.SavePurchase(request);
                MessageBox.Show(string.Format("Purchase saved successfully.\nPurchase No: {0}", result.PurchaseNo), "Purchase Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblStatus.Text = string.Format("Last saved purchase: {0}", result.PurchaseNo);
                btnClear_Click(null, EventArgs.Empty);
                LoadProducts();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Purchase save failed";
                MessageBox.Show(ex.Message, "Save Purchase Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RecalculateTotals()
        {
            decimal subtotal = 0;
            for (int i = 0; i < _cartItems.Count; i++)
            {
                subtotal += _cartItems[i].LineTotal;
            }

            decimal grandTotal = subtotal - nudDiscount.Value + nudOtherCharges.Value;
            if (grandTotal < 0) grandTotal = 0;

            if (!_isPaidAmountManuallyChanged && nudPaidAmount.Value != grandTotal)
            {
                SetPaidAmountProgrammatically(grandTotal);
            }

            decimal due = grandTotal - nudPaidAmount.Value;
            if (due < 0) due = 0;

            lblSubtotalValue.Text = FormatCurrency(subtotal);
            lblGrandTotalValue.Text = FormatCurrency(grandTotal);
            lblDueValue.Text = FormatCurrency(due);

            if (grandTotal <= 0)
            {
                lblPaymentStatus.Text = "Status: No items added";
            }
            else if (due <= 0)
            {
                lblPaymentStatus.Text = "Status: Full Paid";
            }
            else if (nudPaidAmount.Value <= 0)
            {
                lblPaymentStatus.Text = "Status: Full Credit Purchase";
            }
            else
            {
                lblPaymentStatus.Text = string.Format("Status: Partial Payment, Paid {0}, Due {1}", FormatCurrency(nudPaidAmount.Value), FormatCurrency(due));
            }
        }

        private void SetPaidAmountProgrammatically(decimal value)
        {
            _isAutoUpdatingPaidAmount = true;
            nudPaidAmount.Value = value;
            _isAutoUpdatingPaidAmount = false;
        }

        private void RefreshCartGrid() { dgvCart.Refresh(); }

        private void FocusExpiryCellForRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvCart.Rows.Count)
            {
                return;
            }

            DataGridViewColumn expiryColumn = dgvCart.Columns["ExpiryDate"];
            if (expiryColumn == null)
            {
                return;
            }

            dgvCart.ClearSelection();
            dgvCart.CurrentCell = dgvCart.Rows[rowIndex].Cells[expiryColumn.Index];
            dgvCart.Rows[rowIndex].Selected = true;
            ShowExpiryEditor(rowIndex, expiryColumn.Index);
        }

        private void ShowExpiryEditor(int rowIndex, int columnIndex)
        {
            Rectangle cellRectangle = dgvCart.GetCellDisplayRectangle(columnIndex, rowIndex, true);
            if (cellRectangle.Width <= 0 || cellRectangle.Height <= 0)
            {
                return;
            }

            PurchaseCartItem item = _cartItems[rowIndex];
            dtpCartExpiryEditor.Value = item.ExpiryDate.HasValue ? item.ExpiryDate.Value.Date : DateTime.Today;
            dtpCartExpiryEditor.Bounds = new Rectangle(
                cellRectangle.X,
                cellRectangle.Y,
                cellRectangle.Width,
                cellRectangle.Height);
            dtpCartExpiryEditor.Visible = true;
            dtpCartExpiryEditor.BringToFront();
            dtpCartExpiryEditor.Focus();
        }

        private void HideExpiryEditor()
        {
            if (dtpCartExpiryEditor != null)
            {
                dtpCartExpiryEditor.Visible = false;
            }
        }

        private void ConfigureProductGrid()
        {
            ApplyGridStyle(dgvProducts);
            dgvProducts.Columns.Add(CreateTextColumn("ProductCode", "Code", 65F, null, true));
            dgvProducts.Columns.Add(CreateTextColumn("ProductName", "Product", 135F, null, true));
            dgvProducts.Columns.Add(CreateTextColumn("PreferredVendorName", "Vendor", 110F, null, true));
            dgvProducts.Columns.Add(CreateTextColumn("PurchasePrice", "Cost", 65F, "N2", true));
            dgvProducts.Columns.Add(CreateTextColumn("SalePrice", "Sale", 60F, "N2", true));
        }

        private void ConfigureCartGrid()
        {
            ApplyGridStyle(dgvCart);
            dgvCart.Columns.Add(CreateTextColumn("ProductCode", "Code", 60F, null, true));
            dgvCart.Columns.Add(CreateTextColumn("ProductName", "Product", 135F, null, true));
            dgvCart.Columns.Add(CreateTextColumn("UnitName", "Unit", 50F, null, true));
            dgvCart.Columns.Add(CreateTextColumn("Quantity", "Qty", 55F, "N2", false));
            dgvCart.Columns.Add(CreateTextColumn("Rate", "Cost", 65F, "N2", false));
            dgvCart.Columns.Add(CreateTextColumn("BatchNo", "Batch", 75F, null, false));
            dgvCart.Columns.Add(CreateTextColumn("ExpiryDate", "Expiry", 80F, "MM/dd/yyyy", false));
            dgvCart.Columns.Add(CreateTextColumn("LineTotal", "Total", 75F, "N2", true));

            DataGridViewButtonColumn removeColumn = new DataGridViewButtonColumn();
            removeColumn.Name = "colRemove";
            removeColumn.HeaderText = "";
            removeColumn.Text = "Remove";
            removeColumn.UseColumnTextForButtonValue = true;
            removeColumn.FillWeight = 75F;
            dgvCart.Columns.Add(removeColumn);
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

        private static DataGridViewTextBoxColumn CreateTextColumn(string propertyName, string headerText, float fillWeight, string format, bool readOnly)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.Name = propertyName;
            column.DataPropertyName = propertyName;
            column.HeaderText = headerText;
            column.FillWeight = fillWeight;
            column.ReadOnly = readOnly;
            if (!string.IsNullOrWhiteSpace(format)) column.DefaultCellStyle.Format = format;
            return column;
        }

        private static ComboBox CreateComboBox()
        {
            ComboBox comboBox = new ComboBox();
            comboBox.Font = new Font("Segoe UI", 10F);
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            return comboBox;
        }

        private static NumericUpDown CreateMoneyNumeric()
        {
            NumericUpDown numeric = new NumericUpDown();
            numeric.DecimalPlaces = 2;
            numeric.Maximum = 100000000;
            numeric.Font = new Font("Segoe UI", 10F);
            numeric.ThousandsSeparator = true;
            return numeric;
        }

        private static void AddInfoLabel(Control parent, string text, int left, int top)
        {
            Label label = new Label();
            label.AutoSize = true;
            label.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            label.Location = new Point(left, top + 4);
            label.Text = text;
            parent.Controls.Add(label);
        }

        private static Label AddSummaryRow(Control parent, string title, int left, int top)
        {
            Label caption = new Label();
            caption.AutoSize = true;
            caption.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            caption.Location = new Point(left, top + 4);
            caption.Text = title;
            parent.Controls.Add(caption);

            Label value = new Label();
            value.AutoSize = true;
            value.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold);
            value.ForeColor = Color.FromArgb(121, 84, 46);
            value.Location = new Point(left + 165, top);
            value.Text = "Rs. 0.00";
            parent.Controls.Add(value);
            return value;
        }

        private static int? GetSelectedId(ComboBox comboBox)
        {
            LookupOption option = comboBox.SelectedItem as LookupOption;
            if (option == null || option.Id <= 0) return null;
            return option.Id;
        }

        private static string FormatCurrency(decimal amount)
        {
            return string.Format("Rs. {0:N2}", amount);
        }
    }
}
