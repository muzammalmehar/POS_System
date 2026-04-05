namespace ShopPOS
{
    partial class ServiceCenterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiceCenterForm));
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.lblHeaderSubtitle = new System.Windows.Forms.Label();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.lblSearchTitle = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnOpenTransactionsLeft = new System.Windows.Forms.Button();
            this.btnNewServiceTypeLeft = new System.Windows.Forms.Button();
            this.btnEditServiceTypeLeft = new System.Windows.Forms.Button();
            this.lblTrackedCustomers = new System.Windows.Forms.Label();
            this.dgvProfiles = new System.Windows.Forms.DataGridView();
            this.lblLeftHint = new System.Windows.Forms.Label();
            this.panelRight = new System.Windows.Forms.Panel();
            this.lblMode = new System.Windows.Forms.Label();
            this.lblEntryModeHint = new System.Windows.Forms.Label();
            this.actionBar = new System.Windows.Forms.Panel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSaveService = new System.Windows.Forms.Button();
            this.btnSaveProfile = new System.Windows.Forms.Button();
            this.btnClearProfile = new System.Windows.Forms.Button();
            this.btnRecentTransactionsRight = new System.Windows.Forms.Button();
            this.servicePanel = new System.Windows.Forms.Panel();
            this.lblServicePanelTitle = new System.Windows.Forms.Label();
            this.lblServiceType = new System.Windows.Forms.Label();
            this.cboServiceType = new System.Windows.Forms.ComboBox();
            this.btnInlineNewService = new System.Windows.Forms.Button();
            this.btnInlineEditService = new System.Windows.Forms.Button();
            this.lblWallet = new System.Windows.Forms.Label();
            this.cboWallet = new System.Windows.Forms.ComboBox();
            this.lblServiceSummary = new System.Windows.Forms.Label();
            this.customerPanel = new System.Windows.Forms.Panel();
            this.lblCustomerPanelTitle = new System.Windows.Forms.Label();
            this.chkWalkInCustomer = new System.Windows.Forms.CheckBox();
            this.lblProfileSummary = new System.Windows.Forms.Label();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.lblMobile = new System.Windows.Forms.Label();
            this.txtMobile = new System.Windows.Forms.TextBox();
            this.lblReference = new System.Windows.Forms.Label();
            this.txtReference = new System.Windows.Forms.TextBox();
            this.lblBillCategory = new System.Windows.Forms.Label();
            this.cboBillCategory = new System.Windows.Forms.ComboBox();
            this.transactionPanel = new System.Windows.Forms.Panel();
            this.lblTransactionPanelTitle = new System.Windows.Forms.Label();
            this.lblTxnDate = new System.Windows.Forms.Label();
            this.dtpTxnDate = new System.Windows.Forms.DateTimePicker();
            this.lblAmount = new System.Windows.Forms.Label();
            this.nudAmount = new System.Windows.Forms.NumericUpDown();
            this.lblCommissionTitle = new System.Windows.Forms.Label();
            this.nudCommission = new System.Windows.Forms.NumericUpDown();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.lblExpectedCommissionTitle = new System.Windows.Forms.Label();
            this.lblCommission = new System.Windows.Forms.Label();
            this.lblPaymentMethod = new System.Windows.Forms.Label();
            this.cboPaymentMethod = new System.Windows.Forms.ComboBox();
            this.lblPaymentAccount = new System.Windows.Forms.Label();
            this.txtCustomerAccountNo = new System.Windows.Forms.TextBox();
            this.lblTransactionIdTitle = new System.Windows.Forms.Label();
            this.txtTransactionId = new System.Windows.Forms.TextBox();
            this.chkTransactionIdNotApplicable = new System.Windows.Forms.CheckBox();
            this.billingPanel = new System.Windows.Forms.Panel();
            this.lblBillingPanelTitle = new System.Windows.Forms.Label();
            this.lblRecurrence = new System.Windows.Forms.Label();
            this.cboRecurrence = new System.Windows.Forms.ComboBox();
            this.lblDueDay = new System.Windows.Forms.Label();
            this.nudDueDay = new System.Windows.Forms.NumericUpDown();
            this.chkNextDue = new System.Windows.Forms.CheckBox();
            this.dtpNextDue = new System.Windows.Forms.DateTimePicker();
            this.chkSaveProfile = new System.Windows.Forms.CheckBox();
            this.notesPanel = new System.Windows.Forms.Panel();
            this.lblNotesPanelTitle = new System.Windows.Forms.Label();
            this.lblRemarksTitle = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.lblQuickTip = new System.Windows.Forms.Label();
            this.headerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfiles)).BeginInit();
            this.panelRight.SuspendLayout();
            this.actionBar.SuspendLayout();
            this.servicePanel.SuspendLayout();
            this.customerPanel.SuspendLayout();
            this.transactionPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCommission)).BeginInit();
            this.billingPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDueDay)).BeginInit();
            this.notesPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(44)))), ((int)(((byte)(145)))));
            this.headerPanel.Controls.Add(this.lblHeaderTitle);
            this.headerPanel.Controls.Add(this.lblHeaderSubtitle);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(1370, 92);
            this.headerPanel.TabIndex = 0;
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 20F, System.Drawing.FontStyle.Bold);
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.White;
            this.lblHeaderTitle.Location = new System.Drawing.Point(22, 18);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(193, 37);
            this.lblHeaderTitle.TabIndex = 0;
            this.lblHeaderTitle.Text = "Service Center";
            // 
            // lblHeaderSubtitle
            // 
            this.lblHeaderSubtitle.AutoSize = true;
            this.lblHeaderSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblHeaderSubtitle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblHeaderSubtitle.Location = new System.Drawing.Point(26, 58);
            this.lblHeaderSubtitle.Name = "lblHeaderSubtitle";
            this.lblHeaderSubtitle.Size = new System.Drawing.Size(527, 19);
            this.lblHeaderSubtitle.TabIndex = 1;
            this.lblHeaderSubtitle.Text = "Fast service entry plus recurring customer tracking for monthly bills and repeat " +
    "visits.";
            // 
            // splitMain
            // 
            this.splitMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitMain.Location = new System.Drawing.Point(18, 106);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.panelLeft);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.panelRight);
            this.splitMain.Size = new System.Drawing.Size(1334, 609);
            this.splitMain.SplitterDistance = 433;
            this.splitMain.TabIndex = 1;
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.White;
            this.panelLeft.Controls.Add(this.lblSearchTitle);
            this.panelLeft.Controls.Add(this.txtSearch);
            this.panelLeft.Controls.Add(this.btnOpenTransactionsLeft);
            this.panelLeft.Controls.Add(this.btnNewServiceTypeLeft);
            this.panelLeft.Controls.Add(this.btnEditServiceTypeLeft);
            this.panelLeft.Controls.Add(this.lblTrackedCustomers);
            this.panelLeft.Controls.Add(this.dgvProfiles);
            this.panelLeft.Controls.Add(this.lblLeftHint);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(433, 609);
            this.panelLeft.TabIndex = 0;
            // 
            // lblSearchTitle
            // 
            this.lblSearchTitle.AutoSize = true;
            this.lblSearchTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblSearchTitle.Location = new System.Drawing.Point(16, 14);
            this.lblSearchTitle.Name = "lblSearchTitle";
            this.lblSearchTitle.Size = new System.Drawing.Size(241, 19);
            this.lblSearchTitle.TabIndex = 0;
            this.lblSearchTitle.Text = "Search Customer / Bill Ref / Bill Type";
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtSearch.Location = new System.Drawing.Point(20, 40);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(400, 27);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // btnOpenTransactionsLeft
            // 
            this.btnOpenTransactionsLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(44)))), ((int)(((byte)(145)))));
            this.btnOpenTransactionsLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenTransactionsLeft.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnOpenTransactionsLeft.ForeColor = System.Drawing.Color.White;
            this.btnOpenTransactionsLeft.Location = new System.Drawing.Point(20, 78);
            this.btnOpenTransactionsLeft.Name = "btnOpenTransactionsLeft";
            this.btnOpenTransactionsLeft.Size = new System.Drawing.Size(180, 36);
            this.btnOpenTransactionsLeft.TabIndex = 2;
            this.btnOpenTransactionsLeft.Text = "Recent Transactions";
            this.btnOpenTransactionsLeft.UseVisualStyleBackColor = false;
            this.btnOpenTransactionsLeft.Click += new System.EventHandler(this.btnOpenTransactions_Click);
            // 
            // btnNewServiceTypeLeft
            // 
            this.btnNewServiceTypeLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(129)))), ((int)(((byte)(95)))));
            this.btnNewServiceTypeLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewServiceTypeLeft.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnNewServiceTypeLeft.ForeColor = System.Drawing.Color.White;
            this.btnNewServiceTypeLeft.Location = new System.Drawing.Point(210, 78);
            this.btnNewServiceTypeLeft.Name = "btnNewServiceTypeLeft";
            this.btnNewServiceTypeLeft.Size = new System.Drawing.Size(100, 36);
            this.btnNewServiceTypeLeft.TabIndex = 3;
            this.btnNewServiceTypeLeft.Text = "New Service";
            this.btnNewServiceTypeLeft.UseVisualStyleBackColor = false;
            this.btnNewServiceTypeLeft.Click += new System.EventHandler(this.btnNewServiceType_Click);
            // 
            // btnEditServiceTypeLeft
            // 
            this.btnEditServiceTypeLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditServiceTypeLeft.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnEditServiceTypeLeft.Location = new System.Drawing.Point(320, 78);
            this.btnEditServiceTypeLeft.Name = "btnEditServiceTypeLeft";
            this.btnEditServiceTypeLeft.Size = new System.Drawing.Size(100, 36);
            this.btnEditServiceTypeLeft.TabIndex = 4;
            this.btnEditServiceTypeLeft.Text = "Edit Service";
            this.btnEditServiceTypeLeft.UseVisualStyleBackColor = true;
            this.btnEditServiceTypeLeft.Click += new System.EventHandler(this.btnEditServiceType_Click);
            // 
            // lblTrackedCustomers
            // 
            this.lblTrackedCustomers.AutoSize = true;
            this.lblTrackedCustomers.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblTrackedCustomers.Location = new System.Drawing.Point(16, 126);
            this.lblTrackedCustomers.Name = "lblTrackedCustomers";
            this.lblTrackedCustomers.Size = new System.Drawing.Size(127, 19);
            this.lblTrackedCustomers.TabIndex = 5;
            this.lblTrackedCustomers.Text = "Tracked Customers";
            // 
            // dgvProfiles
            // 
            this.dgvProfiles.AllowUserToAddRows = false;
            this.dgvProfiles.AllowUserToDeleteRows = false;
            this.dgvProfiles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProfiles.BackgroundColor = System.Drawing.Color.White;
            this.dgvProfiles.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvProfiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProfiles.EnableHeadersVisualStyles = false;
            this.dgvProfiles.GridColor = System.Drawing.Color.Gainsboro;
            this.dgvProfiles.Location = new System.Drawing.Point(20, 152);
            this.dgvProfiles.MultiSelect = false;
            this.dgvProfiles.Name = "dgvProfiles";
            this.dgvProfiles.ReadOnly = true;
            this.dgvProfiles.RowHeadersVisible = false;
            this.dgvProfiles.RowTemplate.Height = 30;
            this.dgvProfiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProfiles.Size = new System.Drawing.Size(400, 356);
            this.dgvProfiles.TabIndex = 6;
            this.dgvProfiles.SelectionChanged += new System.EventHandler(this.dgvProfiles_SelectionChanged);
            // 
            // lblLeftHint
            // 
            this.lblLeftHint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(244)))), ((int)(((byte)(252)))));
            this.lblLeftHint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLeftHint.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblLeftHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblLeftHint.Location = new System.Drawing.Point(20, 526);
            this.lblLeftHint.Name = "lblLeftHint";
            this.lblLeftHint.Size = new System.Drawing.Size(400, 138);
            this.lblLeftHint.TabIndex = 7;
            this.lblLeftHint.Text = resources.GetString("lblLeftHint.Text");
            // 
            // panelRight
            // 
            this.panelRight.AutoScroll = true;
            this.panelRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.panelRight.Controls.Add(this.lblMode);
            this.panelRight.Controls.Add(this.lblEntryModeHint);
            this.panelRight.Controls.Add(this.actionBar);
            this.panelRight.Controls.Add(this.servicePanel);
            this.panelRight.Controls.Add(this.customerPanel);
            this.panelRight.Controls.Add(this.transactionPanel);
            this.panelRight.Controls.Add(this.billingPanel);
            this.panelRight.Controls.Add(this.notesPanel);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(0, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(897, 609);
            this.panelRight.TabIndex = 0;
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblMode.Location = new System.Drawing.Point(18, 7);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(169, 25);
            this.lblMode.TabIndex = 0;
            this.lblMode.Text = "New Service Entry";
            // 
            // lblEntryModeHint
            // 
            this.lblEntryModeHint.AutoSize = true;
            this.lblEntryModeHint.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblEntryModeHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblEntryModeHint.Location = new System.Drawing.Point(20, 29);
            this.lblEntryModeHint.Name = "lblEntryModeHint";
            this.lblEntryModeHint.Size = new System.Drawing.Size(529, 17);
            this.lblEntryModeHint.TabIndex = 1;
            this.lblEntryModeHint.Text = "Use this form for both quick walk-in service entries and recurring bill-payment c" +
    "ustomers.";
            // 
            // actionBar
            // 
            this.actionBar.BackColor = System.Drawing.Color.White;
            this.actionBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.actionBar.Controls.Add(this.btnNew);
            this.actionBar.Controls.Add(this.btnSaveService);
            this.actionBar.Controls.Add(this.btnSaveProfile);
            this.actionBar.Controls.Add(this.btnClearProfile);
            this.actionBar.Controls.Add(this.btnRecentTransactionsRight);
            this.actionBar.Location = new System.Drawing.Point(18, 54);
            this.actionBar.Name = "actionBar";
            this.actionBar.Size = new System.Drawing.Size(760, 58);
            this.actionBar.TabIndex = 2;
            // 
            // btnNew
            // 
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnNew.Location = new System.Drawing.Point(14, 11);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(90, 36);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSaveService
            // 
            this.btnSaveService.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(44)))), ((int)(((byte)(145)))));
            this.btnSaveService.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveService.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnSaveService.ForeColor = System.Drawing.Color.White;
            this.btnSaveService.Location = new System.Drawing.Point(114, 11);
            this.btnSaveService.Name = "btnSaveService";
            this.btnSaveService.Size = new System.Drawing.Size(150, 36);
            this.btnSaveService.TabIndex = 1;
            this.btnSaveService.Text = "Quick Save Service";
            this.btnSaveService.UseVisualStyleBackColor = false;
            this.btnSaveService.Click += new System.EventHandler(this.btnSaveService_Click);
            // 
            // btnSaveProfile
            // 
            this.btnSaveProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(129)))), ((int)(((byte)(95)))));
            this.btnSaveProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveProfile.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnSaveProfile.ForeColor = System.Drawing.Color.White;
            this.btnSaveProfile.Location = new System.Drawing.Point(274, 11);
            this.btnSaveProfile.Name = "btnSaveProfile";
            this.btnSaveProfile.Size = new System.Drawing.Size(130, 36);
            this.btnSaveProfile.TabIndex = 2;
            this.btnSaveProfile.Text = "Save Profile";
            this.btnSaveProfile.UseVisualStyleBackColor = false;
            this.btnSaveProfile.Click += new System.EventHandler(this.btnSaveProfile_Click);
            // 
            // btnClearProfile
            // 
            this.btnClearProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearProfile.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnClearProfile.Location = new System.Drawing.Point(414, 11);
            this.btnClearProfile.Name = "btnClearProfile";
            this.btnClearProfile.Size = new System.Drawing.Size(90, 36);
            this.btnClearProfile.TabIndex = 3;
            this.btnClearProfile.Text = "Clear";
            this.btnClearProfile.UseVisualStyleBackColor = true;
            this.btnClearProfile.Click += new System.EventHandler(this.btnClearProfile_Click);
            // 
            // btnRecentTransactionsRight
            // 
            this.btnRecentTransactionsRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecentTransactionsRight.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnRecentTransactionsRight.Location = new System.Drawing.Point(514, 11);
            this.btnRecentTransactionsRight.Name = "btnRecentTransactionsRight";
            this.btnRecentTransactionsRight.Size = new System.Drawing.Size(170, 36);
            this.btnRecentTransactionsRight.TabIndex = 4;
            this.btnRecentTransactionsRight.Text = "Recent Transactions";
            this.btnRecentTransactionsRight.UseVisualStyleBackColor = true;
            this.btnRecentTransactionsRight.Click += new System.EventHandler(this.btnOpenTransactions_Click);
            // 
            // servicePanel
            // 
            this.servicePanel.BackColor = System.Drawing.Color.White;
            this.servicePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.servicePanel.Controls.Add(this.lblServicePanelTitle);
            this.servicePanel.Controls.Add(this.lblServiceType);
            this.servicePanel.Controls.Add(this.cboServiceType);
            this.servicePanel.Controls.Add(this.btnInlineNewService);
            this.servicePanel.Controls.Add(this.btnInlineEditService);
            this.servicePanel.Controls.Add(this.lblWallet);
            this.servicePanel.Controls.Add(this.cboWallet);
            this.servicePanel.Controls.Add(this.lblServiceSummary);
            this.servicePanel.Location = new System.Drawing.Point(18, 114);
            this.servicePanel.Name = "servicePanel";
            this.servicePanel.Size = new System.Drawing.Size(760, 148);
            this.servicePanel.TabIndex = 3;
            // 
            // lblServicePanelTitle
            // 
            this.lblServicePanelTitle.AutoSize = true;
            this.lblServicePanelTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblServicePanelTitle.Location = new System.Drawing.Point(16, 12);
            this.lblServicePanelTitle.Name = "lblServicePanelTitle";
            this.lblServicePanelTitle.Size = new System.Drawing.Size(145, 20);
            this.lblServicePanelTitle.TabIndex = 0;
            this.lblServicePanelTitle.Text = "Quick Service Setup";
            // 
            // lblServiceType
            // 
            this.lblServiceType.AutoSize = true;
            this.lblServiceType.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblServiceType.Location = new System.Drawing.Point(18, 42);
            this.lblServiceType.Name = "lblServiceType";
            this.lblServiceType.Size = new System.Drawing.Size(83, 17);
            this.lblServiceType.TabIndex = 1;
            this.lblServiceType.Text = "Service Type";
            // 
            // cboServiceType
            // 
            this.cboServiceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboServiceType.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboServiceType.FormattingEnabled = true;
            this.cboServiceType.Location = new System.Drawing.Point(18, 66);
            this.cboServiceType.Name = "cboServiceType";
            this.cboServiceType.Size = new System.Drawing.Size(280, 25);
            this.cboServiceType.TabIndex = 2;
            this.cboServiceType.SelectedIndexChanged += new System.EventHandler(this.cboServiceType_SelectedIndexChanged);
            // 
            // btnInlineNewService
            // 
            this.btnInlineNewService.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(129)))), ((int)(((byte)(95)))));
            this.btnInlineNewService.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInlineNewService.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnInlineNewService.ForeColor = System.Drawing.Color.White;
            this.btnInlineNewService.Location = new System.Drawing.Point(308, 62);
            this.btnInlineNewService.Name = "btnInlineNewService";
            this.btnInlineNewService.Size = new System.Drawing.Size(82, 36);
            this.btnInlineNewService.TabIndex = 3;
            this.btnInlineNewService.Text = "+ New";
            this.btnInlineNewService.UseVisualStyleBackColor = false;
            this.btnInlineNewService.Click += new System.EventHandler(this.btnNewServiceType_Click);
            // 
            // btnInlineEditService
            // 
            this.btnInlineEditService.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInlineEditService.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnInlineEditService.Location = new System.Drawing.Point(396, 62);
            this.btnInlineEditService.Name = "btnInlineEditService";
            this.btnInlineEditService.Size = new System.Drawing.Size(72, 36);
            this.btnInlineEditService.TabIndex = 4;
            this.btnInlineEditService.Text = "Edit";
            this.btnInlineEditService.UseVisualStyleBackColor = true;
            this.btnInlineEditService.Click += new System.EventHandler(this.btnEditServiceType_Click);
            // 
            // lblWallet
            // 
            this.lblWallet.AutoSize = true;
            this.lblWallet.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblWallet.Location = new System.Drawing.Point(486, 42);
            this.lblWallet.Name = "lblWallet";
            this.lblWallet.Size = new System.Drawing.Size(205, 17);
            this.lblWallet.TabIndex = 5;
            this.lblWallet.Text = "Store Wallet / Settlement Source";
            // 
            // cboWallet
            // 
            this.cboWallet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWallet.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboWallet.FormattingEnabled = true;
            this.cboWallet.Location = new System.Drawing.Point(486, 66);
            this.cboWallet.Name = "cboWallet";
            this.cboWallet.Size = new System.Drawing.Size(240, 25);
            this.cboWallet.TabIndex = 6;
            // 
            // lblServiceSummary
            // 
            this.lblServiceSummary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            this.lblServiceSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblServiceSummary.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblServiceSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.lblServiceSummary.Location = new System.Drawing.Point(18, 103);
            this.lblServiceSummary.Name = "lblServiceSummary";
            this.lblServiceSummary.Size = new System.Drawing.Size(708, 40);
            this.lblServiceSummary.TabIndex = 7;
            this.lblServiceSummary.Text = "Service details will appear here after you select a service.";
            // 
            // customerPanel
            // 
            this.customerPanel.BackColor = System.Drawing.Color.White;
            this.customerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.customerPanel.Controls.Add(this.lblCustomerPanelTitle);
            this.customerPanel.Controls.Add(this.chkWalkInCustomer);
            this.customerPanel.Controls.Add(this.lblProfileSummary);
            this.customerPanel.Controls.Add(this.lblCustomerName);
            this.customerPanel.Controls.Add(this.txtCustomer);
            this.customerPanel.Controls.Add(this.lblMobile);
            this.customerPanel.Controls.Add(this.txtMobile);
            this.customerPanel.Controls.Add(this.lblReference);
            this.customerPanel.Controls.Add(this.txtReference);
            this.customerPanel.Controls.Add(this.lblBillCategory);
            this.customerPanel.Controls.Add(this.cboBillCategory);
            this.customerPanel.Location = new System.Drawing.Point(18, 264);
            this.customerPanel.Name = "customerPanel";
            this.customerPanel.Size = new System.Drawing.Size(760, 202);
            this.customerPanel.TabIndex = 4;
            // 
            // lblCustomerPanelTitle
            // 
            this.lblCustomerPanelTitle.AutoSize = true;
            this.lblCustomerPanelTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblCustomerPanelTitle.Location = new System.Drawing.Point(16, 12);
            this.lblCustomerPanelTitle.Name = "lblCustomerPanelTitle";
            this.lblCustomerPanelTitle.Size = new System.Drawing.Size(124, 20);
            this.lblCustomerPanelTitle.TabIndex = 0;
            this.lblCustomerPanelTitle.Text = "Customer Details";
            // 
            // chkWalkInCustomer
            // 
            this.chkWalkInCustomer.AutoSize = true;
            this.chkWalkInCustomer.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkWalkInCustomer.Location = new System.Drawing.Point(18, 40);
            this.chkWalkInCustomer.Name = "chkWalkInCustomer";
            this.chkWalkInCustomer.Size = new System.Drawing.Size(135, 23);
            this.chkWalkInCustomer.TabIndex = 1;
            this.chkWalkInCustomer.Text = "Walk-in customer";
            this.chkWalkInCustomer.UseVisualStyleBackColor = true;
            this.chkWalkInCustomer.CheckedChanged += new System.EventHandler(this.chkWalkInCustomer_CheckedChanged);
            // 
            // lblProfileSummary
            // 
            this.lblProfileSummary.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblProfileSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.lblProfileSummary.Location = new System.Drawing.Point(170, 41);
            this.lblProfileSummary.Name = "lblProfileSummary";
            this.lblProfileSummary.Size = new System.Drawing.Size(556, 20);
            this.lblProfileSummary.TabIndex = 2;
            this.lblProfileSummary.Text = "No repeat-customer profile selected";
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblCustomerName.Location = new System.Drawing.Point(18, 72);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(107, 17);
            this.lblCustomerName.TabIndex = 3;
            this.lblCustomerName.Text = "Customer Name";
            // 
            // txtCustomer
            // 
            this.txtCustomer.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtCustomer.Location = new System.Drawing.Point(18, 96);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(330, 25);
            this.txtCustomer.TabIndex = 4;
            // 
            // lblMobile
            // 
            this.lblMobile.AutoSize = true;
            this.lblMobile.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblMobile.Location = new System.Drawing.Point(370, 72);
            this.lblMobile.Name = "lblMobile";
            this.lblMobile.Size = new System.Drawing.Size(49, 17);
            this.lblMobile.TabIndex = 5;
            this.lblMobile.Text = "Mobile";
            // 
            // txtMobile
            // 
            this.txtMobile.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMobile.Location = new System.Drawing.Point(370, 96);
            this.txtMobile.Name = "txtMobile";
            this.txtMobile.Size = new System.Drawing.Size(180, 25);
            this.txtMobile.TabIndex = 6;
            // 
            // lblReference
            // 
            this.lblReference.AutoSize = true;
            this.lblReference.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblReference.Location = new System.Drawing.Point(18, 132);
            this.lblReference.Name = "lblReference";
            this.lblReference.Size = new System.Drawing.Size(164, 17);
            this.lblReference.TabIndex = 7;
            this.lblReference.Text = "Consumer / Reference No";
            // 
            // txtReference
            // 
            this.txtReference.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtReference.Location = new System.Drawing.Point(18, 156);
            this.txtReference.Name = "txtReference";
            this.txtReference.Size = new System.Drawing.Size(532, 25);
            this.txtReference.TabIndex = 8;
            // 
            // lblBillCategory
            // 
            this.lblBillCategory.AutoSize = true;
            this.lblBillCategory.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblBillCategory.Location = new System.Drawing.Point(570, 132);
            this.lblBillCategory.Name = "lblBillCategory";
            this.lblBillCategory.Size = new System.Drawing.Size(85, 17);
            this.lblBillCategory.TabIndex = 9;
            this.lblBillCategory.Text = "Bill Category";
            // 
            // cboBillCategory
            // 
            this.cboBillCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBillCategory.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboBillCategory.FormattingEnabled = true;
            this.cboBillCategory.Items.AddRange(new object[] {
            "Not Applicable",
            "WAPDA",
            "Internet",
            "Agriculture",
            "Other"});
            this.cboBillCategory.Location = new System.Drawing.Point(570, 156);
            this.cboBillCategory.Name = "cboBillCategory";
            this.cboBillCategory.Size = new System.Drawing.Size(156, 25);
            this.cboBillCategory.TabIndex = 10;
            this.cboBillCategory.SelectedIndexChanged += new System.EventHandler(this.cboBillCategory_SelectedIndexChanged);
            // 
            // transactionPanel
            // 
            this.transactionPanel.BackColor = System.Drawing.Color.White;
            this.transactionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.transactionPanel.Controls.Add(this.lblTransactionPanelTitle);
            this.transactionPanel.Controls.Add(this.lblTxnDate);
            this.transactionPanel.Controls.Add(this.dtpTxnDate);
            this.transactionPanel.Controls.Add(this.lblAmount);
            this.transactionPanel.Controls.Add(this.nudAmount);
            this.transactionPanel.Controls.Add(this.lblCommissionTitle);
            this.transactionPanel.Controls.Add(this.nudCommission);
            this.transactionPanel.Controls.Add(this.lblStatus);
            this.transactionPanel.Controls.Add(this.cboStatus);
            this.transactionPanel.Controls.Add(this.lblExpectedCommissionTitle);
            this.transactionPanel.Controls.Add(this.lblCommission);
            this.transactionPanel.Controls.Add(this.lblPaymentMethod);
            this.transactionPanel.Controls.Add(this.cboPaymentMethod);
            this.transactionPanel.Controls.Add(this.lblPaymentAccount);
            this.transactionPanel.Controls.Add(this.txtCustomerAccountNo);
            this.transactionPanel.Controls.Add(this.lblTransactionIdTitle);
            this.transactionPanel.Controls.Add(this.txtTransactionId);
            this.transactionPanel.Controls.Add(this.chkTransactionIdNotApplicable);
            this.transactionPanel.Location = new System.Drawing.Point(18, 468);
            this.transactionPanel.Name = "transactionPanel";
            this.transactionPanel.Size = new System.Drawing.Size(760, 194);
            this.transactionPanel.TabIndex = 5;
            // 
            // lblTransactionPanelTitle
            // 
            this.lblTransactionPanelTitle.AutoSize = true;
            this.lblTransactionPanelTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblTransactionPanelTitle.Location = new System.Drawing.Point(16, 12);
            this.lblTransactionPanelTitle.Name = "lblTransactionPanelTitle";
            this.lblTransactionPanelTitle.Size = new System.Drawing.Size(137, 20);
            this.lblTransactionPanelTitle.TabIndex = 0;
            this.lblTransactionPanelTitle.Text = "Transaction Details";
            // 
            // lblTxnDate
            // 
            this.lblTxnDate.AutoSize = true;
            this.lblTxnDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTxnDate.Location = new System.Drawing.Point(18, 42);
            this.lblTxnDate.Name = "lblTxnDate";
            this.lblTxnDate.Size = new System.Drawing.Size(109, 17);
            this.lblTxnDate.TabIndex = 1;
            this.lblTxnDate.Text = "Transaction Date";
            // 
            // dtpTxnDate
            // 
            this.dtpTxnDate.CustomFormat = "dd MMM yyyy hh:mm tt";
            this.dtpTxnDate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpTxnDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTxnDate.Location = new System.Drawing.Point(18, 66);
            this.dtpTxnDate.Name = "dtpTxnDate";
            this.dtpTxnDate.Size = new System.Drawing.Size(220, 25);
            this.dtpTxnDate.TabIndex = 2;
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblAmount.Location = new System.Drawing.Point(258, 42);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(58, 17);
            this.lblAmount.TabIndex = 3;
            this.lblAmount.Text = "Amount";
            // 
            // nudAmount
            // 
            this.nudAmount.DecimalPlaces = 2;
            this.nudAmount.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.nudAmount.Location = new System.Drawing.Point(258, 66);
            this.nudAmount.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nudAmount.Name = "nudAmount";
            this.nudAmount.Size = new System.Drawing.Size(120, 25);
            this.nudAmount.TabIndex = 4;
            this.nudAmount.ThousandsSeparator = true;
            this.nudAmount.ValueChanged += new System.EventHandler(this.AnyAmountChanged);
            // 
            // lblCommissionTitle
            // 
            this.lblCommissionTitle.AutoSize = true;
            this.lblCommissionTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblCommissionTitle.Location = new System.Drawing.Point(398, 42);
            this.lblCommissionTitle.Name = "lblCommissionTitle";
            this.lblCommissionTitle.Size = new System.Drawing.Size(82, 17);
            this.lblCommissionTitle.TabIndex = 5;
            this.lblCommissionTitle.Text = "Commission";
            // 
            // nudCommission
            // 
            this.nudCommission.DecimalPlaces = 2;
            this.nudCommission.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.nudCommission.Location = new System.Drawing.Point(398, 66);
            this.nudCommission.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nudCommission.Name = "nudCommission";
            this.nudCommission.Size = new System.Drawing.Size(140, 25);
            this.nudCommission.TabIndex = 6;
            this.nudCommission.ThousandsSeparator = true;
            this.nudCommission.ValueChanged += new System.EventHandler(this.nudCommission_ValueChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(558, 42);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(46, 17);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "Status";
            // 
            // cboStatus
            // 
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Items.AddRange(new object[] {
            "Completed",
            "Pending",
            "Cancelled"});
            this.cboStatus.Location = new System.Drawing.Point(558, 66);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(168, 25);
            this.cboStatus.TabIndex = 8;
            // 
            // lblExpectedCommissionTitle
            // 
            this.lblExpectedCommissionTitle.AutoSize = true;
            this.lblExpectedCommissionTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblExpectedCommissionTitle.Location = new System.Drawing.Point(18, 100);
            this.lblExpectedCommissionTitle.Name = "lblExpectedCommissionTitle";
            this.lblExpectedCommissionTitle.Size = new System.Drawing.Size(141, 17);
            this.lblExpectedCommissionTitle.TabIndex = 9;
            this.lblExpectedCommissionTitle.Text = "Expected Commission";
            // 
            // lblCommission
            // 
            this.lblCommission.AutoSize = true;
            this.lblCommission.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lblCommission.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(44)))), ((int)(((byte)(145)))));
            this.lblCommission.Location = new System.Drawing.Point(179, 98);
            this.lblCommission.Name = "lblCommission";
            this.lblCommission.Size = new System.Drawing.Size(66, 21);
            this.lblCommission.TabIndex = 10;
            this.lblCommission.Text = "Rs. 0.00";
            // 
            // lblPaymentMethod
            // 
            this.lblPaymentMethod.AutoSize = true;
            this.lblPaymentMethod.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPaymentMethod.Location = new System.Drawing.Point(18, 132);
            this.lblPaymentMethod.Name = "lblPaymentMethod";
            this.lblPaymentMethod.Size = new System.Drawing.Size(114, 17);
            this.lblPaymentMethod.TabIndex = 11;
            this.lblPaymentMethod.Text = "Payment Method";
            // 
            // cboPaymentMethod
            // 
            this.cboPaymentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPaymentMethod.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboPaymentMethod.FormattingEnabled = true;
            this.cboPaymentMethod.Items.AddRange(new object[] {
            "Cash",
            "JazzCash",
            "EasyPaisa",
            "Bank",
            "Other"});
            this.cboPaymentMethod.Location = new System.Drawing.Point(18, 156);
            this.cboPaymentMethod.Name = "cboPaymentMethod";
            this.cboPaymentMethod.Size = new System.Drawing.Size(170, 25);
            this.cboPaymentMethod.TabIndex = 12;
            this.cboPaymentMethod.SelectedIndexChanged += new System.EventHandler(this.cboPaymentMethod_SelectedIndexChanged);
            // 
            // lblPaymentAccount
            // 
            this.lblPaymentAccount.AutoSize = true;
            this.lblPaymentAccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPaymentAccount.Location = new System.Drawing.Point(208, 132);
            this.lblPaymentAccount.Name = "lblPaymentAccount";
            this.lblPaymentAccount.Size = new System.Drawing.Size(134, 17);
            this.lblPaymentAccount.TabIndex = 13;
            this.lblPaymentAccount.Text = "Account / Mobile No";
            // 
            // txtCustomerAccountNo
            // 
            this.txtCustomerAccountNo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtCustomerAccountNo.Location = new System.Drawing.Point(208, 156);
            this.txtCustomerAccountNo.Name = "txtCustomerAccountNo";
            this.txtCustomerAccountNo.Size = new System.Drawing.Size(220, 25);
            this.txtCustomerAccountNo.TabIndex = 14;
            // 
            // lblTransactionIdTitle
            // 
            this.lblTransactionIdTitle.AutoSize = true;
            this.lblTransactionIdTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTransactionIdTitle.Location = new System.Drawing.Point(448, 132);
            this.lblTransactionIdTitle.Name = "lblTransactionIdTitle";
            this.lblTransactionIdTitle.Size = new System.Drawing.Size(94, 17);
            this.lblTransactionIdTitle.TabIndex = 15;
            this.lblTransactionIdTitle.Text = "Transaction ID";
            // 
            // txtTransactionId
            // 
            this.txtTransactionId.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTransactionId.Location = new System.Drawing.Point(448, 156);
            this.txtTransactionId.Name = "txtTransactionId";
            this.txtTransactionId.Size = new System.Drawing.Size(180, 25);
            this.txtTransactionId.TabIndex = 16;
            // 
            // chkTransactionIdNotApplicable
            // 
            this.chkTransactionIdNotApplicable.AutoSize = true;
            this.chkTransactionIdNotApplicable.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.chkTransactionIdNotApplicable.Location = new System.Drawing.Point(448, 108);
            this.chkTransactionIdNotApplicable.Name = "chkTransactionIdNotApplicable";
            this.chkTransactionIdNotApplicable.Size = new System.Drawing.Size(114, 21);
            this.chkTransactionIdNotApplicable.TabIndex = 17;
            this.chkTransactionIdNotApplicable.Text = "Not Applicable";
            this.chkTransactionIdNotApplicable.UseVisualStyleBackColor = true;
            this.chkTransactionIdNotApplicable.CheckedChanged += new System.EventHandler(this.chkTransactionIdNotApplicable_CheckedChanged);
            // 
            // billingPanel
            // 
            this.billingPanel.BackColor = System.Drawing.Color.White;
            this.billingPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.billingPanel.Controls.Add(this.lblBillingPanelTitle);
            this.billingPanel.Controls.Add(this.lblRecurrence);
            this.billingPanel.Controls.Add(this.cboRecurrence);
            this.billingPanel.Controls.Add(this.lblDueDay);
            this.billingPanel.Controls.Add(this.nudDueDay);
            this.billingPanel.Controls.Add(this.chkNextDue);
            this.billingPanel.Controls.Add(this.dtpNextDue);
            this.billingPanel.Controls.Add(this.chkSaveProfile);
            this.billingPanel.Location = new System.Drawing.Point(18, 664);
            this.billingPanel.Name = "billingPanel";
            this.billingPanel.Size = new System.Drawing.Size(760, 140);
            this.billingPanel.TabIndex = 6;
            // 
            // lblBillingPanelTitle
            // 
            this.lblBillingPanelTitle.AutoSize = true;
            this.lblBillingPanelTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblBillingPanelTitle.Location = new System.Drawing.Point(16, 12);
            this.lblBillingPanelTitle.Name = "lblBillingPanelTitle";
            this.lblBillingPanelTitle.Size = new System.Drawing.Size(146, 20);
            this.lblBillingPanelTitle.TabIndex = 0;
            this.lblBillingPanelTitle.Text = "Repeat Billing Setup";
            // 
            // lblRecurrence
            // 
            this.lblRecurrence.AutoSize = true;
            this.lblRecurrence.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblRecurrence.Location = new System.Drawing.Point(18, 42);
            this.lblRecurrence.Name = "lblRecurrence";
            this.lblRecurrence.Size = new System.Drawing.Size(75, 17);
            this.lblRecurrence.TabIndex = 1;
            this.lblRecurrence.Text = "Recurrence";
            // 
            // cboRecurrence
            // 
            this.cboRecurrence.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRecurrence.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboRecurrence.FormattingEnabled = true;
            this.cboRecurrence.Items.AddRange(new object[] {
            "OnDemand",
            "Monthly",
            "Weekly"});
            this.cboRecurrence.Location = new System.Drawing.Point(18, 66);
            this.cboRecurrence.Name = "cboRecurrence";
            this.cboRecurrence.Size = new System.Drawing.Size(180, 25);
            this.cboRecurrence.TabIndex = 2;
            this.cboRecurrence.SelectedIndexChanged += new System.EventHandler(this.cboRecurrence_SelectedIndexChanged);
            // 
            // lblDueDay
            // 
            this.lblDueDay.AutoSize = true;
            this.lblDueDay.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblDueDay.Location = new System.Drawing.Point(220, 42);
            this.lblDueDay.Name = "lblDueDay";
            this.lblDueDay.Size = new System.Drawing.Size(114, 17);
            this.lblDueDay.TabIndex = 3;
            this.lblDueDay.Text = "Monthly Due Day";
            // 
            // nudDueDay
            // 
            this.nudDueDay.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.nudDueDay.Location = new System.Drawing.Point(220, 66);
            this.nudDueDay.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nudDueDay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDueDay.Name = "nudDueDay";
            this.nudDueDay.Size = new System.Drawing.Size(120, 25);
            this.nudDueDay.TabIndex = 4;
            this.nudDueDay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkNextDue
            // 
            this.chkNextDue.AutoSize = true;
            this.chkNextDue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkNextDue.Location = new System.Drawing.Point(360, 42);
            this.chkNextDue.Name = "chkNextDue";
            this.chkNextDue.Size = new System.Drawing.Size(135, 23);
            this.chkNextDue.TabIndex = 5;
            this.chkNextDue.Text = "Set next due date";
            this.chkNextDue.UseVisualStyleBackColor = true;
            this.chkNextDue.CheckedChanged += new System.EventHandler(this.chkNextDue_CheckedChanged);
            // 
            // dtpNextDue
            // 
            this.dtpNextDue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpNextDue.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNextDue.Location = new System.Drawing.Point(360, 66);
            this.dtpNextDue.Name = "dtpNextDue";
            this.dtpNextDue.Size = new System.Drawing.Size(180, 25);
            this.dtpNextDue.TabIndex = 6;
            // 
            // chkSaveProfile
            // 
            this.chkSaveProfile.AutoSize = true;
            this.chkSaveProfile.Checked = true;
            this.chkSaveProfile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveProfile.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkSaveProfile.Location = new System.Drawing.Point(18, 104);
            this.chkSaveProfile.Name = "chkSaveProfile";
            this.chkSaveProfile.Size = new System.Drawing.Size(388, 23);
            this.chkSaveProfile.TabIndex = 7;
            this.chkSaveProfile.Text = "Save or update this customer profile for future repeat visits";
            this.chkSaveProfile.UseVisualStyleBackColor = true;
            // 
            // notesPanel
            // 
            this.notesPanel.BackColor = System.Drawing.Color.White;
            this.notesPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.notesPanel.Controls.Add(this.lblNotesPanelTitle);
            this.notesPanel.Controls.Add(this.lblRemarksTitle);
            this.notesPanel.Controls.Add(this.txtRemarks);
            this.notesPanel.Controls.Add(this.lblQuickTip);
            this.notesPanel.Location = new System.Drawing.Point(18, 806);
            this.notesPanel.Name = "notesPanel";
            this.notesPanel.Size = new System.Drawing.Size(760, 166);
            this.notesPanel.TabIndex = 7;
            // 
            // lblNotesPanelTitle
            // 
            this.lblNotesPanelTitle.AutoSize = true;
            this.lblNotesPanelTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblNotesPanelTitle.Location = new System.Drawing.Point(16, 12);
            this.lblNotesPanelTitle.Name = "lblNotesPanelTitle";
            this.lblNotesPanelTitle.Size = new System.Drawing.Size(171, 20);
            this.lblNotesPanelTitle.TabIndex = 0;
            this.lblNotesPanelTitle.Text = "Remarks and Quick Tips";
            // 
            // lblRemarksTitle
            // 
            this.lblRemarksTitle.AutoSize = true;
            this.lblRemarksTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblRemarksTitle.Location = new System.Drawing.Point(18, 42);
            this.lblRemarksTitle.Name = "lblRemarksTitle";
            this.lblRemarksTitle.Size = new System.Drawing.Size(60, 17);
            this.lblRemarksTitle.TabIndex = 1;
            this.lblRemarksTitle.Text = "Remarks";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtRemarks.Location = new System.Drawing.Point(18, 66);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(520, 72);
            this.txtRemarks.TabIndex = 2;
            // 
            // lblQuickTip
            // 
            this.lblQuickTip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(244)))), ((int)(((byte)(252)))));
            this.lblQuickTip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblQuickTip.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblQuickTip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblQuickTip.Location = new System.Drawing.Point(552, 42);
            this.lblQuickTip.Name = "lblQuickTip";
            this.lblQuickTip.Size = new System.Drawing.Size(174, 96);
            this.lblQuickTip.TabIndex = 3;
            this.lblQuickTip.Text = "Tips:\r\n1. Choose bill type for WAPDA, internet, agriculture, or other bills.\r\n2. " +
    "Keep Walk-in checked for one-time customers.\r\n3. Save profile only for repeat bi" +
    "lls.";
            // 
            // ServiceCenterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1370, 749);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.headerPanel);
            this.MinimumSize = new System.Drawing.Size(1364, 726);
            this.Name = "ServiceCenterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Service Center";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ServiceCenterForm_Load);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.panelLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfiles)).EndInit();
            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            this.actionBar.ResumeLayout(false);
            this.servicePanel.ResumeLayout(false);
            this.servicePanel.PerformLayout();
            this.customerPanel.ResumeLayout(false);
            this.customerPanel.PerformLayout();
            this.transactionPanel.ResumeLayout(false);
            this.transactionPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCommission)).EndInit();
            this.billingPanel.ResumeLayout(false);
            this.billingPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDueDay)).EndInit();
            this.notesPanel.ResumeLayout(false);
            this.notesPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblHeaderSubtitle;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Label lblSearchTitle;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnOpenTransactionsLeft;
        private System.Windows.Forms.Button btnNewServiceTypeLeft;
        private System.Windows.Forms.Button btnEditServiceTypeLeft;
        private System.Windows.Forms.Label lblTrackedCustomers;
        private System.Windows.Forms.DataGridView dgvProfiles;
        private System.Windows.Forms.Label lblLeftHint;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label lblEntryModeHint;
        private System.Windows.Forms.Panel actionBar;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSaveService;
        private System.Windows.Forms.Button btnSaveProfile;
        private System.Windows.Forms.Button btnClearProfile;
        private System.Windows.Forms.Button btnRecentTransactionsRight;
        private System.Windows.Forms.Panel servicePanel;
        private System.Windows.Forms.Label lblServicePanelTitle;
        private System.Windows.Forms.Label lblServiceType;
        private System.Windows.Forms.ComboBox cboServiceType;
        private System.Windows.Forms.Button btnInlineNewService;
        private System.Windows.Forms.Button btnInlineEditService;
        private System.Windows.Forms.Label lblWallet;
        private System.Windows.Forms.ComboBox cboWallet;
        private System.Windows.Forms.Label lblServiceSummary;
        private System.Windows.Forms.Panel customerPanel;
        private System.Windows.Forms.Label lblCustomerPanelTitle;
        private System.Windows.Forms.CheckBox chkWalkInCustomer;
        private System.Windows.Forms.Label lblProfileSummary;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.TextBox txtCustomer;
        private System.Windows.Forms.Label lblMobile;
        private System.Windows.Forms.TextBox txtMobile;
        private System.Windows.Forms.Label lblReference;
        private System.Windows.Forms.TextBox txtReference;
        private System.Windows.Forms.Label lblBillCategory;
        private System.Windows.Forms.ComboBox cboBillCategory;
        private System.Windows.Forms.Panel transactionPanel;
        private System.Windows.Forms.Label lblTransactionPanelTitle;
        private System.Windows.Forms.Label lblTxnDate;
        private System.Windows.Forms.DateTimePicker dtpTxnDate;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.NumericUpDown nudAmount;
        private System.Windows.Forms.Label lblCommissionTitle;
        private System.Windows.Forms.NumericUpDown nudCommission;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Label lblExpectedCommissionTitle;
        private System.Windows.Forms.Label lblCommission;
        private System.Windows.Forms.Label lblPaymentMethod;
        private System.Windows.Forms.ComboBox cboPaymentMethod;
        private System.Windows.Forms.Label lblPaymentAccount;
        private System.Windows.Forms.TextBox txtCustomerAccountNo;
        private System.Windows.Forms.Label lblTransactionIdTitle;
        private System.Windows.Forms.TextBox txtTransactionId;
        private System.Windows.Forms.CheckBox chkTransactionIdNotApplicable;
        private System.Windows.Forms.Panel billingPanel;
        private System.Windows.Forms.Label lblBillingPanelTitle;
        private System.Windows.Forms.Label lblRecurrence;
        private System.Windows.Forms.ComboBox cboRecurrence;
        private System.Windows.Forms.Label lblDueDay;
        private System.Windows.Forms.NumericUpDown nudDueDay;
        private System.Windows.Forms.CheckBox chkNextDue;
        private System.Windows.Forms.DateTimePicker dtpNextDue;
        private System.Windows.Forms.CheckBox chkSaveProfile;
        private System.Windows.Forms.Panel notesPanel;
        private System.Windows.Forms.Label lblNotesPanelTitle;
        private System.Windows.Forms.Label lblRemarksTitle;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label lblQuickTip;
    }
}
