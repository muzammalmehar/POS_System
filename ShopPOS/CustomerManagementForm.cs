using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using ShopPOS.Models;
using ShopPOS.Services;

namespace ShopPOS
{
    public partial class CustomerManagementForm : Form
    {
        private readonly UserSession _session;
        private readonly CustomerService _customerService;
        private readonly SalesService _salesService;
        private List<CustomerRecord> _customers;
        private int? _customerId;
        private TextBox txtSearch;
        private DataGridView dgvCustomers;
        private DataGridView dgvSalesHistory;
        private DataGridView dgvLedger;
        private TextBox txtName;
        private TextBox txtPhone;
        private TextBox txtAddress;
        private NumericUpDown nudOpeningBalance;
        private ComboBox cboBalanceType;
        private CheckBox chkIsActive;
        private Label lblNetBalance;
        private Label lblSaleDue;
        private ComboBox cboWallet;
        private NumericUpDown nudPaymentAmount;
        private DateTimePicker dtpPaymentDate;
        private TextBox txtPaymentRemarks;
        private Button btnPreviewReceipt;
        private Button btnPrintReceipt;
        private CustomerPaymentReceipt _lastReceipt;

        public CustomerManagementForm(UserSession session)
        {
            if (session == null) throw new ArgumentNullException("session");
            _session = session;
            _customerService = new CustomerService();
            _salesService = new SalesService();
            _customers = new List<CustomerRecord>();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1440, 860);
            MinimumSize = new Size(1458, 907);
            StartPosition = FormStartPosition.CenterParent;
            WindowState = FormWindowState.Maximized;
            Text = "Customer Management";

            Panel header = new Panel { BackColor = Color.FromArgb(24, 108, 83), Dock = DockStyle.Top, Height = 92 };
            Controls.Add(header);
            header.Controls.Add(new Label { AutoSize = true, Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold), ForeColor = Color.White, Location = new Point(22, 18), Text = "Customer Management" });
            header.Controls.Add(new Label { AutoSize = true, Font = new Font("Segoe UI", 10F), ForeColor = Color.WhiteSmoke, Location = new Point(26, 58), Text = "Track purchases, customer credit, payment recovery, and ledger history." });

            SplitContainer split = new SplitContainer { Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right, Location = new Point(18, 106), Size = new Size(1404, 716), SplitterDistance = 430 };
            Controls.Add(split);

            Panel left = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            split.Panel1.Controls.Add(left);
            left.Controls.Add(new Label { AutoSize = true, Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), Location = new Point(16, 14), Text = "Search Customer" });
            txtSearch = new TextBox { Font = new Font("Segoe UI", 11F), Location = new Point(20, 40), Size = new Size(380, 32) };
            txtSearch.TextChanged += txtSearch_TextChanged;
            left.Controls.Add(txtSearch);
            dgvCustomers = CreateGrid(new Point(20, 88), new Size(380, 590));
            dgvCustomers.SelectionChanged += dgvCustomers_SelectionChanged;
            dgvCustomers.CellClick += dgvCustomers_CellClick;
            left.Controls.Add(dgvCustomers);
            ConfigureCustomersGrid();

            Panel right = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, AutoScroll = true };
            split.Panel2.Controls.Add(right);
            right.Controls.Add(new Label { AutoSize = true, Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold), Location = new Point(18, 14), Text = "Customer Overview" });
            Button btnNew = MakeButton("New Customer", Color.White, Color.Black, 18, 52, 130); btnNew.FlatStyle = FlatStyle.Flat; btnNew.Click += btnNew_Click; right.Controls.Add(btnNew);
            Button btnSave = MakeButton("Edit Customer", Color.FromArgb(24, 108, 83), Color.White, 158, 52, 140); btnSave.Click += btnEdit_Click; right.Controls.Add(btnSave);

            int x1 = 18; int x2 = 330; int y = 108;
            right.Controls.Add(MakeLabel("Customer Name", x1, y)); txtName = MakeText(x1, y + 24, 280); right.Controls.Add(txtName);
            right.Controls.Add(MakeLabel("Phone", x2, y)); txtPhone = MakeText(x2, y + 24, 220); right.Controls.Add(txtPhone);
            y += 68;
            right.Controls.Add(MakeLabel("Address", x1, y)); txtAddress = MakeText(x1, y + 24, 532); right.Controls.Add(txtAddress);
            y += 68;
            right.Controls.Add(MakeLabel("Opening Balance", x1, y)); nudOpeningBalance = MakeMoney(x1, y + 24, 160); right.Controls.Add(nudOpeningBalance);
            right.Controls.Add(MakeLabel("Balance Type", 200, y)); cboBalanceType = MakeCombo(200, y + 24, 160); cboBalanceType.Items.AddRange(new object[] { "Receivable", "Payable" }); right.Controls.Add(cboBalanceType);
            chkIsActive = new CheckBox { AutoSize = true, Font = new Font("Segoe UI", 10F), Location = new Point(390, y + 28), Text = "Customer is active", Checked = true }; right.Controls.Add(chkIsActive);
            txtName.ReadOnly = true; txtName.BackColor = Color.FromArgb(248, 250, 253);
            txtPhone.ReadOnly = true; txtPhone.BackColor = Color.FromArgb(248, 250, 253);
            txtAddress.ReadOnly = true; txtAddress.BackColor = Color.FromArgb(248, 250, 253);
            nudOpeningBalance.Enabled = false;
            cboBalanceType.Enabled = false;
            chkIsActive.Enabled = false;
            y += 78;

            GroupBox summary = new GroupBox { Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), Text = "Credit Summary", Location = new Point(18, y), Size = new Size(560, 88) };
            summary.Controls.Add(new Label { AutoSize = true, Font = new Font("Segoe UI", 10F), Location = new Point(16, 30), Text = "Net Balance" });
            lblNetBalance = new Label { AutoSize = true, Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold), ForeColor = Color.Firebrick, Location = new Point(16, 50), Text = "Rs. 0.00" };
            summary.Controls.Add(lblNetBalance);
            summary.Controls.Add(new Label { AutoSize = true, Font = new Font("Segoe UI", 10F), Location = new Point(260, 30), Text = "Sale Due" });
            lblSaleDue = new Label { AutoSize = true, Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold), ForeColor = Color.FromArgb(24, 108, 83), Location = new Point(260, 50), Text = "Rs. 0.00" };
            summary.Controls.Add(lblSaleDue);
            right.Controls.Add(summary);
            y += 104;

            GroupBox payment = new GroupBox { Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), Text = "Receive Payment", Location = new Point(18, y), Size = new Size(700, 150) };
            payment.Controls.Add(MakeLabel("Wallet", 16, 30)); cboWallet = MakeCombo(16, 54, 220); payment.Controls.Add(cboWallet);
            payment.Controls.Add(MakeLabel("Amount", 256, 30)); nudPaymentAmount = MakeMoney(256, 54, 120); payment.Controls.Add(nudPaymentAmount);
            payment.Controls.Add(MakeLabel("Payment Date", 396, 30)); dtpPaymentDate = new DateTimePicker { Font = new Font("Segoe UI", 10F), CustomFormat = "dd MMM yyyy hh:mm tt", Format = DateTimePickerFormat.Custom, Location = new Point(396, 54), Size = new Size(220, 30) }; payment.Controls.Add(dtpPaymentDate);
            payment.Controls.Add(MakeLabel("Remarks", 16, 90)); txtPaymentRemarks = MakeText(16, 114, 470); payment.Controls.Add(txtPaymentRemarks);
            Button btnReceive = MakeButton("Receive Payment", Color.FromArgb(39, 110, 241), Color.White, 506, 110, 150); btnReceive.Click += btnReceive_Click; payment.Controls.Add(btnReceive);
            btnPreviewReceipt = MakeButton("Preview Receipt", Color.White, Color.Black, 506, 68, 150); btnPreviewReceipt.FlatStyle = FlatStyle.Flat; btnPreviewReceipt.Enabled = false; btnPreviewReceipt.Click += btnPreviewReceipt_Click; payment.Controls.Add(btnPreviewReceipt);
            btnPrintReceipt = MakeButton("Print Receipt", Color.White, Color.Black, 506, 26, 150); btnPrintReceipt.FlatStyle = FlatStyle.Flat; btnPrintReceipt.Enabled = false; btnPrintReceipt.Click += btnPrintReceipt_Click; payment.Controls.Add(btnPrintReceipt);
            right.Controls.Add(payment);
            y += 166;

            GroupBox salesHistory = new GroupBox { Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), Text = "Purchase History", Location = new Point(18, y), Size = new Size(900, 210) };
            dgvSalesHistory = CreateGrid(new Point(14, 28), new Size(870, 164));
            salesHistory.Controls.Add(dgvSalesHistory);
            right.Controls.Add(salesHistory);
            ConfigureSalesHistoryGrid();
            y += 226;

            GroupBox ledger = new GroupBox { Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), Text = "Payment + Ledger History", Location = new Point(18, y), Size = new Size(900, 270) };
            dgvLedger = CreateGrid(new Point(14, 28), new Size(870, 224));
            ledger.Controls.Add(dgvLedger);
            right.Controls.Add(ledger);
            ConfigureLedgerGrid();

            Load += CustomerManagementForm_Load;
            ResumeLayout(false);
        }

        private void CustomerManagementForm_Load(object sender, EventArgs e)
        {
            cboWallet.DataSource = _salesService.GetWalletAccounts();
            cboWallet.DisplayMember = "Name";
            LoadCustomers();
            ResetForm();
        }

        private void LoadCustomers()
        {
            _customers = _customerService.GetCustomers();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            List<CustomerRecord> filtered = new List<CustomerRecord>();
            string search = txtSearch.Text.Trim().ToLowerInvariant();
            for (int i = 0; i < _customers.Count; i++)
            {
                CustomerRecord item = _customers[i];
                string text = string.Format("{0} {1} {2}", item.CustomerName, item.Phone, item.Address).ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(search) || text.Contains(search))
                {
                    filtered.Add(item);
                }
            }

            dgvCustomers.DataSource = null;
            dgvCustomers.DataSource = filtered;
            dgvCustomers.ClearSelection();
            dgvCustomers.CurrentCell = null;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) { ApplyFilter(); }

        private void btnNew_Click(object sender, EventArgs e)
        {
            using (CustomerEntryForm form = new CustomerEntryForm())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    LoadCustomers();
                    SelectCustomerInGrid(form.SavedCustomerId);
                }
            }
        }

        private void dgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            CustomerRecord item = dgvCustomers.CurrentRow == null ? null : dgvCustomers.CurrentRow.DataBoundItem as CustomerRecord;
            if (item == null) return;

            LoadCustomerIntoForm(item);
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvCustomers.Rows.Count)
            {
                return;
            }

            CustomerRecord item = dgvCustomers.Rows[e.RowIndex].DataBoundItem as CustomerRecord;
            if (item == null)
            {
                return;
            }

            LoadCustomerIntoForm(item);
        }

        private void LoadCustomerIntoForm(CustomerRecord item)
        {
            _customerId = item.CustomerId;
            txtName.Text = item.CustomerName;
            txtPhone.Text = item.Phone;
            txtAddress.Text = item.Address;
            nudOpeningBalance.Value = item.OpeningBalance;
            cboBalanceType.SelectedItem = item.BalanceType;
            chkIsActive.Checked = item.IsActive;
            lblNetBalance.Text = string.Format("Rs. {0:N2} ({1})", Math.Abs(item.NetBalance), item.BalanceStatus);
            lblSaleDue.Text = string.Format("Rs. {0:N2}", item.SaleDueAmount);
            LoadSalesHistory(item.CustomerId);
            LoadLedger(item.CustomerId);
        }

        private void LoadSalesHistory(int customerId)
        {
            dgvSalesHistory.DataSource = null;
            dgvSalesHistory.DataSource = _customerService.GetCustomerSaleHistory(customerId);
        }

        private void LoadLedger(int customerId)
        {
            dgvLedger.DataSource = null;
            dgvLedger.DataSource = _customerService.GetCustomerLedger(customerId);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!_customerId.HasValue)
            {
                MessageBox.Show("Select a customer first.", "Customer Management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CustomerRecord selectedCustomer = GetSelectedCustomer();
            if (selectedCustomer == null)
            {
                return;
            }

            using (CustomerEntryForm form = new CustomerEntryForm(selectedCustomer))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    LoadCustomers();
                    SelectCustomerInGrid(form.SavedCustomerId);
                }
            }
        }

        private void btnReceive_Click(object sender, EventArgs e)
        {
            try
            {
                LookupOption wallet = cboWallet.SelectedItem as LookupOption;
                CustomerPaymentRequest request = new CustomerPaymentRequest();
                request.CustomerId = _customerId.GetValueOrDefault();
                request.WalletAccountId = wallet == null ? 0 : wallet.Id;
                request.Amount = nudPaymentAmount.Value;
                request.PaymentDate = dtpPaymentDate.Value;
                request.Remarks = txtPaymentRemarks.Text;
                request.UserId = _session.UserId;
                long paymentId = _customerService.SaveCustomerPayment(request);
                _lastReceipt = _customerService.GetCustomerPaymentReceipt(paymentId);
                btnPreviewReceipt.Enabled = true;
                btnPrintReceipt.Enabled = true;
                nudPaymentAmount.Value = 0;
                txtPaymentRemarks.Clear();
                LoadCustomers();
                if (_customerId.HasValue)
                {
                    LoadLedger(_customerId.Value);
                }
                MessageBox.Show("Customer payment received successfully.", "Customer Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Customer Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetForm()
        {
            _customerId = null;
            txtName.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            nudOpeningBalance.Value = 0;
            cboBalanceType.SelectedItem = "Receivable";
            chkIsActive.Checked = true;
            lblNetBalance.Text = "Rs. 0.00";
            lblSaleDue.Text = "Rs. 0.00";
            nudPaymentAmount.Value = 0;
            txtPaymentRemarks.Clear();
            dtpPaymentDate.Value = DateTime.Now;
            dgvSalesHistory.DataSource = null;
            dgvLedger.DataSource = null;
            dgvCustomers.ClearSelection();
            dgvCustomers.CurrentCell = null;
            if (cboWallet.Items.Count > 0) cboWallet.SelectedIndex = 0;
        }

        private CustomerRecord GetSelectedCustomer()
        {
            if (dgvCustomers.CurrentRow == null)
            {
                return null;
            }

            return dgvCustomers.CurrentRow.DataBoundItem as CustomerRecord;
        }

        private void SelectCustomerInGrid(int customerId)
        {
            for (int i = 0; i < dgvCustomers.Rows.Count; i++)
            {
                CustomerRecord item = dgvCustomers.Rows[i].DataBoundItem as CustomerRecord;
                if (item != null && item.CustomerId == customerId)
                {
                    dgvCustomers.ClearSelection();
                    dgvCustomers.Rows[i].Selected = true;
                    dgvCustomers.CurrentCell = dgvCustomers.Rows[i].Cells[0];
                    break;
                }
            }
        }

        private void ConfigureCustomersGrid()
        {
            StyleGrid(dgvCustomers);
            dgvCustomers.Columns.Add(MakeColumn("CustomerName", "Customer", 120F, null));
            dgvCustomers.Columns.Add(MakeColumn("Phone", "Phone", 90F, null));
            dgvCustomers.Columns.Add(MakeColumn("SaleDueAmount", "Sale Due", 80F, "N2"));
            dgvCustomers.Columns.Add(MakeColumn("NetBalance", "Net", 80F, "N2"));
            dgvCustomers.Columns.Add(MakeColumn("BalanceStatus", "Status", 70F, null));
        }

        private void ConfigureLedgerGrid()
        {
            StyleGrid(dgvLedger);
            dgvLedger.Columns.Add(MakeColumn("EntryDate", "Date", 100F, "dd MMM yyyy hh:mm tt"));
            dgvLedger.Columns.Add(MakeColumn("EntryType", "Type", 70F, null));
            dgvLedger.Columns.Add(MakeColumn("ReferenceNo", "Reference", 90F, null));
            dgvLedger.Columns.Add(MakeColumn("Debit", "Debit", 70F, "N2"));
            dgvLedger.Columns.Add(MakeColumn("Credit", "Credit", 70F, "N2"));
            dgvLedger.Columns.Add(MakeColumn("Remarks", "Remarks", 140F, null));
        }

        private void ConfigureSalesHistoryGrid()
        {
            StyleGrid(dgvSalesHistory);
            dgvSalesHistory.Columns.Add(MakeColumn("SaleNo", "Sale No", 80F, null));
            dgvSalesHistory.Columns.Add(MakeColumn("SaleDate", "Date", 100F, "dd MMM yyyy hh:mm tt"));
            dgvSalesHistory.Columns.Add(MakeColumn("GrandTotal", "Grand", 75F, "N2"));
            dgvSalesHistory.Columns.Add(MakeColumn("PaidAmount", "Paid", 70F, "N2"));
            dgvSalesHistory.Columns.Add(MakeColumn("DueAmount", "Due", 70F, "N2"));
            dgvSalesHistory.Columns.Add(MakeColumn("PaymentMethod", "Method", 70F, null));
            dgvSalesHistory.Columns.Add(MakeColumn("Remarks", "Remarks", 120F, null));
        }

        private static DataGridView CreateGrid(Point p, Size s)
        {
            return new DataGridView { AllowUserToAddRows = false, AllowUserToDeleteRows = false, AutoGenerateColumns = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, EnableHeadersVisualStyles = false, GridColor = Color.Gainsboro, Location = p, MultiSelect = false, ReadOnly = true, RowHeadersVisible = false, RowTemplate = { Height = 30 }, SelectionMode = DataGridViewSelectionMode.FullRowSelect, Size = s };
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

        private void btnPreviewReceipt_Click(object sender, EventArgs e)
        {
            if (_lastReceipt == null)
            {
                return;
            }

            PrintPreviewDialog dialog = new PrintPreviewDialog();
            dialog.Document = CreateReceiptDocument();
            dialog.Width = 900;
            dialog.Height = 700;
            dialog.ShowDialog(this);
        }

        private void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            if (_lastReceipt == null)
            {
                return;
            }

            PrintDocument document = CreateReceiptDocument();
            PrintDialog dialog = new PrintDialog();
            dialog.Document = document;
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                document.Print();
            }
        }

        private PrintDocument CreateReceiptDocument()
        {
            PrintDocument document = new PrintDocument();
            document.DefaultPageSettings.Margins = new Margins(45, 45, 45, 45);
            document.PrintPage += document_PrintPage;
            return document;
        }

        private void document_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (_lastReceipt == null)
            {
                return;
            }

            Graphics g = e.Graphics;
            Rectangle pageBounds = e.MarginBounds;
            Color brand = Color.FromArgb(24, 108, 83);
            Color lightBrand = Color.FromArgb(232, 244, 240);
            Color muted = Color.FromArgb(110, 110, 110);
            Font shopFont = new Font("Segoe UI Semibold", 26F, FontStyle.Bold);
            Font titleFont = new Font("Segoe UI Semibold", 15F, FontStyle.Bold);
            Font sectionFont = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            Font labelFont = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            Font bodyFont = new Font("Segoe UI", 10F);
            Font amountFont = new Font("Segoe UI Semibold", 24F, FontStyle.Bold);
            Font footerFont = new Font("Segoe UI", 8.5F);
            int x = pageBounds.Left;
            int y = pageBounds.Top;
            int width = pageBounds.Width;

            using (SolidBrush brandBrush = new SolidBrush(brand))
            using (SolidBrush lightBrush = new SolidBrush(lightBrand))
            using (SolidBrush mutedBrush = new SolidBrush(muted))
            using (Pen borderPen = new Pen(Color.FromArgb(210, 210, 210), 1.1F))
            {
                Rectangle headerRect = new Rectangle(x, y, width, 108);
                g.FillRectangle(brandBrush, headerRect);
                g.DrawString(ShopBranding.ShopName, shopFont, Brushes.White, x + 24, y + 18);
                g.DrawString("Customer Payment Receipt", titleFont, Brushes.White, x + 28, y + 62);
                g.DrawString("A4 Recovery Voucher", bodyFont, Brushes.WhiteSmoke, x + width - 180, y + 70);
                y += 126;

                Rectangle receiptInfoRect = new Rectangle(x, y, width, 72);
                g.FillRectangle(lightBrush, receiptInfoRect);
                g.DrawRectangle(borderPen, receiptInfoRect);
                DrawField(g, "Receipt No", _lastReceipt.ReceiptNo, x + 18, y + 14, 230, labelFont, bodyFont, Brushes.Black, mutedBrush);
                DrawField(g, "Payment Date", _lastReceipt.PaymentDate.ToString("dd MMM yyyy hh:mm tt"), x + 280, y + 14, 250, labelFont, bodyFont, Brushes.Black, mutedBrush);
                DrawField(g, "Printed On", DateTime.Now.ToString("dd MMM yyyy hh:mm tt"), x + width - 210, y + 14, 180, labelFont, bodyFont, Brushes.Black, mutedBrush);
                y += 94;

                Rectangle customerRect = new Rectangle(x, y, width, 146);
                g.DrawRectangle(borderPen, customerRect);
                g.DrawString("Customer Details", sectionFont, brandBrush, x + 18, y + 16);
                DrawField(g, "Customer Name", _lastReceipt.CustomerName, x + 18, y + 48, 350, labelFont, bodyFont, Brushes.Black, mutedBrush);
                DrawField(g, "Wallet Used", _lastReceipt.WalletName, x + 390, y + 48, 250, labelFont, bodyFont, Brushes.Black, mutedBrush);
                DrawField(g, "Handled By", _lastReceipt.CreatedByName, x + 18, y + 92, 350, labelFont, bodyFont, Brushes.Black, mutedBrush);
                DrawField(g, "Remaining Receivable", "Rs. " + _lastReceipt.RemainingReceivable.ToString("N2"), x + 390, y + 92, 250, labelFont, bodyFont, Brushes.Black, mutedBrush);
                y += 164;

                Rectangle amountRect = new Rectangle(x, y, width, 112);
                g.FillRectangle(lightBrush, amountRect);
                g.DrawRectangle(borderPen, amountRect);
                g.DrawString("Amount Received", sectionFont, brandBrush, x + 18, y + 18);
                g.DrawString("Rs. " + _lastReceipt.Amount.ToString("N2"), amountFont, brandBrush, x + 18, y + 42);
                g.DrawString("This receipt confirms a customer recovery/payment has been recorded in the system.", bodyFont, mutedBrush, x + 360, y + 52);
                y += 130;

                Rectangle notesRect = new Rectangle(x, y, width, 120);
                g.DrawRectangle(borderPen, notesRect);
                g.DrawString("Remarks", sectionFont, brandBrush, x + 18, y + 16);
                string notes = string.IsNullOrWhiteSpace(_lastReceipt.Remarks) ? "No remarks added for this receipt." : _lastReceipt.Remarks;
                g.DrawString(notes, bodyFont, Brushes.Black, new RectangleF(x + 18, y + 42, width - 36, 58));

                y += 150;
                int signatureWidth = 220;
                int signatureTop = y + 24;
                g.DrawLine(Pens.Gray, x + 20, signatureTop, x + 20 + signatureWidth, signatureTop);
                g.DrawString("Authorized By", footerFont, mutedBrush, x + 20, signatureTop + 8);
                g.DrawLine(Pens.Gray, x + width - 20 - signatureWidth, signatureTop, x + width - 20, signatureTop);
                g.DrawString("Received By", footerFont, mutedBrush, x + width - 20 - signatureWidth, signatureTop + 8);
                string footer = "Generated from " + ShopBranding.ShopName + " customer receivable system";
                SizeF footerSize = g.MeasureString(footer, footerFont);
                g.DrawString(footer, footerFont, mutedBrush, x + width - footerSize.Width, pageBounds.Bottom - 22);
            }
        }

        private static void DrawField(Graphics g, string label, string value, int x, int y, int width, Font labelFont, Font bodyFont, Brush valueBrush, Brush labelBrush)
        {
            g.DrawString(label, labelFont, labelBrush, new RectangleF(x, y, width, 18));
            g.DrawString(string.IsNullOrWhiteSpace(value) ? "-" : value, bodyFont, valueBrush, new RectangleF(x, y + 18, width, 22));
        }
    }
}
