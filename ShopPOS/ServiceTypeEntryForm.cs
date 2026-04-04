using System;
using System.Drawing;
using System.Windows.Forms;
using ShopPOS.Models;
using ShopPOS.Services;

namespace ShopPOS
{
    public class ServiceTypeEntryForm : Form
    {
        private readonly ServiceCenterService _serviceService;
        private readonly int? _serviceTypeId;

        private TextBox txtServiceName;
        private TextBox txtProvider;
        private ComboBox cboCommissionType;
        private NumericUpDown nudCommissionValue;
        private NumericUpDown nudDefaultCharge;
        private CheckBox chkIsActive;

        public int SavedServiceTypeId { get; private set; }

        public ServiceTypeEntryForm(int? serviceTypeId = null)
        {
            _serviceService = new ServiceCenterService();
            _serviceTypeId = serviceTypeId;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(820, 520);
            MinimumSize = new Size(760, 500);
            StartPosition = FormStartPosition.CenterParent;
            WindowState = FormWindowState.Maximized;
            Text = _serviceTypeId.HasValue ? "Edit Service Type" : "New Service Type";

            Panel header = new Panel();
            header.BackColor = Color.FromArgb(107, 44, 145);
            header.Dock = DockStyle.Top;
            header.Height = 92;
            Controls.Add(header);

            Label lblTitle = new Label();
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(24, 18);
            lblTitle.Text = _serviceTypeId.HasValue ? "Edit Service Type" : "New Service Type";
            header.Controls.Add(lblTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.WhiteSmoke;
            lblSubtitle.Location = new Point(28, 58);
            lblSubtitle.Text = "Add a new service or update commission rules without going to the database.";
            header.Controls.Add(lblSubtitle);

            Panel body = new Panel();
            body.BackColor = Color.White;
            body.Location = new Point(24, 116);
            body.Size = new Size(760, 308);
            Controls.Add(body);

            body.Controls.Add(MakeLabel("Service Name", 22, 24));
            txtServiceName = MakeText(22, 48, 280);
            body.Controls.Add(txtServiceName);

            body.Controls.Add(MakeLabel("Provider / Company", 322, 24));
            txtProvider = MakeText(322, 48, 240);
            body.Controls.Add(txtProvider);

            body.Controls.Add(MakeLabel("Commission Type", 22, 104));
            cboCommissionType = MakeCombo(22, 128, 180);
            cboCommissionType.Items.AddRange(new object[] { "Fixed", "Percent" });
            body.Controls.Add(cboCommissionType);

            body.Controls.Add(MakeLabel("Commission Value", 222, 104));
            nudCommissionValue = MakeMoney(222, 128, 160);
            body.Controls.Add(nudCommissionValue);

            body.Controls.Add(MakeLabel("Default Charge", 402, 104));
            nudDefaultCharge = MakeMoney(402, 128, 160);
            body.Controls.Add(nudDefaultCharge);

            chkIsActive = new CheckBox();
            chkIsActive.AutoSize = true;
            chkIsActive.Checked = true;
            chkIsActive.CheckState = CheckState.Checked;
            chkIsActive.Font = new Font("Segoe UI", 10F);
            chkIsActive.Location = new Point(22, 188);
            chkIsActive.Text = "Service type is active";
            body.Controls.Add(chkIsActive);

            Label lblHelp = new Label();
            lblHelp.AutoSize = false;
            lblHelp.Font = new Font("Segoe UI", 9.5F);
            lblHelp.ForeColor = Color.DimGray;
            lblHelp.Location = new Point(22, 228);
            lblHelp.Size = new Size(700, 54);
            lblHelp.Text = "Use Fixed for a fixed rupee commission like Rs. 10 or Rs. 30. For withdrawal services, enter the charge per Rs. 1000 here. Use Percent only when the commission should be calculated from the amount automatically.";
            body.Controls.Add(lblHelp);

            Button btnSave = new Button();
            btnSave.BackColor = Color.FromArgb(107, 44, 145);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(24, 442);
            btnSave.Size = new Size(160, 42);
            btnSave.Text = _serviceTypeId.HasValue ? "Update Service" : "Create Service";
            btnSave.Click += btnSave_Click;
            Controls.Add(btnSave);

            Button btnClose = new Button();
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnClose.Location = new Point(196, 442);
            btnClose.Size = new Size(120, 42);
            btnClose.Text = "Close";
            btnClose.Click += delegate(object sender, EventArgs e) { Close(); };
            Controls.Add(btnClose);

            Load += ServiceTypeEntryForm_Load;
            ResumeLayout(false);
        }

        private void ServiceTypeEntryForm_Load(object sender, EventArgs e)
        {
            cboCommissionType.SelectedItem = "Fixed";
            if (_serviceTypeId.HasValue)
            {
                ServiceTypeRecord item = _serviceService.GetServiceType(_serviceTypeId.Value);
                txtServiceName.Text = item.ServiceName;
                txtProvider.Text = item.ProviderName;
                string commissionType = ServiceCenterService.NormalizeCommissionType(item.CommissionType);
                cboCommissionType.SelectedItem = string.IsNullOrWhiteSpace(commissionType) ? "Fixed" : commissionType;
                nudCommissionValue.Value = item.CommissionValue;
                nudDefaultCharge.Value = item.DefaultCharge;
                chkIsActive.Checked = item.IsActive;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceTypeRecord item = new ServiceTypeRecord();
                item.ServiceTypeId = _serviceTypeId.GetValueOrDefault();
                item.ServiceName = txtServiceName.Text;
                item.ProviderName = txtProvider.Text;
                item.CommissionType = Convert.ToString(cboCommissionType.SelectedItem);
                item.CommissionValue = nudCommissionValue.Value;
                item.DefaultCharge = nudDefaultCharge.Value;
                item.IsActive = chkIsActive.Checked;

                SavedServiceTypeId = _serviceService.SaveServiceType(item);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Service Type Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static Label MakeLabel(string text, int x, int y)
        {
            return new Label { AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold), Location = new Point(x, y), Text = text };
        }

        private static TextBox MakeText(int x, int y, int w)
        {
            return new TextBox { Font = new Font("Segoe UI", 10F), Location = new Point(x, y), Size = new Size(w, 28) };
        }

        private static ComboBox MakeCombo(int x, int y, int w)
        {
            return new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10F), Location = new Point(x, y), Size = new Size(w, 30) };
        }

        private static NumericUpDown MakeMoney(int x, int y, int w)
        {
            return new NumericUpDown { DecimalPlaces = 2, Maximum = 100000000, ThousandsSeparator = true, Font = new Font("Segoe UI", 10F), Location = new Point(x, y), Size = new Size(w, 30) };
        }
    }
}
