using System.Drawing;
using System.Windows.Forms;

namespace ShopPOS
{
    partial class CustomerEntryForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel headerPanel;
        private Panel bodyPanel;
        private Panel detailsPanel;
        private Panel photoPanel;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblDetailsTitle;
        private Label lblPhotoTitle;
        private TextBox txtName;
        private TextBox txtPhone;
        private TextBox txtAddress;
        private NumericUpDown nudOpeningBalance;
        private ComboBox cboBalanceType;
        private CheckBox chkIsActive;
        private PictureBox picCustomer;
        private Button btnChooseImage;
        private Button btnCaptureImage;
        private Button btnClearImage;
        private Button btnSave;
        private Button btnClose;
        private Label lblName;
        private Label lblPhone;
        private Label lblAddress;
        private Label lblOpeningBalance;
        private Label lblBalanceType;

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
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.bodyPanel = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.photoPanel = new System.Windows.Forms.Panel();
            this.btnClearImage = new System.Windows.Forms.Button();
            this.btnCaptureImage = new System.Windows.Forms.Button();
            this.btnChooseImage = new System.Windows.Forms.Button();
            this.picCustomer = new System.Windows.Forms.PictureBox();
            this.lblPhotoTitle = new System.Windows.Forms.Label();
            this.detailsPanel = new System.Windows.Forms.Panel();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.cboBalanceType = new System.Windows.Forms.ComboBox();
            this.nudOpeningBalance = new System.Windows.Forms.NumericUpDown();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblBalanceType = new System.Windows.Forms.Label();
            this.lblOpeningBalance = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblDetailsTitle = new System.Windows.Forms.Label();
            this.headerPanel.SuspendLayout();
            this.bodyPanel.SuspendLayout();
            this.photoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCustomer)).BeginInit();
            this.detailsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOpeningBalance)).BeginInit();
            this.SuspendLayout();
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(108)))), ((int)(((byte)(83)))));
            this.headerPanel.Controls.Add(this.lblSubtitle);
            this.headerPanel.Controls.Add(this.lblTitle);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(1180, 88);
            this.headerPanel.TabIndex = 0;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblSubtitle.Location = new System.Drawing.Point(26, 52);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(361, 19);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Create or update customer profile details for sales and credit tracking.";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(22, 18);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(168, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "New Customer";
            // 
            // bodyPanel
            // 
            this.bodyPanel.BackColor = System.Drawing.Color.White;
            this.bodyPanel.Controls.Add(this.btnClose);
            this.bodyPanel.Controls.Add(this.btnSave);
            this.bodyPanel.Controls.Add(this.photoPanel);
            this.bodyPanel.Controls.Add(this.detailsPanel);
            this.bodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bodyPanel.Location = new System.Drawing.Point(0, 88);
            this.bodyPanel.Name = "bodyPanel";
            this.bodyPanel.Padding = new System.Windows.Forms.Padding(24, 20, 24, 24);
            this.bodyPanel.Size = new System.Drawing.Size(1180, 612);
            this.bodyPanel.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(190, 390);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 42);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(108)))), ((int)(((byte)(83)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(24, 390);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(150, 42);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save Customer";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // photoPanel
            // 
            this.photoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.photoPanel.Controls.Add(this.btnClearImage);
            this.photoPanel.Controls.Add(this.btnCaptureImage);
            this.photoPanel.Controls.Add(this.btnChooseImage);
            this.photoPanel.Controls.Add(this.picCustomer);
            this.photoPanel.Controls.Add(this.lblPhotoTitle);
            this.photoPanel.Location = new System.Drawing.Point(756, 20);
            this.photoPanel.Name = "photoPanel";
            this.photoPanel.Size = new System.Drawing.Size(300, 350);
            this.photoPanel.TabIndex = 1;
            // 
            // btnClearImage
            // 
            this.btnClearImage.BackColor = System.Drawing.Color.White;
            this.btnClearImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearImage.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnClearImage.Location = new System.Drawing.Point(28, 288);
            this.btnClearImage.Name = "btnClearImage";
            this.btnClearImage.Size = new System.Drawing.Size(240, 32);
            this.btnClearImage.TabIndex = 4;
            this.btnClearImage.Text = "Clear Picture";
            this.btnClearImage.UseVisualStyleBackColor = false;
            this.btnClearImage.Click += new System.EventHandler(this.btnClearImage_Click);
            // 
            // btnCaptureImage
            // 
            this.btnCaptureImage.BackColor = System.Drawing.Color.White;
            this.btnCaptureImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCaptureImage.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnCaptureImage.Location = new System.Drawing.Point(158, 242);
            this.btnCaptureImage.Name = "btnCaptureImage";
            this.btnCaptureImage.Size = new System.Drawing.Size(110, 36);
            this.btnCaptureImage.TabIndex = 3;
            this.btnCaptureImage.Text = "Live Capture";
            this.btnCaptureImage.UseVisualStyleBackColor = false;
            this.btnCaptureImage.Click += new System.EventHandler(this.btnCaptureImage_Click);
            // 
            // btnChooseImage
            // 
            this.btnChooseImage.BackColor = System.Drawing.Color.White;
            this.btnChooseImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChooseImage.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnChooseImage.Location = new System.Drawing.Point(28, 242);
            this.btnChooseImage.Name = "btnChooseImage";
            this.btnChooseImage.Size = new System.Drawing.Size(110, 36);
            this.btnChooseImage.TabIndex = 2;
            this.btnChooseImage.Text = "Choose File";
            this.btnChooseImage.UseVisualStyleBackColor = false;
            this.btnChooseImage.Click += new System.EventHandler(this.btnChooseImage_Click);
            // 
            // picCustomer
            // 
            this.picCustomer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(253)))));
            this.picCustomer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCustomer.Location = new System.Drawing.Point(28, 52);
            this.picCustomer.Name = "picCustomer";
            this.picCustomer.Size = new System.Drawing.Size(240, 170);
            this.picCustomer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picCustomer.TabIndex = 1;
            this.picCustomer.TabStop = false;
            // 
            // lblPhotoTitle
            // 
            this.lblPhotoTitle.AutoSize = true;
            this.lblPhotoTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblPhotoTitle.Location = new System.Drawing.Point(28, 18);
            this.lblPhotoTitle.Name = "lblPhotoTitle";
            this.lblPhotoTitle.Size = new System.Drawing.Size(117, 20);
            this.lblPhotoTitle.TabIndex = 0;
            this.lblPhotoTitle.Text = "Customer Picture";
            // 
            // detailsPanel
            // 
            this.detailsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.detailsPanel.Controls.Add(this.chkIsActive);
            this.detailsPanel.Controls.Add(this.cboBalanceType);
            this.detailsPanel.Controls.Add(this.nudOpeningBalance);
            this.detailsPanel.Controls.Add(this.txtAddress);
            this.detailsPanel.Controls.Add(this.txtPhone);
            this.detailsPanel.Controls.Add(this.txtName);
            this.detailsPanel.Controls.Add(this.lblBalanceType);
            this.detailsPanel.Controls.Add(this.lblOpeningBalance);
            this.detailsPanel.Controls.Add(this.lblAddress);
            this.detailsPanel.Controls.Add(this.lblPhone);
            this.detailsPanel.Controls.Add(this.lblName);
            this.detailsPanel.Controls.Add(this.lblDetailsTitle);
            this.detailsPanel.Location = new System.Drawing.Point(24, 20);
            this.detailsPanel.Name = "detailsPanel";
            this.detailsPanel.Size = new System.Drawing.Size(700, 350);
            this.detailsPanel.TabIndex = 0;
            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkIsActive.Location = new System.Drawing.Point(392, 285);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(133, 23);
            this.chkIsActive.TabIndex = 11;
            this.chkIsActive.Text = "Customer is active";
            this.chkIsActive.UseVisualStyleBackColor = true;
            // 
            // cboBalanceType
            // 
            this.cboBalanceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBalanceType.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboBalanceType.FormattingEnabled = true;
            this.cboBalanceType.Items.AddRange(new object[] {
            "Receivable",
            "Payable"});
            this.cboBalanceType.Location = new System.Drawing.Point(204, 282);
            this.cboBalanceType.Name = "cboBalanceType";
            this.cboBalanceType.Size = new System.Drawing.Size(160, 25);
            this.cboBalanceType.TabIndex = 10;
            // 
            // nudOpeningBalance
            // 
            this.nudOpeningBalance.DecimalPlaces = 2;
            this.nudOpeningBalance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.nudOpeningBalance.Location = new System.Drawing.Point(20, 282);
            this.nudOpeningBalance.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nudOpeningBalance.Name = "nudOpeningBalance";
            this.nudOpeningBalance.Size = new System.Drawing.Size(160, 25);
            this.nudOpeningBalance.TabIndex = 9;
            this.nudOpeningBalance.ThousandsSeparator = true;
            // 
            // txtAddress
            // 
            this.txtAddress.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtAddress.Location = new System.Drawing.Point(20, 176);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(548, 56);
            this.txtAddress.TabIndex = 8;
            // 
            // txtPhone
            // 
            this.txtPhone.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPhone.Location = new System.Drawing.Point(348, 88);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(220, 25);
            this.txtPhone.TabIndex = 7;
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtName.Location = new System.Drawing.Point(20, 88);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(300, 25);
            this.txtName.TabIndex = 6;
            // 
            // lblBalanceType
            // 
            this.lblBalanceType.AutoSize = true;
            this.lblBalanceType.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblBalanceType.Location = new System.Drawing.Point(204, 254);
            this.lblBalanceType.Name = "lblBalanceType";
            this.lblBalanceType.Size = new System.Drawing.Size(84, 17);
            this.lblBalanceType.TabIndex = 5;
            this.lblBalanceType.Text = "Balance Type";
            // 
            // lblOpeningBalance
            // 
            this.lblOpeningBalance.AutoSize = true;
            this.lblOpeningBalance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblOpeningBalance.Location = new System.Drawing.Point(20, 254);
            this.lblOpeningBalance.Name = "lblOpeningBalance";
            this.lblOpeningBalance.Size = new System.Drawing.Size(105, 17);
            this.lblOpeningBalance.TabIndex = 4;
            this.lblOpeningBalance.Text = "Opening Balance";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblAddress.Location = new System.Drawing.Point(20, 148);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(54, 17);
            this.lblAddress.TabIndex = 3;
            this.lblAddress.Text = "Address";
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPhone.Location = new System.Drawing.Point(348, 60);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(96, 17);
            this.lblPhone.TabIndex = 2;
            this.lblPhone.Text = "Phone Number";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(20, 60);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(100, 17);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Customer Name";
            // 
            // lblDetailsTitle
            // 
            this.lblDetailsTitle.AutoSize = true;
            this.lblDetailsTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblDetailsTitle.Location = new System.Drawing.Point(20, 18);
            this.lblDetailsTitle.Name = "lblDetailsTitle";
            this.lblDetailsTitle.Size = new System.Drawing.Size(114, 20);
            this.lblDetailsTitle.TabIndex = 0;
            this.lblDetailsTitle.Text = "Customer Details";
            // 
            // CustomerEntryForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1180, 700);
            this.Controls.Add(this.bodyPanel);
            this.Controls.Add(this.headerPanel);
            this.MinimumSize = new System.Drawing.Size(1198, 747);
            this.Name = "CustomerEntryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Customer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.CustomerEntryForm_Load);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.bodyPanel.ResumeLayout(false);
            this.photoPanel.ResumeLayout(false);
            this.photoPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCustomer)).EndInit();
            this.detailsPanel.ResumeLayout(false);
            this.detailsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOpeningBalance)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
