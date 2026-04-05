using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ShopPOS.Models;
using ShopPOS.Services;

namespace ShopPOS
{
    public partial class MainForm : Form
    {
        private readonly UserSession _session;
        private readonly DashboardService _dashboardService;
        private bool _dashboardGridEventsAttached;

        public MainForm()
        {
            _session = new UserSession
            {
                UserId = 0,
                FullName = "Designer User",
                Username = "designer",
                RoleName = "Administrator"
            };
            _dashboardService = new DashboardService();
            InitializeComponent();
            EnsureDashboardGridConfiguration();
        }

        public MainForm(UserSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            _session = session;
            _dashboardService = new DashboardService();
            InitializeComponent();
            EnsureDashboardGridConfiguration();
        }

        private Control CreateHeaderPanel()
        {
            Panel header = new Panel
            {
                BackColor = Color.FromArgb(27, 107, 83),
                Dock = DockStyle.Top,
                Height = 122
            };

            TableLayoutPanel headerLayout = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                Padding = new Padding(24, 18, 24, 18),
                RowCount = 1
            };
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64F));
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36F));
            header.Controls.Add(headerLayout);

            TableLayoutPanel titleLayout = new TableLayoutPanel
            {
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                RowCount = 3
            };
            titleLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            titleLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            titleLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            headerLayout.Controls.Add(titleLayout, 0, 0);

            lblWelcome = new Label
            {
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 21F, FontStyle.Bold),
                ForeColor = Color.White,
                Margin = new Padding(0),
                Text = ShopBranding.ShopName,
                TextAlign = ContentAlignment.MiddleLeft
            };
            titleLayout.Controls.Add(lblWelcome, 0, 0);

            lblRole = new Label
            {
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.WhiteSmoke,
                Margin = new Padding(0),
                Text = "Central dashboard for grocery sales, services, stock, vendors, and accounts.",
                TextAlign = ContentAlignment.MiddleLeft
            };
            titleLayout.Controls.Add(lblRole, 0, 1);

            lblUsername = new Label
            {
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.WhiteSmoke,
                Margin = new Padding(0),
                Text = "Signed in user",
                TextAlign = ContentAlignment.MiddleLeft
            };
            titleLayout.Controls.Add(lblUsername, 0, 2);

            TableLayoutPanel actionLayout = new TableLayoutPanel
            {
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                RowCount = 2
            };
            actionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            actionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            headerLayout.Controls.Add(actionLayout, 1, 0);

            FlowLayoutPanel actions = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Margin = new Padding(0),
                WrapContents = false
            };
            actionLayout.Controls.Add(actions, 0, 0);

            btnLogout = CreateTopButton("Logout", Color.FromArgb(214, 70, 74), btnLogout_Click);
            actions.Controls.Add(btnLogout);

            btnRefresh = CreateTopButton("Refresh", Color.FromArgb(47, 128, 237), btnRefresh_Click);
            actions.Controls.Add(btnRefresh);

            btnNewSale = CreateTopButton("New Sale", Color.FromArgb(24, 125, 68), btnNewSale_Click);
            actions.Controls.Add(btnNewSale);

            Label lblActionHint = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gainsboro,
                Margin = new Padding(0),
                Text = "Quick controls for daily cashier work",
                TextAlign = ContentAlignment.MiddleRight
            };
            actionLayout.Controls.Add(lblActionHint, 0, 1);

            return header;
        }

        private Control CreateScrollHost()
        {
            scrollHost = new Panel
            {
                AutoScroll = true,
                BackColor = Color.FromArgb(241, 244, 249),
                Dock = DockStyle.Fill
            };
            scrollHost.Resize += scrollHost_Resize;

            dashboardCanvas = CreateDashboardCanvas();
            scrollHost.Controls.Add(dashboardCanvas);
            return scrollHost;
        }

        private Panel CreateDashboardCanvas()
        {
            Panel canvas = new Panel
            {
                BackColor = Color.Transparent,
                Location = new Point(24, 18),
                Size = new Size(1270, 1108)
            };
            canvas.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            TableLayoutPanel layout = new TableLayoutPanel
            {
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                RowCount = 5
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 226F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 310F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 404F));
            canvas.Controls.Add(layout);

            Panel overviewPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0)
            };
            layout.Controls.Add(overviewPanel, 0, 0);

            Label lblOverview = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(38, 48, 61),
                Location = new Point(0, 0),
                Text = "Today Overview"
            };
            overviewPanel.Controls.Add(lblOverview);

            Label lblOverviewHint = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(107, 120, 132),
                Location = new Point(0, 24),
                Text = "Important today figures with credit, cash, services, and inventory attention."
            };
            overviewPanel.Controls.Add(lblOverviewHint);

            Panel metricsSection = CreateContentCard(210, new Padding(12));
            metricsSection.Dock = DockStyle.Fill;
            metricsSection.Margin = new Padding(0, 0, 0, 18);
            metricsSection.Controls.Add(CreateMetricsGrid());
            layout.Controls.Add(metricsSection, 0, 1);

            Panel statusPanel = CreateStatusPanel();
            statusPanel.Dock = DockStyle.Fill;
            statusPanel.Margin = new Padding(0, 0, 0, 18);
            layout.Controls.Add(statusPanel, 0, 2);

            Panel actionsSection = CreateContentCard(276, new Padding(16, 12, 16, 16));
            actionsSection.Dock = DockStyle.Fill;
            actionsSection.Margin = new Padding(0, 0, 0, 18);
            TableLayoutPanel actionsLayout = new TableLayoutPanel
            {
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                RowCount = 2
            };
            actionsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            actionsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            actionsSection.Controls.Add(actionsLayout);

            Panel actionsHeader = CreateSectionHeader(
                "Quick Actions",
                "Open the main working screens directly from here.");
            actionsLayout.Controls.Add(actionsHeader, 0, 0);

            TableLayoutPanel actionsHost = new TableLayoutPanel
            {
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 0, 0, 0),
                RowCount = 1
            };
            TableLayoutPanel actionsGrid = CreateQuickActionsGrid();
            actionsGrid.Dock = DockStyle.Fill;
            actionsHost.Controls.Add(actionsGrid, 0, 0);
            actionsLayout.Controls.Add(actionsHost, 0, 1);
            layout.Controls.Add(actionsSection, 0, 3);

            Panel tablesSection = CreateContentCard(360, new Padding(14));
            tablesSection.Dock = DockStyle.Fill;
            tablesSection.Margin = new Padding(0);
            TableLayoutPanel tablesLayout = new TableLayoutPanel
            {
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                RowCount = 2
            };
            tablesLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tablesLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tablesSection.Controls.Add(tablesLayout);

            Panel tablesHeader = CreateSectionHeader(
                "Operational Snapshot",
                "Low stock and recent grocery invoices in one place.");
            tablesLayout.Controls.Add(tablesHeader, 0, 0);

            Panel tablesHost = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0)
            };
            TableLayoutPanel tablesGrid = CreateOperationalTables();
            tablesGrid.Dock = DockStyle.Fill;
            tablesHost.Controls.Add(tablesGrid);
            tablesLayout.Controls.Add(tablesHost, 0, 1);
            layout.Controls.Add(tablesSection, 0, 4);

            return canvas;
        }

        private TableLayoutPanel CreateMetricsGrid()
        {
            TableLayoutPanel metricTable = new TableLayoutPanel
            {
                ColumnCount = 4,
                Dock = DockStyle.Fill,
                Padding = new Padding(2, 2, 2, 4),
                RowCount = 2
            };
            metricTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            metricTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            metricTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            metricTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            metricTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            metricTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            metricTable.Controls.Add(CreateSalesBreakdownCard(), 0, 0);
            metricTable.Controls.Add(CreateMetricCard("Today Grocery Profit", Color.FromArgb(39, 110, 241), "Rs. 0.00", "Gross profit from sold grocery items today.", false, out lblTodaySalesProfitValue), 1, 0);
            metricTable.Controls.Add(CreateMetricCard("Today Service Sales", Color.FromArgb(129, 84, 201), "Rs. 0.00", "Completed service volume recorded today.", false, out lblTodayServiceSalesValue), 2, 0);
            metricTable.Controls.Add(CreateMetricCard("Today Service Profit", Color.FromArgb(163, 74, 42), "Rs. 0.00", "Commission earned from services today.", false, out lblTodayServiceProfitValue), 3, 0);
            metricTable.Controls.Add(CreateMetricCard("Today Expenses", Color.FromArgb(214, 70, 74), "Rs. 0.00", "Expenses booked during today.", false, out lblTodayExpensesValue), 0, 1);
            metricTable.Controls.Add(CreateMetricCard("Low Stock Alerts", Color.FromArgb(223, 132, 34), "0", "Products at or below reorder level.", true, out lblLowStockValue), 1, 1);
            metricTable.Controls.Add(CreateExpiryAttentionCard(), 2, 1);
            metricTable.Controls.Add(CreateMetricCard("Today Orders", Color.FromArgb(35, 139, 117), "0", "Total grocery sale invoices today.", true, out lblTodayOrdersValue), 3, 1);

            return metricTable;
        }

        private Panel CreateSalesBreakdownCard()
        {
            Panel panel = CreateMetricShell();
            TableLayoutPanel layout = new TableLayoutPanel
            {
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(16, 14, 16, 12),
                RowCount = 4
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            panel.Controls.Add(layout);

            layout.Controls.Add(CreateCardCaption("Today Grocery Sales"), 0, 0);

            lblTodaySalesValue = CreateCardValue(Color.FromArgb(24, 125, 68), false);
            lblTodaySalesValue.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            layout.Controls.Add(lblTodaySalesValue, 0, 1);

            layout.Controls.Add(CreateInlineBreakdown("Credit Sales", Color.FromArgb(171, 72, 122), out lblTodayCreditSalesValue), 0, 2);
            layout.Controls.Add(CreateInlineBreakdown("Cash Sales", Color.FromArgb(24, 125, 68), out lblTodayCashSalesValue), 0, 3);

            return panel;
        }

        private Panel CreateExpiryAttentionCard()
        {
            Panel panel = CreateMetricShell();
            TableLayoutPanel layout = new TableLayoutPanel
            {
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(16, 14, 16, 12),
                RowCount = 3
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panel.Controls.Add(layout);

            layout.Controls.Add(CreateCardCaption("Expiry Attention"), 0, 0);
            lblExpiryAttentionValue = CreateCardValue(Color.FromArgb(142, 68, 33), true);
            layout.Controls.Add(lblExpiryAttentionValue, 0, 1);

            lblExpiryAttentionMeta = new Label
            {
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 8.75F),
                ForeColor = Color.FromArgb(110, 120, 131),
                Margin = new Padding(0, 4, 0, 0),
                TextAlign = ContentAlignment.TopLeft,
                Text = "No expiry alerts right now."
            };
            layout.Controls.Add(lblExpiryAttentionMeta, 0, 2);

            return panel;
        }

        private Panel CreateMetricCard(string title, Color accentColor, string initialValue, string detailText, bool compactValue, out Label valueLabel)
        {
            Panel panel = CreateMetricShell();
            TableLayoutPanel layout = new TableLayoutPanel
            {
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(16, 14, 16, 12),
                RowCount = 3
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, compactValue ? 38F : 40F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panel.Controls.Add(layout);

            layout.Controls.Add(CreateCardCaption(title), 0, 0);

            valueLabel = CreateCardValue(accentColor, compactValue);
            valueLabel.Text = initialValue;
            layout.Controls.Add(valueLabel, 0, 1);

            Label detail = new Label
            {
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 8.75F),
                ForeColor = Color.FromArgb(110, 120, 131),
                Margin = new Padding(0, 4, 0, 0),
                TextAlign = ContentAlignment.TopLeft,
                Text = detailText
            };
            layout.Controls.Add(detail, 0, 2);

            return panel;
        }

        private TableLayoutPanel CreateQuickActionsGrid()
        {
            TableLayoutPanel actionsGrid = new TableLayoutPanel
            {
                ColumnCount = 4,
                Dock = DockStyle.Fill,
                RowCount = 4
            };

            for (int i = 0; i < actionsGrid.ColumnCount; i++)
            {
                actionsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            }

            actionsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            actionsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            actionsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            actionsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));

            int index = 0;
            AddActionButton(actionsGrid, ref index, "Sales Screen", Color.FromArgb(24, 125, 68), btnNewSale_Click, out btnNewSalePrimary);
            AddActionButton(actionsGrid, ref index, "Recent Sales", Color.FromArgb(59, 89, 152), btnRecentSalesPrimary_Click, out btnRecentSalesPrimary);
            AddActionButton(actionsGrid, ref index, "Customers", Color.FromArgb(24, 108, 83), btnCustomerPrimary_Click, out btnCustomerPrimary);
            AddActionButton(actionsGrid, ref index, "Service Center", Color.FromArgb(107, 44, 145), btnServicePrimary_Click, out btnServicePrimary);
            AddActionButton(actionsGrid, ref index, "Service History", Color.FromArgb(86, 36, 117), btnServiceTransactionsPrimary_Click, out btnServiceTransactionsPrimary);
            AddActionButton(actionsGrid, ref index, "Stock", Color.FromArgb(92, 43, 130), btnStockPrimary_Click, out btnStockPrimary);
            AddActionButton(actionsGrid, ref index, "Products", Color.FromArgb(196, 106, 33), btnProductPrimary_Click, out btnProductPrimary);
            AddActionButton(actionsGrid, ref index, "Purchases", Color.FromArgb(145, 96, 42), btnPurchasePrimary_Click, out btnPurchasePrimary);
            AddActionButton(actionsGrid, ref index, "Vendors", Color.FromArgb(121, 84, 46), btnVendorPrimary_Click, out btnVendorPrimary);
            AddActionButton(actionsGrid, ref index, "Vendor Payments", Color.FromArgb(91, 72, 56), btnVendorPaymentPrimary_Click, out btnVendorPaymentPrimary);
            AddActionButton(actionsGrid, ref index, "Expenses", Color.FromArgb(181, 55, 55), btnExpensePrimary_Click, out btnExpensePrimary);
            AddActionButton(actionsGrid, ref index, "Manage Expiry", Color.FromArgb(142, 68, 33), btnExpiryPrimary_Click, out btnExpiryPrimary);
            AddActionButton(actionsGrid, ref index, "Accounts && P&L", Color.FromArgb(52, 73, 94), btnAccountsPrimary_Click, out btnAccountsPrimary);

            return actionsGrid;
        }

        private TableLayoutPanel CreateOperationalTables()
        {
            TableLayoutPanel split = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                RowCount = 1
            };
            split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            split.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            dgvLowStock = CreateDashboardGrid();
            ConfigureLowStockGrid();
            Panel lowStockSection = CreateGridSection(
                "Low Stock Products",
                "Products at or below reorder level right now.",
                dgvLowStock);
            lowStockSection.Margin = new Padding(0, 0, 10, 0);
            split.Controls.Add(lowStockSection, 0, 0);

            dgvRecentSales = CreateDashboardGrid();
            ConfigureRecentSalesGrid();
            Panel recentSalesSection = CreateGridSection(
                "Recent Sales & Services",
                "Latest grocery and completed service entries in the system.",
                dgvRecentSales);
            recentSalesSection.Margin = new Padding(10, 0, 0, 0);
            split.Controls.Add(recentSalesSection, 1, 0);

            return split;
        }

        private Panel CreateStatusPanel()
        {
            Panel panel = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Height = 76,
                Margin = new Padding(0, 0, 0, 16),
                Dock = DockStyle.Fill
            };

            TableLayoutPanel textLayout = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                Padding = new Padding(22, 10, 18, 10),
                RowCount = 2
            };
            textLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68F));
            textLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            textLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            textLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));

            Panel accent = new Panel
            {
                BackColor = Color.FromArgb(47, 128, 237),
                Dock = DockStyle.Left,
                Width = 8
            };
            panel.Controls.Add(accent);
            panel.Controls.Add(textLayout);

            lblDashboardStatus = new Label
            {
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 52, 61),
                Margin = new Padding(0),
                Text = "Dashboard ready.",
                TextAlign = ContentAlignment.MiddleLeft
            };
            textLayout.Controls.Add(lblDashboardStatus, 0, 0);
            textLayout.SetColumnSpan(lblDashboardStatus, 1);

            Label secondaryNote = new Label
            {
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(109, 118, 128),
                Margin = new Padding(0),
                Text = "Use the quick actions below to jump into sales, stock, vendors, services, or accounts.",
                TextAlign = ContentAlignment.MiddleLeft
            };
            textLayout.Controls.Add(secondaryNote, 0, 1);

            lblLastUpdated = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(109, 118, 128),
                Margin = new Padding(0),
                TextAlign = ContentAlignment.MiddleRight,
                Text = "Last updated: --"
            };
            textLayout.Controls.Add(lblLastUpdated, 1, 0);
            textLayout.SetRowSpan(lblLastUpdated, 2);

            return panel;
        }

        private Panel CreateContentCard(int height, Padding padding)
        {
            return new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Height = height,
                Margin = new Padding(0, 0, 0, 16),
                Dock = DockStyle.Fill,
                Padding = padding
            };
        }

        private Panel CreateMetricShell()
        {
            return new Panel
            {
                BackColor = Color.FromArgb(251, 252, 254),
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                Margin = new Padding(6),
                Padding = new Padding(0)
            };
        }

        private Panel CreateSectionHeader(string title, string subtitle)
        {
            Panel header = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0)
            };

            Label titleLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 56, 71),
                Location = new Point(2, 0),
                Text = title
            };
            header.Controls.Add(titleLabel);

            Label subtitleLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(107, 120, 132),
                Location = new Point(2, 21),
                Text = subtitle
            };
            header.Controls.Add(subtitleLabel);

            return header;
        }

        private Panel CreateGridSection(string title, string subtitle, Control body)
        {
            Panel section = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill
            };

            Panel header = new Panel
            {
                BackColor = Color.White,
                Dock = DockStyle.Top,
                Height = 60
            };
            section.Controls.Add(header);

            Label titleLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 56, 71),
                Location = new Point(14, 8),
                Text = title
            };
            header.Controls.Add(titleLabel);

            Label subtitleLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(107, 120, 132),
                Location = new Point(14, 31),
                Text = subtitle
            };
            header.Controls.Add(subtitleLabel);

            Panel bodyHost = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12, 6, 12, 12)
            };
            body.Dock = DockStyle.Fill;
            bodyHost.Controls.Add(body);
            section.Controls.Add(bodyHost);

            return section;
        }

        private Label CreateCardCaption(string text)
        {
            return new Label
            {
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.DimGray,
                Margin = new Padding(0),
                Text = text,
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private Label CreateCardValue(Color accentColor, bool compact)
        {
            return new Label
            {
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                Font = compact
                    ? new Font("Segoe UI Semibold", 23F, FontStyle.Bold)
                    : new Font("Segoe UI Semibold", 18F, FontStyle.Bold),
                ForeColor = accentColor,
                Margin = new Padding(0),
                TextAlign = ContentAlignment.MiddleLeft,
                Text = compact ? "0" : "Rs. 0.00"
            };
        }

        private Control CreateBreakdownSummary(string caption, Color accentColor, out Label valueLabel)
        {
            TableLayoutPanel block = new TableLayoutPanel
            {
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                RowCount = 2
            };
            block.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));
            block.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Label captionLabel = new Label
            {
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold),
                ForeColor = Color.DimGray,
                Margin = new Padding(0),
                Text = caption,
                TextAlign = ContentAlignment.BottomLeft
            };
            block.Controls.Add(captionLabel, 0, 0);

            valueLabel = new Label
            {
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = accentColor,
                Margin = new Padding(0),
                Text = "Rs. 0.00",
                TextAlign = ContentAlignment.TopLeft
            };
            block.Controls.Add(valueLabel, 0, 1);

            return block;
        }

        private Control CreateInlineBreakdown(string caption, Color accentColor, out Label valueLabel)
        {
            TableLayoutPanel line = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                RowCount = 1
            };
            line.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
            line.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
            line.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Label captionLabel = new Label
            {
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 8.75F, FontStyle.Bold),
                ForeColor = Color.DimGray,
                Margin = new Padding(0),
                Text = caption,
                TextAlign = ContentAlignment.MiddleLeft
            };
            line.Controls.Add(captionLabel, 0, 0);

            valueLabel = new Label
            {
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 8.75F),
                ForeColor = accentColor,
                Margin = new Padding(0),
                Text = "Rs. 0.00",
                TextAlign = ContentAlignment.MiddleLeft
            };
            line.Controls.Add(valueLabel, 1, 0);

            return line;
        }

        private static Label CreateBreakdownLabel(string text)
        {
            return new Label
            {
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 8.75F, FontStyle.Bold),
                ForeColor = Color.DimGray,
                Margin = new Padding(0),
                Text = text,
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private Button CreateTopButton(string text, Color backColor, EventHandler clickHandler)
        {
            Button button = new Button
            {
                BackColor = backColor,
                FlatAppearance = { BorderSize = 0 },
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Height = 38,
                Margin = new Padding(12, 0, 0, 0),
                Padding = new Padding(16, 0, 16, 0),
                Text = text,
                UseVisualStyleBackColor = false,
                Width = text == "Logout" ? 106 : 104
            };
            button.Click += clickHandler;
            return button;
        }

        private void AddActionButton(TableLayoutPanel grid, ref int index, string text, Color backColor, EventHandler clickHandler, out Button button)
        {
            button = new Button
            {
                BackColor = backColor,
                Dock = DockStyle.Fill,
                FlatAppearance = { BorderSize = 0 },
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Margin = new Padding(8),
                Text = text,
                UseVisualStyleBackColor = false
            };
            button.Click += clickHandler;
            grid.Controls.Add(button, index % grid.ColumnCount, index / grid.ColumnCount);
            index++;
        }

        private DataGridView CreateDashboardGrid()
        {
            DataGridView grid = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill,
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(229, 233, 239),
                MultiSelect = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                RowTemplate = { Height = 30 },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            ApplyGridStyle(grid);
            return grid;
        }

        private void ConfigureLowStockGrid()
        {
            ApplyGridStyle(dgvLowStock);
            dgvLowStock.Columns.Clear();
            dgvLowStock.Columns.Add(CreateTextColumn("ProductCode", "Code", 76F, null, DataGridViewContentAlignment.MiddleLeft, 88));
            dgvLowStock.Columns.Add(CreateTextColumn("ProductName", "Product", 150F, null, DataGridViewContentAlignment.MiddleLeft, 160));
            dgvLowStock.Columns.Add(CreateTextColumn("CurrentStock", "Current", 64F, "N2", DataGridViewContentAlignment.MiddleRight, 82));
            dgvLowStock.Columns.Add(CreateTextColumn("ReorderLevel", "Reorder", 64F, "N2", DataGridViewContentAlignment.MiddleRight, 82));
            dgvLowStock.Columns.Add(CreateTextColumn("Status", "Status", 78F, null, DataGridViewContentAlignment.MiddleCenter, 92));
        }

        private void ConfigureRecentSalesGrid()
        {
            ApplyGridStyle(dgvRecentSales);
            dgvRecentSales.Columns.Clear();
            dgvRecentSales.Columns.Add(CreateTextColumn("EntryType", "Type", 60F, null, DataGridViewContentAlignment.MiddleCenter, 82));
            dgvRecentSales.Columns.Add(CreateTextColumn("SaleNo", "Ref No", 80F, null, DataGridViewContentAlignment.MiddleLeft, 94));
            dgvRecentSales.Columns.Add(CreateTextColumn("SaleDate", "Date", 130F, "dd MMM yyyy hh:mm tt", DataGridViewContentAlignment.MiddleLeft, 156));
            dgvRecentSales.Columns.Add(CreateTextColumn("GrandTotal", "Amount", 74F, "N2", DataGridViewContentAlignment.MiddleRight, 88));
            dgvRecentSales.Columns.Add(CreateTextColumn("PaymentMethod", "Payment", 70F, null, DataGridViewContentAlignment.MiddleCenter, 84));
            dgvRecentSales.Columns.Add(CreateTextColumn("Cashier", "Cashier", 86F, null, DataGridViewContentAlignment.MiddleLeft, 98));
        }

        private void EnsureDashboardGridConfiguration()
        {
            if (dgvLowStock != null && dgvLowStock.Columns.Count == 0)
            {
                ConfigureLowStockGrid();
            }

            if (dgvRecentSales != null && dgvRecentSales.Columns.Count == 0)
            {
                ConfigureRecentSalesGrid();
            }

            AttachDashboardGridEvents();
            FitDashboardGridColumns();
        }

        private void AttachDashboardGridEvents()
        {
            if (_dashboardGridEventsAttached)
            {
                return;
            }

            if (dgvLowStock != null)
            {
                dgvLowStock.Resize += DashboardGrid_Resize;
            }

            if (dgvRecentSales != null)
            {
                dgvRecentSales.Resize += DashboardGrid_Resize;
            }

            _dashboardGridEventsAttached = true;
        }

        private void DashboardGrid_Resize(object sender, EventArgs e)
        {
            FitDashboardGridColumns();
        }

        private void FitDashboardGridColumns()
        {
            FitLowStockGridColumns();
            FitRecentSalesGridColumns();
        }

        private void FitLowStockGridColumns()
        {
            if (dgvLowStock == null || dgvLowStock.Columns.Count < 5)
            {
                return;
            }

            int availableWidth = dgvLowStock.ClientSize.Width - 8;
            if (availableWidth <= 0)
            {
                return;
            }

            int codeWidth = Math.Max(80, (int)(availableWidth * 0.16M));
            int productWidth = Math.Max(180, (int)(availableWidth * 0.34M));
            int currentWidth = Math.Max(78, (int)(availableWidth * 0.14M));
            int reorderWidth = Math.Max(78, (int)(availableWidth * 0.14M));
            int statusWidth = Math.Max(96, availableWidth - codeWidth - productWidth - currentWidth - reorderWidth - 4);

            dgvLowStock.Columns[0].Width = codeWidth;
            dgvLowStock.Columns[1].Width = productWidth;
            dgvLowStock.Columns[2].Width = currentWidth;
            dgvLowStock.Columns[3].Width = reorderWidth;
            dgvLowStock.Columns[4].Width = statusWidth;
        }

        private void FitRecentSalesGridColumns()
        {
            if (dgvRecentSales == null || dgvRecentSales.Columns.Count < 6)
            {
                return;
            }

            int availableWidth = dgvRecentSales.ClientSize.Width - 8;
            if (availableWidth <= 0)
            {
                return;
            }

            int typeWidth = Math.Max(82, (int)(availableWidth * 0.14M));
            int saleNoWidth = Math.Max(94, (int)(availableWidth * 0.17M));
            int dateWidth = Math.Max(156, (int)(availableWidth * 0.27M));
            int amountWidth = Math.Max(88, (int)(availableWidth * 0.14M));
            int paymentWidth = Math.Max(84, (int)(availableWidth * 0.14M));
            int cashierWidth = Math.Max(98, availableWidth - typeWidth - saleNoWidth - dateWidth - amountWidth - paymentWidth - 5);

            dgvRecentSales.Columns[0].Width = typeWidth;
            dgvRecentSales.Columns[1].Width = saleNoWidth;
            dgvRecentSales.Columns[2].Width = dateWidth;
            dgvRecentSales.Columns[3].Width = amountWidth;
            dgvRecentSales.Columns[4].Width = paymentWidth;
            dgvRecentSales.Columns[5].Width = cashierWidth;
        }

        private static DataGridViewTextBoxColumn CreateTextColumn(
            string propertyName,
            string headerText,
            float fillWeight,
            string format,
            DataGridViewContentAlignment alignment = DataGridViewContentAlignment.MiddleLeft,
            int minimumWidth = 70)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn
            {
                DataPropertyName = propertyName,
                HeaderText = headerText,
                FillWeight = fillWeight,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                MinimumWidth = minimumWidth,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            if (!string.IsNullOrWhiteSpace(format))
            {
                column.DefaultCellStyle.Format = format;
            }

            column.DefaultCellStyle.Alignment = alignment;

            return column;
        }

        private static void ApplyGridStyle(DataGridView grid)
        {
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.EnableHeadersVisualStyles = false;
            grid.GridColor = Color.FromArgb(229, 233, 239);
            grid.RowHeadersVisible = false;
            grid.RowTemplate.Height = 34;
            grid.AllowUserToResizeRows = false;
            grid.AllowUserToResizeColumns = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.ScrollBars = ScrollBars.Vertical;
            grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.FromArgb(230, 241, 252),
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(42, 54, 68),
                Padding = new Padding(8, 0, 8, 0),
                SelectionBackColor = Color.FromArgb(230, 241, 252),
                SelectionForeColor = Color.FromArgb(42, 54, 68),
                WrapMode = DataGridViewTriState.True
            };
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersHeight = 38;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(45, 52, 61),
                Padding = new Padding(6, 0, 6, 0),
                SelectionBackColor = Color.FromArgb(233, 240, 255),
                SelectionForeColor = Color.Black
            };
            grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(249, 251, 254),
                ForeColor = Color.FromArgb(45, 52, 61),
                SelectionBackColor = Color.FromArgb(233, 240, 255),
                SelectionForeColor = Color.Black
            };
            grid.RowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(45, 52, 61),
                SelectionBackColor = Color.FromArgb(233, 240, 255),
                SelectionForeColor = Color.Black
            };
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            EnsureDashboardGridConfiguration();

            lblWelcome.Text = ShopBranding.ShopName;
            lblRole.Text = string.IsNullOrWhiteSpace(_session.RoleName)
                ? "Central dashboard for grocery sales, services, stock, vendors, and accounts."
                : string.Format(
                    "Role: {0} | Central dashboard for grocery sales, services, stock, vendors, and accounts.",
                    _session.RoleName);

            string displayName = !string.IsNullOrWhiteSpace(_session.FullName)
                ? _session.FullName
                : _session.Username;
            lblUsername.Text = string.IsNullOrWhiteSpace(displayName)
                ? "Signed in user"
                : string.Format("Signed in as {0}", displayName);

            scrollHost_Resize(this, EventArgs.Empty);

            if (IsInDesignMode())
            {
                ApplyMetrics(CreateDesignPreviewMetrics());
                lblDashboardStatus.Text = "Designer preview loaded.";
                lblLastUpdated.Text = "Last updated: preview mode";
                return;
            }

            LoadDashboard();
        }

        private void scrollHost_Resize(object sender, EventArgs e)
        {
            if (dashboardCanvas == null || scrollHost == null)
            {
                return;
            }

            int availableWidth = scrollHost.ClientSize.Width - 24;
            dashboardCanvas.Width = Math.Max(1180, availableWidth);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboard();
        }

        private void btnNewSale_Click(object sender, EventArgs e)
        {
            OpenModule(new SalesForm(_session));
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Do you want to log out and return to the login screen?",
                "Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Close();
            }
        }

        private void btnStockPrimary_Click(object sender, EventArgs e)
        {
            OpenModule(new StockManagementForm(_session));
        }

        private void btnProductPrimary_Click(object sender, EventArgs e)
        {
            OpenModule(new ProductManagementForm());
        }

        private void btnVendorPrimary_Click(object sender, EventArgs e)
        {
            OpenModule(new VendorManagementForm(_session));
        }

        private void btnPurchasePrimary_Click(object sender, EventArgs e)
        {
            OpenModule(new PurchaseForm(_session));
        }

        private void btnVendorPaymentPrimary_Click(object sender, EventArgs e)
        {
            OpenModule(new VendorPaymentForm(_session));
        }

        private void btnExpensePrimary_Click(object sender, EventArgs e)
        {
            OpenModule(new ExpenseManagementForm(_session));
        }

        private void btnExpiryPrimary_Click(object sender, EventArgs e)
        {
            OpenModule(new ExpiryManagementForm(_session));
        }

        private void btnServicePrimary_Click(object sender, EventArgs e)
        {
            OpenModule(new ServiceCenterForm(_session));
        }

        private void btnServiceTransactionsPrimary_Click(object sender, EventArgs e)
        {
            OpenModule(new ServiceTransactionsForm(_session));
        }

        private void btnCustomerPrimary_Click(object sender, EventArgs e)
        {
            OpenModule(new CustomerManagementForm(_session));
        }

        private void btnAccountsPrimary_Click(object sender, EventArgs e)
        {
            OpenModule(new AccountManagementForm());
        }

        private void btnRecentSalesPrimary_Click(object sender, EventArgs e)
        {
            OpenModule(new RecentSalesForm(_session));
        }

        private void OpenModule(Form form)
        {
            using (form)
            {
                form.ShowDialog(this);
            }

            if (!IsDisposed && !IsInDesignMode())
            {
                LoadDashboard();
            }
        }

        private void LoadDashboard()
        {
            if (IsInDesignMode())
            {
                return;
            }

            ToggleDashboard(true, "Loading dashboard...");

            try
            {
                DashboardMetrics metrics = _dashboardService.GetMetrics();
                ApplyMetrics(metrics);
                lblDashboardStatus.Text = BuildStatusText(metrics);
                lblLastUpdated.Text = string.Format("Last updated: {0:dd MMM yyyy hh:mm tt}", DateTime.Now);
            }
            catch (Exception ex)
            {
                lblDashboardStatus.Text = "Unable to load dashboard data right now.";
                lblLastUpdated.Text = "Last updated: failed";
                MessageBox.Show(
                    ex.Message,
                    "Dashboard Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                ToggleDashboard(false, lblDashboardStatus.Text);
            }
        }

        private void ApplyMetrics(DashboardMetrics metrics)
        {
            decimal cashSales = metrics.TodaySalesAmount - metrics.TodayCreditSalesAmount;
            if (cashSales < 0)
            {
                cashSales = 0;
            }

            lblTodaySalesValue.Text = FormatCurrency(metrics.TodaySalesAmount);
            lblTodayCreditSalesValue.Text = FormatCurrency(metrics.TodayCreditSalesAmount);
            lblTodayCashSalesValue.Text = FormatCurrency(cashSales);
            lblTodaySalesProfitValue.Text = FormatCurrency(metrics.TodaySalesProfit);
            lblTodayServiceSalesValue.Text = FormatCurrency(metrics.TodayServiceAmount);
            lblTodayServiceProfitValue.Text = FormatCurrency(metrics.TodayServiceIncome);
            lblTodayExpensesValue.Text = FormatCurrency(metrics.TodayExpensesAmount);
            lblLowStockValue.Text = metrics.LowStockCount.ToString("N0");
            lblTodayOrdersValue.Text = metrics.TodaySalesCount.ToString("N0");

            int expiryAttentionTotal = metrics.ExpiryAlertCount + metrics.ExpiredPendingCount;
            lblExpiryAttentionValue.Text = expiryAttentionTotal.ToString("N0");
            lblExpiryAttentionMeta.Text = expiryAttentionTotal == 0
                ? "No expiring batches or pending expired-stock actions."
                : string.Format(
                    "{0} expiring soon | {1} pending expired-stock actions",
                    metrics.ExpiryAlertCount,
                    metrics.ExpiredPendingCount);

            dgvLowStock.DataSource = null;
            dgvLowStock.DataSource = metrics.LowStockItems;
            dgvRecentSales.DataSource = null;
            dgvRecentSales.DataSource = metrics.RecentSales;
            FitDashboardGridColumns();
        }

        private string BuildStatusText(DashboardMetrics metrics)
        {
            if (metrics.LowStockCount == 0 && metrics.ExpiryAlertCount == 0 && metrics.ExpiredPendingCount == 0)
            {
                return "Everything looks steady today. No low-stock or expiry escalations are waiting.";
            }

            return string.Format(
                "Attention needed: {0} low-stock product(s), {1} expiring batch(es), and {2} expired-stock record(s) still pending action.",
                metrics.LowStockCount,
                metrics.ExpiryAlertCount,
                metrics.ExpiredPendingCount);
        }

        private void ToggleDashboard(bool isLoading, string statusText)
        {
            UseWaitCursor = isLoading;

            Button[] buttons =
            {
                btnRefresh,
                btnLogout,
                btnNewSale,
                btnNewSalePrimary,
                btnStockPrimary,
                btnProductPrimary,
                btnPurchasePrimary,
                btnVendorPrimary,
                btnVendorPaymentPrimary,
                btnExpensePrimary,
                btnExpiryPrimary,
                btnServicePrimary,
                btnServiceTransactionsPrimary,
                btnCustomerPrimary,
                btnAccountsPrimary,
                btnRecentSalesPrimary
            };

            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] != null)
                {
                    buttons[i].Enabled = !isLoading;
                }
            }

            if (!string.IsNullOrWhiteSpace(statusText) && lblDashboardStatus != null)
            {
                lblDashboardStatus.Text = statusText;
            }
        }

        private DashboardMetrics CreateDesignPreviewMetrics()
        {
            DashboardMetrics metrics = new DashboardMetrics();
            metrics.TodaySalesAmount = 2360M;
            metrics.TodayCreditSalesAmount = 1820M;
            metrics.TodaySalesProfit = 420M;
            metrics.TodaySalesCount = 12;
            metrics.TodayServiceAmount = 14850M;
            metrics.TodayServiceIncome = 220M;
            metrics.TodayExpensesAmount = 315M;
            metrics.LowStockCount = 5;
            metrics.ExpiryAlertCount = 2;
            metrics.ExpiredPendingCount = 1;
            metrics.LowStockItems.Add(new LowStockItem
            {
                ProductCode = "PRD-0007",
                ProductName = "Milk Pack 1L",
                CurrentStock = 4,
                ReorderLevel = 10
            });
            metrics.LowStockItems.Add(new LowStockItem
            {
                ProductCode = "PRD-0018",
                ProductName = "Biscuits Family Pack",
                CurrentStock = 0,
                ReorderLevel = 8
            });
            metrics.RecentSales.Add(new RecentSaleItem
            {
                EntryType = "Grocery",
                SaleNo = "SAL-1042",
                SaleDate = DateTime.Now.AddMinutes(-18),
                GrandTotal = 1850M,
                PaymentMethod = "Cash",
                Cashier = "Designer"
            });
            metrics.RecentSales.Add(new RecentSaleItem
            {
                EntryType = "Service",
                SaleNo = "SRV-0418",
                SaleDate = DateTime.Now.AddMinutes(-42),
                GrandTotal = 2500M,
                PaymentMethod = "JazzCash",
                Cashier = "Designer"
            });
            return metrics;
        }

        private static string FormatCurrency(decimal amount)
        {
            return string.Format("Rs. {0:N2}", amount);
        }

        private bool IsInDesignMode()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime ||
                   (Site != null && Site.DesignMode);
        }
    }
}
