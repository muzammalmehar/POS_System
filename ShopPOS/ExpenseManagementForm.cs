using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ShopPOS.Models;
using ShopPOS.Services;

namespace ShopPOS
{
    public partial class ExpenseManagementForm : Form
    {
        private readonly UserSession _session;
        private readonly ExpenseService _expenseService;
        private readonly SalesService _salesService;
        private ComboBox cboExpenseType;
        private ComboBox cboWallet;
        private NumericUpDown nudAmount;
        private DateTimePicker dtpExpenseDate;
        private TextBox txtDescription;
        private DataGridView dgvExpenses;
        private Label lblTodayTotalValue;
        private Label lblRecentCountValue;
        private Label lblStatus;
        private List<ExpenseRecord> _recentExpenses;

        public ExpenseManagementForm(UserSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            _session = session;
            _expenseService = new ExpenseService();
            _salesService = new SalesService();
            _recentExpenses = new List<ExpenseRecord>();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1280, 800);
            MinimumSize = new Size(1298, 847);
            StartPosition = FormStartPosition.CenterParent;
            WindowState = FormWindowState.Maximized;
            Text = "Expense Management";

            Panel header = new Panel();
            header.BackColor = Color.FromArgb(181, 55, 55);
            header.Dock = DockStyle.Top;
            header.Height = 96;
            Controls.Add(header);

            Label lblTitle = new Label();
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(24, 18);
            lblTitle.Text = "Expense Management";
            header.Controls.Add(lblTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.WhiteSmoke;
            lblSubtitle.Location = new Point(28, 58);
            lblSubtitle.Text = "Record outgoing expenses clearly and keep recent spending easy to review.";
            header.Controls.Add(lblSubtitle);

            SplitContainer split = new SplitContainer();
            split.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            split.Location = new Point(20, 114);
            split.Size = new Size(1240, 666);
            split.SplitterDistance = 390;
            Controls.Add(split);

            Panel left = new Panel();
            left.Dock = DockStyle.Fill;
            left.BackColor = Color.White;
            left.Padding = new Padding(16);
            split.Panel1.Controls.Add(left);

            Label lblFormTitle = new Label();
            lblFormTitle.AutoSize = true;
            lblFormTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblFormTitle.Location = new Point(16, 14);
            lblFormTitle.Text = "New Expense Entry";
            left.Controls.Add(lblFormTitle);

            lblStatus = new Label();
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9.5F);
            lblStatus.ForeColor = Color.DimGray;
            lblStatus.Location = new Point(18, 42);
            lblStatus.Text = "Fill the details and save the expense.";
            left.Controls.Add(lblStatus);

            Panel summaryPanel = new Panel();
            summaryPanel.Location = new Point(18, 74);
            summaryPanel.Size = new Size(340, 96);
            left.Controls.Add(summaryPanel);

            lblTodayTotalValue = AddMiniCard(summaryPanel, "Today Total", 0, 0, 164, Color.FromArgb(181, 55, 55), "Rs. 0.00");
            lblRecentCountValue = AddMiniCard(summaryPanel, "Recent Entries", 176, 0, 164, Color.FromArgb(52, 73, 94), "0");

            GroupBox formGroup = new GroupBox();
            formGroup.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            formGroup.Text = "Expense Details";
            formGroup.Location = new Point(18, 184);
            formGroup.Size = new Size(340, 360);
            left.Controls.Add(formGroup);

            AddLabel(formGroup, "Expense Type", 16, 30);
            cboExpenseType = CreateComboBox(16, 56, 290);
            formGroup.Controls.Add(cboExpenseType);

            AddLabel(formGroup, "Wallet", 16, 96);
            cboWallet = CreateComboBox(16, 122, 290);
            formGroup.Controls.Add(cboWallet);

            AddLabel(formGroup, "Amount", 16, 162);
            nudAmount = CreateMoneyNumeric(16, 188, 160);
            formGroup.Controls.Add(nudAmount);

            AddLabel(formGroup, "Expense Date", 16, 228);
            dtpExpenseDate = new DateTimePicker();
            dtpExpenseDate.Font = new Font("Segoe UI", 10F);
            dtpExpenseDate.CustomFormat = "dd MMM yyyy hh:mm tt";
            dtpExpenseDate.Format = DateTimePickerFormat.Custom;
            dtpExpenseDate.Location = new Point(16, 254);
            dtpExpenseDate.Size = new Size(230, 30);
            formGroup.Controls.Add(dtpExpenseDate);

            AddLabel(formGroup, "Description", 16, 294);
            txtDescription = new TextBox();
            txtDescription.Font = new Font("Segoe UI", 10F);
            txtDescription.Location = new Point(16, 320);
            txtDescription.Multiline = true;
            txtDescription.Size = new Size(290, 24);
            formGroup.Controls.Add(txtDescription);

            Button btnSave = new Button();
            btnSave.BackColor = Color.FromArgb(181, 55, 55);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(18, 560);
            btnSave.Size = new Size(160, 40);
            btnSave.Text = "Save Expense";
            btnSave.Click += btnSave_Click;
            left.Controls.Add(btnSave);

            Button btnClear = new Button();
            btnClear.BackColor = Color.White;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnClear.Location = new Point(196, 560);
            btnClear.Size = new Size(120, 40);
            btnClear.Text = "Clear";
            btnClear.Click += btnClear_Click;
            left.Controls.Add(btnClear);

            Panel right = new Panel();
            right.Dock = DockStyle.Fill;
            right.BackColor = Color.White;
            right.Padding = new Padding(16);
            split.Panel2.Controls.Add(right);

            Label lblHistory = new Label();
            lblHistory.AutoSize = true;
            lblHistory.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblHistory.Location = new Point(16, 14);
            lblHistory.Text = "Recent Expense History";
            right.Controls.Add(lblHistory);

            Label lblHistorySub = new Label();
            lblHistorySub.AutoSize = true;
            lblHistorySub.Font = new Font("Segoe UI", 9.5F);
            lblHistorySub.ForeColor = Color.DimGray;
            lblHistorySub.Location = new Point(18, 42);
            lblHistorySub.Text = "Latest expense entries with payment source and operator details.";
            right.Controls.Add(lblHistorySub);

            GroupBox gridGroup = new GroupBox();
            gridGroup.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            gridGroup.Text = "Expense Register";
            gridGroup.Location = new Point(18, 74);
            gridGroup.Size = new Size(780, 560);
            gridGroup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            right.Controls.Add(gridGroup);

            dgvExpenses = new DataGridView();
            dgvExpenses.AllowUserToAddRows = false;
            dgvExpenses.AllowUserToDeleteRows = false;
            dgvExpenses.AutoGenerateColumns = false;
            dgvExpenses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvExpenses.BackgroundColor = Color.White;
            dgvExpenses.BorderStyle = BorderStyle.None;
            dgvExpenses.Dock = DockStyle.Fill;
            dgvExpenses.EnableHeadersVisualStyles = false;
            dgvExpenses.GridColor = Color.Gainsboro;
            dgvExpenses.ReadOnly = true;
            dgvExpenses.RowHeadersVisible = false;
            dgvExpenses.RowTemplate.Height = 30;
            dgvExpenses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridGroup.Controls.Add(dgvExpenses);
            ConfigureGrid();

            Load += ExpenseManagementForm_Load;
            ResumeLayout(false);
        }

        private void ExpenseManagementForm_Load(object sender, EventArgs e)
        {
            cboExpenseType.DataSource = _expenseService.GetExpenseTypes();
            cboExpenseType.DisplayMember = "Name";
            cboExpenseType.ValueMember = "Id";

            cboWallet.DataSource = _salesService.GetWalletAccounts();
            cboWallet.DisplayMember = "Name";
            cboWallet.ValueMember = "Id";

            dtpExpenseDate.Value = DateTime.Now;
            LoadExpenses();
        }

        private void LoadExpenses()
        {
            _recentExpenses = _expenseService.GetRecentExpenses();
            dgvExpenses.DataSource = null;
            dgvExpenses.DataSource = _recentExpenses;
            UpdateSummary();
        }

        private void UpdateSummary()
        {
            decimal todayTotal = 0;
            for (int i = 0; i < _recentExpenses.Count; i++)
            {
                if (_recentExpenses[i].ExpenseDate.Date == DateTime.Today)
                {
                    todayTotal += _recentExpenses[i].Amount;
                }
            }

            lblTodayTotalValue.Text = string.Format("Rs. {0:N2}", todayTotal);
            lblRecentCountValue.Text = _recentExpenses.Count.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            LookupOption expenseType = cboExpenseType.SelectedItem as LookupOption;
            LookupOption wallet = cboWallet.SelectedItem as LookupOption;
            if (expenseType == null || wallet == null)
            {
                return;
            }

            ExpenseSaveRequest request = new ExpenseSaveRequest();
            request.ExpenseTypeId = expenseType.Id;
            request.ExpenseDate = dtpExpenseDate.Value;
            request.Amount = nudAmount.Value;
            request.WalletAccountId = wallet.Id;
            request.Description = txtDescription.Text;
            request.UserId = _session.UserId;

            try
            {
                _expenseService.SaveExpense(request);
                MessageBox.Show("Expense saved successfully.", "Expense Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblStatus.Text = "Expense saved and wallet balance updated.";
                ResetForm();
                LoadExpenses();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Expense save failed.";
                MessageBox.Show(ex.Message, "Expense Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            if (cboExpenseType.Items.Count > 0) cboExpenseType.SelectedIndex = 0;
            if (cboWallet.Items.Count > 0) cboWallet.SelectedIndex = 0;
            nudAmount.Value = 0;
            dtpExpenseDate.Value = DateTime.Now;
            txtDescription.Clear();
            lblStatus.Text = "Fill the details and save the expense.";
        }

        private void ConfigureGrid()
        {
            ApplyGridStyle(dgvExpenses);
            dgvExpenses.Columns.Add(CreateColumn("ExpenseTypeName", "Type", 110F, null));
            dgvExpenses.Columns.Add(CreateColumn("ExpenseDate", "Date", 105F, "dd MMM yyyy hh:mm tt"));
            dgvExpenses.Columns.Add(CreateColumn("Amount", "Amount", 70F, "N2"));
            dgvExpenses.Columns.Add(CreateColumn("WalletName", "Wallet", 100F, null));
            dgvExpenses.Columns.Add(CreateColumn("Description", "Description", 170F, null));
            dgvExpenses.Columns.Add(CreateColumn("CreatedByName", "By", 90F, null));
        }

        private static Label AddMiniCard(Control parent, string title, int left, int top, int width, Color valueColor, string defaultValue)
        {
            Panel panel = new Panel();
            panel.BackColor = Color.FromArgb(248, 250, 253);
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Location = new Point(left, top);
            panel.Size = new Size(width, 86);
            parent.Controls.Add(panel);

            Label caption = new Label();
            caption.AutoSize = true;
            caption.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            caption.ForeColor = Color.DimGray;
            caption.Location = new Point(12, 12);
            caption.Text = title;
            panel.Controls.Add(caption);

            Label value = new Label();
            value.AutoSize = true;
            value.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            value.ForeColor = valueColor;
            value.Location = new Point(10, 36);
            value.Text = defaultValue;
            panel.Controls.Add(value);

            return value;
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

        private static DataGridViewTextBoxColumn CreateColumn(string propertyName, string headerText, float fillWeight, string format)
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

        private static void AddLabel(Control parent, string text, int left, int top)
        {
            Label label = new Label();
            label.AutoSize = true;
            label.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            label.Location = new Point(left, top);
            label.Text = text;
            parent.Controls.Add(label);
        }
    }
}
