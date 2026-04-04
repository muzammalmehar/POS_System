using System;
using System.Drawing;
using System.Windows.Forms;
using ShopPOS.Models;
using ShopPOS.Services;

namespace ShopPOS
{
    public class ExpiryManagementForm : Form
    {
        private readonly UserSession _session;
        private readonly ExpiryService _expiryService;
        private DataGridView dgvExpiring;
        private DataGridView dgvExpired;
        private NumericUpDown nudDaysAhead;
        private ComboBox cboResolution;
        private TextBox txtRemarks;

        public ExpiryManagementForm(UserSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            _session = session;
            _expiryService = new ExpiryService();
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
            Text = "Expiry Management";

            Panel header = new Panel();
            header.BackColor = Color.FromArgb(142, 68, 33);
            header.Dock = DockStyle.Top;
            header.Height = 90;
            Controls.Add(header);

            Label lblTitle = new Label();
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(24, 18);
            lblTitle.Text = "Expiry Management";
            header.Controls.Add(lblTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.WhiteSmoke;
            lblSubtitle.Location = new Point(28, 57);
            lblSubtitle.Text = "Track expiring batches, move expired stock out, and mark vendor return or burn actions.";
            header.Controls.Add(lblSubtitle);

            Panel filterPanel = new Panel();
            filterPanel.BackColor = Color.White;
            filterPanel.Location = new Point(20, 108);
            filterPanel.Size = new Size(1320, 72);
            filterPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Controls.Add(filterPanel);

            Label lblDays = new Label();
            lblDays.AutoSize = true;
            lblDays.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblDays.Location = new Point(16, 14);
            lblDays.Text = "Show Expiring Within (Days)";
            filterPanel.Controls.Add(lblDays);

            nudDaysAhead = new NumericUpDown();
            nudDaysAhead.Font = new Font("Segoe UI", 10F);
            nudDaysAhead.Location = new Point(20, 38);
            nudDaysAhead.Maximum = 365;
            nudDaysAhead.Minimum = 1;
            nudDaysAhead.Size = new Size(90, 30);
            nudDaysAhead.Value = 30;
            nudDaysAhead.ValueChanged += nudDaysAhead_ValueChanged;
            filterPanel.Controls.Add(nudDaysAhead);

            Button btnMoveExpired = new Button();
            btnMoveExpired.BackColor = Color.FromArgb(142, 68, 33);
            btnMoveExpired.FlatAppearance.BorderSize = 0;
            btnMoveExpired.FlatStyle = FlatStyle.Flat;
            btnMoveExpired.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnMoveExpired.ForeColor = Color.White;
            btnMoveExpired.Location = new Point(140, 34);
            btnMoveExpired.Size = new Size(210, 34);
            btnMoveExpired.Text = "Move Selected To Expired";
            btnMoveExpired.Click += btnMoveExpired_Click;
            filterPanel.Controls.Add(btnMoveExpired);

            Label lblResolution = new Label();
            lblResolution.AutoSize = true;
            lblResolution.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblResolution.Location = new Point(384, 14);
            lblResolution.Text = "Resolution";
            filterPanel.Controls.Add(lblResolution);

            cboResolution = new ComboBox();
            cboResolution.DropDownStyle = ComboBoxStyle.DropDownList;
            cboResolution.Font = new Font("Segoe UI", 10F);
            cboResolution.Items.AddRange(new object[] { "ReturnedToVendor", "Burnt", "Adjusted" });
            cboResolution.Location = new Point(388, 38);
            cboResolution.Size = new Size(180, 31);
            cboResolution.SelectedIndex = 0;
            filterPanel.Controls.Add(cboResolution);

            txtRemarks = new TextBox();
            txtRemarks.Font = new Font("Segoe UI", 10F);
            txtRemarks.Location = new Point(586, 38);
            txtRemarks.Size = new Size(420, 30);
            filterPanel.Controls.Add(txtRemarks);

            Button btnResolve = new Button();
            btnResolve.BackColor = Color.White;
            btnResolve.FlatStyle = FlatStyle.Flat;
            btnResolve.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnResolve.Location = new Point(1020, 34);
            btnResolve.Size = new Size(190, 34);
            btnResolve.Text = "Update Selected Record";
            btnResolve.Click += btnResolve_Click;
            filterPanel.Controls.Add(btnResolve);

            SplitContainer splitMain = new SplitContainer();
            splitMain.Location = new Point(20, 196);
            splitMain.Size = new Size(1320, 602);
            splitMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitMain.Orientation = Orientation.Horizontal;
            splitMain.SplitterDistance = 285;
            Controls.Add(splitMain);

            GroupBox expiringGroup = new GroupBox();
            expiringGroup.Dock = DockStyle.Fill;
            expiringGroup.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            expiringGroup.Text = "Expiring / Expired Batches Still In Stock";
            splitMain.Panel1.Controls.Add(expiringGroup);

            dgvExpiring = new DataGridView();
            dgvExpiring.Dock = DockStyle.Fill;
            dgvExpiring.AllowUserToAddRows = false;
            dgvExpiring.AllowUserToDeleteRows = false;
            dgvExpiring.AutoGenerateColumns = false;
            dgvExpiring.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvExpiring.BackgroundColor = Color.White;
            dgvExpiring.BorderStyle = BorderStyle.None;
            dgvExpiring.EnableHeadersVisualStyles = false;
            dgvExpiring.GridColor = Color.Gainsboro;
            dgvExpiring.MultiSelect = false;
            dgvExpiring.ReadOnly = true;
            dgvExpiring.RowHeadersVisible = false;
            dgvExpiring.RowTemplate.Height = 30;
            dgvExpiring.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            expiringGroup.Controls.Add(dgvExpiring);
            ConfigureExpiringGrid();

            GroupBox expiredGroup = new GroupBox();
            expiredGroup.Dock = DockStyle.Fill;
            expiredGroup.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            expiredGroup.Text = "Expired Stock Records";
            splitMain.Panel2.Controls.Add(expiredGroup);

            dgvExpired = new DataGridView();
            dgvExpired.Dock = DockStyle.Fill;
            dgvExpired.AllowUserToAddRows = false;
            dgvExpired.AllowUserToDeleteRows = false;
            dgvExpired.AutoGenerateColumns = false;
            dgvExpired.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvExpired.BackgroundColor = Color.White;
            dgvExpired.BorderStyle = BorderStyle.None;
            dgvExpired.EnableHeadersVisualStyles = false;
            dgvExpired.GridColor = Color.Gainsboro;
            dgvExpired.MultiSelect = false;
            dgvExpired.ReadOnly = true;
            dgvExpired.RowHeadersVisible = false;
            dgvExpired.RowTemplate.Height = 30;
            dgvExpired.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            expiredGroup.Controls.Add(dgvExpired);
            ConfigureExpiredGrid();

            Load += ExpiryManagementForm_Load;
            ResumeLayout(false);
        }

        private void ExpiryManagementForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void nudDaysAhead_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnMoveExpired_Click(object sender, EventArgs e)
        {
            ExpiringBatchItem item = GetSelectedExpiringItem();
            if (item == null)
            {
                MessageBox.Show("Select an expiring batch first.", "Expiry Management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _expiryService.MoveExpiredBatchToRecords(item.BatchId, _session.UserId, txtRemarks.Text);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Expiry Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnResolve_Click(object sender, EventArgs e)
        {
            ExpiredStockRecord item = GetSelectedExpiredRecord();
            if (item == null)
            {
                MessageBox.Show("Select an expired stock record first.", "Expiry Management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _expiryService.UpdateExpiredRecordResolution(item.ExpiredRecordId, Convert.ToString(cboResolution.SelectedItem), txtRemarks.Text);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Expiry Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            dgvExpiring.DataSource = null;
            dgvExpired.DataSource = null;
            dgvExpiring.DataSource = _expiryService.GetExpiringBatches(Decimal.ToInt32(nudDaysAhead.Value));
            dgvExpired.DataSource = _expiryService.GetExpiredStockRecords();
        }

        private ExpiringBatchItem GetSelectedExpiringItem()
        {
            if (dgvExpiring.CurrentRow == null)
            {
                return null;
            }

            return dgvExpiring.CurrentRow.DataBoundItem as ExpiringBatchItem;
        }

        private ExpiredStockRecord GetSelectedExpiredRecord()
        {
            if (dgvExpired.CurrentRow == null)
            {
                return null;
            }

            return dgvExpired.CurrentRow.DataBoundItem as ExpiredStockRecord;
        }

        private void ConfigureExpiringGrid()
        {
            ApplyGridStyle(dgvExpiring);
            dgvExpiring.Columns.Add(CreateTextColumn("ProductCode", "Code", 65F, null));
            dgvExpiring.Columns.Add(CreateTextColumn("ProductName", "Product", 140F, null));
            dgvExpiring.Columns.Add(CreateTextColumn("SupplierName", "Vendor", 100F, null));
            dgvExpiring.Columns.Add(CreateTextColumn("BatchNo", "Batch", 80F, null));
            dgvExpiring.Columns.Add(CreateTextColumn("ExpiryDate", "Expiry", 80F, "dd MMM yyyy"));
            dgvExpiring.Columns.Add(CreateTextColumn("RemainingQty", "Qty", 65F, "N2"));
            dgvExpiring.Columns.Add(CreateTextColumn("AgeStatus", "Status", 110F, null));
        }

        private void ConfigureExpiredGrid()
        {
            ApplyGridStyle(dgvExpired);
            dgvExpired.Columns.Add(CreateTextColumn("ProductCode", "Code", 65F, null));
            dgvExpired.Columns.Add(CreateTextColumn("ProductName", "Product", 130F, null));
            dgvExpired.Columns.Add(CreateTextColumn("SupplierName", "Vendor", 100F, null));
            dgvExpired.Columns.Add(CreateTextColumn("BatchNo", "Batch", 70F, null));
            dgvExpired.Columns.Add(CreateTextColumn("ExpiryDate", "Expiry", 80F, "dd MMM yyyy"));
            dgvExpired.Columns.Add(CreateTextColumn("Quantity", "Qty", 60F, "N2"));
            dgvExpired.Columns.Add(CreateTextColumn("ResolutionStatus", "Resolution", 100F, null));
            dgvExpired.Columns.Add(CreateTextColumn("ProcessedAt", "Moved On", 95F, "dd MMM yyyy"));
            dgvExpired.Columns.Add(CreateTextColumn("Remarks", "Remarks", 120F, null));
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
    }
}
