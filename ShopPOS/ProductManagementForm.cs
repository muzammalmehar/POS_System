using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ShopPOS.Models;
using ShopPOS.Services;

namespace ShopPOS
{
    public partial class ProductManagementForm : Form
    {
        private readonly ProductService _productService;
        private readonly Dictionary<string, Image> _imageCache;
        private List<ProductRecord> _allProducts;

        public ProductManagementForm()
        {
            _productService = new ProductService();
            _imageCache = new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);
            _allProducts = new List<ProductRecord>();
            InitializeComponent();
            dgvProducts.AutoGenerateColumns = false;
            ConfigureProductsGrid();
        }

        private void ProductManagementForm_Load(object sender, EventArgs e)
        {
            LoadLookups();
            LoadProducts();
        }

        private void LoadLookups()
        {
            List<LookupOption> categories = _productService.GetCategories();
            categories.Insert(0, new LookupOption { Id = 0, Name = "All Categories" });
            cboCategoryFilter.DataSource = categories;
            cboCategoryFilter.DisplayMember = "Name";
            cboCategoryFilter.ValueMember = "Id";

            List<LookupOption> vendors = _productService.GetVendors();
            if (vendors.Count > 0 && vendors[0].Id == 0)
            {
                vendors[0].Name = "All Vendors";
            }
            else
            {
                vendors.Insert(0, new LookupOption { Id = 0, Name = "All Vendors" });
            }

            cboVendorFilter.DataSource = vendors;
            cboVendorFilter.DisplayMember = "Name";
            cboVendorFilter.ValueMember = "Id";

            cboStatusFilter.Items.Clear();
            cboStatusFilter.Items.AddRange(new object[] { "All Products", "Active Only", "Inactive Only", "Expiry Tracked" });
            cboStatusFilter.SelectedIndex = 0;
        }

        private void LoadProducts()
        {
            _allProducts = _productService.GetProducts();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            dgvProducts.AutoGenerateColumns = false;
            string search = txtSearch.Text.Trim().ToLowerInvariant();
            int categoryId = GetLookupId(cboCategoryFilter);
            int vendorId = GetLookupId(cboVendorFilter);
            string status = Convert.ToString(cboStatusFilter.SelectedItem);
            List<ProductRecord> filtered = new List<ProductRecord>();

            for (int i = 0; i < _allProducts.Count; i++)
            {
                ProductRecord item = _allProducts[i];
                string haystack = string.Format("{0} {1} {2} {3} {4}", item.ProductCode, item.Barcode, item.ProductName, item.CategoryName, item.PreferredVendorName).ToLowerInvariant();
                if (!string.IsNullOrWhiteSpace(search) && !haystack.Contains(search))
                {
                    continue;
                }

                if (categoryId > 0 && item.CategoryId != categoryId)
                {
                    continue;
                }

                if (vendorId > 0 && item.PreferredVendorId.GetValueOrDefault() != vendorId)
                {
                    continue;
                }

                if (status == "Active Only" && !item.IsActive)
                {
                    continue;
                }

                if (status == "Inactive Only" && item.IsActive)
                {
                    continue;
                }

                if (status == "Expiry Tracked" && !item.TrackExpiry)
                {
                    continue;
                }

                filtered.Add(item);
            }

            dgvProducts.DataSource = null;
            dgvProducts.DataSource = filtered;
            lblSummary.Text = string.Format("Showing {0} product(s)", filtered.Count);
        }

        private void FilterChanged(object sender, EventArgs e) { ApplyFilter(); }

        private void btnNewProduct_Click(object sender, EventArgs e)
        {
            using (ProductEntryForm form = new ProductEntryForm())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
        }

        private void btnEditProduct_Click(object sender, EventArgs e)
        {
            ProductRecord selected = GetSelectedProduct();
            if (selected == null)
            {
                MessageBox.Show("Select a product to edit.", "Product Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (ProductEntryForm form = new ProductEntryForm(selected))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
        }

        private ProductRecord GetSelectedProduct()
        {
            if (dgvProducts.CurrentRow == null)
            {
                return null;
            }

            return dgvProducts.CurrentRow.DataBoundItem as ProductRecord;
        }

        private void ConfigureProductsGrid()
        {
            if (dgvProducts.Columns.Count > 0)
            {
                return;
            }

            ApplyGridStyle(dgvProducts);
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn.Name = "colImage";
            imageColumn.HeaderText = "Image";
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imageColumn.FillWeight = 50F;
            imageColumn.ReadOnly = true;
            dgvProducts.Columns.Add(imageColumn);
            dgvProducts.Columns.Add(CreateTextColumn("ProductCode", "Code", 70F, null));
            dgvProducts.Columns.Add(CreateTextColumn("ProductName", "Product", 150F, null));
            dgvProducts.Columns.Add(CreateTextColumn("CategoryName", "Category", 100F, null));
            dgvProducts.Columns.Add(CreateTextColumn("BrandName", "Brand", 90F, null));
            dgvProducts.Columns.Add(CreateTextColumn("PreferredVendorName", "Vendor", 110F, null));
            dgvProducts.Columns.Add(CreateTextColumn("UnitName", "Unit", 55F, null));
            dgvProducts.Columns.Add(CreateTextColumn("PurchasePrice", "Cost", 70F, "N2"));
            dgvProducts.Columns.Add(CreateTextColumn("SalePrice", "Sale", 70F, "N2"));
            dgvProducts.Columns.Add(CreateTextColumn("ReorderLevel", "Reorder", 75F, "N2"));
            dgvProducts.Columns.Add(CreateTextColumn("TrackExpiryText", "Expiry", 70F, null));
        }

        private void dgvProducts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            ProductRecord item = dgvProducts.Rows[e.RowIndex].DataBoundItem as ProductRecord;
            if (item == null)
            {
                return;
            }

            string columnName = dgvProducts.Columns[e.ColumnIndex].Name;
            if (columnName == "colImage")
            {
                e.Value = GetProductThumbnail(item.ImagePath);
                e.FormattingApplied = true;
                return;
            }

            if (dgvProducts.Columns[e.ColumnIndex].DataPropertyName == "TrackExpiryText")
            {
                e.Value = item.TrackExpiry ? "Yes" : "No";
                e.FormattingApplied = true;
                return;
            }

        }

        private void dgvProducts_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
            e.Cancel = true;
        }

        private Image GetProductThumbnail(string imagePath)
        {
            string resolvedPath = ResolveImagePath(imagePath);
            if (string.IsNullOrWhiteSpace(resolvedPath) || !File.Exists(resolvedPath))
            {
                return null;
            }

            if (_imageCache.ContainsKey(resolvedPath))
            {
                return _imageCache[resolvedPath];
            }

            using (FileStream stream = new FileStream(resolvedPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (Image source = Image.FromStream(stream))
            {
                Image copy = new Bitmap(source);
                _imageCache[resolvedPath] = copy;
                return copy;
            }
        }

        private static string ResolveImagePath(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                return null;
            }

            if (Path.IsPathRooted(imagePath))
            {
                return imagePath;
            }

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);
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

        private static DataGridViewTextBoxColumn CreateTextColumn(string propertyName, string headerText, float fillWeight, string format)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = propertyName;
            column.HeaderText = headerText;
            column.FillWeight = fillWeight;
            column.ReadOnly = true;
            if (!string.IsNullOrWhiteSpace(format))
            {
                column.DefaultCellStyle.Format = format;
            }

            return column;
        }

        private static int GetLookupId(ComboBox comboBox)
        {
            LookupOption option = comboBox.SelectedItem as LookupOption;
            return option == null ? 0 : option.Id;
        }
    }
}
