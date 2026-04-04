namespace ShopPOS
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelHero;
        private System.Windows.Forms.Panel panelCard;
        private System.Windows.Forms.Panel panelCardTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblHeroTitle;
        private System.Windows.Forms.Label lblHeroSubtitle;
        private System.Windows.Forms.Label lblHeroPoints;
        private System.Windows.Forms.Label lblCardTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox chkRememberMe;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label lblConnectionHint;

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
            this.components = new System.ComponentModel.Container();
            this.panelHero = new System.Windows.Forms.Panel();
            this.lblHeroPoints = new System.Windows.Forms.Label();
            this.lblHeroSubtitle = new System.Windows.Forms.Label();
            this.lblHeroTitle = new System.Windows.Forms.Label();
            this.panelCard = new System.Windows.Forms.Panel();
            this.panelCardTop = new System.Windows.Forms.Panel();
            this.lblCardTitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblConnectionHint = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.chkRememberMe = new System.Windows.Forms.CheckBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.panelHero.SuspendLayout();
            this.panelCard.SuspendLayout();
            this.panelCardTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHero
            // 
            this.panelHero.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelHero.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(70)))), ((int)(((byte)(60)))));
            this.panelHero.Controls.Add(this.lblHeroPoints);
            this.panelHero.Controls.Add(this.lblHeroSubtitle);
            this.panelHero.Controls.Add(this.lblHeroTitle);
            this.panelHero.Location = new System.Drawing.Point(68, 73);
            this.panelHero.Margin = new System.Windows.Forms.Padding(2);
            this.panelHero.Name = "panelHero";
            this.panelHero.Size = new System.Drawing.Size(315, 422);
            this.panelHero.TabIndex = 0;
            // 
            // lblHeroPoints
            // 
            this.lblHeroPoints.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblHeroPoints.Location = new System.Drawing.Point(28, 179);
            this.lblHeroPoints.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHeroPoints.Name = "lblHeroPoints";
            this.lblHeroPoints.Size = new System.Drawing.Size(248, 146);
            this.lblHeroPoints.TabIndex = 2;
            // 
            // lblHeroSubtitle
            // 
            this.lblHeroSubtitle.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblHeroSubtitle.Location = new System.Drawing.Point(28, 120);
            this.lblHeroSubtitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHeroSubtitle.Name = "lblHeroSubtitle";
            this.lblHeroSubtitle.Size = new System.Drawing.Size(249, 44);
            this.lblHeroSubtitle.TabIndex = 1;
            this.lblHeroSubtitle.Text = "Welcome back. Sign in to manage billing, stock, services, vendor payments, custom" +
    "er credit, and daily reports.";
            // 
            // lblHeroTitle
            // 
            this.lblHeroTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblHeroTitle.ForeColor = System.Drawing.Color.White;
            this.lblHeroTitle.Location = new System.Drawing.Point(25, 36);
            this.lblHeroTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHeroTitle.Name = "lblHeroTitle";
            this.lblHeroTitle.Size = new System.Drawing.Size(265, 115);
            this.lblHeroTitle.TabIndex = 0;
            this.lblHeroTitle.Text = "Arslan Communication and Karyana Store";
            // 
            // panelCard
            // 
            this.panelCard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelCard.BackColor = System.Drawing.Color.White;
            this.panelCard.Controls.Add(this.panelCardTop);
            this.panelCard.Controls.Add(this.lblTitle);
            this.panelCard.Controls.Add(this.lblConnectionHint);
            this.panelCard.Controls.Add(this.lblStatus);
            this.panelCard.Controls.Add(this.btnLogin);
            this.panelCard.Controls.Add(this.chkRememberMe);
            this.panelCard.Controls.Add(this.txtPassword);
            this.panelCard.Controls.Add(this.lblPassword);
            this.panelCard.Controls.Add(this.txtUsername);
            this.panelCard.Controls.Add(this.lblUsername);
            this.panelCard.Controls.Add(this.lblSubtitle);
            this.panelCard.Location = new System.Drawing.Point(382, 73);
            this.panelCard.Margin = new System.Windows.Forms.Padding(2);
            this.panelCard.Name = "panelCard";
            this.panelCard.Size = new System.Drawing.Size(322, 422);
            this.panelCard.TabIndex = 0;
            // 
            // panelCardTop
            // 
            this.panelCardTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(242)))), ((int)(((byte)(237)))));
            this.panelCardTop.Controls.Add(this.lblCardTitle);
            this.panelCardTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCardTop.Location = new System.Drawing.Point(0, 0);
            this.panelCardTop.Margin = new System.Windows.Forms.Padding(2);
            this.panelCardTop.Name = "panelCardTop";
            this.panelCardTop.Size = new System.Drawing.Size(322, 60);
            this.panelCardTop.TabIndex = 10;
            // 
            // lblCardTitle
            // 
            this.lblCardTitle.AutoSize = true;
            this.lblCardTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 15F, System.Drawing.FontStyle.Bold);
            this.lblCardTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(70)))), ((int)(((byte)(60)))));
            this.lblCardTitle.Location = new System.Drawing.Point(21, 16);
            this.lblCardTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCardTitle.Name = "lblCardTitle";
            this.lblCardTitle.Size = new System.Drawing.Size(76, 28);
            this.lblCardTitle.TabIndex = 0;
            this.lblCardTitle.Text = "Sign In";
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(32)))), ((int)(((byte)(44)))));
            this.lblTitle.Location = new System.Drawing.Point(21, 80);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(274, 50);
            this.lblTitle.TabIndex = 9;
            this.lblTitle.Text = "Arslan Communication and Karyana Store";
            // 
            // lblConnectionHint
            // 
            this.lblConnectionHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblConnectionHint.Location = new System.Drawing.Point(25, 359);
            this.lblConnectionHint.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblConnectionHint.Name = "lblConnectionHint";
            this.lblConnectionHint.Size = new System.Drawing.Size(274, 47);
            this.lblConnectionHint.TabIndex = 8;
            this.lblConnectionHint.Text = "Connection:";
            // 
            // lblStatus
            // 
            this.lblStatus.ForeColor = System.Drawing.Color.Firebrick;
            this.lblStatus.Location = new System.Drawing.Point(25, 318);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(274, 32);
            this.lblStatus.TabIndex = 7;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(120)))), ((int)(((byte)(91)))));
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(25, 276);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(2);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(274, 34);
            this.btnLogin.TabIndex = 6;
            this.btnLogin.Text = "Sign In";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // chkRememberMe
            // 
            this.chkRememberMe.AutoSize = true;
            this.chkRememberMe.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.chkRememberMe.Location = new System.Drawing.Point(25, 247);
            this.chkRememberMe.Margin = new System.Windows.Forms.Padding(2);
            this.chkRememberMe.Name = "chkRememberMe";
            this.chkRememberMe.Size = new System.Drawing.Size(113, 21);
            this.chkRememberMe.TabIndex = 10;
            this.chkRememberMe.Text = "Remember me";
            this.chkRememberMe.UseVisualStyleBackColor = true;
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.txtPassword.Location = new System.Drawing.Point(25, 211);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(2);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(275, 26);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblPassword.Location = new System.Drawing.Point(24, 192);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(64, 17);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Password";
            // 
            // txtUsername
            // 
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.txtUsername.Location = new System.Drawing.Point(25, 164);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(2);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(275, 26);
            this.txtUsername.TabIndex = 3;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblUsername.Location = new System.Drawing.Point(24, 145);
            this.lblUsername.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(67, 17);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "Username";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblSubtitle.Location = new System.Drawing.Point(24, 119);
            this.lblSubtitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(274, 20);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Sign in to continue to your store desk";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(242)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(771, 569);
            this.Controls.Add(this.panelHero);
            this.Controls.Add(this.panelCard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Store Login";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.panelHero.ResumeLayout(false);
            this.panelCard.ResumeLayout(false);
            this.panelCard.PerformLayout();
            this.panelCardTop.ResumeLayout(false);
            this.panelCardTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
