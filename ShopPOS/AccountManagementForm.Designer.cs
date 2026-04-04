namespace ShopPOS
{
    partial class AccountManagementForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.Panel pnlToolbarHeader;
            System.Windows.Forms.Label lblToolbarTitle;
            System.Windows.Forms.Label lblToolbarSubtitle;
            System.Windows.Forms.TableLayoutPanel pnlMetricsLayout;
            System.Windows.Forms.TableLayoutPanel pnlDataLayout;
            System.Windows.Forms.Panel pnlAccountsHeader;
            System.Windows.Forms.Label lblAccountsSubtitle;
            System.Windows.Forms.Panel pnlAccountsBody;
            System.Windows.Forms.Panel pnlVouchersHeader;
            System.Windows.Forms.Label lblVouchersSubtitle;
            System.Windows.Forms.Panel pnlVouchersBody;
            System.Windows.Forms.Panel pnlProfitLossHeader;
            System.Windows.Forms.Label lblProfitLossSubtitle;
            System.Windows.Forms.Panel pnlProfitLossBody;

            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblHeaderSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.scrollHost = new System.Windows.Forms.Panel();
            this.contentLayout = new System.Windows.Forms.TableLayoutPanel();
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.lblPeriodHint = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnThisMonth = new System.Windows.Forms.Button();
            this.btnToday = new System.Windows.Forms.Button();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.pnlMetrics = new System.Windows.Forms.TableLayoutPanel();
            this.cardGrocerySales = new System.Windows.Forms.Panel();
            this.lblGrocerySalesMeta = new System.Windows.Forms.Label();
            this.lblGrocerySalesValue = new System.Windows.Forms.Label();
            this.lblGrocerySalesTitle = new System.Windows.Forms.Label();
            this.cardGroceryProfit = new System.Windows.Forms.Panel();
            this.lblGroceryProfitMeta = new System.Windows.Forms.Label();
            this.lblGroceryProfitValue = new System.Windows.Forms.Label();
            this.lblGroceryProfitTitle = new System.Windows.Forms.Label();
            this.cardServiceSales = new System.Windows.Forms.Panel();
            this.lblServiceSalesMeta = new System.Windows.Forms.Label();
            this.lblServiceSalesValue = new System.Windows.Forms.Label();
            this.lblServiceSalesTitle = new System.Windows.Forms.Label();
            this.cardServiceProfit = new System.Windows.Forms.Panel();
            this.lblServiceProfitMeta = new System.Windows.Forms.Label();
            this.lblServiceProfitValue = new System.Windows.Forms.Label();
            this.lblServiceProfitTitle = new System.Windows.Forms.Label();
            this.cardTotalExpense = new System.Windows.Forms.Panel();
            this.lblTotalExpenseMeta = new System.Windows.Forms.Label();
            this.lblTotalExpenseValue = new System.Windows.Forms.Label();
            this.lblTotalExpenseTitle = new System.Windows.Forms.Label();
            this.cardNetResult = new System.Windows.Forms.Panel();
            this.lblNetResultMeta = new System.Windows.Forms.Label();
            this.lblNetResultValue = new System.Windows.Forms.Label();
            this.lblNetResultTitle = new System.Windows.Forms.Label();
            this.pnlGrids = new System.Windows.Forms.TableLayoutPanel();
            this.pnlAccountsSection = new System.Windows.Forms.Panel();
            this.lblAccountsTitle = new System.Windows.Forms.Label();
            this.dgvAccounts = new System.Windows.Forms.DataGridView();
            this.pnlVouchersSection = new System.Windows.Forms.Panel();
            this.lblVouchersTitle = new System.Windows.Forms.Label();
            this.dgvVouchers = new System.Windows.Forms.DataGridView();
            this.pnlProfitLossSection = new System.Windows.Forms.Panel();
            this.lblProfitLossTitle = new System.Windows.Forms.Label();
            this.dgvProfitLoss = new System.Windows.Forms.DataGridView();
            pnlToolbarHeader = new System.Windows.Forms.Panel();
            lblToolbarTitle = new System.Windows.Forms.Label();
            lblToolbarSubtitle = new System.Windows.Forms.Label();
            pnlMetricsLayout = new System.Windows.Forms.TableLayoutPanel();
            pnlDataLayout = new System.Windows.Forms.TableLayoutPanel();
            pnlAccountsHeader = new System.Windows.Forms.Panel();
            lblAccountsSubtitle = new System.Windows.Forms.Label();
            pnlAccountsBody = new System.Windows.Forms.Panel();
            pnlVouchersHeader = new System.Windows.Forms.Panel();
            lblVouchersSubtitle = new System.Windows.Forms.Label();
            pnlVouchersBody = new System.Windows.Forms.Panel();
            pnlProfitLossHeader = new System.Windows.Forms.Panel();
            lblProfitLossSubtitle = new System.Windows.Forms.Label();
            pnlProfitLossBody = new System.Windows.Forms.Panel();
            this.headerPanel.SuspendLayout();
            this.scrollHost.SuspendLayout();
            this.contentLayout.SuspendLayout();
            this.pnlToolbar.SuspendLayout();
            pnlToolbarHeader.SuspendLayout();
            this.pnlMetrics.SuspendLayout();
            pnlMetricsLayout.SuspendLayout();
            this.cardGrocerySales.SuspendLayout();
            this.cardGroceryProfit.SuspendLayout();
            this.cardServiceSales.SuspendLayout();
            this.cardServiceProfit.SuspendLayout();
            this.cardTotalExpense.SuspendLayout();
            this.cardNetResult.SuspendLayout();
            this.pnlGrids.SuspendLayout();
            pnlDataLayout.SuspendLayout();
            this.pnlAccountsSection.SuspendLayout();
            pnlAccountsHeader.SuspendLayout();
            pnlAccountsBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccounts)).BeginInit();
            this.pnlVouchersSection.SuspendLayout();
            pnlVouchersHeader.SuspendLayout();
            pnlVouchersBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVouchers)).BeginInit();
            this.pnlProfitLossSection.SuspendLayout();
            pnlProfitLossHeader.SuspendLayout();
            pnlProfitLossBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfitLoss)).BeginInit();
            this.SuspendLayout();

            //
            // headerPanel
            //
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(107)))), ((int)(((byte)(83)))));
            this.headerPanel.Controls.Add(this.lblHeaderSubtitle);
            this.headerPanel.Controls.Add(this.lblTitle);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Padding = new System.Windows.Forms.Padding(22, 16, 22, 16);
            this.headerPanel.Size = new System.Drawing.Size(1440, 88);
            this.headerPanel.TabIndex = 0;
            //
            // lblHeaderSubtitle
            //
            this.lblHeaderSubtitle.AutoSize = true;
            this.lblHeaderSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblHeaderSubtitle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblHeaderSubtitle.Location = new System.Drawing.Point(24, 50);
            this.lblHeaderSubtitle.Name = "lblHeaderSubtitle";
            this.lblHeaderSubtitle.Size = new System.Drawing.Size(393, 17);
            this.lblHeaderSubtitle.TabIndex = 1;
            this.lblHeaderSubtitle.Text = "Review business totals, account balances, vouchers, and profit loss.";
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 19F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(22, 13);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(305, 36);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Accounts and Profit Loss";
            //
            // scrollHost
            //
            this.scrollHost.AutoScroll = true;
            this.scrollHost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.scrollHost.Controls.Add(this.contentLayout);
            this.scrollHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scrollHost.Location = new System.Drawing.Point(0, 88);
            this.scrollHost.Name = "scrollHost";
            this.scrollHost.Size = new System.Drawing.Size(1440, 772);
            this.scrollHost.TabIndex = 1;
            this.scrollHost.Resize += new System.EventHandler(this.scrollHost_Resize);
            //
            // contentLayout
            //
            this.contentLayout.ColumnCount = 1;
            this.contentLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.contentLayout.Controls.Add(this.pnlToolbar, 0, 0);
            this.contentLayout.Controls.Add(this.pnlMetrics, 0, 1);
            this.contentLayout.Controls.Add(this.pnlGrids, 0, 2);
            this.contentLayout.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentLayout.Location = new System.Drawing.Point(0, 0);
            this.contentLayout.MinimumSize = new System.Drawing.Size(1120, 0);
            this.contentLayout.Name = "contentLayout";
            this.contentLayout.Padding = new System.Windows.Forms.Padding(20);
            this.contentLayout.RowCount = 3;
            this.contentLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 138F));
            this.contentLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 238F));
            this.contentLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 680F));
            this.contentLayout.Size = new System.Drawing.Size(1390, 1096);
            this.contentLayout.TabIndex = 0;

            //
            // pnlToolbar
            //
            this.pnlToolbar.BackColor = System.Drawing.Color.White;
            this.pnlToolbar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlToolbar.Controls.Add(this.lblPeriodHint);
            this.pnlToolbar.Controls.Add(this.btnRefresh);
            this.pnlToolbar.Controls.Add(this.btnThisMonth);
            this.pnlToolbar.Controls.Add(this.btnToday);
            this.pnlToolbar.Controls.Add(this.dtpTo);
            this.pnlToolbar.Controls.Add(this.lblTo);
            this.pnlToolbar.Controls.Add(this.dtpFrom);
            this.pnlToolbar.Controls.Add(this.lblFrom);
            this.pnlToolbar.Controls.Add(pnlToolbarHeader);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlToolbar.Margin = new System.Windows.Forms.Padding(0, 0, 0, 18);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Size = new System.Drawing.Size(1350, 100);
            this.pnlToolbar.TabIndex = 0;
            //
            // pnlToolbarHeader
            //
            pnlToolbarHeader.Controls.Add(lblToolbarSubtitle);
            pnlToolbarHeader.Controls.Add(lblToolbarTitle);
            pnlToolbarHeader.Location = new System.Drawing.Point(18, 14);
            pnlToolbarHeader.Name = "pnlToolbarHeader";
            pnlToolbarHeader.Size = new System.Drawing.Size(320, 40);
            pnlToolbarHeader.TabIndex = 0;
            //
            // lblToolbarTitle
            //
            lblToolbarTitle.AutoSize = true;
            lblToolbarTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11.5F, System.Drawing.FontStyle.Bold);
            lblToolbarTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(43)))), ((int)(((byte)(56)))));
            lblToolbarTitle.Location = new System.Drawing.Point(0, 0);
            lblToolbarTitle.Name = "lblToolbarTitle";
            lblToolbarTitle.Size = new System.Drawing.Size(88, 21);
            lblToolbarTitle.TabIndex = 0;
            lblToolbarTitle.Text = "Date Filter";
            //
            // lblToolbarSubtitle
            //
            lblToolbarSubtitle.AutoSize = true;
            lblToolbarSubtitle.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            lblToolbarSubtitle.ForeColor = System.Drawing.Color.DimGray;
            lblToolbarSubtitle.Location = new System.Drawing.Point(1, 22);
            lblToolbarSubtitle.Name = "lblToolbarSubtitle";
            lblToolbarSubtitle.Size = new System.Drawing.Size(264, 15);
            lblToolbarSubtitle.TabIndex = 1;
            lblToolbarSubtitle.Text = "Refresh balances, vouchers, and P&L together.";
            //
            // lblFrom
            //
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(74)))), ((int)(((byte)(85)))));
            this.lblFrom.Location = new System.Drawing.Point(18, 66);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(37, 15);
            this.lblFrom.TabIndex = 1;
            this.lblFrom.Text = "From";
            //
            // dtpFrom
            //
            this.dtpFrom.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(60, 62);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(136, 24);
            this.dtpFrom.TabIndex = 2;
            //
            // lblTo
            //
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(74)))), ((int)(((byte)(85)))));
            this.lblTo.Location = new System.Drawing.Point(213, 66);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(20, 15);
            this.lblTo.TabIndex = 3;
            this.lblTo.Text = "To";
            //
            // dtpTo
            //
            this.dtpTo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(239, 62);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(136, 24);
            this.dtpTo.TabIndex = 4;
            //
            // btnToday
            //
            this.btnToday.BackColor = System.Drawing.Color.White;
            this.btnToday.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(211)))), ((int)(((byte)(220)))));
            this.btnToday.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToday.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnToday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(71)))));
            this.btnToday.Location = new System.Drawing.Point(392, 60);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(84, 29);
            this.btnToday.TabIndex = 5;
            this.btnToday.Text = "Today";
            this.btnToday.UseVisualStyleBackColor = false;
            this.btnToday.Click += new System.EventHandler(this.btnToday_Click);
            //
            // btnThisMonth
            //
            this.btnThisMonth.BackColor = System.Drawing.Color.White;
            this.btnThisMonth.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(211)))), ((int)(((byte)(220)))));
            this.btnThisMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThisMonth.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnThisMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(71)))));
            this.btnThisMonth.Location = new System.Drawing.Point(482, 60);
            this.btnThisMonth.Name = "btnThisMonth";
            this.btnThisMonth.Size = new System.Drawing.Size(96, 29);
            this.btnThisMonth.TabIndex = 6;
            this.btnThisMonth.Text = "This Month";
            this.btnThisMonth.UseVisualStyleBackColor = false;
            this.btnThisMonth.Click += new System.EventHandler(this.btnThisMonth_Click);
            //
            // btnRefresh
            //
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(107)))), ((int)(((byte)(83)))));
            this.btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(107)))), ((int)(((byte)(83)))));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(586, 60);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(96, 29);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // lblPeriodHint
            //
            this.lblPeriodHint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPeriodHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(107)))), ((int)(((byte)(120)))));
            this.lblPeriodHint.Location = new System.Drawing.Point(702, 58);
            this.lblPeriodHint.Name = "lblPeriodHint";
            this.lblPeriodHint.Size = new System.Drawing.Size(620, 31);
            this.lblPeriodHint.TabIndex = 8;
            this.lblPeriodHint.Text = "Showing figures for the selected period.";
            this.lblPeriodHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            //
            // pnlMetrics
            //
            this.pnlMetrics.ColumnCount = 1;
            this.pnlMetrics.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlMetrics.Controls.Add(pnlMetricsLayout, 0, 0);
            this.pnlMetrics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMetrics.Margin = new System.Windows.Forms.Padding(0, 0, 0, 18);
            this.pnlMetrics.Name = "pnlMetrics";
            this.pnlMetrics.RowCount = 1;
            this.pnlMetrics.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlMetrics.Size = new System.Drawing.Size(1350, 200);
            this.pnlMetrics.TabIndex = 1;
            //
            // pnlMetricsLayout
            //
            pnlMetricsLayout.ColumnCount = 3;
            pnlMetricsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            pnlMetricsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            pnlMetricsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            pnlMetricsLayout.Controls.Add(this.cardGrocerySales, 0, 0);
            pnlMetricsLayout.Controls.Add(this.cardGroceryProfit, 1, 0);
            pnlMetricsLayout.Controls.Add(this.cardServiceSales, 2, 0);
            pnlMetricsLayout.Controls.Add(this.cardServiceProfit, 0, 1);
            pnlMetricsLayout.Controls.Add(this.cardTotalExpense, 1, 1);
            pnlMetricsLayout.Controls.Add(this.cardNetResult, 2, 1);
            pnlMetricsLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlMetricsLayout.Location = new System.Drawing.Point(0, 0);
            pnlMetricsLayout.Name = "pnlMetricsLayout";
            pnlMetricsLayout.RowCount = 2;
            pnlMetricsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            pnlMetricsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            pnlMetricsLayout.Size = new System.Drawing.Size(1350, 200);
            pnlMetricsLayout.TabIndex = 0;
            //
            // cardGrocerySales
            //
            this.cardGrocerySales.BackColor = System.Drawing.Color.White;
            this.cardGrocerySales.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardGrocerySales.Controls.Add(this.lblGrocerySalesMeta);
            this.cardGrocerySales.Controls.Add(this.lblGrocerySalesValue);
            this.cardGrocerySales.Controls.Add(this.lblGrocerySalesTitle);
            this.cardGrocerySales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardGrocerySales.Margin = new System.Windows.Forms.Padding(0, 0, 12, 12);
            this.cardGrocerySales.Name = "cardGrocerySales";
            this.cardGrocerySales.Padding = new System.Windows.Forms.Padding(18, 14, 18, 14);
            this.cardGrocerySales.Size = new System.Drawing.Size(438, 88);
            this.cardGrocerySales.TabIndex = 0;
            //
            // lblGrocerySalesTitle
            //
            this.lblGrocerySalesTitle.AutoSize = true;
            this.lblGrocerySalesTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblGrocerySalesTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(100)))), ((int)(((byte)(74)))));
            this.lblGrocerySalesTitle.Location = new System.Drawing.Point(18, 14);
            this.lblGrocerySalesTitle.Name = "lblGrocerySalesTitle";
            this.lblGrocerySalesTitle.Size = new System.Drawing.Size(85, 15);
            this.lblGrocerySalesTitle.TabIndex = 0;
            this.lblGrocerySalesTitle.Text = "Grocery Sales";
            //
            // lblGrocerySalesValue
            //
            this.lblGrocerySalesValue.AutoSize = true;
            this.lblGrocerySalesValue.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblGrocerySalesValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(100)))), ((int)(((byte)(74)))));
            this.lblGrocerySalesValue.Location = new System.Drawing.Point(18, 34);
            this.lblGrocerySalesValue.Name = "lblGrocerySalesValue";
            this.lblGrocerySalesValue.Size = new System.Drawing.Size(81, 30);
            this.lblGrocerySalesValue.TabIndex = 1;
            this.lblGrocerySalesValue.Text = "Rs. 0.00";
            //
            // lblGrocerySalesMeta
            //
            this.lblGrocerySalesMeta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGrocerySalesMeta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(118)))), ((int)(((byte)(129)))));
            this.lblGrocerySalesMeta.Location = new System.Drawing.Point(18, 66);
            this.lblGrocerySalesMeta.Name = "lblGrocerySalesMeta";
            this.lblGrocerySalesMeta.Size = new System.Drawing.Size(400, 15);
            this.lblGrocerySalesMeta.TabIndex = 2;
            this.lblGrocerySalesMeta.Text = "No orders in period";
            //
            // cardGroceryProfit
            //
            this.cardGroceryProfit.BackColor = System.Drawing.Color.White;
            this.cardGroceryProfit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardGroceryProfit.Controls.Add(this.lblGroceryProfitMeta);
            this.cardGroceryProfit.Controls.Add(this.lblGroceryProfitValue);
            this.cardGroceryProfit.Controls.Add(this.lblGroceryProfitTitle);
            this.cardGroceryProfit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardGroceryProfit.Margin = new System.Windows.Forms.Padding(0, 0, 12, 12);
            this.cardGroceryProfit.Name = "cardGroceryProfit";
            this.cardGroceryProfit.Padding = new System.Windows.Forms.Padding(18, 14, 18, 14);
            this.cardGroceryProfit.Size = new System.Drawing.Size(438, 88);
            this.cardGroceryProfit.TabIndex = 1;
            //
            // lblGroceryProfitTitle
            //
            this.lblGroceryProfitTitle.AutoSize = true;
            this.lblGroceryProfitTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblGroceryProfitTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(82)))), ((int)(((byte)(154)))));
            this.lblGroceryProfitTitle.Location = new System.Drawing.Point(18, 14);
            this.lblGroceryProfitTitle.Name = "lblGroceryProfitTitle";
            this.lblGroceryProfitTitle.Size = new System.Drawing.Size(89, 15);
            this.lblGroceryProfitTitle.TabIndex = 0;
            this.lblGroceryProfitTitle.Text = "Grocery Profit";
            //
            // lblGroceryProfitValue
            //
            this.lblGroceryProfitValue.AutoSize = true;
            this.lblGroceryProfitValue.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblGroceryProfitValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(82)))), ((int)(((byte)(154)))));
            this.lblGroceryProfitValue.Location = new System.Drawing.Point(18, 34);
            this.lblGroceryProfitValue.Name = "lblGroceryProfitValue";
            this.lblGroceryProfitValue.Size = new System.Drawing.Size(81, 30);
            this.lblGroceryProfitValue.TabIndex = 1;
            this.lblGroceryProfitValue.Text = "Rs. 0.00";
            //
            // lblGroceryProfitMeta
            //
            this.lblGroceryProfitMeta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGroceryProfitMeta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(118)))), ((int)(((byte)(129)))));
            this.lblGroceryProfitMeta.Location = new System.Drawing.Point(18, 66);
            this.lblGroceryProfitMeta.Name = "lblGroceryProfitMeta";
            this.lblGroceryProfitMeta.Size = new System.Drawing.Size(400, 15);
            this.lblGroceryProfitMeta.TabIndex = 2;
            this.lblGroceryProfitMeta.Text = "No margin to calculate";
            //
            // cardServiceSales
            //
            this.cardServiceSales.BackColor = System.Drawing.Color.White;
            this.cardServiceSales.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardServiceSales.Controls.Add(this.lblServiceSalesMeta);
            this.cardServiceSales.Controls.Add(this.lblServiceSalesValue);
            this.cardServiceSales.Controls.Add(this.lblServiceSalesTitle);
            this.cardServiceSales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardServiceSales.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.cardServiceSales.Name = "cardServiceSales";
            this.cardServiceSales.Padding = new System.Windows.Forms.Padding(18, 14, 18, 14);
            this.cardServiceSales.Size = new System.Drawing.Size(438, 88);
            this.cardServiceSales.TabIndex = 2;
            //
            // lblServiceSalesTitle
            //
            this.lblServiceSalesTitle.AutoSize = true;
            this.lblServiceSalesTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblServiceSalesTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(74)))), ((int)(((byte)(182)))));
            this.lblServiceSalesTitle.Location = new System.Drawing.Point(18, 14);
            this.lblServiceSalesTitle.Name = "lblServiceSalesTitle";
            this.lblServiceSalesTitle.Size = new System.Drawing.Size(77, 15);
            this.lblServiceSalesTitle.TabIndex = 0;
            this.lblServiceSalesTitle.Text = "Service Sales";
            //
            // lblServiceSalesValue
            //
            this.lblServiceSalesValue.AutoSize = true;
            this.lblServiceSalesValue.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblServiceSalesValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(74)))), ((int)(((byte)(182)))));
            this.lblServiceSalesValue.Location = new System.Drawing.Point(18, 34);
            this.lblServiceSalesValue.Name = "lblServiceSalesValue";
            this.lblServiceSalesValue.Size = new System.Drawing.Size(81, 30);
            this.lblServiceSalesValue.TabIndex = 1;
            this.lblServiceSalesValue.Text = "Rs. 0.00";
            //
            // lblServiceSalesMeta
            //
            this.lblServiceSalesMeta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblServiceSalesMeta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(118)))), ((int)(((byte)(129)))));
            this.lblServiceSalesMeta.Location = new System.Drawing.Point(18, 66);
            this.lblServiceSalesMeta.Name = "lblServiceSalesMeta";
            this.lblServiceSalesMeta.Size = new System.Drawing.Size(400, 15);
            this.lblServiceSalesMeta.TabIndex = 2;
            this.lblServiceSalesMeta.Text = "No transactions in period";
            //
            // cardServiceProfit
            //
            this.cardServiceProfit.BackColor = System.Drawing.Color.White;
            this.cardServiceProfit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardServiceProfit.Controls.Add(this.lblServiceProfitMeta);
            this.cardServiceProfit.Controls.Add(this.lblServiceProfitValue);
            this.cardServiceProfit.Controls.Add(this.lblServiceProfitTitle);
            this.cardServiceProfit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardServiceProfit.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.cardServiceProfit.Name = "cardServiceProfit";
            this.cardServiceProfit.Padding = new System.Windows.Forms.Padding(18, 14, 18, 14);
            this.cardServiceProfit.Size = new System.Drawing.Size(438, 88);
            this.cardServiceProfit.TabIndex = 3;
            //
            // lblServiceProfitTitle
            //
            this.lblServiceProfitTitle.AutoSize = true;
            this.lblServiceProfitTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblServiceProfitTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(104)))), ((int)(((byte)(32)))));
            this.lblServiceProfitTitle.Location = new System.Drawing.Point(18, 14);
            this.lblServiceProfitTitle.Name = "lblServiceProfitTitle";
            this.lblServiceProfitTitle.Size = new System.Drawing.Size(81, 15);
            this.lblServiceProfitTitle.TabIndex = 0;
            this.lblServiceProfitTitle.Text = "Service Profit";
            //
            // lblServiceProfitValue
            //
            this.lblServiceProfitValue.AutoSize = true;
            this.lblServiceProfitValue.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblServiceProfitValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(104)))), ((int)(((byte)(32)))));
            this.lblServiceProfitValue.Location = new System.Drawing.Point(18, 34);
            this.lblServiceProfitValue.Name = "lblServiceProfitValue";
            this.lblServiceProfitValue.Size = new System.Drawing.Size(81, 30);
            this.lblServiceProfitValue.TabIndex = 1;
            this.lblServiceProfitValue.Text = "Rs. 0.00";
            //
            // lblServiceProfitMeta
            //
            this.lblServiceProfitMeta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblServiceProfitMeta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(118)))), ((int)(((byte)(129)))));
            this.lblServiceProfitMeta.Location = new System.Drawing.Point(18, 66);
            this.lblServiceProfitMeta.Name = "lblServiceProfitMeta";
            this.lblServiceProfitMeta.Size = new System.Drawing.Size(400, 15);
            this.lblServiceProfitMeta.TabIndex = 2;
            this.lblServiceProfitMeta.Text = "No earnings posted in period";
            //
            // cardTotalExpense
            //
            this.cardTotalExpense.BackColor = System.Drawing.Color.White;
            this.cardTotalExpense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardTotalExpense.Controls.Add(this.lblTotalExpenseMeta);
            this.cardTotalExpense.Controls.Add(this.lblTotalExpenseValue);
            this.cardTotalExpense.Controls.Add(this.lblTotalExpenseTitle);
            this.cardTotalExpense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardTotalExpense.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.cardTotalExpense.Name = "cardTotalExpense";
            this.cardTotalExpense.Padding = new System.Windows.Forms.Padding(18, 14, 18, 14);
            this.cardTotalExpense.Size = new System.Drawing.Size(438, 88);
            this.cardTotalExpense.TabIndex = 4;
            //
            // lblTotalExpenseTitle
            //
            this.lblTotalExpenseTitle.AutoSize = true;
            this.lblTotalExpenseTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalExpenseTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(69)))), ((int)(((byte)(58)))));
            this.lblTotalExpenseTitle.Location = new System.Drawing.Point(18, 14);
            this.lblTotalExpenseTitle.Name = "lblTotalExpenseTitle";
            this.lblTotalExpenseTitle.Size = new System.Drawing.Size(80, 15);
            this.lblTotalExpenseTitle.TabIndex = 0;
            this.lblTotalExpenseTitle.Text = "Total Expense";
            //
            // lblTotalExpenseValue
            //
            this.lblTotalExpenseValue.AutoSize = true;
            this.lblTotalExpenseValue.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblTotalExpenseValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(69)))), ((int)(((byte)(58)))));
            this.lblTotalExpenseValue.Location = new System.Drawing.Point(18, 34);
            this.lblTotalExpenseValue.Name = "lblTotalExpenseValue";
            this.lblTotalExpenseValue.Size = new System.Drawing.Size(81, 30);
            this.lblTotalExpenseValue.TabIndex = 1;
            this.lblTotalExpenseValue.Text = "Rs. 0.00";
            //
            // lblTotalExpenseMeta
            //
            this.lblTotalExpenseMeta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalExpenseMeta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(118)))), ((int)(((byte)(129)))));
            this.lblTotalExpenseMeta.Location = new System.Drawing.Point(18, 66);
            this.lblTotalExpenseMeta.Name = "lblTotalExpenseMeta";
            this.lblTotalExpenseMeta.Size = new System.Drawing.Size(400, 15);
            this.lblTotalExpenseMeta.TabIndex = 2;
            this.lblTotalExpenseMeta.Text = "No expense movement in period";
            //
            // cardNetResult
            //
            this.cardNetResult.BackColor = System.Drawing.Color.White;
            this.cardNetResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardNetResult.Controls.Add(this.lblNetResultMeta);
            this.cardNetResult.Controls.Add(this.lblNetResultValue);
            this.cardNetResult.Controls.Add(this.lblNetResultTitle);
            this.cardNetResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardNetResult.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.cardNetResult.Name = "cardNetResult";
            this.cardNetResult.Padding = new System.Windows.Forms.Padding(18, 14, 18, 14);
            this.cardNetResult.Size = new System.Drawing.Size(438, 88);
            this.cardNetResult.TabIndex = 5;
            //
            // lblNetResultTitle
            //
            this.lblNetResultTitle.AutoSize = true;
            this.lblNetResultTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblNetResultTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(66)))), ((int)(((byte)(104)))));
            this.lblNetResultTitle.Location = new System.Drawing.Point(18, 14);
            this.lblNetResultTitle.Name = "lblNetResultTitle";
            this.lblNetResultTitle.Size = new System.Drawing.Size(63, 15);
            this.lblNetResultTitle.TabIndex = 0;
            this.lblNetResultTitle.Text = "Net Result";
            //
            // lblNetResultValue
            //
            this.lblNetResultValue.AutoSize = true;
            this.lblNetResultValue.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblNetResultValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(66)))), ((int)(((byte)(104)))));
            this.lblNetResultValue.Location = new System.Drawing.Point(18, 34);
            this.lblNetResultValue.Name = "lblNetResultValue";
            this.lblNetResultValue.Size = new System.Drawing.Size(81, 30);
            this.lblNetResultValue.TabIndex = 1;
            this.lblNetResultValue.Text = "Rs. 0.00";
            //
            // lblNetResultMeta
            //
            this.lblNetResultMeta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNetResultMeta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(118)))), ((int)(((byte)(129)))));
            this.lblNetResultMeta.Location = new System.Drawing.Point(18, 66);
            this.lblNetResultMeta.Name = "lblNetResultMeta";
            this.lblNetResultMeta.Size = new System.Drawing.Size(400, 15);
            this.lblNetResultMeta.TabIndex = 2;
            this.lblNetResultMeta.Text = "Income minus expense";

            //
            // pnlGrids
            //
            this.pnlGrids.ColumnCount = 2;
            this.pnlGrids.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.pnlGrids.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.pnlGrids.Controls.Add(pnlDataLayout, 0, 0);
            this.pnlGrids.Controls.Add(this.pnlProfitLossSection, 0, 1);
            this.pnlGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrids.Margin = new System.Windows.Forms.Padding(0);
            this.pnlGrids.Name = "pnlGrids";
            this.pnlGrids.RowCount = 2;
            this.pnlGrids.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 290F));
            this.pnlGrids.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 360F));
            this.pnlGrids.Size = new System.Drawing.Size(1350, 640);
            this.pnlGrids.TabIndex = 2;
            this.pnlGrids.SetColumnSpan(this.pnlProfitLossSection, 2);
            //
            // pnlDataLayout
            //
            pnlDataLayout.ColumnCount = 2;
            pnlDataLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            pnlDataLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            pnlDataLayout.Controls.Add(this.pnlAccountsSection, 0, 0);
            pnlDataLayout.Controls.Add(this.pnlVouchersSection, 1, 0);
            pnlDataLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlDataLayout.Margin = new System.Windows.Forms.Padding(0, 0, 0, 18);
            pnlDataLayout.Name = "pnlDataLayout";
            pnlDataLayout.RowCount = 1;
            pnlDataLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            pnlDataLayout.Size = new System.Drawing.Size(1350, 272);
            pnlDataLayout.TabIndex = 0;
            this.pnlGrids.SetColumnSpan(pnlDataLayout, 2);
            //
            // pnlAccountsSection
            //
            this.pnlAccountsSection.BackColor = System.Drawing.Color.White;
            this.pnlAccountsSection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAccountsSection.Controls.Add(pnlAccountsBody);
            this.pnlAccountsSection.Controls.Add(pnlAccountsHeader);
            this.pnlAccountsSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAccountsSection.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.pnlAccountsSection.Name = "pnlAccountsSection";
            this.pnlAccountsSection.Size = new System.Drawing.Size(798, 272);
            this.pnlAccountsSection.TabIndex = 0;
            //
            // pnlAccountsHeader
            //
            pnlAccountsHeader.BackColor = System.Drawing.Color.White;
            pnlAccountsHeader.Controls.Add(lblAccountsSubtitle);
            pnlAccountsHeader.Controls.Add(this.lblAccountsTitle);
            pnlAccountsHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlAccountsHeader.Location = new System.Drawing.Point(0, 0);
            pnlAccountsHeader.Name = "pnlAccountsHeader";
            pnlAccountsHeader.Size = new System.Drawing.Size(796, 58);
            pnlAccountsHeader.TabIndex = 0;
            //
            // lblAccountsTitle
            //
            this.lblAccountsTitle.AutoSize = true;
            this.lblAccountsTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblAccountsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(43)))), ((int)(((byte)(56)))));
            this.lblAccountsTitle.Location = new System.Drawing.Point(16, 10);
            this.lblAccountsTitle.Name = "lblAccountsTitle";
            this.lblAccountsTitle.Size = new System.Drawing.Size(122, 20);
            this.lblAccountsTitle.TabIndex = 0;
            this.lblAccountsTitle.Text = "Account Balances";
            //
            // lblAccountsSubtitle
            //
            lblAccountsSubtitle.AutoSize = true;
            lblAccountsSubtitle.ForeColor = System.Drawing.Color.DimGray;
            lblAccountsSubtitle.Location = new System.Drawing.Point(17, 33);
            lblAccountsSubtitle.Name = "lblAccountsSubtitle";
            lblAccountsSubtitle.Size = new System.Drawing.Size(265, 15);
            lblAccountsSubtitle.TabIndex = 1;
            lblAccountsSubtitle.Text = "Opening, movement, and closing by account.";
            //
            // pnlAccountsBody
            //
            pnlAccountsBody.Controls.Add(this.dgvAccounts);
            pnlAccountsBody.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlAccountsBody.Location = new System.Drawing.Point(0, 58);
            pnlAccountsBody.Name = "pnlAccountsBody";
            pnlAccountsBody.Padding = new System.Windows.Forms.Padding(16, 0, 16, 16);
            pnlAccountsBody.Size = new System.Drawing.Size(796, 212);
            pnlAccountsBody.TabIndex = 1;
            //
            // dgvAccounts
            //
            this.dgvAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAccounts.Location = new System.Drawing.Point(16, 0);
            this.dgvAccounts.Name = "dgvAccounts";
            this.dgvAccounts.Size = new System.Drawing.Size(764, 196);
            this.dgvAccounts.TabIndex = 0;
            //
            // pnlVouchersSection
            //
            this.pnlVouchersSection.BackColor = System.Drawing.Color.White;
            this.pnlVouchersSection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlVouchersSection.Controls.Add(pnlVouchersBody);
            this.pnlVouchersSection.Controls.Add(pnlVouchersHeader);
            this.pnlVouchersSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlVouchersSection.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.pnlVouchersSection.Name = "pnlVouchersSection";
            this.pnlVouchersSection.Size = new System.Drawing.Size(528, 272);
            this.pnlVouchersSection.TabIndex = 1;
            //
            // pnlVouchersHeader
            //
            pnlVouchersHeader.BackColor = System.Drawing.Color.White;
            pnlVouchersHeader.Controls.Add(lblVouchersSubtitle);
            pnlVouchersHeader.Controls.Add(this.lblVouchersTitle);
            pnlVouchersHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlVouchersHeader.Location = new System.Drawing.Point(0, 0);
            pnlVouchersHeader.Name = "pnlVouchersHeader";
            pnlVouchersHeader.Size = new System.Drawing.Size(526, 58);
            pnlVouchersHeader.TabIndex = 0;
            //
            // lblVouchersTitle
            //
            this.lblVouchersTitle.AutoSize = true;
            this.lblVouchersTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblVouchersTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(43)))), ((int)(((byte)(56)))));
            this.lblVouchersTitle.Location = new System.Drawing.Point(16, 10);
            this.lblVouchersTitle.Name = "lblVouchersTitle";
            this.lblVouchersTitle.Size = new System.Drawing.Size(114, 20);
            this.lblVouchersTitle.TabIndex = 0;
            this.lblVouchersTitle.Text = "Recent Vouchers";
            //
            // lblVouchersSubtitle
            //
            lblVouchersSubtitle.AutoSize = true;
            lblVouchersSubtitle.ForeColor = System.Drawing.Color.DimGray;
            lblVouchersSubtitle.Location = new System.Drawing.Point(17, 33);
            lblVouchersSubtitle.Name = "lblVouchersSubtitle";
            lblVouchersSubtitle.Size = new System.Drawing.Size(286, 15);
            lblVouchersSubtitle.TabIndex = 1;
            lblVouchersSubtitle.Text = "Latest ledger activity posted in the selected period.";
            //
            // pnlVouchersBody
            //
            pnlVouchersBody.Controls.Add(this.dgvVouchers);
            pnlVouchersBody.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlVouchersBody.Location = new System.Drawing.Point(0, 58);
            pnlVouchersBody.Name = "pnlVouchersBody";
            pnlVouchersBody.Padding = new System.Windows.Forms.Padding(16, 0, 16, 16);
            pnlVouchersBody.Size = new System.Drawing.Size(526, 212);
            pnlVouchersBody.TabIndex = 1;
            //
            // dgvVouchers
            //
            this.dgvVouchers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVouchers.Location = new System.Drawing.Point(16, 0);
            this.dgvVouchers.Name = "dgvVouchers";
            this.dgvVouchers.Size = new System.Drawing.Size(494, 196);
            this.dgvVouchers.TabIndex = 0;
            //
            // pnlProfitLossSection
            //
            this.pnlProfitLossSection.BackColor = System.Drawing.Color.White;
            this.pnlProfitLossSection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProfitLossSection.Controls.Add(pnlProfitLossBody);
            this.pnlProfitLossSection.Controls.Add(pnlProfitLossHeader);
            this.pnlProfitLossSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProfitLossSection.Margin = new System.Windows.Forms.Padding(0);
            this.pnlProfitLossSection.Name = "pnlProfitLossSection";
            this.pnlProfitLossSection.Size = new System.Drawing.Size(1350, 350);
            this.pnlProfitLossSection.TabIndex = 1;
            //
            // pnlProfitLossHeader
            //
            pnlProfitLossHeader.BackColor = System.Drawing.Color.White;
            pnlProfitLossHeader.Controls.Add(lblProfitLossSubtitle);
            pnlProfitLossHeader.Controls.Add(this.lblProfitLossTitle);
            pnlProfitLossHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlProfitLossHeader.Location = new System.Drawing.Point(0, 0);
            pnlProfitLossHeader.Name = "pnlProfitLossHeader";
            pnlProfitLossHeader.Size = new System.Drawing.Size(1348, 58);
            pnlProfitLossHeader.TabIndex = 0;
            //
            // lblProfitLossTitle
            //
            this.lblProfitLossTitle.AutoSize = true;
            this.lblProfitLossTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblProfitLossTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(43)))), ((int)(((byte)(56)))));
            this.lblProfitLossTitle.Location = new System.Drawing.Point(16, 10);
            this.lblProfitLossTitle.Name = "lblProfitLossTitle";
            this.lblProfitLossTitle.Size = new System.Drawing.Size(92, 20);
            this.lblProfitLossTitle.TabIndex = 0;
            this.lblProfitLossTitle.Text = "Profit && Loss";
            //
            // lblProfitLossSubtitle
            //
            lblProfitLossSubtitle.AutoSize = true;
            lblProfitLossSubtitle.ForeColor = System.Drawing.Color.DimGray;
            lblProfitLossSubtitle.Location = new System.Drawing.Point(17, 33);
            lblProfitLossSubtitle.Name = "lblProfitLossSubtitle";
            lblProfitLossSubtitle.Size = new System.Drawing.Size(275, 15);
            lblProfitLossSubtitle.TabIndex = 1;
            lblProfitLossSubtitle.Text = "Income, expense, and summary result for period.";
            //
            // pnlProfitLossBody
            //
            pnlProfitLossBody.Controls.Add(this.dgvProfitLoss);
            pnlProfitLossBody.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlProfitLossBody.Location = new System.Drawing.Point(0, 58);
            pnlProfitLossBody.Name = "pnlProfitLossBody";
            pnlProfitLossBody.Padding = new System.Windows.Forms.Padding(16, 0, 16, 16);
            pnlProfitLossBody.Size = new System.Drawing.Size(1348, 290);
            pnlProfitLossBody.TabIndex = 1;
            //
            // dgvProfitLoss
            //
            this.dgvProfitLoss.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProfitLoss.Location = new System.Drawing.Point(16, 0);
            this.dgvProfitLoss.Name = "dgvProfitLoss";
            this.dgvProfitLoss.Size = new System.Drawing.Size(1316, 274);
            this.dgvProfitLoss.TabIndex = 0;

            //
            // AccountManagementForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1440, 860);
            this.Controls.Add(this.scrollHost);
            this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1200, 760);
            this.Name = "AccountManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Accounts and Profit Loss";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.AccountManagementForm_Load);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.scrollHost.ResumeLayout(false);
            this.contentLayout.ResumeLayout(false);
            this.pnlToolbar.ResumeLayout(false);
            this.pnlToolbar.PerformLayout();
            pnlToolbarHeader.ResumeLayout(false);
            pnlToolbarHeader.PerformLayout();
            this.pnlMetrics.ResumeLayout(false);
            pnlMetricsLayout.ResumeLayout(false);
            this.cardGrocerySales.ResumeLayout(false);
            this.cardGrocerySales.PerformLayout();
            this.cardGroceryProfit.ResumeLayout(false);
            this.cardGroceryProfit.PerformLayout();
            this.cardServiceSales.ResumeLayout(false);
            this.cardServiceSales.PerformLayout();
            this.cardServiceProfit.ResumeLayout(false);
            this.cardServiceProfit.PerformLayout();
            this.cardTotalExpense.ResumeLayout(false);
            this.cardTotalExpense.PerformLayout();
            this.cardNetResult.ResumeLayout(false);
            this.cardNetResult.PerformLayout();
            this.pnlGrids.ResumeLayout(false);
            pnlDataLayout.ResumeLayout(false);
            this.pnlAccountsSection.ResumeLayout(false);
            pnlAccountsHeader.ResumeLayout(false);
            pnlAccountsHeader.PerformLayout();
            pnlAccountsBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccounts)).EndInit();
            this.pnlVouchersSection.ResumeLayout(false);
            pnlVouchersHeader.ResumeLayout(false);
            pnlVouchersHeader.PerformLayout();
            pnlVouchersBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVouchers)).EndInit();
            this.pnlProfitLossSection.ResumeLayout(false);
            pnlProfitLossHeader.ResumeLayout(false);
            pnlProfitLossHeader.PerformLayout();
            pnlProfitLossBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfitLoss)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblHeaderSubtitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel scrollHost;
        private System.Windows.Forms.TableLayoutPanel contentLayout;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.Label lblPeriodHint;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnThisMonth;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.TableLayoutPanel pnlMetrics;
        private System.Windows.Forms.Panel cardGrocerySales;
        private System.Windows.Forms.Label lblGrocerySalesMeta;
        private System.Windows.Forms.Label lblGrocerySalesValue;
        private System.Windows.Forms.Label lblGrocerySalesTitle;
        private System.Windows.Forms.Panel cardGroceryProfit;
        private System.Windows.Forms.Label lblGroceryProfitMeta;
        private System.Windows.Forms.Label lblGroceryProfitValue;
        private System.Windows.Forms.Label lblGroceryProfitTitle;
        private System.Windows.Forms.Panel cardServiceSales;
        private System.Windows.Forms.Label lblServiceSalesMeta;
        private System.Windows.Forms.Label lblServiceSalesValue;
        private System.Windows.Forms.Label lblServiceSalesTitle;
        private System.Windows.Forms.Panel cardServiceProfit;
        private System.Windows.Forms.Label lblServiceProfitMeta;
        private System.Windows.Forms.Label lblServiceProfitValue;
        private System.Windows.Forms.Label lblServiceProfitTitle;
        private System.Windows.Forms.Panel cardTotalExpense;
        private System.Windows.Forms.Label lblTotalExpenseMeta;
        private System.Windows.Forms.Label lblTotalExpenseValue;
        private System.Windows.Forms.Label lblTotalExpenseTitle;
        private System.Windows.Forms.Panel cardNetResult;
        private System.Windows.Forms.Label lblNetResultMeta;
        private System.Windows.Forms.Label lblNetResultValue;
        private System.Windows.Forms.Label lblNetResultTitle;
        private System.Windows.Forms.TableLayoutPanel pnlGrids;
        private System.Windows.Forms.Panel pnlAccountsSection;
        private System.Windows.Forms.Label lblAccountsTitle;
        private System.Windows.Forms.DataGridView dgvAccounts;
        private System.Windows.Forms.Panel pnlVouchersSection;
        private System.Windows.Forms.Label lblVouchersTitle;
        private System.Windows.Forms.DataGridView dgvVouchers;
        private System.Windows.Forms.Panel pnlProfitLossSection;
        private System.Windows.Forms.Label lblProfitLossTitle;
        private System.Windows.Forms.DataGridView dgvProfitLoss;
    }
}
