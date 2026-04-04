namespace ShopPOS
{
    partial class VendorEntryForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblFooterNote;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.GroupBox groupProfile;
        private System.Windows.Forms.GroupBox groupProducts;
        private System.Windows.Forms.Label lblVendorName;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblOpeningBalance;
        private System.Windows.Forms.Label lblBalanceType;
        private System.Windows.Forms.Label lblVisitDay;
        private System.Windows.Forms.Label lblPaymentCycle;
        private System.Windows.Forms.Label lblCreditDays;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtVendorName;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.NumericUpDown nudOpeningBalance;
        private System.Windows.Forms.ComboBox cboBalanceType;
        private System.Windows.Forms.ComboBox cboVisitDay;
        private System.Windows.Forms.ComboBox cboPaymentCycle;
        private System.Windows.Forms.NumericUpDown nudCreditDays;
        private System.Windows.Forms.CheckBox chkHasNextPaymentDate;
        private System.Windows.Forms.DateTimePicker dtpNextPaymentDate;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.DataGridView dgvVendorProducts;

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
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblFooterNote = new System.Windows.Forms.Label();
            this.panelBody = new System.Windows.Forms.Panel();
            this.panelContent = new System.Windows.Forms.Panel();
            this.groupProfile = new System.Windows.Forms.GroupBox();
            this.lblVendorName = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblOpeningBalance = new System.Windows.Forms.Label();
            this.lblBalanceType = new System.Windows.Forms.Label();
            this.lblVisitDay = new System.Windows.Forms.Label();
            this.lblPaymentCycle = new System.Windows.Forms.Label();
            this.lblCreditDays = new System.Windows.Forms.Label();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtVendorName = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.nudOpeningBalance = new System.Windows.Forms.NumericUpDown();
            this.cboBalanceType = new System.Windows.Forms.ComboBox();
            this.cboVisitDay = new System.Windows.Forms.ComboBox();
            this.cboPaymentCycle = new System.Windows.Forms.ComboBox();
            this.nudCreditDays = new System.Windows.Forms.NumericUpDown();
            this.chkHasNextPaymentDate = new System.Windows.Forms.CheckBox();
            this.dtpNextPaymentDate = new System.Windows.Forms.DateTimePicker();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.groupProducts = new System.Windows.Forms.GroupBox();
            this.dgvVendorProducts = new System.Windows.Forms.DataGridView();
            this.panelHeader.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.groupProfile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOpeningBalance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCreditDays)).BeginInit();
            this.groupProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendorProducts)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(84)))), ((int)(((byte)(46)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Controls.Add(this.lblSubtitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1280, 90);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(24, 18);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(168, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "New Vendor";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblSubtitle.Location = new System.Drawing.Point(28, 57);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(540, 19);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Maintain vendor profile, visit schedule, payment terms, and linked products.";
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.Color.White;
            this.panelFooter.Controls.Add(this.btnSave);
            this.panelFooter.Controls.Add(this.btnClose);
            this.panelFooter.Controls.Add(this.lblFooterNote);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 738);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(1280, 82);
            this.panelFooter.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(84)))), ((int)(((byte)(46)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(24, 18);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(170, 42);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Create Vendor";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(206, 18);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(130, 42);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblFooterNote
            // 
            this.lblFooterNote.AutoSize = true;
            this.lblFooterNote.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblFooterNote.ForeColor = System.Drawing.Color.DimGray;
            this.lblFooterNote.Location = new System.Drawing.Point(356, 30);
            this.lblFooterNote.Name = "lblFooterNote";
            this.lblFooterNote.Size = new System.Drawing.Size(613, 17);
            this.lblFooterNote.TabIndex = 2;
            this.lblFooterNote.Text = "Save the vendor profile first, then linked products and payment terms will stay a" +
    "ttached to this vendor.";
            // 
            // panelBody
            // 
            this.panelBody.AutoScroll = true;
            this.panelBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.panelBody.Controls.Add(this.panelContent);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 90);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(20, 18, 20, 20);
            this.panelBody.Size = new System.Drawing.Size(1280, 648);
            this.panelBody.TabIndex = 1;
            // 
            // panelContent
            // 
            this.panelContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelContent.Controls.Add(this.groupProfile);
            this.panelContent.Controls.Add(this.groupProducts);
            this.panelContent.Location = new System.Drawing.Point(20, 18);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1180, 700);
            this.panelContent.TabIndex = 0;
            // 
            // groupProfile
            // 
            this.groupProfile.Controls.Add(this.lblVendorName);
            this.groupProfile.Controls.Add(this.lblPhone);
            this.groupProfile.Controls.Add(this.lblAddress);
            this.groupProfile.Controls.Add(this.lblOpeningBalance);
            this.groupProfile.Controls.Add(this.lblBalanceType);
            this.groupProfile.Controls.Add(this.lblVisitDay);
            this.groupProfile.Controls.Add(this.lblPaymentCycle);
            this.groupProfile.Controls.Add(this.lblCreditDays);
            this.groupProfile.Controls.Add(this.lblNotes);
            this.groupProfile.Controls.Add(this.txtVendorName);
            this.groupProfile.Controls.Add(this.txtPhone);
            this.groupProfile.Controls.Add(this.txtAddress);
            this.groupProfile.Controls.Add(this.nudOpeningBalance);
            this.groupProfile.Controls.Add(this.cboBalanceType);
            this.groupProfile.Controls.Add(this.cboVisitDay);
            this.groupProfile.Controls.Add(this.cboPaymentCycle);
            this.groupProfile.Controls.Add(this.nudCreditDays);
            this.groupProfile.Controls.Add(this.chkHasNextPaymentDate);
            this.groupProfile.Controls.Add(this.dtpNextPaymentDate);
            this.groupProfile.Controls.Add(this.chkIsActive);
            this.groupProfile.Controls.Add(this.txtNotes);
            this.groupProfile.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.groupProfile.Location = new System.Drawing.Point(0, 0);
            this.groupProfile.Name = "groupProfile";
            this.groupProfile.Size = new System.Drawing.Size(1140, 330);
            this.groupProfile.TabIndex = 0;
            this.groupProfile.TabStop = false;
            this.groupProfile.Text = "Vendor Profile";
            // 
            // lblVendorName
            // 
            this.lblVendorName.AutoSize = true;
            this.lblVendorName.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblVendorName.Location = new System.Drawing.Point(18, 34);
            this.lblVendorName.Name = "lblVendorName";
            this.lblVendorName.Size = new System.Drawing.Size(87, 17);
            this.lblVendorName.TabIndex = 0;
            this.lblVendorName.Text = "Vendor Name";
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPhone.Location = new System.Drawing.Point(320, 34);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(46, 17);
            this.lblPhone.TabIndex = 1;
            this.lblPhone.Text = "Phone";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblAddress.Location = new System.Drawing.Point(18, 100);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(57, 17);
            this.lblAddress.TabIndex = 2;
            this.lblAddress.Text = "Address";
            // 
            // lblOpeningBalance
            // 
            this.lblOpeningBalance.AutoSize = true;
            this.lblOpeningBalance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblOpeningBalance.Location = new System.Drawing.Point(18, 166);
            this.lblOpeningBalance.Name = "lblOpeningBalance";
            this.lblOpeningBalance.Size = new System.Drawing.Size(108, 17);
            this.lblOpeningBalance.TabIndex = 3;
            this.lblOpeningBalance.Text = "Opening Balance";
            // 
            // lblBalanceType
            // 
            this.lblBalanceType.AutoSize = true;
            this.lblBalanceType.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblBalanceType.Location = new System.Drawing.Point(188, 166);
            this.lblBalanceType.Name = "lblBalanceType";
            this.lblBalanceType.Size = new System.Drawing.Size(84, 17);
            this.lblBalanceType.TabIndex = 4;
            this.lblBalanceType.Text = "Balance Type";
            // 
            // lblVisitDay
            // 
            this.lblVisitDay.AutoSize = true;
            this.lblVisitDay.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblVisitDay.Location = new System.Drawing.Point(358, 166);
            this.lblVisitDay.Name = "lblVisitDay";
            this.lblVisitDay.Size = new System.Drawing.Size(57, 17);
            this.lblVisitDay.TabIndex = 5;
            this.lblVisitDay.Text = "Visit Day";
            // 
            // lblPaymentCycle
            // 
            this.lblPaymentCycle.AutoSize = true;
            this.lblPaymentCycle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPaymentCycle.Location = new System.Drawing.Point(18, 232);
            this.lblPaymentCycle.Name = "lblPaymentCycle";
            this.lblPaymentCycle.Size = new System.Drawing.Size(94, 17);
            this.lblPaymentCycle.TabIndex = 6;
            this.lblPaymentCycle.Text = "Payment Cycle";
            // 
            // lblCreditDays
            // 
            this.lblCreditDays.AutoSize = true;
            this.lblCreditDays.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblCreditDays.Location = new System.Drawing.Point(208, 232);
            this.lblCreditDays.Name = "lblCreditDays";
            this.lblCreditDays.Size = new System.Drawing.Size(71, 17);
            this.lblCreditDays.TabIndex = 7;
            this.lblCreditDays.Text = "Credit Days";
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblNotes.Location = new System.Drawing.Point(540, 100);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(134, 17);
            this.lblNotes.TabIndex = 8;
            this.lblNotes.Text = "Notes / Payment Detail";
            // 
            // txtVendorName
            // 
            this.txtVendorName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtVendorName.Location = new System.Drawing.Point(18, 60);
            this.txtVendorName.Name = "txtVendorName";
            this.txtVendorName.Size = new System.Drawing.Size(280, 25);
            this.txtVendorName.TabIndex = 9;
            // 
            // txtPhone
            // 
            this.txtPhone.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPhone.Location = new System.Drawing.Point(320, 60);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(180, 25);
            this.txtPhone.TabIndex = 10;
            // 
            // txtAddress
            // 
            this.txtAddress.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtAddress.Location = new System.Drawing.Point(18, 126);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(482, 25);
            this.txtAddress.TabIndex = 11;
            // 
            // nudOpeningBalance
            // 
            this.nudOpeningBalance.DecimalPlaces = 2;
            this.nudOpeningBalance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.nudOpeningBalance.Location = new System.Drawing.Point(18, 192);
            this.nudOpeningBalance.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nudOpeningBalance.Name = "nudOpeningBalance";
            this.nudOpeningBalance.Size = new System.Drawing.Size(150, 25);
            this.nudOpeningBalance.TabIndex = 12;
            this.nudOpeningBalance.ThousandsSeparator = true;
            // 
            // cboBalanceType
            // 
            this.cboBalanceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBalanceType.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboBalanceType.FormattingEnabled = true;
            this.cboBalanceType.Items.AddRange(new object[] {
            "Payable",
            "Receivable"});
            this.cboBalanceType.Location = new System.Drawing.Point(188, 192);
            this.cboBalanceType.Name = "cboBalanceType";
            this.cboBalanceType.Size = new System.Drawing.Size(150, 25);
            this.cboBalanceType.TabIndex = 13;
            // 
            // cboVisitDay
            // 
            this.cboVisitDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVisitDay.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboVisitDay.FormattingEnabled = true;
            this.cboVisitDay.Items.AddRange(new object[] {
            "",
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday"});
            this.cboVisitDay.Location = new System.Drawing.Point(358, 192);
            this.cboVisitDay.Name = "cboVisitDay";
            this.cboVisitDay.Size = new System.Drawing.Size(142, 25);
            this.cboVisitDay.TabIndex = 14;
            // 
            // cboPaymentCycle
            // 
            this.cboPaymentCycle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPaymentCycle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboPaymentCycle.FormattingEnabled = true;
            this.cboPaymentCycle.Items.AddRange(new object[] {
            "",
            "Weekly",
            "Fixed Date",
            "Extended",
            "Flexible"});
            this.cboPaymentCycle.Location = new System.Drawing.Point(18, 258);
            this.cboPaymentCycle.Name = "cboPaymentCycle";
            this.cboPaymentCycle.Size = new System.Drawing.Size(170, 25);
            this.cboPaymentCycle.TabIndex = 15;
            // 
            // nudCreditDays
            // 
            this.nudCreditDays.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.nudCreditDays.Location = new System.Drawing.Point(208, 258);
            this.nudCreditDays.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.nudCreditDays.Name = "nudCreditDays";
            this.nudCreditDays.Size = new System.Drawing.Size(110, 25);
            this.nudCreditDays.TabIndex = 16;
            // 
            // chkHasNextPaymentDate
            // 
            this.chkHasNextPaymentDate.AutoSize = true;
            this.chkHasNextPaymentDate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkHasNextPaymentDate.Location = new System.Drawing.Point(338, 230);
            this.chkHasNextPaymentDate.Name = "chkHasNextPaymentDate";
            this.chkHasNextPaymentDate.Size = new System.Drawing.Size(161, 23);
            this.chkHasNextPaymentDate.TabIndex = 17;
            this.chkHasNextPaymentDate.Text = "Set next payment date";
            this.chkHasNextPaymentDate.UseVisualStyleBackColor = true;
            this.chkHasNextPaymentDate.CheckedChanged += new System.EventHandler(this.chkHasNextPaymentDate_CheckedChanged);
            // 
            // dtpNextPaymentDate
            // 
            this.dtpNextPaymentDate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpNextPaymentDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNextPaymentDate.Location = new System.Drawing.Point(338, 258);
            this.dtpNextPaymentDate.Name = "dtpNextPaymentDate";
            this.dtpNextPaymentDate.Size = new System.Drawing.Size(162, 25);
            this.dtpNextPaymentDate.TabIndex = 18;
            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkIsActive.Location = new System.Drawing.Point(540, 58);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(114, 23);
            this.chkIsActive.TabIndex = 19;
            this.chkIsActive.Text = "Vendor is active";
            this.chkIsActive.UseVisualStyleBackColor = true;
            // 
            // txtNotes
            // 
            this.txtNotes.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtNotes.Location = new System.Drawing.Point(540, 126);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(560, 162);
            this.txtNotes.TabIndex = 20;
            // 
            // groupProducts
            // 
            this.groupProducts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupProducts.Controls.Add(this.dgvVendorProducts);
            this.groupProducts.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.groupProducts.Location = new System.Drawing.Point(0, 348);
            this.groupProducts.Name = "groupProducts";
            this.groupProducts.Size = new System.Drawing.Size(1140, 310);
            this.groupProducts.TabIndex = 1;
            this.groupProducts.TabStop = false;
            this.groupProducts.Text = "Linked Products";
            // 
            // dgvVendorProducts
            // 
            this.dgvVendorProducts.AllowUserToAddRows = false;
            this.dgvVendorProducts.AllowUserToDeleteRows = false;
            this.dgvVendorProducts.AutoGenerateColumns = false;
            this.dgvVendorProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvVendorProducts.BackgroundColor = System.Drawing.Color.White;
            this.dgvVendorProducts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvVendorProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVendorProducts.EnableHeadersVisualStyles = false;
            this.dgvVendorProducts.GridColor = System.Drawing.Color.Gainsboro;
            this.dgvVendorProducts.Location = new System.Drawing.Point(3, 21);
            this.dgvVendorProducts.Name = "dgvVendorProducts";
            this.dgvVendorProducts.RowHeadersVisible = false;
            this.dgvVendorProducts.RowTemplate.Height = 30;
            this.dgvVendorProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVendorProducts.Size = new System.Drawing.Size(1134, 286);
            this.dgvVendorProducts.TabIndex = 0;
            // 
            // VendorEntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1280, 820);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1180, 820);
            this.Name = "VendorEntryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vendor Entry";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.VendorEntryForm_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            this.panelFooter.PerformLayout();
            this.panelBody.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.groupProfile.ResumeLayout(false);
            this.groupProfile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOpeningBalance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCreditDays)).EndInit();
            this.groupProducts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendorProducts)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
