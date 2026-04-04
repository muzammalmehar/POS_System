using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ShopPOS.Models;
using ShopPOS.Services;

namespace ShopPOS
{
    public partial class StockManagementForm : Form
    {
        private readonly UserSession _session;
        private readonly StockService _stockService;

        private List<StockOverviewItem> _allStockItems;
        private TextBox txtSearch;
        private DataGridView dgvStock;
        private DataGridView dgvMovements;
        private ComboBox cboAdjustmentType;
        private NumericUpDown nudQuantity;
        private NumericUpDown nudUnitCost;
        private TextBox txtRemarks;
        private Label lblSelectedProduct;
        private Label lblStockStatus;
        private Label lblExpiryHelp;
        private Button btnSaveAdjustment;
        private Button btnManageProducts;
        private Button btnManageExpiry;

        public StockManagementForm(UserSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            _session = session;
            _stockService = new StockService();
            _allStockItems = new List<StockOverviewItem>();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1380, 820);
            MinimumSize = new Size(1398, 867);
            StartPosition = FormStartPosition.CenterParent;
            WindowState = FormWindowState.Maximized;
            Text = "Stock Management";

            Panel header = new Panel();
            header.BackColor = Color.FromArgb(92, 43, 130);
            header.Dock = DockStyle.Top;
            header.Height = 90;
            Controls.Add(header);

            Label lblTitle = new Label();
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(24, 18);
            lblTitle.Text = "Stock Management";
            header.Controls.Add(lblTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.WhiteSmoke;
            lblSubtitle.Location = new Point(28, 57);
            lblSubtitle.Text = "Review stock levels, inspect movement history, and post manual adjustments.";
            header.Controls.Add(lblSubtitle);

            SplitContainer splitMain = new SplitContainer();
            splitMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitMain.Location = new Point(20, 108);
            splitMain.Size = new Size(1340, 690);
            splitMain.SplitterDistance = 780;
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
            lblSearch.Text = "Search Product";
            leftPanel.Controls.Add(lblSearch);

            txtSearch = new TextBox();
            txtSearch.Font = new Font("Segoe UI", 11F);
            txtSearch.Location = new Point(18, 42);
            txtSearch.Size = new Size(734, 32);
            txtSearch.TextChanged += txtSearch_TextChanged;
            leftPanel.Controls.Add(txtSearch);

            dgvStock = new DataGridView();
            dgvStock.AllowUserToAddRows = false;
            dgvStock.AllowUserToDeleteRows = false;
            dgvStock.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvStock.AutoGenerateColumns = false;
            dgvStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStock.BackgroundColor = Color.White;
            dgvStock.BorderStyle = BorderStyle.None;
            dgvStock.EnableHeadersVisualStyles = false;
            dgvStock.GridColor = Color.Gainsboro;
            dgvStock.Location = new Point(18, 88);
            dgvStock.MultiSelect = false;
            dgvStock.ReadOnly = true;
            dgvStock.RowHeadersVisible = false;
            dgvStock.RowTemplate.Height = 30;
            dgvStock.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStock.Size = new Size(734, 575);
            dgvStock.SelectionChanged += dgvStock_SelectionChanged;
            leftPanel.Controls.Add(dgvStock);
            ConfigureStockGrid();

            Panel rightPanel = new Panel();
            rightPanel.Dock = DockStyle.Fill;
            rightPanel.BackColor = Color.White;
            rightPanel.Padding = new Padding(14);
            splitMain.Panel2.Controls.Add(rightPanel);

            lblSelectedProduct = new Label();
            lblSelectedProduct.AutoSize = true;
            lblSelectedProduct.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
            lblSelectedProduct.Location = new Point(14, 12);
            lblSelectedProduct.Text = "Select a product";
            rightPanel.Controls.Add(lblSelectedProduct);

            lblStockStatus = new Label();
            lblStockStatus.AutoSize = true;
            lblStockStatus.Font = new Font("Segoe UI", 10F);
            lblStockStatus.Location = new Point(15, 43);
            lblStockStatus.Text = "Stock status";
            rightPanel.Controls.Add(lblStockStatus);

            lblExpiryHelp = new Label();
            lblExpiryHelp.Font = new Font("Segoe UI", 9F);
            lblExpiryHelp.ForeColor = Color.DimGray;
            lblExpiryHelp.Location = new Point(18, 92);
            lblExpiryHelp.Size = new Size(320, 48);
            lblExpiryHelp.Text = "Manual stock adjustments update quantity directly. Use Purchase Entry when you need exact vendor batch and expiry details.";
            rightPanel.Controls.Add(lblExpiryHelp);

            btnManageProducts = new Button();
            btnManageProducts.BackColor = Color.White;
            btnManageProducts.FlatStyle = FlatStyle.Flat;
            btnManageProducts.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnManageProducts.Location = new Point(354, 14);
            btnManageProducts.Size = new Size(168, 36);
            btnManageProducts.Text = "Manage Products";
            btnManageProducts.Click += btnManageProducts_Click;
            rightPanel.Controls.Add(btnManageProducts);

            btnManageExpiry = new Button();
            btnManageExpiry.BackColor = Color.White;
            btnManageExpiry.FlatStyle = FlatStyle.Flat;
            btnManageExpiry.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnManageExpiry.Location = new Point(354, 56);
            btnManageExpiry.Size = new Size(168, 36);
            btnManageExpiry.Text = "Manage Expiry";
            btnManageExpiry.Click += btnManageExpiry_Click;
            rightPanel.Controls.Add(btnManageExpiry);

            GroupBox adjustmentGroup = new GroupBox();
            adjustmentGroup.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            adjustmentGroup.Location = new Point(18, 142);
            adjustmentGroup.Size = new Size(504, 220);
            adjustmentGroup.Text = "Manual Stock Adjustment";
            rightPanel.Controls.Add(adjustmentGroup);

            AddFieldLabel(adjustmentGroup, "Adjustment Type", 16, 33);
            cboAdjustmentType = new ComboBox();
            cboAdjustmentType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAdjustmentType.Font = new Font("Segoe UI", 10F);
            cboAdjustmentType.Items.AddRange(new object[] { "OpeningStock", "StockAdjustIn", "StockAdjustOut", "Damage" });
            cboAdjustmentType.Location = new Point(20, 58);
            cboAdjustmentType.Size = new Size(210, 31);
            adjustmentGroup.Controls.Add(cboAdjustmentType);

            AddFieldLabel(adjustmentGroup, "Quantity", 250, 33);
            nudQuantity = CreateMoneyNumeric();
            nudQuantity.Location = new Point(254, 58);
            nudQuantity.Size = new Size(110, 30);
            adjustmentGroup.Controls.Add(nudQuantity);

            AddFieldLabel(adjustmentGroup, "Unit Cost", 378, 33);
            nudUnitCost = CreateMoneyNumeric();
            nudUnitCost.Location = new Point(382, 58);
            nudUnitCost.Size = new Size(105, 30);
            adjustmentGroup.Controls.Add(nudUnitCost);

            AddFieldLabel(adjustmentGroup, "Remarks", 16, 102);
            txtRemarks = new TextBox();
            txtRemarks.Font = new Font("Segoe UI", 10F);
            txtRemarks.Location = new Point(20, 127);
            txtRemarks.Size = new Size(467, 30);
            adjustmentGroup.Controls.Add(txtRemarks);

            btnSaveAdjustment = new Button();
            btnSaveAdjustment.BackColor = Color.FromArgb(92, 43, 130);
            btnSaveAdjustment.FlatAppearance.BorderSize = 0;
            btnSaveAdjustment.FlatStyle = FlatStyle.Flat;
            btnSaveAdjustment.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnSaveAdjustment.ForeColor = Color.White;
            btnSaveAdjustment.Location = new Point(20, 171);
            btnSaveAdjustment.Size = new Size(180, 35);
            btnSaveAdjustment.Text = "Save Adjustment";
            btnSaveAdjustment.Click += btnSaveAdjustment_Click;
            adjustmentGroup.Controls.Add(btnSaveAdjustment);

            GroupBox movementGroup = new GroupBox();
            movementGroup.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            movementGroup.Location = new Point(18, 376);
            movementGroup.Size = new Size(504, 287);
            movementGroup.Text = "Recent Stock Movements";
            rightPanel.Controls.Add(movementGroup);

            dgvMovements = new DataGridView();
            dgvMovements.AllowUserToAddRows = false;
            dgvMovements.AllowUserToDeleteRows = false;
            dgvMovements.AutoGenerateColumns = false;
            dgvMovements.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMovements.BackgroundColor = Color.White;
            dgvMovements.BorderStyle = BorderStyle.None;
            dgvMovements.Dock = DockStyle.Fill;
            dgvMovements.EnableHeadersVisualStyles = false;
            dgvMovements.GridColor = Color.Gainsboro;
            dgvMovements.ReadOnly = true;
            dgvMovements.RowHeadersVisible = false;
            dgvMovements.RowTemplate.Height = 30;
            dgvMovements.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            movementGroup.Controls.Add(dgvMovements);
            ConfigureMovementGrid();

            Load += StockManagementForm_Load;

            ResumeLayout(false);
        }

        private void StockManagementForm_Load(object sender, EventArgs e)
        {
            cboAdjustmentType.SelectedIndex = 0;
            LoadStockData(null);
        }

        private void LoadStockData(int? selectedProductId)
        {
            _allStockItems = _stockService.GetStockOverview();
            ApplyFilter(selectedProductId);
        }

        private void ApplyFilter(int? selectedProductId = null)
        {
            string search = txtSearch == null ? string.Empty : txtSearch.Text.Trim().ToLowerInvariant();
            List<StockOverviewItem> filtered = new List<StockOverviewItem>();
            int index;

            for (index = 0; index < _allStockItems.Count; index++)
            {
                StockOverviewItem item = _allStockItems[index];
                string haystack = string.Format("{0} {1}", item.ProductCode, item.ProductName).ToLowerInvariant();

                if (string.IsNullOrWhiteSpace(search) || haystack.Contains(search))
                {
                    filtered.Add(item);
                }
            }

            dgvStock.DataSource = null;
            dgvStock.DataSource = filtered;

            bool selected = false;
            if (selectedProductId.HasValue)
            {
                for (index = 0; index < dgvStock.Rows.Count; index++)
                {
                    StockOverviewItem stockItem = dgvStock.Rows[index].DataBoundItem as StockOverviewItem;
                    if (stockItem != null && stockItem.ProductId == selectedProductId.Value)
                    {
                        dgvStock.ClearSelection();
                        dgvStock.Rows[index].Selected = true;
                        dgvStock.CurrentCell = dgvStock.Rows[index].Cells[0];
                        selected = true;
                        break;
                    }
                }
            }

            if (!selected && dgvStock.Rows.Count > 0)
            {
                dgvStock.ClearSelection();
                dgvStock.Rows[0].Selected = true;
                dgvStock.CurrentCell = dgvStock.Rows[0].Cells[0];
            }

            LoadMovementForSelectedProduct();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void dgvStock_SelectionChanged(object sender, EventArgs e)
        {
            LoadMovementForSelectedProduct();
        }

        private void LoadMovementForSelectedProduct()
        {
            StockOverviewItem item = GetSelectedStockItem();
            if (item == null)
            {
                dgvMovements.DataSource = null;
                lblSelectedProduct.Text = "Select a product";
                lblStockStatus.Text = "Stock status";
                lblExpiryHelp.Text = "Manual stock adjustments update quantity directly. Use Purchase Entry when you need exact vendor batch and expiry details.";
                return;
            }

            lblSelectedProduct.Text = string.Format("{0} - {1}", item.ProductCode, item.ProductName);
            lblStockStatus.Text = string.Format(
                "Current stock: {0:N2} {1} | Reorder level: {2:N2} | Status: {3}",
                item.CurrentStock,
                item.UnitName,
                item.ReorderLevel,
                item.StockStatus);

            if (item.StockStatus == "Out of stock")
            {
                lblStockStatus.ForeColor = Color.Firebrick;
            }
            else if (item.StockStatus == "Low stock")
            {
                lblStockStatus.ForeColor = Color.DarkOrange;
            }
            else
            {
                lblStockStatus.ForeColor = Color.SeaGreen;
            }

            nudUnitCost.Value = item.PurchasePrice >= 0 ? item.PurchasePrice : 0;
            lblExpiryHelp.Text = item.TrackExpiry
                ? "This product tracks expiry. Manual stock changes now update internal batches too, but use Purchase Entry for exact batch number and expiry-date receiving."
                : "This product does not track expiry. Manual stock changes update quantity directly.";
            lblExpiryHelp.ForeColor = item.TrackExpiry ? Color.FromArgb(142, 68, 33) : Color.DimGray;

            dgvMovements.DataSource = null;
            dgvMovements.DataSource = _stockService.GetRecentMovements(item.ProductId);
        }

        private void btnSaveAdjustment_Click(object sender, EventArgs e)
        {
            StockOverviewItem item = GetSelectedStockItem();
            if (item == null)
            {
                MessageBox.Show("Select a product first.", "Stock Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ToggleBusy(false);

                StockAdjustmentRequest request = new StockAdjustmentRequest();
                request.ProductId = item.ProductId;
                request.TransactionType = Convert.ToString(cboAdjustmentType.SelectedItem);
                request.Quantity = nudQuantity.Value;
                request.UnitCost = nudUnitCost.Value;
                request.Remarks = txtRemarks.Text;
                request.UserId = _session.UserId;

                _stockService.SaveAdjustment(request);

                MessageBox.Show("Stock adjustment saved successfully.", "Stock Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Information);

                nudQuantity.Value = 0;
                nudUnitCost.Value = 0;
                txtRemarks.Clear();

                LoadStockData(item.ProductId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Stock Adjustment Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ToggleBusy(true);
            }
        }

        private void btnManageProducts_Click(object sender, EventArgs e)
        {
            StockOverviewItem item = GetSelectedStockItem();
            using (ProductManagementForm productForm = new ProductManagementForm())
            {
                productForm.ShowDialog(this);
            }

            LoadStockData(item == null ? (int?)null : item.ProductId);
        }

        private void btnManageExpiry_Click(object sender, EventArgs e)
        {
            StockOverviewItem item = GetSelectedStockItem();
            using (ExpiryManagementForm expiryForm = new ExpiryManagementForm(_session))
            {
                expiryForm.ShowDialog(this);
            }

            LoadStockData(item == null ? (int?)null : item.ProductId);
        }

        private StockOverviewItem GetSelectedStockItem()
        {
            if (dgvStock.CurrentRow == null)
            {
                return null;
            }

            return dgvStock.CurrentRow.DataBoundItem as StockOverviewItem;
        }

        private void ConfigureStockGrid()
        {
            ApplyGridStyle(dgvStock);
            dgvStock.Columns.Add(CreateTextColumn("ProductCode", "Code", 65F, null));
            dgvStock.Columns.Add(CreateTextColumn("ProductName", "Product", 145F, null));
            dgvStock.Columns.Add(CreateTextColumn("UnitName", "Unit", 45F, null));
            dgvStock.Columns.Add(CreateTextColumn("CurrentStock", "Current", 70F, "N2"));
            dgvStock.Columns.Add(CreateTextColumn("ReorderLevel", "Reorder", 70F, "N2"));
            dgvStock.Columns.Add(CreateTextColumn("PurchasePrice", "Cost", 65F, "N2"));
            dgvStock.Columns.Add(CreateTextColumn("StockValue", "Value", 80F, "N2"));
            dgvStock.Columns.Add(CreateTextColumn("StockStatus", "Status", 70F, null));
        }

        private void ConfigureMovementGrid()
        {
            ApplyGridStyle(dgvMovements);
            dgvMovements.Columns.Add(CreateTextColumn("CreatedAt", "Date", 110F, "dd MMM yyyy hh:mm tt"));
            dgvMovements.Columns.Add(CreateTextColumn("TransactionType", "Type", 90F, null));
            dgvMovements.Columns.Add(CreateTextColumn("QtyIn", "In", 55F, "N2"));
            dgvMovements.Columns.Add(CreateTextColumn("QtyOut", "Out", 55F, "N2"));
            dgvMovements.Columns.Add(CreateTextColumn("UnitCost", "Cost", 60F, "N2"));
            dgvMovements.Columns.Add(CreateTextColumn("Remarks", "Remarks", 120F, null));
            dgvMovements.Columns.Add(CreateTextColumn("CreatedByName", "By", 80F, null));
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
            column.ReadOnly = true;

            if (!string.IsNullOrWhiteSpace(format))
            {
                column.DefaultCellStyle.Format = format;
            }

            return column;
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

        private static void AddFieldLabel(Control parent, string text, int left, int top)
        {
            Label label = new Label();
            label.AutoSize = true;
            label.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            label.Location = new Point(left, top);
            label.Text = text;
            parent.Controls.Add(label);
        }

        private void ToggleBusy(bool isEnabled)
        {
            btnSaveAdjustment.Enabled = isEnabled;
            UseWaitCursor = !isEnabled;
        }
    }
}
