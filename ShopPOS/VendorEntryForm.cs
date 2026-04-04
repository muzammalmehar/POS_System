using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ShopPOS.Models;
using ShopPOS.Services;

namespace ShopPOS
{
    public partial class VendorEntryForm : Form
    {
        private readonly VendorService _vendorService;
        private readonly int? _supplierId;

        public int SavedSupplierId { get; private set; }

        public VendorEntryForm(int? supplierId = null)
        {
            _vendorService = new VendorService();
            _supplierId = supplierId;
            InitializeComponent();
            ApplyFormMode();
            ConfigureVendorProductsGrid();
        }

        private void ApplyFormMode()
        {
            bool isEdit = _supplierId.HasValue;
            Text = isEdit ? "Edit Vendor" : "New Vendor";
            lblTitle.Text = isEdit ? "Edit Vendor" : "New Vendor";
            btnSave.Text = isEdit ? "Update Vendor" : "Create Vendor";
        }

        private void VendorEntryForm_Load(object sender, EventArgs e)
        {
            cboBalanceType.SelectedIndex = 0;
            cboVisitDay.SelectedIndex = 0;
            cboPaymentCycle.SelectedIndex = 0;
            chkHasNextPaymentDate.Checked = false;
            dtpNextPaymentDate.Enabled = false;
            LoadVendorProducts();

            if (_supplierId.HasValue)
            {
                LoadVendor();
            }
        }

        private void LoadVendor()
        {
            List<VendorRecord> vendors = _vendorService.GetVendors();
            for (int i = 0; i < vendors.Count; i++)
            {
                if (vendors[i].SupplierId != _supplierId.Value)
                {
                    continue;
                }

                VendorRecord item = vendors[i];
                txtVendorName.Text = item.SupplierName;
                txtPhone.Text = item.Phone;
                txtAddress.Text = item.Address;
                nudOpeningBalance.Value = item.OpeningBalance;
                cboBalanceType.SelectedItem = item.BalanceType;
                cboVisitDay.SelectedItem = string.IsNullOrWhiteSpace(item.PreferredVisitDay) ? "" : item.PreferredVisitDay;
                cboPaymentCycle.SelectedItem = string.IsNullOrWhiteSpace(item.PaymentCycle) ? "" : item.PaymentCycle;
                nudCreditDays.Value = item.CreditDays;
                chkHasNextPaymentDate.Checked = item.NextPaymentDate.HasValue;
                dtpNextPaymentDate.Value = item.NextPaymentDate ?? DateTime.Today;
                chkIsActive.Checked = item.IsActive;
                txtNotes.Text = item.Notes;
                LoadVendorProducts();
                break;
            }
        }

        private void LoadVendorProducts()
        {
            dgvVendorProducts.DataSource = null;
            dgvVendorProducts.DataSource = _vendorService.GetVendorProducts(_supplierId);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtVendorName.Text))
                {
                    throw new InvalidOperationException("Enter vendor name before saving.");
                }

                dgvVendorProducts.EndEdit();

                VendorSaveRequest request = new VendorSaveRequest();
                request.SupplierId = _supplierId;
                request.SupplierName = txtVendorName.Text;
                request.Phone = txtPhone.Text;
                request.Address = txtAddress.Text;
                request.OpeningBalance = nudOpeningBalance.Value;
                request.BalanceType = Convert.ToString(cboBalanceType.SelectedItem);
                request.IsActive = chkIsActive.Checked;
                request.PreferredVisitDay = Convert.ToString(cboVisitDay.SelectedItem);
                request.PaymentCycle = Convert.ToString(cboPaymentCycle.SelectedItem);
                request.CreditDays = Convert.ToInt32(nudCreditDays.Value);
                request.NextPaymentDate = chkHasNextPaymentDate.Checked ? (DateTime?)dtpNextPaymentDate.Value.Date : null;
                request.Notes = txtNotes.Text;

                SavedSupplierId = _vendorService.SaveVendor(request);
                _vendorService.SaveVendorProductLinks(SavedSupplierId, GetVendorProductItems());

                MessageBox.Show("Vendor saved successfully.", "Vendor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Vendor Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void chkHasNextPaymentDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpNextPaymentDate.Enabled = chkHasNextPaymentDate.Checked;
        }

        private List<VendorProductLinkItem> GetVendorProductItems()
        {
            List<VendorProductLinkItem> items = new List<VendorProductLinkItem>();
            for (int i = 0; i < dgvVendorProducts.Rows.Count; i++)
            {
                VendorProductLinkItem item = dgvVendorProducts.Rows[i].DataBoundItem as VendorProductLinkItem;
                if (item != null)
                {
                    items.Add(item);
                }
            }

            return items;
        }

        private void ConfigureVendorProductsGrid()
        {
            if (dgvVendorProducts.Columns.Count > 0)
            {
                return;
            }

            ApplyGridStyle(dgvVendorProducts);
            dgvVendorProducts.Columns.Add(CreateCheckColumn("IsLinked", "Link", 45F));
            dgvVendorProducts.Columns.Add(CreateTextColumn("ProductCode", "Code", 70F, null, true));
            dgvVendorProducts.Columns.Add(CreateTextColumn("ProductName", "Product", 170F, null, true));
            dgvVendorProducts.Columns.Add(CreateTextColumn("SalePrice", "Sale", 65F, "N2", true));
            dgvVendorProducts.Columns.Add(CreateTextColumn("LastPurchasePrice", "Last Cost", 80F, "N2", false));
            dgvVendorProducts.Columns.Add(CreateCheckColumn("IsPreferred", "Preferred", 55F));
        }

        private static void ApplyGridStyle(DataGridView grid)
        {
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = Color.FromArgb(243, 246, 251);
            headerStyle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            headerStyle.SelectionBackColor = Color.FromArgb(243, 246, 251);
            headerStyle.SelectionForeColor = Color.Black;
            grid.ColumnHeadersDefaultCellStyle = headerStyle;
            grid.ColumnHeadersHeight = 36;

            DataGridViewCellStyle rowStyle = new DataGridViewCellStyle();
            rowStyle.BackColor = Color.White;
            rowStyle.Font = new Font("Segoe UI", 9F);
            rowStyle.SelectionBackColor = Color.FromArgb(233, 240, 255);
            rowStyle.SelectionForeColor = Color.Black;
            grid.DefaultCellStyle = rowStyle;
        }

        private static DataGridViewTextBoxColumn CreateTextColumn(string propertyName, string headerText, float fillWeight, string format, bool readOnly)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = propertyName;
            column.HeaderText = headerText;
            column.FillWeight = fillWeight;
            column.ReadOnly = readOnly;
            if (!string.IsNullOrWhiteSpace(format))
            {
                column.DefaultCellStyle.Format = format;
            }

            return column;
        }

        private static DataGridViewCheckBoxColumn CreateCheckColumn(string propertyName, string headerText, float fillWeight)
        {
            DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();
            column.DataPropertyName = propertyName;
            column.HeaderText = headerText;
            column.FillWeight = fillWeight;
            return column;
        }

    }
}
