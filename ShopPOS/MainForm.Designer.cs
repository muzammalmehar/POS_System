namespace ShopPOS
{
    partial class MainForm
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
            this.metricsTable = new System.Windows.Forms.TableLayoutPanel();
            this.pnlSalesCard = new System.Windows.Forms.Panel();
            this.lblSalesCardTitle = new System.Windows.Forms.Label();
            this.lblTodaySalesValue = new System.Windows.Forms.Label();
            this.lblCreditSalesCaption = new System.Windows.Forms.Label();
            this.lblTodayCreditSalesValue = new System.Windows.Forms.Label();
            this.lblCashSalesCaption = new System.Windows.Forms.Label();
            this.lblTodayCashSalesValue = new System.Windows.Forms.Label();
            this.pnlSalesProfitCard = new System.Windows.Forms.Panel();
            this.lblSalesProfitTitle = new System.Windows.Forms.Label();
            this.lblTodaySalesProfitValue = new System.Windows.Forms.Label();
            this.lblSalesProfitMeta = new System.Windows.Forms.Label();
            this.pnlServiceSalesCard = new System.Windows.Forms.Panel();
            this.lblServiceSalesTitle = new System.Windows.Forms.Label();
            this.lblTodayServiceSalesValue = new System.Windows.Forms.Label();
            this.lblServiceSalesMeta = new System.Windows.Forms.Label();
            this.pnlServiceProfitCard = new System.Windows.Forms.Panel();
            this.lblServiceProfitTitle = new System.Windows.Forms.Label();
            this.lblTodayServiceProfitValue = new System.Windows.Forms.Label();
            this.lblServiceProfitMeta = new System.Windows.Forms.Label();
            this.pnlExpensesCard = new System.Windows.Forms.Panel();
            this.lblExpensesTitle = new System.Windows.Forms.Label();
            this.lblTodayExpensesValue = new System.Windows.Forms.Label();
            this.lblExpensesMeta = new System.Windows.Forms.Label();
            this.pnlLowStockCard = new System.Windows.Forms.Panel();
            this.lblLowStockTitle = new System.Windows.Forms.Label();
            this.lblLowStockMeta = new System.Windows.Forms.Label();
            this.lblLowStockValue = new System.Windows.Forms.Label();
            this.pnlExpiryCard = new System.Windows.Forms.Panel();
            this.lblExpiryTitle = new System.Windows.Forms.Label();
            this.lblExpiryAttentionMeta = new System.Windows.Forms.Label();
            this.lblExpiryAttentionValue = new System.Windows.Forms.Label();
            this.pnlOrdersCard = new System.Windows.Forms.Panel();
            this.lblOrdersTitle = new System.Windows.Forms.Label();
            this.lblTodayOrdersValue = new System.Windows.Forms.Label();
            this.lblOrdersMeta = new System.Windows.Forms.Label();
            this.actionsGrid = new System.Windows.Forms.TableLayoutPanel();
            this.btnNewSalePrimary = new System.Windows.Forms.Button();
            this.btnRecentSalesPrimary = new System.Windows.Forms.Button();
            this.btnCustomerPrimary = new System.Windows.Forms.Button();
            this.btnServicePrimary = new System.Windows.Forms.Button();
            this.btnServiceTransactionsPrimary = new System.Windows.Forms.Button();
            this.btnStockPrimary = new System.Windows.Forms.Button();
            this.btnProductPrimary = new System.Windows.Forms.Button();
            this.btnPurchasePrimary = new System.Windows.Forms.Button();
            this.btnVendorPrimary = new System.Windows.Forms.Button();
            this.btnVendorPaymentPrimary = new System.Windows.Forms.Button();
            this.btnExpensePrimary = new System.Windows.Forms.Button();
            this.btnExpiryPrimary = new System.Windows.Forms.Button();
            this.btnAccountsPrimary = new System.Windows.Forms.Button();
            this.tablesGrid = new System.Windows.Forms.TableLayoutPanel();
            this.pnlLowStockSection = new System.Windows.Forms.Panel();
            this.lblLowStockGridTitle = new System.Windows.Forms.Label();
            this.lblLowStockGridHint = new System.Windows.Forms.Label();
            this.lowStockHost = new System.Windows.Forms.Panel();
            this.dgvLowStock = new System.Windows.Forms.DataGridView();
            this.pnlRecentSalesSection = new System.Windows.Forms.Panel();
            this.lblRecentSalesGridTitle = new System.Windows.Forms.Label();
            this.lblRecentSalesGridHint = new System.Windows.Forms.Label();
            this.recentSalesHost = new System.Windows.Forms.Panel();
            this.dgvRecentSales = new System.Windows.Forms.DataGridView();
            this.lblActionsSectionTitle = new System.Windows.Forms.Label();
            this.lblActionsSectionHint = new System.Windows.Forms.Label();
            this.lblOpsSectionTitle = new System.Windows.Forms.Label();
            this.lblOpsSectionHint = new System.Windows.Forms.Label();
            this.pnlStatusSection = new System.Windows.Forms.Panel();
            this.lblDashboardStatus = new System.Windows.Forms.Label();
            this.lblDashboardNote = new System.Windows.Forms.Label();
            this.lblLastUpdated = new System.Windows.Forms.Label();
            this.pnlActionsSection = new System.Windows.Forms.Panel();
            this.pnlTablesSection = new System.Windows.Forms.Panel();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblRole = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnNewSale = new System.Windows.Forms.Button();
            this.scrollHost = new System.Windows.Forms.Panel();
            this.dashboardCanvas = new System.Windows.Forms.Panel();
            this.lblOverview = new System.Windows.Forms.Label();
            this.lblOverviewHint = new System.Windows.Forms.Label();
            this.metricsTable.SuspendLayout();
            this.pnlSalesCard.SuspendLayout();
            this.pnlSalesProfitCard.SuspendLayout();
            this.pnlServiceSalesCard.SuspendLayout();
            this.pnlServiceProfitCard.SuspendLayout();
            this.pnlExpensesCard.SuspendLayout();
            this.pnlLowStockCard.SuspendLayout();
            this.pnlExpiryCard.SuspendLayout();
            this.pnlOrdersCard.SuspendLayout();
            this.actionsGrid.SuspendLayout();
            this.tablesGrid.SuspendLayout();
            this.pnlLowStockSection.SuspendLayout();
            this.lowStockHost.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLowStock)).BeginInit();
            this.pnlRecentSalesSection.SuspendLayout();
            this.recentSalesHost.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentSales)).BeginInit();
            this.pnlStatusSection.SuspendLayout();
            this.pnlActionsSection.SuspendLayout();
            this.pnlTablesSection.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.scrollHost.SuspendLayout();
            this.dashboardCanvas.SuspendLayout();
            this.SuspendLayout();
            // 
            // metricsTable
            // 
            this.metricsTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metricsTable.ColumnCount = 4;
            this.metricsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.metricsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.metricsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.metricsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.metricsTable.Controls.Add(this.pnlSalesCard, 0, 0);
            this.metricsTable.Controls.Add(this.pnlSalesProfitCard, 1, 0);
            this.metricsTable.Controls.Add(this.pnlServiceSalesCard, 2, 0);
            this.metricsTable.Controls.Add(this.pnlServiceProfitCard, 3, 0);
            this.metricsTable.Controls.Add(this.pnlExpensesCard, 0, 1);
            this.metricsTable.Controls.Add(this.pnlLowStockCard, 1, 1);
            this.metricsTable.Controls.Add(this.pnlExpiryCard, 2, 1);
            this.metricsTable.Controls.Add(this.pnlOrdersCard, 3, 1);
            this.metricsTable.Location = new System.Drawing.Point(0, 40);
            this.metricsTable.Name = "metricsTable";
            this.metricsTable.RowCount = 2;
            this.metricsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.metricsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.metricsTable.Size = new System.Drawing.Size(1251, 185);
            this.metricsTable.TabIndex = 2;
            // 
            // pnlSalesCard
            // 
            this.pnlSalesCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(252)))), ((int)(((byte)(254)))));
            this.pnlSalesCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSalesCard.Controls.Add(this.lblSalesCardTitle);
            this.pnlSalesCard.Controls.Add(this.lblTodaySalesValue);
            this.pnlSalesCard.Controls.Add(this.lblCreditSalesCaption);
            this.pnlSalesCard.Controls.Add(this.lblTodayCreditSalesValue);
            this.pnlSalesCard.Controls.Add(this.lblCashSalesCaption);
            this.pnlSalesCard.Controls.Add(this.lblTodayCashSalesValue);
            this.pnlSalesCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSalesCard.Location = new System.Drawing.Point(5, 5);
            this.pnlSalesCard.Margin = new System.Windows.Forms.Padding(5);
            this.pnlSalesCard.Name = "pnlSalesCard";
            this.pnlSalesCard.Size = new System.Drawing.Size(302, 82);
            this.pnlSalesCard.TabIndex = 0;
            // 
            // lblSalesCardTitle
            // 
            this.lblSalesCardTitle.AutoSize = true;
            this.lblSalesCardTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblSalesCardTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblSalesCardTitle.Location = new System.Drawing.Point(14, 10);
            this.lblSalesCardTitle.Name = "lblSalesCardTitle";
            this.lblSalesCardTitle.Size = new System.Drawing.Size(136, 19);
            this.lblSalesCardTitle.TabIndex = 0;
            this.lblSalesCardTitle.Text = "Today Grocery Sales";
            // 
            // lblTodaySalesValue
            // 
            this.lblTodaySalesValue.AutoSize = true;
            this.lblTodaySalesValue.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblTodaySalesValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(125)))), ((int)(((byte)(68)))));
            this.lblTodaySalesValue.Location = new System.Drawing.Point(14, 29);
            this.lblTodaySalesValue.Name = "lblTodaySalesValue";
            this.lblTodaySalesValue.Size = new System.Drawing.Size(98, 32);
            this.lblTodaySalesValue.TabIndex = 1;
            this.lblTodaySalesValue.Text = "Rs. 0.00";
            // 
            // lblCreditSalesCaption
            // 
            this.lblCreditSalesCaption.AutoSize = true;
            this.lblCreditSalesCaption.Font = new System.Drawing.Font("Segoe UI Semibold", 8.75F, System.Drawing.FontStyle.Bold);
            this.lblCreditSalesCaption.ForeColor = System.Drawing.Color.DimGray;
            this.lblCreditSalesCaption.Location = new System.Drawing.Point(156, 12);
            this.lblCreditSalesCaption.Name = "lblCreditSalesCaption";
            this.lblCreditSalesCaption.Size = new System.Drawing.Size(68, 15);
            this.lblCreditSalesCaption.TabIndex = 2;
            this.lblCreditSalesCaption.Text = "Credit Sales";
            // 
            // lblTodayCreditSalesValue
            // 
            this.lblTodayCreditSalesValue.AutoSize = true;
            this.lblTodayCreditSalesValue.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this.lblTodayCreditSalesValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(72)))), ((int)(((byte)(122)))));
            this.lblTodayCreditSalesValue.Location = new System.Drawing.Point(230, 12);
            this.lblTodayCreditSalesValue.Name = "lblTodayCreditSalesValue";
            this.lblTodayCreditSalesValue.Size = new System.Drawing.Size(46, 15);
            this.lblTodayCreditSalesValue.TabIndex = 3;
            this.lblTodayCreditSalesValue.Text = "Rs. 0.00";
            // 
            // lblCashSalesCaption
            // 
            this.lblCashSalesCaption.AutoSize = true;
            this.lblCashSalesCaption.Font = new System.Drawing.Font("Segoe UI Semibold", 8.75F, System.Drawing.FontStyle.Bold);
            this.lblCashSalesCaption.ForeColor = System.Drawing.Color.DimGray;
            this.lblCashSalesCaption.Location = new System.Drawing.Point(156, 29);
            this.lblCashSalesCaption.Name = "lblCashSalesCaption";
            this.lblCashSalesCaption.Size = new System.Drawing.Size(62, 15);
            this.lblCashSalesCaption.TabIndex = 4;
            this.lblCashSalesCaption.Text = "Cash Sales";
            // 
            // lblTodayCashSalesValue
            // 
            this.lblTodayCashSalesValue.AutoSize = true;
            this.lblTodayCashSalesValue.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this.lblTodayCashSalesValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(125)))), ((int)(((byte)(68)))));
            this.lblTodayCashSalesValue.Location = new System.Drawing.Point(230, 29);
            this.lblTodayCashSalesValue.Name = "lblTodayCashSalesValue";
            this.lblTodayCashSalesValue.Size = new System.Drawing.Size(46, 15);
            this.lblTodayCashSalesValue.TabIndex = 5;
            this.lblTodayCashSalesValue.Text = "Rs. 0.00";
            // 
            // pnlSalesProfitCard
            // 
            this.pnlSalesProfitCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(252)))), ((int)(((byte)(254)))));
            this.pnlSalesProfitCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSalesProfitCard.Controls.Add(this.lblSalesProfitTitle);
            this.pnlSalesProfitCard.Controls.Add(this.lblTodaySalesProfitValue);
            this.pnlSalesProfitCard.Controls.Add(this.lblSalesProfitMeta);
            this.pnlSalesProfitCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSalesProfitCard.Location = new System.Drawing.Point(317, 5);
            this.pnlSalesProfitCard.Margin = new System.Windows.Forms.Padding(5);
            this.pnlSalesProfitCard.Name = "pnlSalesProfitCard";
            this.pnlSalesProfitCard.Size = new System.Drawing.Size(302, 82);
            this.pnlSalesProfitCard.TabIndex = 1;
            // 
            // lblSalesProfitTitle
            // 
            this.lblSalesProfitTitle.AutoSize = true;
            this.lblSalesProfitTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblSalesProfitTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblSalesProfitTitle.Location = new System.Drawing.Point(14, 10);
            this.lblSalesProfitTitle.Name = "lblSalesProfitTitle";
            this.lblSalesProfitTitle.Size = new System.Drawing.Size(139, 19);
            this.lblSalesProfitTitle.TabIndex = 0;
            this.lblSalesProfitTitle.Text = "Today Grocery Profit";
            // 
            // lblTodaySalesProfitValue
            // 
            this.lblTodaySalesProfitValue.AutoSize = true;
            this.lblTodaySalesProfitValue.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblTodaySalesProfitValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(110)))), ((int)(((byte)(241)))));
            this.lblTodaySalesProfitValue.Location = new System.Drawing.Point(14, 29);
            this.lblTodaySalesProfitValue.Name = "lblTodaySalesProfitValue";
            this.lblTodaySalesProfitValue.Size = new System.Drawing.Size(98, 32);
            this.lblTodaySalesProfitValue.TabIndex = 1;
            this.lblTodaySalesProfitValue.Text = "Rs. 0.00";
            // 
            // lblSalesProfitMeta
            // 
            this.lblSalesProfitMeta.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this.lblSalesProfitMeta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(120)))), ((int)(((byte)(131)))));
            this.lblSalesProfitMeta.Location = new System.Drawing.Point(14, 59);
            this.lblSalesProfitMeta.Name = "lblSalesProfitMeta";
            this.lblSalesProfitMeta.Size = new System.Drawing.Size(230, 21);
            this.lblSalesProfitMeta.TabIndex = 2;
            this.lblSalesProfitMeta.Text = "Gross profit from sold grocery items today.";
            // 
            // pnlServiceSalesCard
            // 
            this.pnlServiceSalesCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(252)))), ((int)(((byte)(254)))));
            this.pnlServiceSalesCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlServiceSalesCard.Controls.Add(this.lblServiceSalesTitle);
            this.pnlServiceSalesCard.Controls.Add(this.lblTodayServiceSalesValue);
            this.pnlServiceSalesCard.Controls.Add(this.lblServiceSalesMeta);
            this.pnlServiceSalesCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlServiceSalesCard.Location = new System.Drawing.Point(629, 5);
            this.pnlServiceSalesCard.Margin = new System.Windows.Forms.Padding(5);
            this.pnlServiceSalesCard.Name = "pnlServiceSalesCard";
            this.pnlServiceSalesCard.Size = new System.Drawing.Size(302, 82);
            this.pnlServiceSalesCard.TabIndex = 2;
            // 
            // lblServiceSalesTitle
            // 
            this.lblServiceSalesTitle.AutoSize = true;
            this.lblServiceSalesTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblServiceSalesTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblServiceSalesTitle.Location = new System.Drawing.Point(14, 10);
            this.lblServiceSalesTitle.Name = "lblServiceSalesTitle";
            this.lblServiceSalesTitle.Size = new System.Drawing.Size(132, 19);
            this.lblServiceSalesTitle.TabIndex = 0;
            this.lblServiceSalesTitle.Text = "Today Service Sales";
            // 
            // lblTodayServiceSalesValue
            // 
            this.lblTodayServiceSalesValue.AutoSize = true;
            this.lblTodayServiceSalesValue.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblTodayServiceSalesValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(129)))), ((int)(((byte)(84)))), ((int)(((byte)(201)))));
            this.lblTodayServiceSalesValue.Location = new System.Drawing.Point(14, 29);
            this.lblTodayServiceSalesValue.Name = "lblTodayServiceSalesValue";
            this.lblTodayServiceSalesValue.Size = new System.Drawing.Size(98, 32);
            this.lblTodayServiceSalesValue.TabIndex = 1;
            this.lblTodayServiceSalesValue.Text = "Rs. 0.00";
            // 
            // lblServiceSalesMeta
            // 
            this.lblServiceSalesMeta.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this.lblServiceSalesMeta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(120)))), ((int)(((byte)(131)))));
            this.lblServiceSalesMeta.Location = new System.Drawing.Point(14, 59);
            this.lblServiceSalesMeta.Name = "lblServiceSalesMeta";
            this.lblServiceSalesMeta.Size = new System.Drawing.Size(230, 21);
            this.lblServiceSalesMeta.TabIndex = 2;
            this.lblServiceSalesMeta.Text = "Completed service volume recorded today.";
            // 
            // pnlServiceProfitCard
            // 
            this.pnlServiceProfitCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(252)))), ((int)(((byte)(254)))));
            this.pnlServiceProfitCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlServiceProfitCard.Controls.Add(this.lblServiceProfitTitle);
            this.pnlServiceProfitCard.Controls.Add(this.lblTodayServiceProfitValue);
            this.pnlServiceProfitCard.Controls.Add(this.lblServiceProfitMeta);
            this.pnlServiceProfitCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlServiceProfitCard.Location = new System.Drawing.Point(941, 5);
            this.pnlServiceProfitCard.Margin = new System.Windows.Forms.Padding(5);
            this.pnlServiceProfitCard.Name = "pnlServiceProfitCard";
            this.pnlServiceProfitCard.Size = new System.Drawing.Size(305, 82);
            this.pnlServiceProfitCard.TabIndex = 3;
            // 
            // lblServiceProfitTitle
            // 
            this.lblServiceProfitTitle.AutoSize = true;
            this.lblServiceProfitTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblServiceProfitTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblServiceProfitTitle.Location = new System.Drawing.Point(14, 10);
            this.lblServiceProfitTitle.Name = "lblServiceProfitTitle";
            this.lblServiceProfitTitle.Size = new System.Drawing.Size(135, 19);
            this.lblServiceProfitTitle.TabIndex = 0;
            this.lblServiceProfitTitle.Text = "Today Service Profit";
            // 
            // lblTodayServiceProfitValue
            // 
            this.lblTodayServiceProfitValue.AutoSize = true;
            this.lblTodayServiceProfitValue.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblTodayServiceProfitValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(74)))), ((int)(((byte)(42)))));
            this.lblTodayServiceProfitValue.Location = new System.Drawing.Point(14, 29);
            this.lblTodayServiceProfitValue.Name = "lblTodayServiceProfitValue";
            this.lblTodayServiceProfitValue.Size = new System.Drawing.Size(98, 32);
            this.lblTodayServiceProfitValue.TabIndex = 1;
            this.lblTodayServiceProfitValue.Text = "Rs. 0.00";
            // 
            // lblServiceProfitMeta
            // 
            this.lblServiceProfitMeta.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this.lblServiceProfitMeta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(120)))), ((int)(((byte)(131)))));
            this.lblServiceProfitMeta.Location = new System.Drawing.Point(14, 59);
            this.lblServiceProfitMeta.Name = "lblServiceProfitMeta";
            this.lblServiceProfitMeta.Size = new System.Drawing.Size(230, 21);
            this.lblServiceProfitMeta.TabIndex = 2;
            this.lblServiceProfitMeta.Text = "Commission earned from services today.";
            // 
            // pnlExpensesCard
            // 
            this.pnlExpensesCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(252)))), ((int)(((byte)(254)))));
            this.pnlExpensesCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlExpensesCard.Controls.Add(this.lblExpensesTitle);
            this.pnlExpensesCard.Controls.Add(this.lblTodayExpensesValue);
            this.pnlExpensesCard.Controls.Add(this.lblExpensesMeta);
            this.pnlExpensesCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlExpensesCard.Location = new System.Drawing.Point(5, 97);
            this.pnlExpensesCard.Margin = new System.Windows.Forms.Padding(5);
            this.pnlExpensesCard.Name = "pnlExpensesCard";
            this.pnlExpensesCard.Size = new System.Drawing.Size(302, 83);
            this.pnlExpensesCard.TabIndex = 4;
            // 
            // lblExpensesTitle
            // 
            this.lblExpensesTitle.AutoSize = true;
            this.lblExpensesTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblExpensesTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblExpensesTitle.Location = new System.Drawing.Point(14, 10);
            this.lblExpensesTitle.Name = "lblExpensesTitle";
            this.lblExpensesTitle.Size = new System.Drawing.Size(106, 19);
            this.lblExpensesTitle.TabIndex = 0;
            this.lblExpensesTitle.Text = "Today Expenses";
            // 
            // lblTodayExpensesValue
            // 
            this.lblTodayExpensesValue.AutoSize = true;
            this.lblTodayExpensesValue.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblTodayExpensesValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(70)))), ((int)(((byte)(74)))));
            this.lblTodayExpensesValue.Location = new System.Drawing.Point(14, 29);
            this.lblTodayExpensesValue.Name = "lblTodayExpensesValue";
            this.lblTodayExpensesValue.Size = new System.Drawing.Size(98, 32);
            this.lblTodayExpensesValue.TabIndex = 1;
            this.lblTodayExpensesValue.Text = "Rs. 0.00";
            // 
            // lblExpensesMeta
            // 
            this.lblExpensesMeta.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this.lblExpensesMeta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(120)))), ((int)(((byte)(131)))));
            this.lblExpensesMeta.Location = new System.Drawing.Point(14, 59);
            this.lblExpensesMeta.Name = "lblExpensesMeta";
            this.lblExpensesMeta.Size = new System.Drawing.Size(230, 21);
            this.lblExpensesMeta.TabIndex = 2;
            this.lblExpensesMeta.Text = "Expenses booked during today.";
            // 
            // pnlLowStockCard
            // 
            this.pnlLowStockCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(252)))), ((int)(((byte)(254)))));
            this.pnlLowStockCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLowStockCard.Controls.Add(this.lblLowStockTitle);
            this.pnlLowStockCard.Controls.Add(this.lblLowStockMeta);
            this.pnlLowStockCard.Controls.Add(this.lblLowStockValue);
            this.pnlLowStockCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLowStockCard.Location = new System.Drawing.Point(317, 97);
            this.pnlLowStockCard.Margin = new System.Windows.Forms.Padding(5);
            this.pnlLowStockCard.Name = "pnlLowStockCard";
            this.pnlLowStockCard.Size = new System.Drawing.Size(302, 83);
            this.pnlLowStockCard.TabIndex = 5;
            // 
            // lblLowStockTitle
            // 
            this.lblLowStockTitle.AutoSize = true;
            this.lblLowStockTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblLowStockTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblLowStockTitle.Location = new System.Drawing.Point(14, 10);
            this.lblLowStockTitle.Name = "lblLowStockTitle";
            this.lblLowStockTitle.Size = new System.Drawing.Size(114, 19);
            this.lblLowStockTitle.TabIndex = 0;
            this.lblLowStockTitle.Text = "Low Stock Alerts";
            // 
            // lblLowStockMeta
            // 
            this.lblLowStockMeta.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this.lblLowStockMeta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(120)))), ((int)(((byte)(131)))));
            this.lblLowStockMeta.Location = new System.Drawing.Point(14, 59);
            this.lblLowStockMeta.Name = "lblLowStockMeta";
            this.lblLowStockMeta.Size = new System.Drawing.Size(230, 21);
            this.lblLowStockMeta.TabIndex = 2;
            this.lblLowStockMeta.Text = "Products at or below reorder level.";
            // 
            // lblLowStockValue
            // 
            this.lblLowStockValue.AutoSize = true;
            this.lblLowStockValue.Font = new System.Drawing.Font("Segoe UI Semibold", 23F, System.Drawing.FontStyle.Bold);
            this.lblLowStockValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(132)))), ((int)(((byte)(34)))));
            this.lblLowStockValue.Location = new System.Drawing.Point(14, 24);
            this.lblLowStockValue.Name = "lblLowStockValue";
            this.lblLowStockValue.Size = new System.Drawing.Size(35, 42);
            this.lblLowStockValue.TabIndex = 1;
            this.lblLowStockValue.Text = "0";
            // 
            // pnlExpiryCard
            // 
            this.pnlExpiryCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(252)))), ((int)(((byte)(254)))));
            this.pnlExpiryCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlExpiryCard.Controls.Add(this.lblExpiryTitle);
            this.pnlExpiryCard.Controls.Add(this.lblExpiryAttentionMeta);
            this.pnlExpiryCard.Controls.Add(this.lblExpiryAttentionValue);
            this.pnlExpiryCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlExpiryCard.Location = new System.Drawing.Point(629, 97);
            this.pnlExpiryCard.Margin = new System.Windows.Forms.Padding(5);
            this.pnlExpiryCard.Name = "pnlExpiryCard";
            this.pnlExpiryCard.Size = new System.Drawing.Size(302, 83);
            this.pnlExpiryCard.TabIndex = 6;
            // 
            // lblExpiryTitle
            // 
            this.lblExpiryTitle.AutoSize = true;
            this.lblExpiryTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblExpiryTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblExpiryTitle.Location = new System.Drawing.Point(14, 10);
            this.lblExpiryTitle.Name = "lblExpiryTitle";
            this.lblExpiryTitle.Size = new System.Drawing.Size(111, 19);
            this.lblExpiryTitle.TabIndex = 0;
            this.lblExpiryTitle.Text = "Expiry Attention";
            // 
            // lblExpiryAttentionMeta
            // 
            this.lblExpiryAttentionMeta.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this.lblExpiryAttentionMeta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(120)))), ((int)(((byte)(131)))));
            this.lblExpiryAttentionMeta.Location = new System.Drawing.Point(14, 58);
            this.lblExpiryAttentionMeta.Name = "lblExpiryAttentionMeta";
            this.lblExpiryAttentionMeta.Size = new System.Drawing.Size(230, 24);
            this.lblExpiryAttentionMeta.TabIndex = 2;
            this.lblExpiryAttentionMeta.Text = "No expiring batches or pending expired-stock actions.";
            // 
            // lblExpiryAttentionValue
            // 
            this.lblExpiryAttentionValue.AutoSize = true;
            this.lblExpiryAttentionValue.Font = new System.Drawing.Font("Segoe UI Semibold", 23F, System.Drawing.FontStyle.Bold);
            this.lblExpiryAttentionValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(68)))), ((int)(((byte)(33)))));
            this.lblExpiryAttentionValue.Location = new System.Drawing.Point(14, 26);
            this.lblExpiryAttentionValue.Name = "lblExpiryAttentionValue";
            this.lblExpiryAttentionValue.Size = new System.Drawing.Size(35, 42);
            this.lblExpiryAttentionValue.TabIndex = 1;
            this.lblExpiryAttentionValue.Text = "0";
            // 
            // pnlOrdersCard
            // 
            this.pnlOrdersCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(252)))), ((int)(((byte)(254)))));
            this.pnlOrdersCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlOrdersCard.Controls.Add(this.lblOrdersTitle);
            this.pnlOrdersCard.Controls.Add(this.lblTodayOrdersValue);
            this.pnlOrdersCard.Controls.Add(this.lblOrdersMeta);
            this.pnlOrdersCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOrdersCard.Location = new System.Drawing.Point(941, 97);
            this.pnlOrdersCard.Margin = new System.Windows.Forms.Padding(5);
            this.pnlOrdersCard.Name = "pnlOrdersCard";
            this.pnlOrdersCard.Size = new System.Drawing.Size(305, 83);
            this.pnlOrdersCard.TabIndex = 7;
            // 
            // lblOrdersTitle
            // 
            this.lblOrdersTitle.AutoSize = true;
            this.lblOrdersTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblOrdersTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblOrdersTitle.Location = new System.Drawing.Point(14, 10);
            this.lblOrdersTitle.Name = "lblOrdersTitle";
            this.lblOrdersTitle.Size = new System.Drawing.Size(92, 19);
            this.lblOrdersTitle.TabIndex = 0;
            this.lblOrdersTitle.Text = "Today Orders";
            // 
            // lblTodayOrdersValue
            // 
            this.lblTodayOrdersValue.AutoSize = true;
            this.lblTodayOrdersValue.Font = new System.Drawing.Font("Segoe UI Semibold", 23F, System.Drawing.FontStyle.Bold);
            this.lblTodayOrdersValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(139)))), ((int)(((byte)(117)))));
            this.lblTodayOrdersValue.Location = new System.Drawing.Point(14, 24);
            this.lblTodayOrdersValue.Name = "lblTodayOrdersValue";
            this.lblTodayOrdersValue.Size = new System.Drawing.Size(35, 42);
            this.lblTodayOrdersValue.TabIndex = 1;
            this.lblTodayOrdersValue.Text = "0";
            // 
            // lblOrdersMeta
            // 
            this.lblOrdersMeta.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this.lblOrdersMeta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(120)))), ((int)(((byte)(131)))));
            this.lblOrdersMeta.Location = new System.Drawing.Point(14, 59);
            this.lblOrdersMeta.Name = "lblOrdersMeta";
            this.lblOrdersMeta.Size = new System.Drawing.Size(230, 21);
            this.lblOrdersMeta.TabIndex = 2;
            this.lblOrdersMeta.Text = "Total grocery sale invoices today.";
            // 
            // actionsGrid
            // 
            this.actionsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.actionsGrid.ColumnCount = 4;
            this.actionsGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.actionsGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.actionsGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.actionsGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.actionsGrid.Controls.Add(this.btnNewSalePrimary, 0, 0);
            this.actionsGrid.Controls.Add(this.btnRecentSalesPrimary, 1, 0);
            this.actionsGrid.Controls.Add(this.btnCustomerPrimary, 2, 0);
            this.actionsGrid.Controls.Add(this.btnServicePrimary, 3, 0);
            this.actionsGrid.Controls.Add(this.btnServiceTransactionsPrimary, 0, 1);
            this.actionsGrid.Controls.Add(this.btnStockPrimary, 1, 1);
            this.actionsGrid.Controls.Add(this.btnProductPrimary, 2, 1);
            this.actionsGrid.Controls.Add(this.btnPurchasePrimary, 3, 1);
            this.actionsGrid.Controls.Add(this.btnVendorPrimary, 0, 2);
            this.actionsGrid.Controls.Add(this.btnVendorPaymentPrimary, 1, 2);
            this.actionsGrid.Controls.Add(this.btnExpensePrimary, 2, 2);
            this.actionsGrid.Controls.Add(this.btnExpiryPrimary, 3, 2);
            this.actionsGrid.Controls.Add(this.btnAccountsPrimary, 0, 3);
            this.actionsGrid.Location = new System.Drawing.Point(14, 54);
            this.actionsGrid.Name = "actionsGrid";
            this.actionsGrid.RowCount = 4;
            this.actionsGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.actionsGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.actionsGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.actionsGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.actionsGrid.Size = new System.Drawing.Size(1221, 180);
            this.actionsGrid.TabIndex = 2;
            // 
            // btnNewSalePrimary
            // 
            this.btnNewSalePrimary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(125)))), ((int)(((byte)(68)))));
            this.btnNewSalePrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNewSalePrimary.FlatAppearance.BorderSize = 0;
            this.btnNewSalePrimary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewSalePrimary.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnNewSalePrimary.ForeColor = System.Drawing.Color.White;
            this.btnNewSalePrimary.Location = new System.Drawing.Point(7, 7);
            this.btnNewSalePrimary.Margin = new System.Windows.Forms.Padding(7);
            this.btnNewSalePrimary.Name = "btnNewSalePrimary";
            this.btnNewSalePrimary.Size = new System.Drawing.Size(291, 31);
            this.btnNewSalePrimary.TabIndex = 0;
            this.btnNewSalePrimary.Text = "Sales Screen";
            this.btnNewSalePrimary.UseVisualStyleBackColor = false;
            this.btnNewSalePrimary.Click += new System.EventHandler(this.btnNewSale_Click);
            // 
            // btnRecentSalesPrimary
            // 
            this.btnRecentSalesPrimary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(89)))), ((int)(((byte)(152)))));
            this.btnRecentSalesPrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRecentSalesPrimary.FlatAppearance.BorderSize = 0;
            this.btnRecentSalesPrimary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecentSalesPrimary.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnRecentSalesPrimary.ForeColor = System.Drawing.Color.White;
            this.btnRecentSalesPrimary.Location = new System.Drawing.Point(312, 7);
            this.btnRecentSalesPrimary.Margin = new System.Windows.Forms.Padding(7);
            this.btnRecentSalesPrimary.Name = "btnRecentSalesPrimary";
            this.btnRecentSalesPrimary.Size = new System.Drawing.Size(291, 31);
            this.btnRecentSalesPrimary.TabIndex = 1;
            this.btnRecentSalesPrimary.Text = "Recent Sales";
            this.btnRecentSalesPrimary.UseVisualStyleBackColor = false;
            this.btnRecentSalesPrimary.Click += new System.EventHandler(this.btnRecentSalesPrimary_Click);
            // 
            // btnCustomerPrimary
            // 
            this.btnCustomerPrimary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(108)))), ((int)(((byte)(83)))));
            this.btnCustomerPrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCustomerPrimary.FlatAppearance.BorderSize = 0;
            this.btnCustomerPrimary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomerPrimary.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnCustomerPrimary.ForeColor = System.Drawing.Color.White;
            this.btnCustomerPrimary.Location = new System.Drawing.Point(617, 7);
            this.btnCustomerPrimary.Margin = new System.Windows.Forms.Padding(7);
            this.btnCustomerPrimary.Name = "btnCustomerPrimary";
            this.btnCustomerPrimary.Size = new System.Drawing.Size(291, 31);
            this.btnCustomerPrimary.TabIndex = 2;
            this.btnCustomerPrimary.Text = "Customers";
            this.btnCustomerPrimary.UseVisualStyleBackColor = false;
            this.btnCustomerPrimary.Click += new System.EventHandler(this.btnCustomerPrimary_Click);
            // 
            // btnServicePrimary
            // 
            this.btnServicePrimary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(44)))), ((int)(((byte)(145)))));
            this.btnServicePrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnServicePrimary.FlatAppearance.BorderSize = 0;
            this.btnServicePrimary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnServicePrimary.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnServicePrimary.ForeColor = System.Drawing.Color.White;
            this.btnServicePrimary.Location = new System.Drawing.Point(922, 7);
            this.btnServicePrimary.Margin = new System.Windows.Forms.Padding(7);
            this.btnServicePrimary.Name = "btnServicePrimary";
            this.btnServicePrimary.Size = new System.Drawing.Size(292, 31);
            this.btnServicePrimary.TabIndex = 3;
            this.btnServicePrimary.Text = "Service Center";
            this.btnServicePrimary.UseVisualStyleBackColor = false;
            this.btnServicePrimary.Click += new System.EventHandler(this.btnServicePrimary_Click);
            // 
            // btnServiceTransactionsPrimary
            // 
            this.btnServiceTransactionsPrimary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(36)))), ((int)(((byte)(117)))));
            this.btnServiceTransactionsPrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnServiceTransactionsPrimary.FlatAppearance.BorderSize = 0;
            this.btnServiceTransactionsPrimary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnServiceTransactionsPrimary.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnServiceTransactionsPrimary.ForeColor = System.Drawing.Color.White;
            this.btnServiceTransactionsPrimary.Location = new System.Drawing.Point(7, 52);
            this.btnServiceTransactionsPrimary.Margin = new System.Windows.Forms.Padding(7);
            this.btnServiceTransactionsPrimary.Name = "btnServiceTransactionsPrimary";
            this.btnServiceTransactionsPrimary.Size = new System.Drawing.Size(291, 31);
            this.btnServiceTransactionsPrimary.TabIndex = 4;
            this.btnServiceTransactionsPrimary.Text = "Service History";
            this.btnServiceTransactionsPrimary.UseVisualStyleBackColor = false;
            this.btnServiceTransactionsPrimary.Click += new System.EventHandler(this.btnServiceTransactionsPrimary_Click);
            // 
            // btnStockPrimary
            // 
            this.btnStockPrimary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(43)))), ((int)(((byte)(130)))));
            this.btnStockPrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStockPrimary.FlatAppearance.BorderSize = 0;
            this.btnStockPrimary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStockPrimary.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnStockPrimary.ForeColor = System.Drawing.Color.White;
            this.btnStockPrimary.Location = new System.Drawing.Point(312, 52);
            this.btnStockPrimary.Margin = new System.Windows.Forms.Padding(7);
            this.btnStockPrimary.Name = "btnStockPrimary";
            this.btnStockPrimary.Size = new System.Drawing.Size(291, 31);
            this.btnStockPrimary.TabIndex = 5;
            this.btnStockPrimary.Text = "Stock";
            this.btnStockPrimary.UseVisualStyleBackColor = false;
            this.btnStockPrimary.Click += new System.EventHandler(this.btnStockPrimary_Click);
            // 
            // btnProductPrimary
            // 
            this.btnProductPrimary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(106)))), ((int)(((byte)(33)))));
            this.btnProductPrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProductPrimary.FlatAppearance.BorderSize = 0;
            this.btnProductPrimary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProductPrimary.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnProductPrimary.ForeColor = System.Drawing.Color.White;
            this.btnProductPrimary.Location = new System.Drawing.Point(617, 52);
            this.btnProductPrimary.Margin = new System.Windows.Forms.Padding(7);
            this.btnProductPrimary.Name = "btnProductPrimary";
            this.btnProductPrimary.Size = new System.Drawing.Size(291, 31);
            this.btnProductPrimary.TabIndex = 6;
            this.btnProductPrimary.Text = "Products";
            this.btnProductPrimary.UseVisualStyleBackColor = false;
            this.btnProductPrimary.Click += new System.EventHandler(this.btnProductPrimary_Click);
            // 
            // btnPurchasePrimary
            // 
            this.btnPurchasePrimary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(145)))), ((int)(((byte)(96)))), ((int)(((byte)(42)))));
            this.btnPurchasePrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPurchasePrimary.FlatAppearance.BorderSize = 0;
            this.btnPurchasePrimary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPurchasePrimary.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnPurchasePrimary.ForeColor = System.Drawing.Color.White;
            this.btnPurchasePrimary.Location = new System.Drawing.Point(922, 52);
            this.btnPurchasePrimary.Margin = new System.Windows.Forms.Padding(7);
            this.btnPurchasePrimary.Name = "btnPurchasePrimary";
            this.btnPurchasePrimary.Size = new System.Drawing.Size(292, 31);
            this.btnPurchasePrimary.TabIndex = 7;
            this.btnPurchasePrimary.Text = "Purchases";
            this.btnPurchasePrimary.UseVisualStyleBackColor = false;
            this.btnPurchasePrimary.Click += new System.EventHandler(this.btnPurchasePrimary_Click);
            // 
            // btnVendorPrimary
            // 
            this.btnVendorPrimary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(84)))), ((int)(((byte)(46)))));
            this.btnVendorPrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVendorPrimary.FlatAppearance.BorderSize = 0;
            this.btnVendorPrimary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVendorPrimary.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnVendorPrimary.ForeColor = System.Drawing.Color.White;
            this.btnVendorPrimary.Location = new System.Drawing.Point(7, 97);
            this.btnVendorPrimary.Margin = new System.Windows.Forms.Padding(7);
            this.btnVendorPrimary.Name = "btnVendorPrimary";
            this.btnVendorPrimary.Size = new System.Drawing.Size(291, 31);
            this.btnVendorPrimary.TabIndex = 8;
            this.btnVendorPrimary.Text = "Vendors";
            this.btnVendorPrimary.UseVisualStyleBackColor = false;
            this.btnVendorPrimary.Click += new System.EventHandler(this.btnVendorPrimary_Click);
            // 
            // btnVendorPaymentPrimary
            // 
            this.btnVendorPaymentPrimary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(72)))), ((int)(((byte)(56)))));
            this.btnVendorPaymentPrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVendorPaymentPrimary.FlatAppearance.BorderSize = 0;
            this.btnVendorPaymentPrimary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVendorPaymentPrimary.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnVendorPaymentPrimary.ForeColor = System.Drawing.Color.White;
            this.btnVendorPaymentPrimary.Location = new System.Drawing.Point(312, 97);
            this.btnVendorPaymentPrimary.Margin = new System.Windows.Forms.Padding(7);
            this.btnVendorPaymentPrimary.Name = "btnVendorPaymentPrimary";
            this.btnVendorPaymentPrimary.Size = new System.Drawing.Size(291, 31);
            this.btnVendorPaymentPrimary.TabIndex = 9;
            this.btnVendorPaymentPrimary.Text = "Vendor Payments";
            this.btnVendorPaymentPrimary.UseVisualStyleBackColor = false;
            this.btnVendorPaymentPrimary.Click += new System.EventHandler(this.btnVendorPaymentPrimary_Click);
            // 
            // btnExpensePrimary
            // 
            this.btnExpensePrimary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.btnExpensePrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExpensePrimary.FlatAppearance.BorderSize = 0;
            this.btnExpensePrimary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpensePrimary.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnExpensePrimary.ForeColor = System.Drawing.Color.White;
            this.btnExpensePrimary.Location = new System.Drawing.Point(617, 97);
            this.btnExpensePrimary.Margin = new System.Windows.Forms.Padding(7);
            this.btnExpensePrimary.Name = "btnExpensePrimary";
            this.btnExpensePrimary.Size = new System.Drawing.Size(291, 31);
            this.btnExpensePrimary.TabIndex = 10;
            this.btnExpensePrimary.Text = "Expenses";
            this.btnExpensePrimary.UseVisualStyleBackColor = false;
            this.btnExpensePrimary.Click += new System.EventHandler(this.btnExpensePrimary_Click);
            // 
            // btnExpiryPrimary
            // 
            this.btnExpiryPrimary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(68)))), ((int)(((byte)(33)))));
            this.btnExpiryPrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExpiryPrimary.FlatAppearance.BorderSize = 0;
            this.btnExpiryPrimary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpiryPrimary.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnExpiryPrimary.ForeColor = System.Drawing.Color.White;
            this.btnExpiryPrimary.Location = new System.Drawing.Point(922, 97);
            this.btnExpiryPrimary.Margin = new System.Windows.Forms.Padding(7);
            this.btnExpiryPrimary.Name = "btnExpiryPrimary";
            this.btnExpiryPrimary.Size = new System.Drawing.Size(292, 31);
            this.btnExpiryPrimary.TabIndex = 11;
            this.btnExpiryPrimary.Text = "Manage Expiry";
            this.btnExpiryPrimary.UseVisualStyleBackColor = false;
            this.btnExpiryPrimary.Click += new System.EventHandler(this.btnExpiryPrimary_Click);
            // 
            // btnAccountsPrimary
            // 
            this.btnAccountsPrimary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnAccountsPrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAccountsPrimary.FlatAppearance.BorderSize = 0;
            this.btnAccountsPrimary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAccountsPrimary.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnAccountsPrimary.ForeColor = System.Drawing.Color.White;
            this.btnAccountsPrimary.Location = new System.Drawing.Point(7, 142);
            this.btnAccountsPrimary.Margin = new System.Windows.Forms.Padding(7);
            this.btnAccountsPrimary.Name = "btnAccountsPrimary";
            this.btnAccountsPrimary.Size = new System.Drawing.Size(291, 31);
            this.btnAccountsPrimary.TabIndex = 12;
            this.btnAccountsPrimary.Text = "Accounts && P&&L";
            this.btnAccountsPrimary.UseVisualStyleBackColor = false;
            this.btnAccountsPrimary.Click += new System.EventHandler(this.btnAccountsPrimary_Click);
            // 
            // tablesGrid
            // 
            this.tablesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tablesGrid.ColumnCount = 2;
            this.tablesGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tablesGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tablesGrid.Controls.Add(this.pnlLowStockSection, 0, 0);
            this.tablesGrid.Controls.Add(this.pnlRecentSalesSection, 1, 0);
            this.tablesGrid.Location = new System.Drawing.Point(12, 57);
            this.tablesGrid.Name = "tablesGrid";
            this.tablesGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 279F));
            this.tablesGrid.Size = new System.Drawing.Size(1227, 279);
            this.tablesGrid.TabIndex = 2;
            // 
            // pnlLowStockSection
            // 
            this.pnlLowStockSection.BackColor = System.Drawing.Color.White;
            this.pnlLowStockSection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLowStockSection.Controls.Add(this.lblLowStockGridTitle);
            this.pnlLowStockSection.Controls.Add(this.lblLowStockGridHint);
            this.pnlLowStockSection.Controls.Add(this.lowStockHost);
            this.pnlLowStockSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLowStockSection.Location = new System.Drawing.Point(0, 0);
            this.pnlLowStockSection.Margin = new System.Windows.Forms.Padding(0, 0, 9, 0);
            this.pnlLowStockSection.Name = "pnlLowStockSection";
            this.pnlLowStockSection.Size = new System.Drawing.Size(604, 279);
            this.pnlLowStockSection.TabIndex = 0;
            // 
            // lblLowStockGridTitle
            // 
            this.lblLowStockGridTitle.AutoSize = true;
            this.lblLowStockGridTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblLowStockGridTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(56)))), ((int)(((byte)(71)))));
            this.lblLowStockGridTitle.Location = new System.Drawing.Point(12, 7);
            this.lblLowStockGridTitle.Name = "lblLowStockGridTitle";
            this.lblLowStockGridTitle.Size = new System.Drawing.Size(133, 19);
            this.lblLowStockGridTitle.TabIndex = 0;
            this.lblLowStockGridTitle.Text = "Low Stock Products";
            // 
            // lblLowStockGridHint
            // 
            this.lblLowStockGridHint.AutoSize = true;
            this.lblLowStockGridHint.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblLowStockGridHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(120)))), ((int)(((byte)(132)))));
            this.lblLowStockGridHint.Location = new System.Drawing.Point(12, 27);
            this.lblLowStockGridHint.Name = "lblLowStockGridHint";
            this.lblLowStockGridHint.Size = new System.Drawing.Size(241, 15);
            this.lblLowStockGridHint.TabIndex = 1;
            this.lblLowStockGridHint.Text = "Products at or below reorder level right now.";
            // 
            // lowStockHost
            // 
            this.lowStockHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lowStockHost.Controls.Add(this.dgvLowStock);
            this.lowStockHost.Location = new System.Drawing.Point(12, 52);
            this.lowStockHost.Name = "lowStockHost";
            this.lowStockHost.Size = new System.Drawing.Size(578, 210);
            this.lowStockHost.TabIndex = 2;
            // 
            // dgvLowStock
            // 
            this.dgvLowStock.AllowUserToAddRows = false;
            this.dgvLowStock.AllowUserToDeleteRows = false;
            this.dgvLowStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLowStock.Location = new System.Drawing.Point(0, 0);
            this.dgvLowStock.Name = "dgvLowStock";
            this.dgvLowStock.ReadOnly = true;
            this.dgvLowStock.RowHeadersVisible = false;
            this.dgvLowStock.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLowStock.Size = new System.Drawing.Size(578, 210);
            this.dgvLowStock.TabIndex = 0;
            // 
            // pnlRecentSalesSection
            // 
            this.pnlRecentSalesSection.BackColor = System.Drawing.Color.White;
            this.pnlRecentSalesSection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRecentSalesSection.Controls.Add(this.lblRecentSalesGridTitle);
            this.pnlRecentSalesSection.Controls.Add(this.lblRecentSalesGridHint);
            this.pnlRecentSalesSection.Controls.Add(this.recentSalesHost);
            this.pnlRecentSalesSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRecentSalesSection.Location = new System.Drawing.Point(622, 0);
            this.pnlRecentSalesSection.Margin = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.pnlRecentSalesSection.Name = "pnlRecentSalesSection";
            this.pnlRecentSalesSection.Size = new System.Drawing.Size(605, 279);
            this.pnlRecentSalesSection.TabIndex = 1;
            // 
            // lblRecentSalesGridTitle
            // 
            this.lblRecentSalesGridTitle.AutoSize = true;
            this.lblRecentSalesGridTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblRecentSalesGridTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(56)))), ((int)(((byte)(71)))));
            this.lblRecentSalesGridTitle.Location = new System.Drawing.Point(12, 7);
            this.lblRecentSalesGridTitle.Name = "lblRecentSalesGridTitle";
            this.lblRecentSalesGridTitle.Size = new System.Drawing.Size(88, 19);
            this.lblRecentSalesGridTitle.TabIndex = 0;
            this.lblRecentSalesGridTitle.Text = "Recent Sales";
            // 
            // lblRecentSalesGridHint
            // 
            this.lblRecentSalesGridHint.AutoSize = true;
            this.lblRecentSalesGridHint.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblRecentSalesGridHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(120)))), ((int)(((byte)(132)))));
            this.lblRecentSalesGridHint.Location = new System.Drawing.Point(12, 27);
            this.lblRecentSalesGridHint.Name = "lblRecentSalesGridHint";
            this.lblRecentSalesGridHint.Size = new System.Drawing.Size(226, 15);
            this.lblRecentSalesGridHint.TabIndex = 1;
            this.lblRecentSalesGridHint.Text = "Latest grocery sale invoices in the system.";
            // 
            // recentSalesHost
            // 
            this.recentSalesHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.recentSalesHost.Controls.Add(this.dgvRecentSales);
            this.recentSalesHost.Location = new System.Drawing.Point(12, 52);
            this.recentSalesHost.Name = "recentSalesHost";
            this.recentSalesHost.Size = new System.Drawing.Size(578, 210);
            this.recentSalesHost.TabIndex = 2;
            // 
            // dgvRecentSales
            // 
            this.dgvRecentSales.AllowUserToAddRows = false;
            this.dgvRecentSales.AllowUserToDeleteRows = false;
            this.dgvRecentSales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRecentSales.Location = new System.Drawing.Point(0, 0);
            this.dgvRecentSales.Name = "dgvRecentSales";
            this.dgvRecentSales.ReadOnly = true;
            this.dgvRecentSales.RowHeadersVisible = false;
            this.dgvRecentSales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecentSales.Size = new System.Drawing.Size(578, 210);
            this.dgvRecentSales.TabIndex = 0;
            // 
            // lblActionsSectionTitle
            // 
            this.lblActionsSectionTitle.AutoSize = true;
            this.lblActionsSectionTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblActionsSectionTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(56)))), ((int)(((byte)(71)))));
            this.lblActionsSectionTitle.Location = new System.Drawing.Point(14, 12);
            this.lblActionsSectionTitle.Name = "lblActionsSectionTitle";
            this.lblActionsSectionTitle.Size = new System.Drawing.Size(102, 20);
            this.lblActionsSectionTitle.TabIndex = 0;
            this.lblActionsSectionTitle.Text = "Quick Actions";
            // 
            // lblActionsSectionHint
            // 
            this.lblActionsSectionHint.AutoSize = true;
            this.lblActionsSectionHint.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblActionsSectionHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(120)))), ((int)(((byte)(132)))));
            this.lblActionsSectionHint.Location = new System.Drawing.Point(14, 31);
            this.lblActionsSectionHint.Name = "lblActionsSectionHint";
            this.lblActionsSectionHint.Size = new System.Drawing.Size(274, 15);
            this.lblActionsSectionHint.TabIndex = 1;
            this.lblActionsSectionHint.Text = "Open the main working screens directly from here.";
            // 
            // lblOpsSectionTitle
            // 
            this.lblOpsSectionTitle.AutoSize = true;
            this.lblOpsSectionTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblOpsSectionTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(56)))), ((int)(((byte)(71)))));
            this.lblOpsSectionTitle.Location = new System.Drawing.Point(14, 12);
            this.lblOpsSectionTitle.Name = "lblOpsSectionTitle";
            this.lblOpsSectionTitle.Size = new System.Drawing.Size(157, 20);
            this.lblOpsSectionTitle.TabIndex = 0;
            this.lblOpsSectionTitle.Text = "Operational Snapshot";
            // 
            // lblOpsSectionHint
            // 
            this.lblOpsSectionHint.AutoSize = true;
            this.lblOpsSectionHint.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblOpsSectionHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(120)))), ((int)(((byte)(132)))));
            this.lblOpsSectionHint.Location = new System.Drawing.Point(14, 31);
            this.lblOpsSectionHint.Name = "lblOpsSectionHint";
            this.lblOpsSectionHint.Size = new System.Drawing.Size(278, 15);
            this.lblOpsSectionHint.TabIndex = 1;
            this.lblOpsSectionHint.Text = "Low stock and recent grocery invoices in one place.";
            // 
            // pnlStatusSection
            // 
            this.pnlStatusSection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlStatusSection.BackColor = System.Drawing.Color.White;
            this.pnlStatusSection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlStatusSection.Controls.Add(this.lblDashboardStatus);
            this.pnlStatusSection.Controls.Add(this.lblDashboardNote);
            this.pnlStatusSection.Controls.Add(this.lblLastUpdated);
            this.pnlStatusSection.Location = new System.Drawing.Point(0, 236);
            this.pnlStatusSection.Name = "pnlStatusSection";
            this.pnlStatusSection.Size = new System.Drawing.Size(1251, 66);
            this.pnlStatusSection.TabIndex = 3;
            // 
            // lblDashboardStatus
            // 
            this.lblDashboardStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblDashboardStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(52)))), ((int)(((byte)(61)))));
            this.lblDashboardStatus.Location = new System.Drawing.Point(19, 10);
            this.lblDashboardStatus.Name = "lblDashboardStatus";
            this.lblDashboardStatus.Size = new System.Drawing.Size(737, 17);
            this.lblDashboardStatus.TabIndex = 0;
            this.lblDashboardStatus.Text = "Dashboard ready.";
            // 
            // lblDashboardNote
            // 
            this.lblDashboardNote.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDashboardNote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(118)))), ((int)(((byte)(128)))));
            this.lblDashboardNote.Location = new System.Drawing.Point(19, 31);
            this.lblDashboardNote.Name = "lblDashboardNote";
            this.lblDashboardNote.Size = new System.Drawing.Size(737, 17);
            this.lblDashboardNote.TabIndex = 1;
            this.lblDashboardNote.Text = "Use the quick actions below to jump into sales, stock, vendors, services, or acco" +
    "unts.";
            // 
            // lblLastUpdated
            // 
            this.lblLastUpdated.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLastUpdated.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(118)))), ((int)(((byte)(128)))));
            this.lblLastUpdated.Location = new System.Drawing.Point(857, 10);
            this.lblLastUpdated.Name = "lblLastUpdated";
            this.lblLastUpdated.Size = new System.Drawing.Size(286, 17);
            this.lblLastUpdated.TabIndex = 2;
            this.lblLastUpdated.Text = "Last updated: --";
            this.lblLastUpdated.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlActionsSection
            // 
            this.pnlActionsSection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlActionsSection.BackColor = System.Drawing.Color.White;
            this.pnlActionsSection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlActionsSection.Controls.Add(this.lblActionsSectionTitle);
            this.pnlActionsSection.Controls.Add(this.lblActionsSectionHint);
            this.pnlActionsSection.Controls.Add(this.actionsGrid);
            this.pnlActionsSection.Location = new System.Drawing.Point(0, 312);
            this.pnlActionsSection.Name = "pnlActionsSection";
            this.pnlActionsSection.Size = new System.Drawing.Size(1251, 257);
            this.pnlActionsSection.TabIndex = 4;
            // 
            // pnlTablesSection
            // 
            this.pnlTablesSection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTablesSection.BackColor = System.Drawing.Color.White;
            this.pnlTablesSection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTablesSection.Controls.Add(this.lblOpsSectionTitle);
            this.pnlTablesSection.Controls.Add(this.lblOpsSectionHint);
            this.pnlTablesSection.Controls.Add(this.tablesGrid);
            this.pnlTablesSection.Location = new System.Drawing.Point(0, 582);
            this.pnlTablesSection.Name = "pnlTablesSection";
            this.pnlTablesSection.Size = new System.Drawing.Size(1251, 350);
            this.pnlTablesSection.TabIndex = 5;
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(107)))), ((int)(((byte)(83)))));
            this.headerPanel.Controls.Add(this.lblWelcome);
            this.headerPanel.Controls.Add(this.lblRole);
            this.headerPanel.Controls.Add(this.lblUsername);
            this.headerPanel.Controls.Add(this.btnLogout);
            this.headerPanel.Controls.Add(this.btnRefresh);
            this.headerPanel.Controls.Add(this.btnNewSale);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(1174, 102);
            this.headerPanel.TabIndex = 1;
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI Semibold", 21F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.White;
            this.lblWelcome.Location = new System.Drawing.Point(21, 16);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(546, 38);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Arslan Communication and Karyana Store";
            // 
            // lblRole
            // 
            this.lblRole.AutoSize = true;
            this.lblRole.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRole.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblRole.Location = new System.Drawing.Point(23, 54);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(466, 19);
            this.lblRole.TabIndex = 1;
            this.lblRole.Text = "Central dashboard for grocery sales, services, stock, vendors, and accounts.";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblUsername.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblUsername.Location = new System.Drawing.Point(23, 73);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(95, 19);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "Signed in user";
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.Red;
            this.btnLogout.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnLogout.Location = new System.Drawing.Point(1123, 17);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(86, 31);
            this.btnLogout.TabIndex = 3;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnRefresh.Location = new System.Drawing.Point(1030, 17);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(86, 31);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnNewSale
            // 
            this.btnNewSale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(125)))), ((int)(((byte)(68)))));
            this.btnNewSale.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(125)))), ((int)(((byte)(68)))));
            this.btnNewSale.FlatAppearance.BorderSize = 0;
            this.btnNewSale.ForeColor = System.Drawing.Color.Snow;
            this.btnNewSale.Location = new System.Drawing.Point(938, 17);
            this.btnNewSale.Name = "btnNewSale";
            this.btnNewSale.Size = new System.Drawing.Size(86, 31);
            this.btnNewSale.TabIndex = 5;
            this.btnNewSale.Text = "New Sale";
            this.btnNewSale.UseVisualStyleBackColor = false;
            this.btnNewSale.Click += new System.EventHandler(this.btnNewSale_Click);
            // 
            // scrollHost
            // 
            this.scrollHost.AutoScroll = true;
            this.scrollHost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(249)))));
            this.scrollHost.Controls.Add(this.dashboardCanvas);
            this.scrollHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scrollHost.Location = new System.Drawing.Point(0, 102);
            this.scrollHost.Name = "scrollHost";
            this.scrollHost.Size = new System.Drawing.Size(1174, 547);
            this.scrollHost.TabIndex = 0;
            this.scrollHost.Resize += new System.EventHandler(this.scrollHost_Resize);
            // 
            // dashboardCanvas
            // 
            this.dashboardCanvas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dashboardCanvas.Controls.Add(this.lblOverview);
            this.dashboardCanvas.Controls.Add(this.lblOverviewHint);
            this.dashboardCanvas.Controls.Add(this.metricsTable);
            this.dashboardCanvas.Controls.Add(this.pnlStatusSection);
            this.dashboardCanvas.Controls.Add(this.pnlActionsSection);
            this.dashboardCanvas.Controls.Add(this.pnlTablesSection);
            this.dashboardCanvas.Location = new System.Drawing.Point(10, 16);
            this.dashboardCanvas.Name = "dashboardCanvas";
            this.dashboardCanvas.Size = new System.Drawing.Size(1253, 945);
            this.dashboardCanvas.TabIndex = 0;
            // 
            // lblOverview
            // 
            this.lblOverview.AutoSize = true;
            this.lblOverview.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.lblOverview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(48)))), ((int)(((byte)(61)))));
            this.lblOverview.Location = new System.Drawing.Point(0, 0);
            this.lblOverview.Name = "lblOverview";
            this.lblOverview.Size = new System.Drawing.Size(144, 25);
            this.lblOverview.TabIndex = 0;
            this.lblOverview.Text = "Today Overview";
            // 
            // lblOverviewHint
            // 
            this.lblOverviewHint.AutoSize = true;
            this.lblOverviewHint.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblOverviewHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(120)))), ((int)(((byte)(132)))));
            this.lblOverviewHint.Location = new System.Drawing.Point(0, 21);
            this.lblOverviewHint.Name = "lblOverviewHint";
            this.lblOverviewHint.Size = new System.Drawing.Size(401, 15);
            this.lblOverviewHint.TabIndex = 1;
            this.lblOverviewHint.Text = "Important today figures with credit, cash, services, and inventory attention.";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1174, 649);
            this.Controls.Add(this.scrollHost);
            this.Controls.Add(this.headerPanel);
            this.MinimumSize = new System.Drawing.Size(1065, 634);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Grocery POS Dashboard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.metricsTable.ResumeLayout(false);
            this.pnlSalesCard.ResumeLayout(false);
            this.pnlSalesCard.PerformLayout();
            this.pnlSalesProfitCard.ResumeLayout(false);
            this.pnlSalesProfitCard.PerformLayout();
            this.pnlServiceSalesCard.ResumeLayout(false);
            this.pnlServiceSalesCard.PerformLayout();
            this.pnlServiceProfitCard.ResumeLayout(false);
            this.pnlServiceProfitCard.PerformLayout();
            this.pnlExpensesCard.ResumeLayout(false);
            this.pnlExpensesCard.PerformLayout();
            this.pnlLowStockCard.ResumeLayout(false);
            this.pnlLowStockCard.PerformLayout();
            this.pnlExpiryCard.ResumeLayout(false);
            this.pnlExpiryCard.PerformLayout();
            this.pnlOrdersCard.ResumeLayout(false);
            this.pnlOrdersCard.PerformLayout();
            this.actionsGrid.ResumeLayout(false);
            this.tablesGrid.ResumeLayout(false);
            this.pnlLowStockSection.ResumeLayout(false);
            this.pnlLowStockSection.PerformLayout();
            this.lowStockHost.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLowStock)).EndInit();
            this.pnlRecentSalesSection.ResumeLayout(false);
            this.pnlRecentSalesSection.PerformLayout();
            this.recentSalesHost.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentSales)).EndInit();
            this.pnlStatusSection.ResumeLayout(false);
            this.pnlActionsSection.ResumeLayout(false);
            this.pnlActionsSection.PerformLayout();
            this.pnlTablesSection.ResumeLayout(false);
            this.pnlTablesSection.PerformLayout();
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.scrollHost.ResumeLayout(false);
            this.dashboardCanvas.ResumeLayout(false);
            this.dashboardCanvas.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnNewSale;
        private System.Windows.Forms.Panel scrollHost;
        private System.Windows.Forms.Panel dashboardCanvas;
        private System.Windows.Forms.Label lblOverview;
        private System.Windows.Forms.Label lblOverviewHint;
        private System.Windows.Forms.Label lblTodaySalesValue;
        private System.Windows.Forms.Label lblTodayCreditSalesValue;
        private System.Windows.Forms.Label lblTodayCashSalesValue;
        private System.Windows.Forms.Label lblTodaySalesProfitValue;
        private System.Windows.Forms.Label lblTodayServiceSalesValue;
        private System.Windows.Forms.Label lblTodayServiceProfitValue;
        private System.Windows.Forms.Label lblTodayExpensesValue;
        private System.Windows.Forms.Label lblLowStockValue;
        private System.Windows.Forms.Label lblExpiryAttentionValue;
        private System.Windows.Forms.Label lblExpiryAttentionMeta;
        private System.Windows.Forms.Label lblTodayOrdersValue;
        private System.Windows.Forms.Label lblDashboardStatus;
        private System.Windows.Forms.Label lblDashboardNote;
        private System.Windows.Forms.Label lblLastUpdated;
        private System.Windows.Forms.Button btnNewSalePrimary;
        private System.Windows.Forms.Button btnRecentSalesPrimary;
        private System.Windows.Forms.Button btnCustomerPrimary;
        private System.Windows.Forms.Button btnServicePrimary;
        private System.Windows.Forms.Button btnServiceTransactionsPrimary;
        private System.Windows.Forms.Button btnStockPrimary;
        private System.Windows.Forms.Button btnProductPrimary;
        private System.Windows.Forms.Button btnPurchasePrimary;
        private System.Windows.Forms.Button btnVendorPrimary;
        private System.Windows.Forms.Button btnVendorPaymentPrimary;
        private System.Windows.Forms.Button btnExpensePrimary;
        private System.Windows.Forms.Button btnExpiryPrimary;
        private System.Windows.Forms.Button btnAccountsPrimary;
        private System.Windows.Forms.DataGridView dgvLowStock;
        private System.Windows.Forms.DataGridView dgvRecentSales;
        private System.Windows.Forms.TableLayoutPanel metricsTable;
        private System.Windows.Forms.Panel pnlSalesCard;
        private System.Windows.Forms.Label lblSalesCardTitle;
        private System.Windows.Forms.Label lblCreditSalesCaption;
        private System.Windows.Forms.Label lblCashSalesCaption;
        private System.Windows.Forms.Panel pnlSalesProfitCard;
        private System.Windows.Forms.Label lblSalesProfitTitle;
        private System.Windows.Forms.Label lblSalesProfitMeta;
        private System.Windows.Forms.Panel pnlServiceSalesCard;
        private System.Windows.Forms.Label lblServiceSalesTitle;
        private System.Windows.Forms.Label lblServiceSalesMeta;
        private System.Windows.Forms.Panel pnlServiceProfitCard;
        private System.Windows.Forms.Label lblServiceProfitTitle;
        private System.Windows.Forms.Label lblServiceProfitMeta;
        private System.Windows.Forms.Panel pnlExpensesCard;
        private System.Windows.Forms.Label lblExpensesTitle;
        private System.Windows.Forms.Label lblExpensesMeta;
        private System.Windows.Forms.Panel pnlLowStockCard;
        private System.Windows.Forms.Label lblLowStockTitle;
        private System.Windows.Forms.Label lblLowStockMeta;
        private System.Windows.Forms.Panel pnlExpiryCard;
        private System.Windows.Forms.Label lblExpiryTitle;
        private System.Windows.Forms.Panel pnlOrdersCard;
        private System.Windows.Forms.Label lblOrdersTitle;
        private System.Windows.Forms.Label lblOrdersMeta;
        private System.Windows.Forms.TableLayoutPanel actionsGrid;
        private System.Windows.Forms.TableLayoutPanel tablesGrid;
        private System.Windows.Forms.Panel pnlLowStockSection;
        private System.Windows.Forms.Label lblLowStockGridTitle;
        private System.Windows.Forms.Label lblLowStockGridHint;
        private System.Windows.Forms.Panel lowStockHost;
        private System.Windows.Forms.Panel pnlRecentSalesSection;
        private System.Windows.Forms.Label lblRecentSalesGridTitle;
        private System.Windows.Forms.Label lblRecentSalesGridHint;
        private System.Windows.Forms.Panel recentSalesHost;
        private System.Windows.Forms.Label lblActionsSectionTitle;
        private System.Windows.Forms.Label lblActionsSectionHint;
        private System.Windows.Forms.Label lblOpsSectionTitle;
        private System.Windows.Forms.Label lblOpsSectionHint;
        private System.Windows.Forms.Panel pnlStatusSection;
        private System.Windows.Forms.Panel pnlActionsSection;
        private System.Windows.Forms.Panel pnlTablesSection;
    }
}
