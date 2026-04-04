using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using ShopPOS.Models;
using ShopPOS.Services;

namespace ShopPOS
{
    public partial class VendorPaymentForm : Form
    {
        private readonly UserSession _session;
        private readonly VendorPaymentService _paymentService;
        private readonly SalesService _salesService;
        private List<VendorDueItem> _vendors;
        private DataGridView dgvVendors;
        private ComboBox cboWallet;
        private NumericUpDown nudAmount;
        private DateTimePicker dtpPaymentDate;
        private CheckBox chkNextPayment;
        private DateTimePicker dtpNextPaymentDate;
        private TextBox txtNotes;
        private VendorPaymentReceipt _lastReceipt;
        private Button btnPreviewReceipt;
        private Button btnPrintReceipt;

        public VendorPaymentForm(UserSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            _session = session;
            _paymentService = new VendorPaymentService();
            _salesService = new SalesService();
            _vendors = new List<VendorDueItem>();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1180, 760);
            MinimumSize = new Size(1198, 807);
            StartPosition = FormStartPosition.CenterParent;
            WindowState = FormWindowState.Maximized;
            Text = "Vendor Payments";

            SplitContainer split = new SplitContainer();
            split.Dock = DockStyle.Fill;
            split.SplitterDistance = 720;
            Controls.Add(split);

            dgvVendors = new DataGridView();
            dgvVendors.AllowUserToAddRows = false;
            dgvVendors.AllowUserToDeleteRows = false;
            dgvVendors.AutoGenerateColumns = false;
            dgvVendors.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVendors.BackgroundColor = Color.White;
            dgvVendors.BorderStyle = BorderStyle.None;
            dgvVendors.Dock = DockStyle.Fill;
            dgvVendors.EnableHeadersVisualStyles = false;
            dgvVendors.GridColor = Color.Gainsboro;
            dgvVendors.ReadOnly = true;
            dgvVendors.RowHeadersVisible = false;
            dgvVendors.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            split.Panel1.Controls.Add(dgvVendors);
            ConfigureGrid();

            Panel form = new Panel();
            form.Dock = DockStyle.Fill;
            form.BackColor = Color.White;
            split.Panel2.Controls.Add(form);

            AddLabel(form, "Wallet", 18, 18);
            cboWallet = new ComboBox();
            cboWallet.DropDownStyle = ComboBoxStyle.DropDownList;
            cboWallet.Font = new Font("Segoe UI", 10F);
            cboWallet.Location = new Point(18, 44);
            cboWallet.Size = new Size(250, 31);
            form.Controls.Add(cboWallet);

            AddLabel(form, "Amount", 18, 88);
            nudAmount = new NumericUpDown();
            nudAmount.DecimalPlaces = 2;
            nudAmount.Maximum = 100000000;
            nudAmount.ThousandsSeparator = true;
            nudAmount.Font = new Font("Segoe UI", 10F);
            nudAmount.Location = new Point(18, 114);
            nudAmount.Size = new Size(160, 30);
            form.Controls.Add(nudAmount);

            AddLabel(form, "Payment Date", 18, 158);
            dtpPaymentDate = new DateTimePicker();
            dtpPaymentDate.Font = new Font("Segoe UI", 10F);
            dtpPaymentDate.Location = new Point(18, 184);
            dtpPaymentDate.Size = new Size(220, 30);
            form.Controls.Add(dtpPaymentDate);

            chkNextPayment = new CheckBox();
            chkNextPayment.AutoSize = true;
            chkNextPayment.Font = new Font("Segoe UI", 10F);
            chkNextPayment.Location = new Point(18, 232);
            chkNextPayment.Text = "Set next payment date";
            chkNextPayment.CheckedChanged += chkNextPayment_CheckedChanged;
            form.Controls.Add(chkNextPayment);

            dtpNextPaymentDate = new DateTimePicker();
            dtpNextPaymentDate.Font = new Font("Segoe UI", 10F);
            dtpNextPaymentDate.Location = new Point(18, 260);
            dtpNextPaymentDate.Size = new Size(220, 30);
            form.Controls.Add(dtpNextPaymentDate);

            AddLabel(form, "Notes", 18, 304);
            txtNotes = new TextBox();
            txtNotes.Font = new Font("Segoe UI", 10F);
            txtNotes.Location = new Point(18, 330);
            txtNotes.Multiline = true;
            txtNotes.Size = new Size(320, 90);
            form.Controls.Add(txtNotes);

            Button btnSave = new Button();
            btnSave.BackColor = Color.FromArgb(121, 84, 46);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(18, 438);
            btnSave.Size = new Size(160, 38);
            btnSave.Text = "Save Payment";
            btnSave.Click += btnSave_Click;
            form.Controls.Add(btnSave);

            btnPreviewReceipt = new Button();
            btnPreviewReceipt.BackColor = Color.White;
            btnPreviewReceipt.FlatStyle = FlatStyle.Flat;
            btnPreviewReceipt.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnPreviewReceipt.Location = new Point(188, 438);
            btnPreviewReceipt.Size = new Size(150, 38);
            btnPreviewReceipt.Text = "Preview Receipt";
            btnPreviewReceipt.Enabled = false;
            btnPreviewReceipt.Click += btnPreviewReceipt_Click;
            form.Controls.Add(btnPreviewReceipt);

            btnPrintReceipt = new Button();
            btnPrintReceipt.BackColor = Color.White;
            btnPrintReceipt.FlatStyle = FlatStyle.Flat;
            btnPrintReceipt.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnPrintReceipt.Location = new Point(18, 486);
            btnPrintReceipt.Size = new Size(150, 38);
            btnPrintReceipt.Text = "Print Receipt";
            btnPrintReceipt.Enabled = false;
            btnPrintReceipt.Click += btnPrintReceipt_Click;
            form.Controls.Add(btnPrintReceipt);

            Load += VendorPaymentForm_Load;
            ResumeLayout(false);
        }

        private void VendorPaymentForm_Load(object sender, EventArgs e)
        {
            cboWallet.DataSource = _salesService.GetWalletAccounts();
            cboWallet.DisplayMember = "Name";
            cboWallet.ValueMember = "Id";
            dtpNextPaymentDate.Enabled = false;
            LoadVendorDues();
        }

        private void LoadVendorDues()
        {
            _vendors = _paymentService.GetVendorDues();
            dgvVendors.DataSource = null;
            dgvVendors.DataSource = _vendors;
        }

        private void chkNextPayment_CheckedChanged(object sender, EventArgs e)
        {
            dtpNextPaymentDate.Enabled = chkNextPayment.Checked;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            VendorDueItem vendor = dgvVendors.CurrentRow == null ? null : dgvVendors.CurrentRow.DataBoundItem as VendorDueItem;
            if (vendor == null)
            {
                MessageBox.Show("Select a vendor first.", "Vendor Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LookupOption wallet = cboWallet.SelectedItem as LookupOption;
            if (wallet == null)
            {
                MessageBox.Show("Select a wallet.", "Vendor Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            VendorPaymentRequest request = new VendorPaymentRequest();
            request.SupplierId = vendor.SupplierId;
            request.WalletAccountId = wallet.Id;
            request.Amount = nudAmount.Value;
            request.PaymentDate = dtpPaymentDate.Value;
            request.Notes = txtNotes.Text;
            request.NextPaymentDate = chkNextPayment.Checked ? (DateTime?)dtpNextPaymentDate.Value.Date : null;
            request.UserId = _session.UserId;

            try
            {
                long paymentId = _paymentService.SaveVendorPayment(request);
                _lastReceipt = _paymentService.GetVendorPaymentReceipt(paymentId);
                btnPreviewReceipt.Enabled = true;
                btnPrintReceipt.Enabled = true;
                MessageBox.Show("Vendor payment saved successfully.", "Vendor Payment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                nudAmount.Value = 0;
                txtNotes.Clear();
                chkNextPayment.Checked = false;
                LoadVendorDues();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Vendor Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureGrid()
        {
            dgvVendors.Columns.Add(CreateColumn("SupplierName", "Vendor", 150F, null));
            dgvVendors.Columns.Add(CreateColumn("VisitDay", "Visit Day", 75F, null));
            dgvVendors.Columns.Add(CreateColumn("PaymentCycle", "Cycle", 80F, null));
            dgvVendors.Columns.Add(CreateColumn("NextPaymentDate", "Next Date", 85F, "dd MMM yyyy"));
            dgvVendors.Columns.Add(CreateColumn("OutstandingAmount", "Outstanding", 90F, "N2"));
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

        private static void AddLabel(Control parent, string text, int left, int top)
        {
            Label label = new Label();
            label.AutoSize = true;
            label.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            label.Location = new Point(left, top);
            label.Text = text;
            parent.Controls.Add(label);
        }

        private void btnPreviewReceipt_Click(object sender, EventArgs e)
        {
            if (_lastReceipt == null)
            {
                return;
            }

            PrintDocument document = CreateReceiptDocument();
            PrintPreviewDialog dialog = new PrintPreviewDialog();
            dialog.Document = document;
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
                e.HasMorePages = false;
                return;
            }

            Graphics g = e.Graphics;
            Rectangle pageBounds = e.MarginBounds;
            Color brand = Color.FromArgb(121, 84, 46);
            Color lightBrand = Color.FromArgb(244, 238, 231);
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
                g.DrawString("Vendor Payment Receipt", titleFont, Brushes.White, x + 28, y + 62);
                g.DrawString("A4 Payment Voucher", bodyFont, Brushes.WhiteSmoke, x + width - 190, y + 70);

                y += 126;

                Rectangle receiptInfoRect = new Rectangle(x, y, width, 72);
                g.FillRectangle(lightBrush, receiptInfoRect);
                g.DrawRectangle(borderPen, receiptInfoRect);
                DrawField(g, "Receipt No", _lastReceipt.ReceiptNo, x + 18, y + 14, 230, labelFont, bodyFont, Brushes.Black, mutedBrush);
                DrawField(g, "Payment Date", _lastReceipt.PaymentDate.ToString("dd MMM yyyy hh:mm tt"), x + 280, y + 14, 250, labelFont, bodyFont, Brushes.Black, mutedBrush);
                DrawField(g, "Printed On", DateTime.Now.ToString("dd MMM yyyy hh:mm tt"), x + width - 210, y + 14, 180, labelFont, bodyFont, Brushes.Black, mutedBrush);

                y += 94;

                Rectangle vendorRect = new Rectangle(x, y, width, 146);
                g.DrawRectangle(borderPen, vendorRect);
                g.DrawString("Vendor Details", sectionFont, brandBrush, x + 18, y + 16);
                DrawField(g, "Vendor Name", _lastReceipt.VendorName, x + 18, y + 48, 350, labelFont, bodyFont, Brushes.Black, mutedBrush);
                DrawField(g, "Wallet Used", _lastReceipt.WalletName, x + 390, y + 48, 250, labelFont, bodyFont, Brushes.Black, mutedBrush);
                DrawField(g, "Handled By", _lastReceipt.CreatedByName, x + 18, y + 92, 350, labelFont, bodyFont, Brushes.Black, mutedBrush);
                DrawField(g, "Remaining Payable", "Rs. " + _lastReceipt.RemainingBalance.ToString("N2"), x + 390, y + 92, 250, labelFont, bodyFont, Brushes.Black, mutedBrush);

                y += 164;

                Rectangle amountRect = new Rectangle(x, y, width, 112);
                g.FillRectangle(lightBrush, amountRect);
                g.DrawRectangle(borderPen, amountRect);
                g.DrawString("Amount Paid", sectionFont, brandBrush, x + 18, y + 18);
                g.DrawString("Rs. " + _lastReceipt.Amount.ToString("N2"), amountFont, brandBrush, x + 18, y + 42);
                g.DrawString("This receipt confirms a vendor credit payment has been recorded in the system.", bodyFont, mutedBrush, x + 360, y + 52);

                y += 130;

                Rectangle notesRect = new Rectangle(x, y, width, 120);
                g.DrawRectangle(borderPen, notesRect);
                g.DrawString("Notes", sectionFont, brandBrush, x + 18, y + 16);
                string notes = string.IsNullOrWhiteSpace(_lastReceipt.Notes) ? "No notes added for this payment." : _lastReceipt.Notes;
                g.DrawString(notes, bodyFont, Brushes.Black, new RectangleF(x + 18, y + 42, width - 36, 58));

                y += 150;

                int signatureWidth = 220;
                int signatureTop = y + 24;
                g.DrawLine(Pens.Gray, x + 20, signatureTop, x + 20 + signatureWidth, signatureTop);
                g.DrawString("Authorized By", footerFont, mutedBrush, x + 20, signatureTop + 8);

                g.DrawLine(Pens.Gray, x + width - 20 - signatureWidth, signatureTop, x + width - 20, signatureTop);
                g.DrawString("Received By", footerFont, mutedBrush, x + width - 20 - signatureWidth, signatureTop + 8);

                string footer = "Generated from " + ShopBranding.ShopName + " vendor payable system";
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
