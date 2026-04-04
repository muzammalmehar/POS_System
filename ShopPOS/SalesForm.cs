using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ShopPOS.Models;
using ShopPOS.Services;

namespace ShopPOS
{
    public partial class SalesForm : Form
    {
        private readonly UserSession _session;
        private readonly SalesService _salesService;
        private readonly BindingList<SaleCartItem> _cartItems;
        private readonly long? _editingSaleId;

        private List<SaleProductLookupItem> _allProducts;
        private List<SaleProductLookupItem> _filteredProducts;

        private TextBox txtSearch;
        private DataGridView dgvProducts;
        private DataGridView dgvCart;
        private ComboBox cboCustomer;
        private Button btnNewCustomer;
        private Button btnOpenStock;
        private ComboBox cboPaymentMethod;
        private ComboBox cboWallet;
        private NumericUpDown nudDiscount;
        private NumericUpDown nudExtraCharges;
        private NumericUpDown nudPaidAmount;
        private TextBox txtRemarks;
        private Label lblSubtotalValue;
        private Label lblGrandTotalValue;
        private Label lblChangeValue;
        private Label lblDueValue;
        private Label lblPaymentStatus;
        private Label lblSaleStatus;
        private Button btnAddSelected;
        private Button btnSaveSale;
        private Button btnClearCart;
        private bool _isAutoUpdatingPaidAmount;
        private bool _isPaidAmountManuallyChanged;
        private bool _isForcedCreditSelection;

        public SalesForm(UserSession session)
            : this(session, null)
        {
        }

        public SalesForm(UserSession session, long? editingSaleId)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            _session = session;
            _salesService = new SalesService();
            _editingSaleId = editingSaleId;
            _cartItems = new BindingList<SaleCartItem>();
            _allProducts = new List<SaleProductLookupItem>();
            _filteredProducts = new List<SaleProductLookupItem>();

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
            Text = "Sales Screen";

            Panel header = new Panel();
            header.BackColor = Color.FromArgb(17, 62, 104);
            header.Dock = DockStyle.Top;
            header.Height = 92;
            Controls.Add(header);

            Label lblTitle = new Label();
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(24, 18);
            lblTitle.Text = _editingSaleId.HasValue ? "Edit Sale" : "New Sale";
            header.Controls.Add(lblTitle);

            Label lblCashier = new Label();
            lblCashier.AutoSize = true;
            lblCashier.Font = new Font("Segoe UI", 10F);
            lblCashier.ForeColor = Color.WhiteSmoke;
            lblCashier.Location = new Point(28, 57);
            lblCashier.Text = string.Format("Cashier: {0} ({1})", _session.FullName, _session.RoleName);
            header.Controls.Add(lblCashier);

            lblSaleStatus = new Label();
            lblSaleStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSaleStatus.ForeColor = Color.Gainsboro;
            lblSaleStatus.Location = new Point(980, 34);
            lblSaleStatus.Size = new Size(350, 24);
            lblSaleStatus.TextAlign = ContentAlignment.MiddleRight;
            lblSaleStatus.Text = "Ready to create sale";
            header.Controls.Add(lblSaleStatus);

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

            btnOpenStock = new Button();
            btnOpenStock.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOpenStock.BackColor = Color.White;
            btnOpenStock.FlatStyle = FlatStyle.Flat;
            btnOpenStock.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnOpenStock.Location = new Point(306, 12);
            btnOpenStock.Size = new Size(120, 31);
            btnOpenStock.Text = "Open Stock";
            btnOpenStock.Click += btnOpenStock_Click;
            productPanel.Controls.Add(btnOpenStock);

            txtSearch = new TextBox();
            txtSearch.Font = new Font("Segoe UI", 11F);
            txtSearch.Location = new Point(18, 44);
            txtSearch.Size = new Size(408, 32);
            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.KeyDown += txtSearch_KeyDown;
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
            dgvProducts.KeyDown += dgvProducts_KeyDown;
            productPanel.Controls.Add(dgvProducts);
            ConfigureProductGrid();

            btnAddSelected = new Button();
            btnAddSelected.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnAddSelected.BackColor = Color.FromArgb(47, 128, 237);
            btnAddSelected.FlatAppearance.BorderSize = 0;
            btnAddSelected.FlatStyle = FlatStyle.Flat;
            btnAddSelected.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnAddSelected.ForeColor = Color.White;
            btnAddSelected.Location = new Point(18, 625);
            btnAddSelected.Size = new Size(408, 40);
            btnAddSelected.Text = "Add Selected Product";
            btnAddSelected.Click += btnAddSelected_Click;
            productPanel.Controls.Add(btnAddSelected);

            Panel salePanel = new Panel();
            salePanel.Dock = DockStyle.Fill;
            salePanel.BackColor = Color.White;
            salePanel.Padding = new Padding(14);
            splitMain.Panel2.Controls.Add(salePanel);

            TableLayoutPanel saleLayout = new TableLayoutPanel();
            saleLayout.Dock = DockStyle.Fill;
            saleLayout.ColumnCount = 1;
            saleLayout.RowCount = 3;
            saleLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F));
            saleLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            saleLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 190F));
            salePanel.Controls.Add(saleLayout);

            Panel infoPanel = new Panel();
            infoPanel.Dock = DockStyle.Fill;
            saleLayout.Controls.Add(infoPanel, 0, 0);

            AddInfoLabel(infoPanel, "Customer", 0, 0);
            cboCustomer = CreateComboBox();
            cboCustomer.Location = new Point(16, 34);
            cboCustomer.Size = new Size(190, 31);
            infoPanel.Controls.Add(cboCustomer);

            btnNewCustomer = new Button();
            btnNewCustomer.BackColor = Color.White;
            btnNewCustomer.FlatStyle = FlatStyle.Flat;
            btnNewCustomer.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnNewCustomer.Location = new Point(214, 34);
            btnNewCustomer.Size = new Size(90, 31);
            btnNewCustomer.Text = "New";
            btnNewCustomer.Click += btnNewCustomer_Click;
            infoPanel.Controls.Add(btnNewCustomer);

            AddInfoLabel(infoPanel, "Payment Method", 320, 0);
            cboPaymentMethod = CreateComboBox();
            cboPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPaymentMethod.Items.AddRange(new object[] { "Cash", "EasyPaisa", "JazzCash", "Bank", "Mixed", "Credit", "Partial Credit" });
            cboPaymentMethod.Location = new Point(320, 34);
            cboPaymentMethod.Size = new Size(180, 31);
            cboPaymentMethod.SelectedIndexChanged += cboPaymentMethod_SelectedIndexChanged;
            infoPanel.Controls.Add(cboPaymentMethod);

            AddInfoLabel(infoPanel, "Wallet Account", 514, 0);
            cboWallet = CreateComboBox();
            cboWallet.Location = new Point(514, 34);
            cboWallet.Size = new Size(190, 31);
            infoPanel.Controls.Add(cboWallet);

            AddInfoLabel(infoPanel, "Discount", 16, 75);
            nudDiscount = CreateMoneyNumeric();
            nudDiscount.Location = new Point(16, 109);
            nudDiscount.Size = new Size(120, 30);
            nudDiscount.ValueChanged += TotalsInputChanged;
            infoPanel.Controls.Add(nudDiscount);

            AddInfoLabel(infoPanel, "Extra Charges", 150, 75);
            nudExtraCharges = CreateMoneyNumeric();
            nudExtraCharges.Location = new Point(150, 109);
            nudExtraCharges.Size = new Size(120, 30);
            nudExtraCharges.ValueChanged += TotalsInputChanged;
            infoPanel.Controls.Add(nudExtraCharges);

            AddInfoLabel(infoPanel, "Paid Amount", 284, 75);
            nudPaidAmount = CreateMoneyNumeric();
            nudPaidAmount.Location = new Point(284, 109);
            nudPaidAmount.Size = new Size(140, 30);
            nudPaidAmount.ValueChanged += TotalsInputChanged;
            infoPanel.Controls.Add(nudPaidAmount);

            AddInfoLabel(infoPanel, "Remarks", 438, 75);
            txtRemarks = new TextBox();
            txtRemarks.Font = new Font("Segoe UI", 10F);
            txtRemarks.Location = new Point(438, 109);
            txtRemarks.Size = new Size(266, 30);
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
            dgvCart.KeyDown += dgvCart_KeyDown;
            saleLayout.Controls.Add(dgvCart, 0, 1);
            ConfigureCartGrid();
            dgvCart.DataSource = _cartItems;

            Panel totalsPanel = new Panel();
            totalsPanel.Dock = DockStyle.Fill;
            totalsPanel.BackColor = Color.FromArgb(248, 250, 253);
            saleLayout.Controls.Add(totalsPanel, 0, 2);

            lblSubtotalValue = AddSummaryRow(totalsPanel, "Subtotal", 22, 20);
            lblGrandTotalValue = AddSummaryRow(totalsPanel, "Grand Total", 22, 60);
            lblChangeValue = AddSummaryRow(totalsPanel, "Change Return", 22, 100);
            lblDueValue = AddSummaryRow(totalsPanel, "Credit Due", 22, 140);
            lblPaymentStatus = new Label();
            lblPaymentStatus.AutoSize = true;
            lblPaymentStatus.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblPaymentStatus.ForeColor = Color.FromArgb(17, 62, 104);
            lblPaymentStatus.Location = new Point(460, 28);
            lblPaymentStatus.Text = "Status: Full Paid";
            totalsPanel.Controls.Add(lblPaymentStatus);

            btnClearCart = new Button();
            btnClearCart.BackColor = Color.White;
            btnClearCart.FlatStyle = FlatStyle.Flat;
            btnClearCart.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnClearCart.Location = new Point(460, 115);
            btnClearCart.Size = new Size(145, 42);
            btnClearCart.Text = "Clear Cart";
            btnClearCart.Click += btnClearCart_Click;
            totalsPanel.Controls.Add(btnClearCart);

            btnSaveSale = new Button();
            btnSaveSale.BackColor = Color.FromArgb(24, 125, 68);
            btnSaveSale.FlatAppearance.BorderSize = 0;
            btnSaveSale.FlatStyle = FlatStyle.Flat;
            btnSaveSale.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnSaveSale.ForeColor = Color.White;
            btnSaveSale.Location = new Point(620, 115);
            btnSaveSale.Size = new Size(190, 42);
            btnSaveSale.Text = _editingSaleId.HasValue ? "Update Sale" : "Save Sale";
            btnSaveSale.Click += btnSaveSale_Click;
            totalsPanel.Controls.Add(btnSaveSale);

            Load += SalesForm_Load;

            ResumeLayout(false);
        }

        private void SalesForm_Load(object sender, EventArgs e)
        {
            LoadLookups();
            LoadProducts();
            cboPaymentMethod.SelectedIndex = 0;
            if (_editingSaleId.HasValue)
            {
                LoadExistingSale(_editingSaleId.Value);
            }
            RecalculateTotals();
        }

        private void LoadLookups()
        {
            List<LookupOption> customers = _salesService.GetCustomers();
            customers.Insert(0, new LookupOption { Id = 0, Name = "Walk-in Customer" });
            cboCustomer.DataSource = customers;
            cboCustomer.DisplayMember = "Name";
            cboCustomer.ValueMember = "Id";

            List<LookupOption> wallets = _salesService.GetWalletAccounts();
            wallets.Insert(0, new LookupOption { Id = 0, Name = "Select Wallet" });
            cboWallet.DataSource = wallets;
            cboWallet.DisplayMember = "Name";
            cboWallet.ValueMember = "Id";
        }

        private void LoadProducts()
        {
            _allProducts = _salesService.GetProducts();
            ApplyProductFilter();
        }

        private void ApplyProductFilter()
        {
            string search = txtSearch == null ? string.Empty : txtSearch.Text.Trim().ToLowerInvariant();
            List<SaleProductLookupItem> filtered = new List<SaleProductLookupItem>();

            int index;
            for (index = 0; index < _allProducts.Count; index++)
            {
                SaleProductLookupItem item = _allProducts[index];
                string haystack = string.Format("{0} {1} {2}", item.ProductCode, item.Barcode, item.ProductName).ToLowerInvariant();

                if (string.IsNullOrWhiteSpace(search) || haystack.Contains(search))
                {
                    filtered.Add(item);
                }
            }

            _filteredProducts = filtered;
            dgvProducts.DataSource = null;
            dgvProducts.DataSource = _filteredProducts;
            FocusFirstProductRow();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyProductFilter();
        }

        private void btnNewCustomer_Click(object sender, EventArgs e)
        {
            using (CustomerEntryForm form = new CustomerEntryForm())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    LoadLookups();
                    SelectCustomer(form.SavedCustomerId);
                }
            }
        }

        private void btnOpenStock_Click(object sender, EventArgs e)
        {
            using (StockManagementForm form = new StockManagementForm(_session))
            {
                form.ShowDialog(this);
            }

            LoadProducts();
            PrepareForNextProductEntry();
        }

        private void btnAddSelected_Click(object sender, EventArgs e)
        {
            SaleProductLookupItem selected = GetSelectedProduct();
            if (selected == null)
            {
                return;
            }

            int index;
            for (index = 0; index < _cartItems.Count; index++)
            {
                if (_cartItems[index].ProductId == selected.ProductId)
                {
                    _cartItems[index].Quantity += 1;
                    RefreshCartGrid();
                    RecalculateTotals();
                    PrepareForNextProductEntry();
                    return;
                }
            }

            SaleCartItem cartItem = new SaleCartItem();
            cartItem.ProductId = selected.ProductId;
            cartItem.UnitId = selected.UnitId;
            cartItem.ProductCode = selected.ProductCode;
            cartItem.ProductName = selected.ProductName;
            cartItem.UnitName = selected.UnitName;
            cartItem.AvailableStock = selected.CurrentStock;
            cartItem.TrackStock = selected.TrackStock;
            cartItem.TrackExpiry = selected.TrackExpiry;
            cartItem.CostRate = selected.PurchasePrice;
            cartItem.Quantity = 1;
            cartItem.Rate = selected.SalePrice;
            _cartItems.Add(cartItem);

            RecalculateTotals();
            PrepareForNextProductEntry();
        }

        private SaleProductLookupItem GetSelectedProduct()
        {
            if (dgvProducts.CurrentRow == null)
            {
                return null;
            }

            return dgvProducts.CurrentRow.DataBoundItem as SaleProductLookupItem;
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                MoveProductSelection(1);
                return;
            }

            if (e.KeyCode == Keys.Up)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                MoveProductSelection(-1);
                return;
            }

            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.Handled = true;
            e.SuppressKeyPress = true;
            btnAddSelected_Click(sender, EventArgs.Empty);
        }

        private void dgvProducts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.Handled = true;
            e.SuppressKeyPress = true;
            btnAddSelected_Click(sender, EventArgs.Empty);
        }

        private void dgvCart_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _cartItems.Count)
            {
                return;
            }

            SaleCartItem item = _cartItems[e.RowIndex];

            if (item.Quantity <= 0)
            {
                _cartItems.RemoveAt(e.RowIndex);
                RecalculateTotals();
                return;
            }

            if (item.Rate <= 0)
            {
                item.Rate = item.CostRate > 0 ? item.CostRate : 1;
            }

            RefreshCartGrid();
            RecalculateTotals();
        }

        private void dgvCart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete || dgvCart.CurrentRow == null)
            {
                return;
            }

            int rowIndex = dgvCart.CurrentRow.Index;
            if (rowIndex < 0 || rowIndex >= _cartItems.Count)
            {
                return;
            }

            e.Handled = true;
            e.SuppressKeyPress = true;
            _cartItems.RemoveAt(rowIndex);
            RecalculateTotals();
        }

        private void dgvCart_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (dgvCart.Columns[e.ColumnIndex].Name != "colRemove")
            {
                return;
            }

            _cartItems.RemoveAt(e.RowIndex);
            RecalculateTotals();
        }

        private void btnClearCart_Click(object sender, EventArgs e)
        {
            _cartItems.Clear();
            nudDiscount.Value = 0;
            nudExtraCharges.Value = 0;
            _isPaidAmountManuallyChanged = false;
            SetPaidAmountProgrammatically(0);
            txtRemarks.Clear();
            RecalculateTotals();
        }

        private void btnSaveSale_Click(object sender, EventArgs e)
        {
            try
            {
                ToggleBusy(false);

                SaleSaveRequest request = new SaleSaveRequest();
                request.UserId = _session.UserId;
                request.CustomerId = GetSelectedId(cboCustomer);
                request.PaymentMethod = Convert.ToString(cboPaymentMethod.SelectedItem);
                request.WalletAccountId = GetSelectedId(cboWallet);
                request.Remarks = txtRemarks.Text;
                request.Discount = nudDiscount.Value;
                request.ExtraCharges = nudExtraCharges.Value;
                request.PaidAmount = nudPaidAmount.Value;
                request.SaleDate = DateTime.Now;

                int index;
                for (index = 0; index < _cartItems.Count; index++)
                {
                    request.Items.Add(_cartItems[index]);
                }

                SaleSaveResult result;
                if (_editingSaleId.HasValue)
                {
                    _salesService.UpdateSale(_editingSaleId.Value, request);
                    result = new SaleSaveResult { SaleId = _editingSaleId.Value, SaleNo = "Updated" };
                }
                else
                {
                    result = _salesService.SaveSale(request);
                }

                MessageBox.Show(
                    _editingSaleId.HasValue
                        ? "Sale updated successfully."
                        : string.Format("Sale saved successfully.\nSale No: {0}", result.SaleNo),
                    _editingSaleId.HasValue ? "Sale Updated" : "Sale Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                lblSaleStatus.Text = _editingSaleId.HasValue
                    ? "Sale updated successfully"
                    : string.Format("Last saved sale: {0}", result.SaleNo);
                if (_editingSaleId.HasValue)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }

                btnClearCart_Click(null, EventArgs.Empty);
                LoadProducts();
            }
            catch (Exception ex)
            {
                lblSaleStatus.Text = "Sale save failed";
                MessageBox.Show(
                    ex.Message,
                    "Save Sale Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                ToggleBusy(true);
            }
        }

        private void cboPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            string paymentMethod = Convert.ToString(cboPaymentMethod.SelectedItem);
            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                return;
            }

            if (paymentMethod == "Credit")
            {
                _isForcedCreditSelection = true;
                _isPaidAmountManuallyChanged = true;
                cboWallet.SelectedIndex = 0;
                SetPaidAmountProgrammatically(0);
                return;
            }

            if (_isForcedCreditSelection && nudPaidAmount.Value == 0)
            {
                _isPaidAmountManuallyChanged = false;
            }

            _isForcedCreditSelection = false;
            RecalculateTotals();

            int index;
            for (index = 0; index < cboWallet.Items.Count; index++)
            {
                LookupOption option = cboWallet.Items[index] as LookupOption;
                if (option == null)
                {
                    continue;
                }

                if (option.Name.IndexOf(paymentMethod, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    cboWallet.SelectedIndex = index;
                    return;
                }

                if (paymentMethod == "Cash" && option.Name.IndexOf("Cash", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    cboWallet.SelectedIndex = index;
                    return;
                }
            }

            cboWallet.SelectedIndex = 0;
        }

        private void TotalsInputChanged(object sender, EventArgs e)
        {
            if (sender == nudPaidAmount && !_isAutoUpdatingPaidAmount)
            {
                _isPaidAmountManuallyChanged = true;
            }

            RecalculateTotals();
        }

        private void RecalculateTotals()
        {
            decimal subtotal = 0;
            int index;

            for (index = 0; index < _cartItems.Count; index++)
            {
                subtotal += _cartItems[index].LineTotal;
            }

            decimal grandTotal = subtotal - nudDiscount.Value + nudExtraCharges.Value;
            if (grandTotal < 0)
            {
                grandTotal = 0;
            }

            string paymentMethod = Convert.ToString(cboPaymentMethod.SelectedItem);
            bool isCreditSale = paymentMethod == "Credit";
            bool isPartialCredit = paymentMethod == "Partial Credit";

            if (isCreditSale)
            {
                if (nudPaidAmount.Value != 0)
                {
                    SetPaidAmountProgrammatically(0);
                }
            }
            else if (!_isPaidAmountManuallyChanged)
            {
                if (nudPaidAmount.Value != grandTotal)
                {
                    SetPaidAmountProgrammatically(grandTotal);
                }
            }

            decimal change = nudPaidAmount.Value - grandTotal;
            decimal due = grandTotal - nudPaidAmount.Value;

            if (due < 0)
            {
                due = 0;
            }

            if (change < 0)
            {
                change = 0;
            }

            lblSubtotalValue.Text = FormatCurrency(subtotal);
            lblGrandTotalValue.Text = FormatCurrency(grandTotal);
            lblChangeValue.Text = FormatCurrency(change);
            lblDueValue.Text = FormatCurrency(due);

            if (grandTotal <= 0)
            {
                lblPaymentStatus.Text = "Status: No items added";
            }
            else if (due <= 0)
            {
                lblPaymentStatus.Text = "Status: Full Paid";
            }
            else if (nudPaidAmount.Value <= 0 || isCreditSale)
            {
                lblPaymentStatus.Text = "Status: Full Credit";
            }
            else if (isPartialCredit || due > 0)
            {
                lblPaymentStatus.Text = string.Format("Status: Partial Credit, Paid {0}, Due {1}", FormatCurrency(nudPaidAmount.Value), FormatCurrency(due));
            }
        }

        private void RefreshCartGrid()
        {
            dgvCart.Refresh();
        }

        private void PrepareForNextProductEntry()
        {
            txtSearch.Focus();
            txtSearch.SelectAll();
        }

        private void FocusFirstProductRow()
        {
            if (dgvProducts.Rows.Count == 0)
            {
                return;
            }

            dgvProducts.ClearSelection();
            dgvProducts.Rows[0].Selected = true;
            dgvProducts.CurrentCell = dgvProducts.Rows[0].Cells[0];
        }

        private void MoveProductSelection(int offset)
        {
            if (dgvProducts.Rows.Count == 0)
            {
                return;
            }

            int currentIndex = dgvProducts.CurrentRow != null ? dgvProducts.CurrentRow.Index : 0;
            int nextIndex = currentIndex + offset;

            if (nextIndex < 0)
            {
                nextIndex = 0;
            }
            else if (nextIndex >= dgvProducts.Rows.Count)
            {
                nextIndex = dgvProducts.Rows.Count - 1;
            }

            dgvProducts.ClearSelection();
            dgvProducts.Rows[nextIndex].Selected = true;
            dgvProducts.CurrentCell = dgvProducts.Rows[nextIndex].Cells[0];
        }

        private void SetPaidAmountProgrammatically(decimal value)
        {
            _isAutoUpdatingPaidAmount = true;
            nudPaidAmount.Value = value;
            _isAutoUpdatingPaidAmount = false;
        }

        private void ToggleBusy(bool isEnabled)
        {
            btnAddSelected.Enabled = isEnabled;
            btnSaveSale.Enabled = isEnabled;
            btnClearCart.Enabled = isEnabled;
            UseWaitCursor = !isEnabled;
        }

        private void ConfigureProductGrid()
        {
            ApplyGridStyle(dgvProducts);
            dgvProducts.Columns.Add(CreateTextColumn("ProductCode", "Code", 70F, null, true));
            dgvProducts.Columns.Add(CreateTextColumn("ProductName", "Product", 145F, null, true));
            dgvProducts.Columns.Add(CreateTextColumn("UnitName", "Unit", 55F, null, true));
            dgvProducts.Columns.Add(CreateTextColumn("SalePrice", "Price", 70F, "N2", true));
            dgvProducts.Columns.Add(CreateTextColumn("CurrentStock", "Stock", 68F, "N2", true));
        }

        private void ConfigureCartGrid()
        {
            ApplyGridStyle(dgvCart);
            dgvCart.Columns.Add(CreateTextColumn("ProductCode", "Code", 60F, null, true));
            dgvCart.Columns.Add(CreateTextColumn("ProductName", "Product", 135F, null, true));
            dgvCart.Columns.Add(CreateTextColumn("UnitName", "Unit", 50F, null, true));
            dgvCart.Columns.Add(CreateTextColumn("AvailableStock", "Stock", 65F, "N2", true));
            dgvCart.Columns.Add(CreateTextColumn("Quantity", "Qty", 55F, "N2", false));
            dgvCart.Columns.Add(CreateTextColumn("Rate", "Rate", 65F, "N2", false));
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
            column.DataPropertyName = propertyName;
            column.HeaderText = headerText;
            column.FillWeight = fillWeight;
            column.ReadOnly = readOnly;

            if (!string.IsNullOrWhiteSpace(format))
            {
                column.DefaultCellStyle.Format = format;
            }

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
            value.ForeColor = Color.FromArgb(17, 62, 104);
            value.Location = new Point(left + 165, top);
            value.Text = "Rs. 0.00";
            parent.Controls.Add(value);

            return value;
        }

        private static int? GetSelectedId(ComboBox comboBox)
        {
            LookupOption option = comboBox.SelectedItem as LookupOption;
            if (option == null || option.Id <= 0)
            {
                return null;
            }

            return option.Id;
        }

        private static string FormatCurrency(decimal amount)
        {
            return string.Format("Rs. {0:N2}", amount);
        }

        private void SelectCustomer(int customerId)
        {
            for (int i = 0; i < cboCustomer.Items.Count; i++)
            {
                LookupOption option = cboCustomer.Items[i] as LookupOption;
                if (option != null && option.Id == customerId)
                {
                    cboCustomer.SelectedIndex = i;
                    break;
                }
            }
        }

        private void LoadExistingSale(long saleId)
        {
            SaleEditRecord sale = _salesService.GetSaleForEdit(saleId);
            SelectCustomer(sale.CustomerId.GetValueOrDefault());
            SelectPaymentMethod(sale.PaymentMethod);
            SelectWallet(sale.WalletAccountId.GetValueOrDefault());
            txtRemarks.Text = sale.Remarks;
            nudDiscount.Value = sale.Discount;
            nudExtraCharges.Value = sale.ExtraCharges;
            _cartItems.Clear();

            for (int i = 0; i < sale.Items.Count; i++)
            {
                _cartItems.Add(sale.Items[i]);
            }

            _isPaidAmountManuallyChanged = true;
            SetPaidAmountProgrammatically(sale.PaidAmount);
            lblSaleStatus.Text = string.Format("Editing sale {0}", sale.SaleNo);
        }

        private void SelectPaymentMethod(string paymentMethod)
        {
            for (int i = 0; i < cboPaymentMethod.Items.Count; i++)
            {
                if (string.Equals(Convert.ToString(cboPaymentMethod.Items[i]), paymentMethod, StringComparison.OrdinalIgnoreCase))
                {
                    cboPaymentMethod.SelectedIndex = i;
                    break;
                }
            }
        }

        private void SelectWallet(int walletId)
        {
            for (int i = 0; i < cboWallet.Items.Count; i++)
            {
                LookupOption option = cboWallet.Items[i] as LookupOption;
                if (option != null && option.Id == walletId)
                {
                    cboWallet.SelectedIndex = i;
                    break;
                }
            }
        }
    }
}
