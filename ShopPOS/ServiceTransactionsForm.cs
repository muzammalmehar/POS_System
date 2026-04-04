using System;
using System.Drawing;
using System.Windows.Forms;
using ShopPOS.Models;
using ShopPOS.Services;

namespace ShopPOS
{
    public class ServiceTransactionsForm : Form
    {
        private readonly UserSession _session;
        private readonly ServiceCenterService _serviceService;

        private TextBox txtCustomerFilter;
        private ComboBox cboStatusFilter;
        private ComboBox cboBillCategoryFilter;
        private ComboBox cboNewStatus;
        private TextBox txtRemarks;
        private DataGridView dgvTransactions;

        public ServiceTransactionsForm(UserSession session)
        {
            if (session == null) throw new ArgumentNullException("session");

            _session = session;
            _serviceService = new ServiceCenterService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1320, 820);
            MinimumSize = new Size(1180, 760);
            StartPosition = FormStartPosition.CenterParent;
            WindowState = FormWindowState.Maximized;
            Text = "Service Transactions";

            Panel header = new Panel();
            header.BackColor = Color.FromArgb(107, 44, 145);
            header.Dock = DockStyle.Top;
            header.Height = 92;
            Controls.Add(header);

            Label lblTitle = new Label();
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(22, 18);
            lblTitle.Text = "Service Transactions";
            header.Controls.Add(lblTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.WhiteSmoke;
            lblSubtitle.Location = new Point(26, 58);
            lblSubtitle.Text = "Review recent service activity, filter by customer or status, and update pending/completed records.";
            header.Controls.Add(lblSubtitle);

            Panel filterPanel = new Panel();
            filterPanel.BackColor = Color.White;
            filterPanel.Location = new Point(18, 108);
            filterPanel.Size = new Size(1284, 134);
            filterPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Controls.Add(filterPanel);

            filterPanel.Controls.Add(MakeLabel("Customer / Mobile / Reference / Txn ID / Bill Type", 18, 16));
            txtCustomerFilter = MakeText(18, 40, 320);
            txtCustomerFilter.TextChanged += FiltersChanged;
            filterPanel.Controls.Add(txtCustomerFilter);

            filterPanel.Controls.Add(MakeLabel("Status", 356, 16));
            cboStatusFilter = MakeCombo(356, 40, 170);
            cboStatusFilter.Items.AddRange(new object[] { "All", "Pending", "Completed", "Cancelled", "Refunded" });
            cboStatusFilter.SelectedIndexChanged += FiltersChanged;
            filterPanel.Controls.Add(cboStatusFilter);

            filterPanel.Controls.Add(MakeLabel("Bill Category", 544, 16));
            cboBillCategoryFilter = MakeCombo(544, 40, 150);
            cboBillCategoryFilter.Items.AddRange(new object[] { "All", "WAPDA", "Internet", "Agriculture", "Other" });
            cboBillCategoryFilter.SelectedIndexChanged += FiltersChanged;
            filterPanel.Controls.Add(cboBillCategoryFilter);

            Button btnRefresh = MakeButton("Refresh", Color.White, Color.Black, 712, 38, 110);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Click += btnRefresh_Click;
            filterPanel.Controls.Add(btnRefresh);

            Button btnEdit = MakeButton("Edit Selected", Color.FromArgb(107, 44, 145), Color.White, 840, 38, 132);
            btnEdit.Click += btnEdit_Click;
            filterPanel.Controls.Add(btnEdit);

            filterPanel.Controls.Add(MakeLabel("Change Selected Status", 18, 78));
            cboNewStatus = MakeCombo(18, 102, 180);
            cboNewStatus.Items.AddRange(new object[] { "Pending", "Completed", "Cancelled" });
            filterPanel.Controls.Add(cboNewStatus);

            filterPanel.Controls.Add(MakeLabel("Status Notes", 216, 78));
            txtRemarks = MakeText(216, 102, 430);
            filterPanel.Controls.Add(txtRemarks);

            Button btnUpdateStatus = MakeButton("Update Status", Color.FromArgb(32, 129, 95), Color.White, 664, 100, 140);
            btnUpdateStatus.Click += btnUpdateStatus_Click;
            filterPanel.Controls.Add(btnUpdateStatus);

            dgvTransactions = new DataGridView();
            dgvTransactions.AllowUserToAddRows = false;
            dgvTransactions.AllowUserToDeleteRows = false;
            dgvTransactions.AutoGenerateColumns = false;
            dgvTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTransactions.BackgroundColor = Color.White;
            dgvTransactions.BorderStyle = BorderStyle.None;
            dgvTransactions.EnableHeadersVisualStyles = false;
            dgvTransactions.GridColor = Color.Gainsboro;
            dgvTransactions.Location = new Point(18, 258);
            dgvTransactions.MultiSelect = false;
            dgvTransactions.ReadOnly = true;
            dgvTransactions.RowHeadersVisible = false;
            dgvTransactions.RowTemplate.Height = 32;
            dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransactions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvTransactions.Size = new Size(1284, 544);
            Controls.Add(dgvTransactions);

            ConfigureGrid();

            Load += ServiceTransactionsForm_Load;
            ResumeLayout(false);
        }

        private void ServiceTransactionsForm_Load(object sender, EventArgs e)
        {
            cboStatusFilter.SelectedItem = "All";
            cboBillCategoryFilter.SelectedItem = "All";
            cboNewStatus.SelectedItem = "Pending";
            LoadTransactions();
        }

        private void FiltersChanged(object sender, EventArgs e)
        {
            LoadTransactions();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTransactions();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            ServiceTransactionRecord selected = GetSelectedTransaction();
            if (selected == null)
            {
                MessageBox.Show("Select a transaction first.", "Service Transactions", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (ServiceCenterForm form = new ServiceCenterForm(_session, selected.ServiceTransactionId))
            {
                form.ShowDialog(this);
            }

            LoadTransactions();
        }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceTransactionRecord selected = GetSelectedTransaction();
                if (selected == null)
                {
                    throw new InvalidOperationException("Select a transaction first.");
                }

                string newStatus = Convert.ToString(cboNewStatus.SelectedItem);
                _serviceService.UpdateTransactionStatus(selected.ServiceTransactionId, newStatus, txtRemarks.Text, _session.UserId);
                LoadTransactions();
                MessageBox.Show("Transaction status updated successfully.", "Service Transactions", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Status Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTransactions()
        {
            dgvTransactions.DataSource = null;
            dgvTransactions.DataSource = _serviceService.GetRecentTransactions(
                txtCustomerFilter.Text,
                Convert.ToString(cboStatusFilter.SelectedItem),
                Convert.ToString(cboBillCategoryFilter == null ? null : cboBillCategoryFilter.SelectedItem));
        }

        private ServiceTransactionRecord GetSelectedTransaction()
        {
            if (dgvTransactions.CurrentRow == null)
            {
                return null;
            }

            return dgvTransactions.CurrentRow.DataBoundItem as ServiceTransactionRecord;
        }

        private void ConfigureGrid()
        {
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = Color.FromArgb(243, 246, 251);
            headerStyle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            headerStyle.SelectionBackColor = Color.FromArgb(243, 246, 251);
            headerStyle.SelectionForeColor = Color.Black;
            dgvTransactions.ColumnHeadersDefaultCellStyle = headerStyle;
            dgvTransactions.ColumnHeadersHeight = 36;

            DataGridViewCellStyle rowStyle = new DataGridViewCellStyle();
            rowStyle.BackColor = Color.White;
            rowStyle.Font = new Font("Segoe UI", 9F);
            rowStyle.SelectionBackColor = Color.FromArgb(233, 240, 255);
            rowStyle.SelectionForeColor = Color.Black;
            dgvTransactions.DefaultCellStyle = rowStyle;

            dgvTransactions.Columns.Add(MakeColumn("TransactionNo", "Txn No", 80F, null));
            dgvTransactions.Columns.Add(MakeColumn("TransactionDate", "Date", 95F, "dd MMM yyyy hh:mm tt"));
            dgvTransactions.Columns.Add(MakeColumn("ServiceName", "Service", 100F, null));
            dgvTransactions.Columns.Add(MakeColumn("BillCategory", "Bill Type", 85F, null));
            dgvTransactions.Columns.Add(MakeColumn("CustomerName", "Customer", 120F, null));
            dgvTransactions.Columns.Add(MakeColumn("CustomerMobile", "Mobile", 90F, null));
            dgvTransactions.Columns.Add(MakeColumn("ReferenceNumber", "Reference", 105F, null));
            dgvTransactions.Columns.Add(MakeColumn("PaymentMethod", "Method", 80F, null));
            dgvTransactions.Columns.Add(MakeColumn("CustomerAccountNumber", "Account No", 110F, null));
            dgvTransactions.Columns.Add(MakeColumn("ExternalTransactionId", "Txn ID", 95F, null));
            dgvTransactions.Columns.Add(MakeColumn("Amount", "Amount", 70F, "N2"));
            dgvTransactions.Columns.Add(MakeColumn("Status", "Status", 80F, null));
            dgvTransactions.Columns.Add(MakeColumn("Remarks", "Remarks", 105F, null));
        }

        private static Label MakeLabel(string text, int x, int y)
        {
            return new Label { AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold), Location = new Point(x, y), Text = text };
        }

        private static TextBox MakeText(int x, int y, int w)
        {
            return new TextBox { Font = new Font("Segoe UI", 10F), Location = new Point(x, y), Size = new Size(w, 28) };
        }

        private static ComboBox MakeCombo(int x, int y, int w)
        {
            return new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10F), Location = new Point(x, y), Size = new Size(w, 30) };
        }

        private static Button MakeButton(string text, Color back, Color fore, int x, int y, int w)
        {
            return new Button { BackColor = back, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), ForeColor = fore, Location = new Point(x, y), Size = new Size(w, 36), Text = text };
        }

        private static DataGridViewTextBoxColumn MakeColumn(string propertyName, string headerText, float fillWeight, string format)
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
    }
}
