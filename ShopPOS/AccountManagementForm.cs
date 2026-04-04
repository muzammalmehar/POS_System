using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ShopPOS.Models;
using ShopPOS.Services;

namespace ShopPOS
{
    /// <summary>
    /// Account Management dashboard — redesigned with a clean card-based layout.
    /// Displays business summary metrics, account balances, profit/loss, and recent vouchers.
    /// </summary>
    public partial class AccountManagementForm : Form
    {
        // ── Palette ──────────────────────────────────────────────────────
        private static readonly Color PageBg = Color.FromArgb(245, 247, 250);
        private static readonly Color SurfaceWhite = Color.White;
        private static readonly Color BorderLight = Color.FromArgb(225, 230, 238);
        private static readonly Color TextPrimary = Color.FromArgb(27, 38, 55);
        private static readonly Color TextMuted = Color.FromArgb(100, 112, 130);
        private static readonly Color GreenAccent = Color.FromArgb(22, 100, 74);
        private static readonly Color GreenBg = Color.FromArgb(232, 248, 241);
        private static readonly Color BlueAccent = Color.FromArgb(22, 80, 148);
        private static readonly Color BlueBg = Color.FromArgb(230, 241, 253);
        private static readonly Color RedAccent = Color.FromArgb(160, 48, 32);
        private static readonly Color RedBg = Color.FromArgb(253, 238, 234);
        private static readonly Color NavyAccent = Color.FromArgb(27, 66, 104);
        private static readonly Color NavyBg = Color.FromArgb(235, 241, 250);
        private static readonly Color HeaderBg = Color.FromArgb(244, 247, 251);
        private static readonly Color GridLine = Color.FromArgb(229, 233, 239);
        private static readonly Color SelectionBg = Color.FromArgb(229, 238, 255);

        private readonly AccountingService _accountingService;

        public AccountManagementForm()
        {
            _accountingService = new AccountingService();
            InitializeComponent();
            ApplyRuntimeStyles();
        }

        // ════════════════════════════════════════════════════════════════
        // LIFECYCLE
        // ════════════════════════════════════════════════════════════════

        private void AccountManagementForm_Load(object sender, EventArgs e)
        {
            EnsureGridConfiguration();

            if (IsInDesignMode())
                return;

            dtpFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpTo.Value = DateTime.Today;
            LoadData();
        }

        // ════════════════════════════════════════════════════════════════
        // RESIZE
        // ════════════════════════════════════════════════════════════════

        private void scrollHost_Resize(object sender, EventArgs e)
        {
            if (contentLayout == null || scrollHost == null)
                return;

            contentLayout.Width = Math.Max(980,
                scrollHost.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 4);
        }

        // ════════════════════════════════════════════════════════════════
        // TOOLBAR EVENTS
        // ════════════════════════════════════════════════════════════════

        private void btnToday_Click(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Today;
            dtpTo.Value = DateTime.Today;
            LoadData();
        }

        private void btnThisMonth_Click(object sender, EventArgs e)
        {
            dtpFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpTo.Value = DateTime.Today;
            LoadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        // ════════════════════════════════════════════════════════════════
        // DATA LOADING
        // ════════════════════════════════════════════════════════════════

        private void LoadData()
        {
            if (IsInDesignMode())
                return;

            EnsureGridConfiguration();

            DateTime fromDate = dtpFrom.Value.Date;
            DateTime toDate = dtpTo.Value.Date;

            // Auto-swap if dates are reversed
            if (fromDate > toDate)
            {
                DateTime swap = fromDate;
                fromDate = toDate;
                toDate = swap;
                dtpFrom.Value = fromDate;
                dtpTo.Value = toDate;
            }

            // Fetch all data
            List<AccountBalanceItem> balances = _accountingService.GetAccountBalances(fromDate, toDate);
            List<ProfitLossLineItem> profitLoss = _accountingService.GetProfitLoss(fromDate, toDate);
            List<LedgerVoucherItem> vouchers = _accountingService.GetRecentVouchers(fromDate, toDate);
            BusinessSummaryMetrics business = _accountingService.GetBusinessSummary(fromDate, toDate);

            // Bind grids
            dgvAccounts.DataSource = null; dgvAccounts.DataSource = balances;
            dgvProfitLoss.DataSource = null; dgvProfitLoss.DataSource = profitLoss;
            dgvVouchers.DataSource = null; dgvVouchers.DataSource = vouchers;

            // Compute P&L totals
            decimal totalIncome = GetSummaryAmount(profitLoss, "Total Income");
            decimal totalExpense = GetSummaryAmount(profitLoss, "Total Expense");
            decimal net = GetSummaryAmount(profitLoss, "Net Profit / Loss");

            // Period hint
            lblPeriodHint.Text = string.Format(
                "Showing figures from {0:dd MMM yyyy} to {1:dd MMM yyyy}. " +
                "Balances include opening before the start date and closing through the end date.",
                fromDate, toDate);

            // ── Metric cards ─────────────────────────────────────────────

            lblGrocerySalesValue.Text = FormatRs(business.GrocerySalesAmount);
            lblGrocerySalesMeta.Text = business.GroceryOrderCount > 0
                ? string.Format("{0} grocery orders in this period", business.GroceryOrderCount)
                : "No grocery orders recorded in this period";

            lblGroceryProfitValue.Text = FormatRs(business.GroceryProfitAmount);
            lblGroceryProfitMeta.Text = business.GrocerySalesAmount > 0
                ? string.Format("Gross margin {0:N1}%",
                    (business.GroceryProfitAmount / business.GrocerySalesAmount) * 100M)
                : "No grocery margin to calculate";

            lblServiceSalesValue.Text = FormatRs(business.ServiceSalesAmount);
            lblServiceSalesMeta.Text = business.ServiceSalesAmount > 0
                ? "Completed service transaction volume"
                : "No completed service transactions in this period";

            lblServiceProfitValue.Text = FormatRs(business.ServiceProfitAmount);
            lblServiceProfitMeta.Text = business.ServiceProfitAmount > 0
                ? "Commission and service income posted"
                : "No service earnings posted in this period";

            lblTotalExpenseValue.Text = FormatRs(totalExpense);
            lblTotalExpenseMeta.Text = totalExpense > 0
                ? "Includes cost of goods sold and expenses"
                : "No expense movement in this period";

            lblNetResultValue.Text = FormatRs(net);
            lblNetResultValue.ForeColor = net >= 0 ? GreenAccent : RedAccent;
            lblNetResultMeta.Text = string.Format(
                "Income {0}  |  Expense {1}", FormatRs(totalIncome), FormatRs(totalExpense));
        }

        // ════════════════════════════════════════════════════════════════
        // GRID CONFIGURATION
        // ════════════════════════════════════════════════════════════════

        private void EnsureGridConfiguration()
        {
            if (dgvAccounts != null && dgvAccounts.Columns.Count == 0) ConfigureAccountsGrid();
            if (dgvVouchers != null && dgvVouchers.Columns.Count == 0) ConfigureVouchersGrid();
            if (dgvProfitLoss != null && dgvProfitLoss.Columns.Count == 0) ConfigureProfitLossGrid();
        }

        private void ConfigureAccountsGrid()
        {
            StyleGrid(dgvAccounts);
            dgvAccounts.Columns.Add(MakeColumn("AccountName", "Account", 150F, null, DataGridViewContentAlignment.MiddleLeft));
            dgvAccounts.Columns.Add(MakeColumn("AccountType", "Type", 82F, null, DataGridViewContentAlignment.MiddleLeft));
            dgvAccounts.Columns.Add(MakeColumn("OpeningBalance", "Opening", 80F, "N2", DataGridViewContentAlignment.MiddleRight));
            dgvAccounts.Columns.Add(MakeColumn("PeriodDebit", "Debit", 72F, "N2", DataGridViewContentAlignment.MiddleRight));
            dgvAccounts.Columns.Add(MakeColumn("PeriodCredit", "Credit", 72F, "N2", DataGridViewContentAlignment.MiddleRight));
            dgvAccounts.Columns.Add(MakeColumn("ClosingBalance", "Closing", 80F, "N2", DataGridViewContentAlignment.MiddleRight));
            dgvAccounts.RowPrePaint += dgvAccounts_RowPrePaint;
        }

        private void ConfigureProfitLossGrid()
        {
            StyleGrid(dgvProfitLoss);
            dgvProfitLoss.Columns.Add(MakeColumn("Section", "Section", 78F, null, DataGridViewContentAlignment.MiddleLeft));
            dgvProfitLoss.Columns.Add(MakeColumn("AccountName", "Account", 160F, null, DataGridViewContentAlignment.MiddleLeft));
            dgvProfitLoss.Columns.Add(MakeColumn("Amount", "Amount", 72F, "N2", DataGridViewContentAlignment.MiddleRight));
            dgvProfitLoss.RowPrePaint += dgvProfitLoss_RowPrePaint;
        }

        private void ConfigureVouchersGrid()
        {
            StyleGrid(dgvVouchers);
            dgvVouchers.Columns.Add(MakeColumn("TransactionDate", "Date", 95F, "dd MMM yyyy hh:mm tt", DataGridViewContentAlignment.MiddleLeft));
            dgvVouchers.Columns.Add(MakeColumn("VoucherType", "Voucher", 72F, null, DataGridViewContentAlignment.MiddleLeft));
            dgvVouchers.Columns.Add(MakeColumn("ReferenceLabel", "Source", 118F, null, DataGridViewContentAlignment.MiddleLeft));
            dgvVouchers.Columns.Add(MakeColumn("TotalAmount", "Amount", 68F, "N2", DataGridViewContentAlignment.MiddleRight));
            dgvVouchers.Columns.Add(MakeColumn("Remarks", "Remarks", 125F, null, DataGridViewContentAlignment.MiddleLeft));
        }

        // ════════════════════════════════════════════════════════════════
        // ROW PAINT EVENTS
        // ════════════════════════════════════════════════════════════════

        private void dgvAccounts_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvAccounts.Rows.Count)
                return;

            AccountBalanceItem item = dgvAccounts.Rows[e.RowIndex].DataBoundItem as AccountBalanceItem;
            if (item == null) return;

            dgvAccounts.Rows[e.RowIndex].DefaultCellStyle.ForeColor =
                item.ClosingBalance < 0 ? RedAccent : TextPrimary;
        }

        private void dgvProfitLoss_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvProfitLoss.Rows.Count)
                return;

            ProfitLossLineItem item = dgvProfitLoss.Rows[e.RowIndex].DataBoundItem as ProfitLossLineItem;
            if (item == null) return;

            DataGridViewRow row = dgvProfitLoss.Rows[e.RowIndex];
            row.DefaultCellStyle.BackColor = SurfaceWhite;
            row.DefaultCellStyle.ForeColor = TextPrimary;
            row.DefaultCellStyle.Font = new Font("Segoe UI", 9F);

            switch (item.Section)
            {
                case "Income":
                    row.DefaultCellStyle.ForeColor = GreenAccent;
                    break;

                case "Expense":
                    row.DefaultCellStyle.ForeColor = RedAccent;
                    break;

                case "Summary":
                    row.DefaultCellStyle.BackColor = NavyBg;
                    row.DefaultCellStyle.ForeColor = (item.AccountName == "Net Profit / Loss" && item.Amount < 0)
                        ? RedAccent
                        : NavyAccent;
                    row.DefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
                    break;
            }
        }

        // ════════════════════════════════════════════════════════════════
        // RUNTIME STYLE POLISH
        // (hover effects, borders etc. that need live handles)
        // ════════════════════════════════════════════════════════════════

        private void ApplyRuntimeStyles()
        {
            // Hover states for toolbar buttons
            foreach (Control c in pnlToolbar.Controls)
            {
                Button btn = c as Button;
                if (btn != null && btn != btnRefresh)
                {
                    btn.MouseEnter += (s, e) => { btn.BackColor = Color.FromArgb(240, 244, 250); };
                    btn.MouseLeave += (s, e) => { btn.BackColor = SurfaceWhite; };
                }
            }

            // Refresh button hover
            btnRefresh.MouseEnter += (s, e) => { btnRefresh.BackColor = Color.FromArgb(18, 88, 66); };
            btnRefresh.MouseLeave += (s, e) => { btnRefresh.BackColor = GreenAccent; };

            // Section panel borders drawn via Paint
            foreach (Control c in pnlGrids.Controls)
            {
                Panel p = c as Panel;
                if (p != null)
                {
                    p.Paint += (s, pe) =>
                    {
                        using (Pen pen = new Pen(BorderLight, 1))
                            pe.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
                    };
                }
            }
        }

        // ════════════════════════════════════════════════════════════════
        // STATIC HELPERS
        // ════════════════════════════════════════════════════════════════

        private static void StyleGrid(DataGridView grid)
        {
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AutoGenerateColumns = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = SurfaceWhite;
            grid.BorderStyle = BorderStyle.None;
            grid.EnableHeadersVisualStyles = false;
            grid.GridColor = GridLine;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowTemplate.Height = 32;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = HeaderBg,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 63, 76),
                SelectionBackColor = HeaderBg,
                SelectionForeColor = Color.FromArgb(52, 63, 76)
            };
            grid.ColumnHeadersHeight = 38;

            grid.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = SurfaceWhite,
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextPrimary,
                SelectionBackColor = SelectionBg,
                SelectionForeColor = TextPrimary
            };

            grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(250, 252, 255),
                SelectionBackColor = SelectionBg,
                SelectionForeColor = TextPrimary
            };
        }

        private static DataGridViewTextBoxColumn MakeColumn(
            string propertyName, string headerText,
            float fillWeight, string format,
            DataGridViewContentAlignment alignment)
        {
            var col = new DataGridViewTextBoxColumn
            {
                DataPropertyName = propertyName,
                FillWeight = fillWeight,
                HeaderText = headerText,
                MinimumWidth = 68
            };

            if (!string.IsNullOrWhiteSpace(format))
                col.DefaultCellStyle.Format = format;

            col.DefaultCellStyle.Alignment = alignment;
            return col;
        }

        private static decimal GetSummaryAmount(List<ProfitLossLineItem> items, string accountName)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].AccountName == accountName)
                    return items[i].Amount;
            }
            return 0M;
        }

        private static string FormatRs(decimal amount)
        {
            return string.Format("Rs. {0:N2}", amount);
        }

        private bool IsInDesignMode()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode;
        }
    }
}
