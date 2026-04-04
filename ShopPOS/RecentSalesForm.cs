using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using ShopPOS.Models;
using ShopPOS.Services;

namespace ShopPOS
{
    public partial class RecentSalesForm : Form
    {
        private readonly UserSession _session;
        private readonly RecentSalesService _recentSalesService;
        private readonly SalesService _salesService;
        private readonly ServiceCenterService _serviceCenterService;

        private ComboBox cboSaleType;
        private ComboBox cboCustomer;
        private DataGridView dgvRecentSales;
        private Label lblStatus;
        private UnifiedRecentSaleItem _printItem;
        private List<UnifiedRecentSaleItem> _printItems;

        public RecentSalesForm(UserSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            _session = session;
            _recentSalesService = new RecentSalesService();
            _salesService = new SalesService();
            _serviceCenterService = new ServiceCenterService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1380, 840);
            MinimumSize = new Size(1398, 887);
            StartPosition = FormStartPosition.CenterParent;
            WindowState = FormWindowState.Maximized;
            Text = "Recent Sales";

            Panel header = new Panel();
            header.BackColor = Color.FromArgb(59, 89, 152);
            header.Dock = DockStyle.Top;
            header.Height = 90;
            Controls.Add(header);

            Label lblTitle = new Label();
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(24, 18);
            lblTitle.Text = "Recent Sales";
            header.Controls.Add(lblTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.WhiteSmoke;
            lblSubtitle.Location = new Point(28, 58);
            lblSubtitle.Text = "Review grocery and service sales together, filter by customer, edit entries, and process refunds.";
            header.Controls.Add(lblSubtitle);

            Panel filters = new Panel();
            filters.BackColor = Color.White;
            filters.Location = new Point(22, 108);
            filters.Size = new Size(1336, 78);
            filters.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Controls.Add(filters);

            filters.Controls.Add(MakeLabel("Sale Type", 16, 16));
            cboSaleType = MakeCombo(16, 40, 180);
            cboSaleType.Items.AddRange(new object[] { "All", "Grocery", "Service" });
            cboSaleType.SelectedIndexChanged += FilterChanged;
            filters.Controls.Add(cboSaleType);

            filters.Controls.Add(MakeLabel("Customer", 220, 16));
            cboCustomer = MakeCombo(220, 40, 280);
            cboCustomer.SelectedIndexChanged += FilterChanged;
            filters.Controls.Add(cboCustomer);

            Button btnRefresh = MakeButton("Refresh", Color.FromArgb(59, 89, 152), Color.White, 520, 38, 110);
            btnRefresh.Click += btnRefresh_Click;
            filters.Controls.Add(btnRefresh);

            Button btnEdit = MakeButton("Edit Selected", Color.FromArgb(24, 125, 68), Color.White, 648, 38, 130);
            btnEdit.Click += btnEdit_Click;
            filters.Controls.Add(btnEdit);

            Button btnRefund = MakeButton("Refund Selected", Color.FromArgb(214, 70, 74), Color.White, 792, 38, 148);
            btnRefund.Click += btnRefund_Click;
            filters.Controls.Add(btnRefund);

            Button btnPreview = MakeButton("Preview Record", Color.White, Color.Black, 954, 38, 136);
            btnPreview.FlatStyle = FlatStyle.Flat;
            btnPreview.Click += btnPreview_Click;
            filters.Controls.Add(btnPreview);

            Button btnPrint = MakeButton("Print Record", Color.White, Color.Black, 1100, 38, 120);
            btnPrint.FlatStyle = FlatStyle.Flat;
            btnPrint.Click += btnPrint_Click;
            filters.Controls.Add(btnPrint);

            lblStatus = new Label();
            lblStatus.AutoSize = false;
            lblStatus.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblStatus.ForeColor = Color.DimGray;
            lblStatus.Location = new Point(960, 10);
            lblStatus.Size = new Size(350, 22);
            lblStatus.TextAlign = ContentAlignment.MiddleRight;
            filters.Controls.Add(lblStatus);

            dgvRecentSales = new DataGridView();
            dgvRecentSales.AllowUserToAddRows = false;
            dgvRecentSales.AllowUserToDeleteRows = false;
            dgvRecentSales.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvRecentSales.AutoGenerateColumns = false;
            dgvRecentSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRecentSales.BackgroundColor = Color.White;
            dgvRecentSales.BorderStyle = BorderStyle.None;
            dgvRecentSales.EnableHeadersVisualStyles = false;
            dgvRecentSales.GridColor = Color.Gainsboro;
            dgvRecentSales.Location = new Point(22, 198);
            dgvRecentSales.MultiSelect = true;
            dgvRecentSales.ReadOnly = true;
            dgvRecentSales.RowHeadersVisible = false;
            dgvRecentSales.RowTemplate.Height = 30;
            dgvRecentSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRecentSales.Size = new Size(1336, 610);
            Controls.Add(dgvRecentSales);
            ConfigureGrid();

            Load += RecentSalesForm_Load;
            ResumeLayout(false);
        }

        private void RecentSalesForm_Load(object sender, EventArgs e)
        {
            cboSaleType.SelectedIndex = 0;
            cboCustomer.DataSource = _recentSalesService.GetCustomerFilters();
            cboCustomer.DisplayMember = "Name";
            cboCustomer.ValueMember = "Id";
            LoadRecentSales();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadRecentSales();
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            if (!IsHandleCreated)
            {
                return;
            }

            LoadRecentSales();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvRecentSales.SelectedRows.Count > 1)
            {
                MessageBox.Show("Select only one sale to edit.", "Recent Sales", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UnifiedRecentSaleItem selected = GetSelectedItem();
            if (selected == null)
            {
                MessageBox.Show("Select a sale first.", "Recent Sales", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selected.Status == "Refunded")
            {
                MessageBox.Show("Refunded sales cannot be edited.", "Recent Sales", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selected.SaleType == "Grocery")
            {
                using (SalesForm form = new SalesForm(_session, selected.RecordId))
                {
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadRecentSales();
                    }
                }
                return;
            }

            using (ServiceCenterForm form = new ServiceCenterForm(_session, selected.RecordId))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    LoadRecentSales();
                }
            }
        }

        private void btnRefund_Click(object sender, EventArgs e)
        {
            if (dgvRecentSales.SelectedRows.Count > 1)
            {
                MessageBox.Show("Select only one sale to refund.", "Recent Sales", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UnifiedRecentSaleItem selected = GetSelectedItem();
            if (selected == null)
            {
                MessageBox.Show("Select a sale first.", "Recent Sales", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selected.Status == "Refunded")
            {
                MessageBox.Show("This sale is already refunded.", "Recent Sales", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string refundRemarks = PromptForRemarks();
            if (refundRemarks == null)
            {
                return;
            }

            try
            {
                if (selected.SaleType == "Grocery")
                {
                    _salesService.RefundSale(selected.RecordId, _session.UserId, refundRemarks);
                }
                else
                {
                    _serviceCenterService.RefundServiceTransaction(selected.RecordId, _session.UserId, refundRemarks);
                }

                LoadRecentSales();
                MessageBox.Show("Refund processed successfully.", "Recent Sales", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Refund Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            List<UnifiedRecentSaleItem> selectedItems = GetSelectedItems();
            if (selectedItems.Count == 0)
            {
                MessageBox.Show("Select a sale first.", "Recent Sales", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _printItems = selectedItems;
            _printItem = selectedItems[0];
            PrintPreviewDialog dialog = new PrintPreviewDialog();
            dialog.Document = CreatePrintDocument();
            dialog.Width = 1000;
            dialog.Height = 760;
            dialog.ShowDialog(this);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            List<UnifiedRecentSaleItem> selectedItems = GetSelectedItems();
            if (selectedItems.Count == 0)
            {
                MessageBox.Show("Select a sale first.", "Recent Sales", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _printItems = selectedItems;
            _printItem = selectedItems[0];
            PrintDocument document = CreatePrintDocument();
            PrintDialog dialog = new PrintDialog();
            dialog.Document = document;
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                document.Print();
            }
        }

        private void LoadRecentSales()
        {
            string saleType = Convert.ToString(cboSaleType.SelectedItem);
            LookupOption customer = cboCustomer.SelectedItem as LookupOption;
            string customerName = customer == null ? null : customer.Name;
            List<UnifiedRecentSaleItem> items = _recentSalesService.GetRecentSales(saleType, customerName);
            dgvRecentSales.DataSource = null;
            dgvRecentSales.DataSource = items;
            lblStatus.Text = string.Format("{0} sale(s) loaded", items.Count);
        }

        private UnifiedRecentSaleItem GetSelectedItem()
        {
            if (dgvRecentSales.CurrentRow == null)
            {
                return null;
            }

            return dgvRecentSales.CurrentRow.DataBoundItem as UnifiedRecentSaleItem;
        }

        private string PromptForRemarks()
        {
            Form prompt = new Form();
            prompt.StartPosition = FormStartPosition.CenterParent;
            prompt.Size = new Size(460, 210);
            prompt.MinimumSize = new Size(478, 257);
            prompt.Text = "Refund Remarks";
            prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
            prompt.MaximizeBox = false;
            prompt.MinimizeBox = false;

            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.Location = new Point(18, 18);
            lbl.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lbl.Text = "Enter refund reason / remarks";
            prompt.Controls.Add(lbl);

            TextBox txt = new TextBox();
            txt.Location = new Point(22, 48);
            txt.Multiline = true;
            txt.Size = new Size(400, 64);
            txt.Font = new Font("Segoe UI", 10F);
            prompt.Controls.Add(txt);

            Button btnOk = MakeButton("Confirm", Color.FromArgb(24, 125, 68), Color.White, 232, 126, 90);
            btnOk.DialogResult = DialogResult.OK;
            prompt.Controls.Add(btnOk);

            Button btnCancel = MakeButton("Cancel", Color.White, Color.Black, 332, 126, 90);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.DialogResult = DialogResult.Cancel;
            prompt.Controls.Add(btnCancel);

            prompt.AcceptButton = btnOk;
            prompt.CancelButton = btnCancel;

            return prompt.ShowDialog(this) == DialogResult.OK ? txt.Text : null;
        }

        private PrintDocument CreatePrintDocument()
        {
            PrintDocument document = new PrintDocument();
            document.DefaultPageSettings.Margins = new Margins(45, 45, 45, 45);
            document.PrintPage += document_PrintPage;
            return document;
        }

        private void document_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (_printItems != null && _printItems.Count > 1)
            {
                DrawMultiPaymentReport(e);
                return;
            }

            if (_printItem == null)
            {
                return;
            }

            if (_printItem.SaleType == "Grocery")
            {
                DrawGroceryRecord(e);
                return;
            }

            DrawServiceRecord(e);
        }

        private void DrawGroceryRecord(PrintPageEventArgs e)
        {
            SaleEditRecord sale = _salesService.GetSaleForEdit(_printItem.RecordId);
            Graphics g = e.Graphics;
            Rectangle pageBounds = e.MarginBounds;
            Color brand = Color.FromArgb(24, 125, 68);
            Color soft = Color.FromArgb(232, 244, 240);
            Color muted = Color.FromArgb(110, 110, 110);
            Font shopFont = new Font("Segoe UI Semibold", 24F, FontStyle.Bold);
            Font titleFont = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            Font sectionFont = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            Font bodyFont = new Font("Segoe UI", 9.5F);
            Font disclaimerFont = new Font("Segoe UI", 8.75F, FontStyle.Italic);
            Font amountFont = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
            int x = pageBounds.Left;
            int y = pageBounds.Top;
            int width = pageBounds.Width;

            using (SolidBrush brandBrush = new SolidBrush(brand))
            using (SolidBrush softBrush = new SolidBrush(soft))
            using (SolidBrush mutedBrush = new SolidBrush(muted))
            using (Pen borderPen = new Pen(Color.FromArgb(210, 210, 210), 1.0F))
            {
                Rectangle header = new Rectangle(x, y, width, 102);
                g.FillRectangle(brandBrush, header);
                g.DrawString(ShopBranding.ShopName, shopFont, Brushes.White, x + 24, y + 16);
                g.DrawString("Grocery Sale Record", titleFont, Brushes.White, x + 28, y + 60);
                g.DrawString(_printItem.Status, bodyFont, Brushes.WhiteSmoke, x + width - 90, y + 66);
                y += 120;

                Rectangle info = new Rectangle(x, y, width, 110);
                g.FillRectangle(softBrush, info);
                g.DrawRectangle(borderPen, info);
                DrawField(g, "Document No", sale.SaleNo, x + 18, y + 14, 200, bodyFont, mutedBrush);
                DrawField(g, "Sale Date", sale.SaleDate.ToString("dd MMM yyyy hh:mm tt"), x + 250, y + 14, 250, bodyFont, mutedBrush);
                DrawField(g, "Customer", _printItem.CustomerName, x + 530, y + 14, 220, bodyFont, mutedBrush);
                DrawField(g, "Payment Method", sale.PaymentMethod, x + 18, y + 58, 180, bodyFont, mutedBrush);
                DrawField(g, "Wallet", _printItem.PaymentInfo, x + 220, y + 58, 220, bodyFont, mutedBrush);
                DrawField(g, "Cashier", _printItem.CashierName, x + 470, y + 58, 200, bodyFont, mutedBrush);
                y += 132;

                g.DrawString("Sale Items", sectionFont, brandBrush, x, y);
                y += 26;
                y = DrawHeader(g, x, y, width, new[] { "Code", "Product", "Qty", "Rate", "Total" });
                for (int i = 0; i < sale.Items.Count && i < 14; i++)
                {
                    SaleCartItem item = sale.Items[i];
                    y = DrawRow(g, x, y, width, new[]
                    {
                        item.ProductCode,
                        item.ProductName,
                        item.Quantity.ToString("N2"),
                        item.Rate.ToString("N2"),
                        item.LineTotal.ToString("N2")
                    }, bodyFont, borderPen);
                }

                decimal subtotal = 0;
                for (int i = 0; i < sale.Items.Count; i++)
                {
                    subtotal += sale.Items[i].LineTotal;
                }

                decimal grandTotal = subtotal - sale.Discount + sale.ExtraCharges;
                if (grandTotal < 0)
                {
                    grandTotal = 0;
                }

                y += 22;
                Rectangle totals = new Rectangle(x, y, width, 118);
                g.FillRectangle(softBrush, totals);
                g.DrawRectangle(borderPen, totals);
                g.DrawString("Grand Total", sectionFont, brandBrush, x + 18, y + 18);
                g.DrawString("Rs. " + grandTotal.ToString("N2"), amountFont, brandBrush, x + 18, y + 42);
                g.DrawString("Paid Amount: Rs. " + sale.PaidAmount.ToString("N2"), bodyFont, Brushes.Black, x + 360, y + 32);
                g.DrawString("Discount: Rs. " + sale.Discount.ToString("N2"), bodyFont, Brushes.Black, x + 360, y + 56);
                g.DrawString("Extra Charges: Rs. " + sale.ExtraCharges.ToString("N2"), bodyFont, Brushes.Black, x + 360, y + 80);
                y += 138;

                Rectangle remarks = new Rectangle(x, y, width, 90);
                g.DrawRectangle(borderPen, remarks);
                g.DrawString("Remarks", sectionFont, brandBrush, x + 18, y + 16);
                g.DrawString(string.IsNullOrWhiteSpace(sale.Remarks) ? "No remarks." : sale.Remarks, bodyFont, Brushes.Black, new RectangleF(x + 18, y + 42, width - 36, 36));
                y += 112;

                Rectangle disclaimer = new Rectangle(x, y, width, 64);
                g.FillRectangle(new SolidBrush(Color.FromArgb(249, 251, 250)), disclaimer);
                g.DrawRectangle(borderPen, disclaimer);
                g.DrawString(
                    "Disclaimer: This report is generated by " + GetPrintedByName() + " on the demand of customer.",
                    disclaimerFont,
                    mutedBrush,
                    new RectangleF(x + 16, y + 16, width - 32, 34));
            }
        }

        private void DrawServiceRecord(PrintPageEventArgs e)
        {
            ServiceTransactionRecord item = _serviceCenterService.GetTransactionRecord(_printItem.RecordId);
            Graphics g = e.Graphics;
            Rectangle pageBounds = e.MarginBounds;
            Color brand = Color.FromArgb(107, 44, 145);
            Color soft = Color.FromArgb(243, 236, 248);
            Color muted = Color.FromArgb(110, 110, 110);
            Font shopFont = new Font("Segoe UI Semibold", 24F, FontStyle.Bold);
            Font titleFont = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            Font sectionFont = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            Font bodyFont = new Font("Segoe UI", 9.5F);
            Font disclaimerFont = new Font("Segoe UI", 8.75F, FontStyle.Italic);
            Font amountFont = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
            int x = pageBounds.Left;
            int y = pageBounds.Top;
            int width = pageBounds.Width;

            using (SolidBrush brandBrush = new SolidBrush(brand))
            using (SolidBrush softBrush = new SolidBrush(soft))
            using (SolidBrush mutedBrush = new SolidBrush(muted))
            using (Pen borderPen = new Pen(Color.FromArgb(210, 210, 210), 1.0F))
            {
                Rectangle header = new Rectangle(x, y, width, 102);
                g.FillRectangle(brandBrush, header);
                g.DrawString(ShopBranding.ShopName, shopFont, Brushes.White, x + 24, y + 16);
                g.DrawString("Service Transaction Record", titleFont, Brushes.White, x + 28, y + 60);
                g.DrawString(item.Status, bodyFont, Brushes.WhiteSmoke, x + width - 90, y + 66);
                y += 120;

                Rectangle info = new Rectangle(x, y, width, 258);
                g.FillRectangle(softBrush, info);
                g.DrawRectangle(borderPen, info);
                DrawField(g, "Transaction No", item.TransactionNo, x + 18, y + 14, 220, bodyFont, mutedBrush);
                DrawField(g, "Date", item.TransactionDate.ToString("dd MMM yyyy hh:mm tt"), x + 260, y + 14, 250, bodyFont, mutedBrush);
                DrawField(g, "Customer", item.CustomerName, x + 530, y + 14, 220, bodyFont, mutedBrush);
                DrawField(g, "Service", item.ServiceName, x + 18, y + 58, 220, bodyFont, mutedBrush);
                DrawField(g, "Provider", item.ProviderName, x + 260, y + 58, 220, bodyFont, mutedBrush);
                DrawField(g, "Wallet", item.WalletName, x + 530, y + 58, 220, bodyFont, mutedBrush);
                DrawField(g, "Mobile", item.CustomerMobile, x + 18, y + 102, 220, bodyFont, mutedBrush);
                DrawField(g, "Reference No", item.ReferenceNumber, x + 260, y + 102, 220, bodyFont, mutedBrush);
                DrawField(g, "Cashier", item.CreatedByName, x + 530, y + 102, 220, bodyFont, mutedBrush);
                DrawField(g, "Payment Method", item.PaymentMethod, x + 18, y + 146, 220, bodyFont, mutedBrush);
                DrawField(g, "Account No", item.CustomerAccountNumber, x + 260, y + 146, 220, bodyFont, mutedBrush);
                DrawField(g, "Transaction ID", item.ExternalTransactionId, x + 530, y + 146, 220, bodyFont, mutedBrush);
                DrawField(g, "Bill Category", item.BillCategory, x + 18, y + 190, 220, bodyFont, mutedBrush);
                y += 278;

                Rectangle totals = new Rectangle(x, y, width, 118);
                g.FillRectangle(softBrush, totals);
                g.DrawRectangle(borderPen, totals);
                g.DrawString("Service Amount", sectionFont, brandBrush, x + 18, y + 18);
                g.DrawString("Rs. " + item.Amount.ToString("N2"), amountFont, brandBrush, x + 18, y + 42);
                g.DrawString("Recorded Status: " + item.Status, bodyFont, Brushes.Black, x + 360, y + 22);
                g.DrawString("Payment Method: " + (string.IsNullOrWhiteSpace(item.PaymentMethod) ? "-" : item.PaymentMethod), bodyFont, Brushes.Black, x + 360, y + 46);
                g.DrawString("Bill Category: " + (string.IsNullOrWhiteSpace(item.BillCategory) ? "-" : item.BillCategory), bodyFont, Brushes.Black, x + 360, y + 70);
                g.DrawString("Wallet: " + (string.IsNullOrWhiteSpace(item.WalletName) ? "-" : item.WalletName), bodyFont, Brushes.Black, x + 360, y + 94);
                y += 138;

                Rectangle remarks = new Rectangle(x, y, width, 100);
                g.DrawRectangle(borderPen, remarks);
                g.DrawString("Remarks", sectionFont, brandBrush, x + 18, y + 16);
                g.DrawString(string.IsNullOrWhiteSpace(item.Remarks) ? "No remarks." : item.Remarks, bodyFont, Brushes.Black, new RectangleF(x + 18, y + 42, width - 36, 44));
                y += 122;

                Rectangle disclaimer = new Rectangle(x, y, width, 64);
                g.FillRectangle(new SolidBrush(Color.FromArgb(250, 247, 252)), disclaimer);
                g.DrawRectangle(borderPen, disclaimer);
                g.DrawString(
                    "Disclaimer: This report is generated by " + GetPrintedByName() + " on the demand of customer.",
                    disclaimerFont,
                    mutedBrush,
                    new RectangleF(x + 16, y + 16, width - 32, 34));
            }
        }

        private void DrawMultiPaymentReport(PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle pageBounds = e.MarginBounds;
            Color brand = Color.FromArgb(59, 89, 152);
            Color muted = Color.FromArgb(110, 110, 110);
            Font shopFont = new Font("Segoe UI Semibold", 24F, FontStyle.Bold);
            Font titleFont = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            Font bodyFont = new Font("Segoe UI", 9.5F);
            Font footerFont = new Font("Segoe UI", 8.5F);
            int x = pageBounds.Left;
            int y = pageBounds.Top;
            int width = pageBounds.Width;

            using (SolidBrush brandBrush = new SolidBrush(brand))
            using (SolidBrush mutedBrush = new SolidBrush(muted))
            using (Pen borderPen = new Pen(Color.FromArgb(210, 210, 210), 1.0F))
            {
                Rectangle header = new Rectangle(x, y, width, 102);
                g.FillRectangle(brandBrush, header);
                g.DrawString(ShopBranding.ShopName, shopFont, Brushes.White, x + 24, y + 16);
                g.DrawString("Customer Payment / Sales Record Statement", titleFont, Brushes.White, x + 28, y + 60);
                g.DrawString(DateTime.Now.ToString("dd MMM yyyy hh:mm tt"), bodyFont, Brushes.WhiteSmoke, x + width - 170, y + 66);
                y += 120;

                string selectedCustomer = ResolveSelectedCustomerName();
                g.DrawString("Customer: " + selectedCustomer, titleFont, Brushes.Black, x, y);
                y += 30;
                g.DrawString("Selected Records: " + _printItems.Count, bodyFont, mutedBrush, x, y);
                y += 28;

                y = DrawHeader(g, x, y, width, new[] { "Type", "Doc No", "Date", "Customer", "Amount", "Status", "Payment" });
                decimal totalAmount = 0;

                for (int i = 0; i < _printItems.Count; i++)
                {
                    UnifiedRecentSaleItem item = _printItems[i];
                    totalAmount += item.GrossAmount;
                    y = DrawRow(g, x, y, width, new[]
                    {
                        item.SaleType,
                        item.DocumentNo,
                        item.TransactionDate.ToString("dd MMM yyyy"),
                        item.CustomerName,
                        item.GrossAmount.ToString("N2"),
                        item.Status,
                        item.PaymentInfo
                    }, bodyFont, borderPen);
                }

                y += 24;
                Rectangle totalRect = new Rectangle(x, y, width, 108);
                g.FillRectangle(new SolidBrush(Color.FromArgb(243, 246, 251)), totalRect);
                g.DrawRectangle(borderPen, totalRect);
                g.DrawString("Total Amount: Rs. " + totalAmount.ToString("N2"), titleFont, Brushes.Black, x + 18, y + 18);
                g.DrawString("Selected Records: " + _printItems.Count, bodyFont, Brushes.Black, x + 18, y + 48);
                g.DrawString(
                    "Disclaimer: This report is generated by " + GetPrintedByName() + " on the demand of customer.",
                    footerFont,
                    mutedBrush,
                    new RectangleF(x + 18, y + 72, width - 36, 24));

                string footer = "Generated from " + ShopBranding.ShopName + " recent sales records";
                SizeF footerSize = g.MeasureString(footer, footerFont);
                g.DrawString(footer, footerFont, mutedBrush, x + width - footerSize.Width, pageBounds.Bottom - 18);
            }
        }

        private static void DrawField(Graphics g, string label, string value, int x, int y, int width, Font bodyFont, Brush labelBrush)
        {
            using (Font labelFont = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold))
            {
                g.DrawString(label, labelFont, labelBrush, new RectangleF(x, y, width, 18));
            }
            g.DrawString(string.IsNullOrWhiteSpace(value) ? "-" : value, bodyFont, Brushes.Black, new RectangleF(x, y + 18, width, 22));
        }

        private static int DrawHeader(Graphics g, int x, int y, int width, string[] columns)
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

        private static int DrawRow(Graphics g, int x, int y, int width, string[] values, Font font, Pen borderPen)
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

        private string GetPrintedByName()
        {
            if (!string.IsNullOrWhiteSpace(_session.FullName))
            {
                return _session.FullName.Trim();
            }

            if (!string.IsNullOrWhiteSpace(_session.Username))
            {
                return _session.Username.Trim();
            }

            return "system user";
        }

        private List<UnifiedRecentSaleItem> GetSelectedItems()
        {
            List<UnifiedRecentSaleItem> items = new List<UnifiedRecentSaleItem>();
            for (int i = 0; i < dgvRecentSales.SelectedRows.Count; i++)
            {
                UnifiedRecentSaleItem item = dgvRecentSales.SelectedRows[i].DataBoundItem as UnifiedRecentSaleItem;
                if (item != null)
                {
                    items.Add(item);
                }
            }

            if (items.Count == 0)
            {
                UnifiedRecentSaleItem current = GetSelectedItem();
                if (current != null)
                {
                    items.Add(current);
                }
            }

            return items;
        }

        private string ResolveSelectedCustomerName()
        {
            if (_printItems == null || _printItems.Count == 0)
            {
                return "Mixed / Multiple";
            }

            string name = _printItems[0].CustomerName;
            for (int i = 1; i < _printItems.Count; i++)
            {
                if (!string.Equals(name, _printItems[i].CustomerName, StringComparison.OrdinalIgnoreCase))
                {
                    return "Mixed / Multiple";
                }
            }

            return string.IsNullOrWhiteSpace(name) ? "Mixed / Multiple" : name;
        }

        private void ConfigureGrid()
        {
            dgvRecentSales.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(243, 246, 251),
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                SelectionBackColor = Color.FromArgb(243, 246, 251),
                SelectionForeColor = Color.Black
            };
            dgvRecentSales.ColumnHeadersHeight = 36;
            dgvRecentSales.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                SelectionBackColor = Color.FromArgb(233, 240, 255),
                SelectionForeColor = Color.Black
            };

            dgvRecentSales.Columns.Add(MakeColumn("SaleType", "Type", 70F, null));
            dgvRecentSales.Columns.Add(MakeColumn("DocumentNo", "Document No", 95F, null));
            dgvRecentSales.Columns.Add(MakeColumn("TransactionDate", "Date", 110F, "dd MMM yyyy hh:mm tt"));
            dgvRecentSales.Columns.Add(MakeColumn("CustomerName", "Customer", 115F, null));
            dgvRecentSales.Columns.Add(MakeColumn("GrossAmount", "Amount", 80F, "N2"));
            dgvRecentSales.Columns.Add(MakeColumn("ProfitAmount", "Profit", 75F, "N2"));
            dgvRecentSales.Columns.Add(MakeColumn("PaymentInfo", "Payment / Wallet", 110F, null));
            dgvRecentSales.Columns.Add(MakeColumn("Status", "Status", 75F, null));
            dgvRecentSales.Columns.Add(MakeColumn("CashierName", "Cashier", 100F, null));
            dgvRecentSales.Columns.Add(MakeColumn("Remarks", "Remarks", 120F, null));
        }

        private static Label MakeLabel(string text, int x, int y)
        {
            return new Label { AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold), Location = new Point(x, y), Text = text };
        }

        private static ComboBox MakeCombo(int x, int y, int w)
        {
            return new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10F), Location = new Point(x, y), Size = new Size(w, 31) };
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
            column.ReadOnly = true;
            if (!string.IsNullOrWhiteSpace(format))
            {
                column.DefaultCellStyle.Format = format;
            }

            return column;
        }
    }
}
