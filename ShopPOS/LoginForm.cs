using System;
using System.Configuration;
using System.Windows.Forms;
using ShopPOS.Models;
using ShopPOS.Services;

namespace ShopPOS
{
    public partial class LoginForm : Form
    {
        private readonly AuthService _authService;
        private readonly LoginPreferenceService _loginPreferenceService;

        public LoginForm()
        {
            InitializeComponent();
            _authService = new AuthService();
            _loginPreferenceService = new LoginPreferenceService();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            Text = ShopBranding.ShopName + " Login";
            lblTitle.Text = ShopBranding.ShopName;
            lblConnectionHint.Text = BuildConnectionHint();
            LoadSavedLogin();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            AttemptLogin();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.SuppressKeyPress = true;
            AttemptLogin();
        }

        private void AttemptLogin()
        {
            errorProvider.Clear();

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                errorProvider.SetError(txtUsername, "Username is required.");
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider.SetError(txtPassword, "Password is required.");
                txtPassword.Focus();
                return;
            }

            ToggleLoginState(false);
            lblStatus.Text = "Checking credentials...";

            try
            {
                UserSession session = _authService.Authenticate(txtUsername.Text, txtPassword.Text);

                if (session == null)
                {
                    lblStatus.Text = "Invalid username or password.";
                    SaveRememberedLogin(false);
                    txtPassword.SelectAll();
                    txtPassword.Focus();
                    return;
                }

                SaveRememberedLogin(chkRememberMe.Checked);
                lblStatus.Text = "Login successful.";
                Hide();

                using (MainForm mainForm = new MainForm(session))
                {
                    mainForm.ShowDialog(this);
                }

                txtPassword.Clear();
                LoadSavedLogin();
                Show();
            }
            catch (ConfigurationErrorsException ex)
            {
                lblStatus.Text = ex.Message;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Database connection failed. Check App.config and MySQL ODBC setup.";
                MessageBox.Show(
                    ex.Message,
                    "Login Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                ToggleLoginState(true);
            }
        }

        private void ToggleLoginState(bool isEnabled)
        {
            txtUsername.Enabled = isEnabled;
            txtPassword.Enabled = isEnabled;
            chkRememberMe.Enabled = isEnabled;
            btnLogin.Enabled = isEnabled;
            UseWaitCursor = !isEnabled;
        }

        private void LoadSavedLogin()
        {
            LoginPreference preference = _loginPreferenceService.Load();
            chkRememberMe.Checked = preference.RememberMe;
            txtUsername.Text = preference.Username;
            txtPassword.Text = preference.Password;

            if (!string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Select(txtPassword.TextLength, 0);
                btnLogin.Focus();
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtPassword.Focus();
                return;
            }

            txtUsername.Focus();
        }

        private void SaveRememberedLogin(bool rememberMe)
        {
            _loginPreferenceService.Save(txtUsername.Text, txtPassword.Text, rememberMe);
        }

        private static string BuildConnectionHint()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["ShopPosDb"];
            if (settings == null)
            {
                return "Database connection string is not configured.";
            }

            return "Connection: " + settings.ConnectionString;
        }
    }
}
