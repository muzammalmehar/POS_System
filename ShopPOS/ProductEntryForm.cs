using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using ShopPOS.Models;
using ShopPOS.Services;

namespace ShopPOS
{
    public class ProductEntryForm : Form
    {
        private readonly ProductService _productService;
        private readonly ProductRecord _product;

        private TextBox txtProductCode;
        private TextBox txtBarcode;
        private TextBox txtProductName;
        private ComboBox cboCategory;
        private ComboBox cboBrand;
        private ComboBox cboUnit;
        private ComboBox cboVendor;
        private NumericUpDown nudPurchasePrice;
        private NumericUpDown nudSalePrice;
        private NumericUpDown nudReorderLevel;
        private DateTimePicker dtpDefaultExpiryDate;
        private CheckBox chkTrackStock;
        private CheckBox chkTrackExpiry;
        private CheckBox chkIsActive;
        private Label lblExpiryPreview;
        private PictureBox picProduct;

        private string _selectedImagePath;
        private bool _imageChanged;

        public ProductEntryForm() : this(null) { }

        public ProductEntryForm(ProductRecord product)
        {
            _productService = new ProductService();
            _product = product;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(820, 760);
            MinimumSize = new Size(838, 807);
            StartPosition = FormStartPosition.CenterParent;
            WindowState = FormWindowState.Maximized;
            Text = _product == null ? "New Product" : "Edit Product";

            Panel header = new Panel();
            header.BackColor = Color.FromArgb(196, 106, 33);
            header.Dock = DockStyle.Top;
            header.Height = 90;
            Controls.Add(header);

            Label lblTitle = new Label();
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(24, 18);
            lblTitle.Text = _product == null ? "New Product" : "Edit Product";
            header.Controls.Add(lblTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.WhiteSmoke;
            lblSubtitle.Location = new Point(28, 57);
            lblSubtitle.Text = "Manage product details, pricing, image, vendor, and expiry settings.";
            header.Controls.Add(lblSubtitle);

            Panel body = new Panel();
            body.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            body.BackColor = Color.White;
            body.Location = new Point(20, 110);
            body.Size = new Size(780, 630);
            Controls.Add(body);

            AddFieldLabel(body, "Product Code", 24, 24);
            txtProductCode = CreateTextBox(24, 50, 260);
            txtProductCode.ReadOnly = true;
            txtProductCode.BackColor = Color.FromArgb(248, 250, 253);
            body.Controls.Add(txtProductCode);

            AddFieldLabel(body, "Barcode", 304, 24);
            txtBarcode = CreateTextBox(304, 50, 230);
            body.Controls.Add(txtBarcode);

            GroupBox imageGroup = new GroupBox();
            imageGroup.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            imageGroup.Location = new Point(554, 18);
            imageGroup.Size = new Size(200, 220);
            imageGroup.Text = "Product Image";
            body.Controls.Add(imageGroup);

            picProduct = new PictureBox();
            picProduct.BackColor = Color.FromArgb(248, 250, 253);
            picProduct.BorderStyle = BorderStyle.FixedSingle;
            picProduct.Location = new Point(20, 30);
            picProduct.Size = new Size(160, 108);
            picProduct.SizeMode = PictureBoxSizeMode.Zoom;
            imageGroup.Controls.Add(picProduct);

            AddActionButton(imageGroup, "Choose", 20, 150, btnChooseImage_Click);
            AddActionButton(imageGroup, "Capture", 101, 150, btnCaptureImage_Click);
            AddActionButton(imageGroup, "Clear", 20, 188, btnClearImage_Click, 160, 28);

            AddFieldLabel(body, "Product Name", 24, 92);
            txtProductName = CreateTextBox(24, 118, 510);
            body.Controls.Add(txtProductName);

            AddFieldLabel(body, "Category", 24, 160);
            cboCategory = CreateComboBox(24, 186, 240);
            body.Controls.Add(cboCategory);

            AddFieldLabel(body, "Brand", 294, 160);
            cboBrand = CreateComboBox(294, 186, 240);
            body.Controls.Add(cboBrand);

            AddFieldLabel(body, "Base Unit", 24, 228);
            cboUnit = CreateComboBox(24, 254, 240);
            body.Controls.Add(cboUnit);

            AddFieldLabel(body, "Preferred Vendor", 294, 228);
            cboVendor = CreateComboBox(294, 254, 240);
            body.Controls.Add(cboVendor);

            AddFieldLabel(body, "Purchase Price", 24, 296);
            nudPurchasePrice = CreateMoneyNumeric(24, 322, 160);
            body.Controls.Add(nudPurchasePrice);

            AddFieldLabel(body, "Sale Price", 204, 296);
            nudSalePrice = CreateMoneyNumeric(204, 322, 160);
            body.Controls.Add(nudSalePrice);

            AddFieldLabel(body, "Reorder Level", 384, 296);
            nudReorderLevel = CreateMoneyNumeric(384, 322, 150);
            body.Controls.Add(nudReorderLevel);

            chkTrackStock = CreateCheckBox("Track stock for this product", 24, 380, true);
            body.Controls.Add(chkTrackStock);

            chkTrackExpiry = CreateCheckBox("Track expiry for this product", 24, 410, false);
            chkTrackExpiry.CheckedChanged += chkTrackExpiry_CheckedChanged;
            body.Controls.Add(chkTrackExpiry);

            AddFieldLabel(body, "Expiry Date", 294, 380);
            dtpDefaultExpiryDate = new DateTimePicker();
            dtpDefaultExpiryDate.Font = new Font("Segoe UI", 10F);
            dtpDefaultExpiryDate.Format = DateTimePickerFormat.Custom;
            dtpDefaultExpiryDate.CustomFormat = "MM/dd/yyyy";
            dtpDefaultExpiryDate.Location = new Point(294, 406);
            dtpDefaultExpiryDate.Size = new Size(180, 30);
            dtpDefaultExpiryDate.Enabled = false;
            dtpDefaultExpiryDate.ValueChanged += dtpDefaultExpiryDate_ValueChanged;
            body.Controls.Add(dtpDefaultExpiryDate);

            lblExpiryPreview = new Label();
            lblExpiryPreview.AutoSize = true;
            lblExpiryPreview.Font = new Font("Segoe UI", 9F);
            lblExpiryPreview.ForeColor = Color.DimGray;
            lblExpiryPreview.Location = new Point(24, 450);
            body.Controls.Add(lblExpiryPreview);

            chkIsActive = CreateCheckBox("Product is active", 24, 488, true);
            body.Controls.Add(chkIsActive);

            AddBottomButton(body, "Save Product", 24, 550, Color.FromArgb(24, 125, 68), Color.White, btnSave_Click, 150);
            AddBottomButton(body, "Close", 188, 550, Color.White, Color.Black, btnClose_Click, 110);

            Load += ProductEntryForm_Load;
            ResumeLayout(false);
        }

        private void ProductEntryForm_Load(object sender, EventArgs e)
        {
            LoadLookups();
            if (_product == null) ResetForm();
            else LoadProduct();
        }

        private void LoadLookups()
        {
            cboCategory.DataSource = _productService.GetCategories();
            cboCategory.DisplayMember = "Name";
            cboCategory.ValueMember = "Id";

            cboBrand.DataSource = _productService.GetBrands();
            cboBrand.DisplayMember = "Name";
            cboBrand.ValueMember = "Id";

            cboUnit.DataSource = _productService.GetUnits();
            cboUnit.DisplayMember = "Name";
            cboUnit.ValueMember = "Id";

            cboVendor.DataSource = _productService.GetVendors();
            cboVendor.DisplayMember = "Name";
            cboVendor.ValueMember = "Id";
        }

        private void ResetForm()
        {
            txtProductCode.Text = _productService.GenerateNextProductCode();
            txtBarcode.Clear();
            txtProductName.Clear();
            cboCategory.SelectedIndex = 0;
            cboBrand.SelectedIndex = 0;
            cboUnit.SelectedIndex = 0;
            cboVendor.SelectedIndex = 0;
            nudPurchasePrice.Value = 0;
            nudSalePrice.Value = 0;
            nudReorderLevel.Value = 0;
            chkTrackStock.Checked = true;
            chkTrackExpiry.Checked = false;
            dtpDefaultExpiryDate.Value = DateTime.Today;
            dtpDefaultExpiryDate.Enabled = false;
            chkIsActive.Checked = true;
            _selectedImagePath = null;
            _imageChanged = false;
            LoadProductImage(null);
            UpdateExpiryPreview();
        }

        private void LoadProduct()
        {
            txtProductCode.Text = _product.ProductCode;
            txtBarcode.Text = _product.Barcode;
            txtProductName.Text = _product.ProductName;
            cboCategory.SelectedValue = _product.CategoryId;
            cboBrand.SelectedValue = _product.BrandId.HasValue ? _product.BrandId.Value : 0;
            cboUnit.SelectedValue = _product.BaseUnitId;
            cboVendor.SelectedValue = _product.PreferredVendorId.HasValue ? _product.PreferredVendorId.Value : 0;
            nudPurchasePrice.Value = _product.PurchasePrice;
            nudSalePrice.Value = _product.SalePrice;
            nudReorderLevel.Value = _product.ReorderLevel;
            chkTrackStock.Checked = _product.TrackStock;
            chkTrackExpiry.Checked = _product.TrackExpiry;
            dtpDefaultExpiryDate.Value = _product.DefaultExpiryDate.HasValue ? _product.DefaultExpiryDate.Value : DateTime.Today;
            dtpDefaultExpiryDate.Enabled = _product.TrackExpiry;
            chkIsActive.Checked = _product.IsActive;
            _selectedImagePath = _product.ImagePath;
            _imageChanged = false;
            LoadProductImage(_product.ImagePath);
            UpdateExpiryPreview();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ProductSaveRequest request = new ProductSaveRequest();
                request.ProductId = _product == null ? (int?)null : _product.ProductId;
                request.ProductCode = txtProductCode.Text;
                request.Barcode = txtBarcode.Text;
                request.ProductName = txtProductName.Text;
                request.CategoryId = GetSelectedLookupId(cboCategory).GetValueOrDefault();
                request.BrandId = GetSelectedLookupId(cboBrand);
                request.BaseUnitId = GetSelectedLookupId(cboUnit).GetValueOrDefault();
                request.PurchasePrice = nudPurchasePrice.Value;
                request.SalePrice = nudSalePrice.Value;
                request.ReorderLevel = nudReorderLevel.Value;
                request.TrackStock = chkTrackStock.Checked;
                request.TrackExpiry = chkTrackExpiry.Checked;
                request.DefaultShelfLifeDays = null;
                request.DefaultExpiryDate = chkTrackExpiry.Checked ? (DateTime?)dtpDefaultExpiryDate.Value.Date : null;
                request.IsActive = chkIsActive.Checked;
                request.ImagePath = SaveImageIfNeeded(request.ProductCode);
                request.PreferredVendorId = GetSelectedLookupId(cboVendor);
                _productService.SaveProduct(request);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Product Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e) { Close(); }

        private void chkTrackExpiry_CheckedChanged(object sender, EventArgs e)
        {
            dtpDefaultExpiryDate.Enabled = chkTrackExpiry.Checked;
            if (!chkTrackExpiry.Checked) dtpDefaultExpiryDate.Value = DateTime.Today;
            UpdateExpiryPreview();
        }

        private void dtpDefaultExpiryDate_ValueChanged(object sender, EventArgs e) { UpdateExpiryPreview(); }

        private void UpdateExpiryPreview()
        {
            lblExpiryPreview.Text = chkTrackExpiry.Checked
                ? string.Format("Default expiry date set to: {0:MM/dd/yyyy}", dtpDefaultExpiryDate.Value.Date)
                : "Expiry tracking is off for this product.";
        }

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            _selectedImagePath = dialog.FileName;
            _imageChanged = true;
            LoadProductImage(_selectedImagePath);
        }

        private void btnCaptureImage_Click(object sender, EventArgs e)
        {
            try
            {
                string capturedPath = CaptureImageFromCamera();
                if (string.IsNullOrWhiteSpace(capturedPath)) return;
                _selectedImagePath = capturedPath;
                _imageChanged = true;
                LoadProductImage(_selectedImagePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Camera Capture", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClearImage_Click(object sender, EventArgs e)
        {
            _selectedImagePath = null;
            _imageChanged = true;
            LoadProductImage(null);
        }

        private string SaveImageIfNeeded(string productCode)
        {
            if (!_imageChanged) return _selectedImagePath;
            if (string.IsNullOrWhiteSpace(_selectedImagePath)) return null;
            string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProductImages");
            Directory.CreateDirectory(imagesFolder);
            string extension = Path.GetExtension(_selectedImagePath);
            if (string.IsNullOrWhiteSpace(extension)) extension = ".jpg";
            string fileName = string.Format("{0}_{1}{2}", MakeSafeFileName(productCode), DateTime.Now.ToString("yyyyMMddHHmmss"), extension);
            string destinationPath = Path.Combine(imagesFolder, fileName);
            File.Copy(_selectedImagePath, destinationPath, true);
            return Path.Combine("ProductImages", fileName);
        }

        private void LoadProductImage(string imagePath)
        {
            if (picProduct.Image != null)
            {
                Image oldImage = picProduct.Image;
                picProduct.Image = null;
                oldImage.Dispose();
            }

            string resolvedPath = ResolveImagePath(imagePath);
            if (string.IsNullOrWhiteSpace(resolvedPath) || !File.Exists(resolvedPath)) return;
            using (FileStream stream = new FileStream(resolvedPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (Image source = Image.FromStream(stream))
            {
                picProduct.Image = new Bitmap(source);
            }
        }

        private static string ResolveImagePath(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath)) return null;
            return Path.IsPathRooted(imagePath) ? imagePath : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);
        }

        private static string CaptureImageFromCamera()
        {
            Type commonDialogType = Type.GetTypeFromProgID("WIA.CommonDialog");
            if (commonDialogType == null) throw new InvalidOperationException("Windows camera capture is not available on this PC.");
            object dialog = Activator.CreateInstance(commonDialogType);
            object image = commonDialogType.InvokeMember("ShowAcquireImage", BindingFlags.InvokeMethod, null, dialog, new object[] { 2, 0, 0, "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}", true, true, false });
            if (image == null) return null;
            string captureFolder = Path.Combine(Path.GetTempPath(), "ShopPOSCamera");
            Directory.CreateDirectory(captureFolder);
            string filePath = Path.Combine(captureFolder, "capture_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg");
            image.GetType().InvokeMember("SaveFile", BindingFlags.InvokeMethod, null, image, new object[] { filePath });
            return filePath;
        }

        private static void AddFieldLabel(Control parent, string text, int left, int top)
        {
            Label label = new Label();
            label.AutoSize = true;
            label.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            label.Location = new Point(left, top);
            label.Text = text;
            parent.Controls.Add(label);
        }

        private static TextBox CreateTextBox(int left, int top, int width)
        {
            TextBox textBox = new TextBox();
            textBox.Font = new Font("Segoe UI", 10F);
            textBox.Location = new Point(left, top);
            textBox.Size = new Size(width, 30);
            return textBox;
        }

        private static ComboBox CreateComboBox(int left, int top, int width)
        {
            ComboBox comboBox = new ComboBox();
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.Font = new Font("Segoe UI", 10F);
            comboBox.Location = new Point(left, top);
            comboBox.Size = new Size(width, 31);
            return comboBox;
        }

        private static NumericUpDown CreateMoneyNumeric(int left, int top, int width)
        {
            NumericUpDown numeric = new NumericUpDown();
            numeric.DecimalPlaces = 2;
            numeric.Maximum = 100000000;
            numeric.ThousandsSeparator = true;
            numeric.Font = new Font("Segoe UI", 10F);
            numeric.Location = new Point(left, top);
            numeric.Size = new Size(width, 30);
            return numeric;
        }

        private static CheckBox CreateCheckBox(string text, int left, int top, bool isChecked)
        {
            CheckBox checkBox = new CheckBox();
            checkBox.AutoSize = true;
            checkBox.Font = new Font("Segoe UI", 10F);
            checkBox.Location = new Point(left, top);
            checkBox.Text = text;
            checkBox.Checked = isChecked;
            return checkBox;
        }

        private static void AddActionButton(Control parent, string text, int left, int top, EventHandler clickHandler, int width = 75, int height = 34)
        {
            Button button = new Button();
            button.BackColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.Font = new Font("Segoe UI", 9.5F);
            button.Location = new Point(left, top);
            button.Size = new Size(width, height);
            button.Text = text;
            button.Click += clickHandler;
            parent.Controls.Add(button);
        }

        private static void AddBottomButton(Control parent, string text, int left, int top, Color backColor, Color foreColor, EventHandler clickHandler, int width)
        {
            Button button = new Button();
            button.BackColor = backColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = backColor == Color.White ? 1 : 0;
            button.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            button.ForeColor = foreColor;
            button.Location = new Point(left, top);
            button.Size = new Size(width, 42);
            button.Text = text;
            button.Click += clickHandler;
            parent.Controls.Add(button);
        }

        private static int? GetSelectedLookupId(ComboBox comboBox)
        {
            LookupOption option = comboBox.SelectedItem as LookupOption;
            return option == null || option.Id <= 0 ? (int?)null : option.Id;
        }

        private static string MakeSafeFileName(string value)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            for (int index = 0; index < invalidChars.Length; index++) value = value.Replace(invalidChars[index], '_');
            return string.IsNullOrWhiteSpace(value) ? "product" : value;
        }
    }
}
