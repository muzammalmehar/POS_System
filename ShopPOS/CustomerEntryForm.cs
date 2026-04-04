using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using ShopPOS.Models;
using ShopPOS.Services;

namespace ShopPOS
{
    public partial class CustomerEntryForm : Form
    {
        private readonly CustomerService _customerService;
        private readonly CustomerRecord _customer;

        private string _selectedImagePath;
        private bool _imageChanged;

        public int SavedCustomerId { get; private set; }

        public CustomerEntryForm(CustomerRecord customer = null)
        {
            _customerService = new CustomerService();
            _customer = customer;
            InitializeComponent();
            Text = _customer == null ? "New Customer" : "Edit Customer";
            lblTitle.Text = Text;
        }

        private void CustomerEntryForm_Load(object sender, EventArgs e)
        {
            cboBalanceType.SelectedItem = "Receivable";
            LoadCustomerImage(null);

            if (_customer == null)
            {
                return;
            }

            txtName.Text = _customer.CustomerName;
            txtPhone.Text = _customer.Phone;
            txtAddress.Text = _customer.Address;
            nudOpeningBalance.Value = _customer.OpeningBalance;
            cboBalanceType.SelectedItem = _customer.BalanceType;
            chkIsActive.Checked = _customer.IsActive;
            _selectedImagePath = _customer.ImagePath;
            _imageChanged = false;
            LoadCustomerImage(_customer.ImagePath);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CustomerRecord customer = new CustomerRecord();
                customer.CustomerId = _customer == null ? 0 : _customer.CustomerId;
                customer.CustomerName = txtName.Text;
                customer.Phone = txtPhone.Text;
                customer.Address = txtAddress.Text;
                customer.ImagePath = SaveImageIfNeeded(customer.CustomerName);
                customer.OpeningBalance = nudOpeningBalance.Value;
                customer.BalanceType = Convert.ToString(cboBalanceType.SelectedItem);
                customer.IsActive = chkIsActive.Checked;

                SavedCustomerId = _customerService.SaveCustomer(customer);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Customer Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            _selectedImagePath = dialog.FileName;
            _imageChanged = true;
            LoadCustomerImage(_selectedImagePath);
        }

        private void btnCaptureImage_Click(object sender, EventArgs e)
        {
            try
            {
                string capturedPath = CaptureImageFromCamera();
                if (string.IsNullOrWhiteSpace(capturedPath))
                {
                    return;
                }

                _selectedImagePath = capturedPath;
                _imageChanged = true;
                LoadCustomerImage(_selectedImagePath);
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
            LoadCustomerImage(null);
        }

        private string SaveImageIfNeeded(string customerName)
        {
            if (!_imageChanged)
            {
                return _selectedImagePath;
            }

            if (string.IsNullOrWhiteSpace(_selectedImagePath))
            {
                return null;
            }

            string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CustomerImages");
            Directory.CreateDirectory(imagesFolder);

            string extension = Path.GetExtension(_selectedImagePath);
            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = ".jpg";
            }

            string safeName = MakeSafeFileName(customerName);
            string fileName = string.Format("{0}_{1}{2}", safeName, DateTime.Now.ToString("yyyyMMddHHmmss"), extension);
            string destinationPath = Path.Combine(imagesFolder, fileName);
            File.Copy(_selectedImagePath, destinationPath, true);

            return Path.Combine("CustomerImages", fileName);
        }

        private void LoadCustomerImage(string imagePath)
        {
            if (picCustomer.Image != null)
            {
                Image oldImage = picCustomer.Image;
                picCustomer.Image = null;
                oldImage.Dispose();
            }

            if (string.IsNullOrWhiteSpace(imagePath))
            {
                return;
            }

            string resolvedPath = imagePath;
            if (!Path.IsPathRooted(resolvedPath))
            {
                resolvedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);
            }

            if (!File.Exists(resolvedPath))
            {
                return;
            }

            using (Image source = Image.FromFile(resolvedPath))
            {
                picCustomer.Image = new Bitmap(source);
            }
        }

        private static string CaptureImageFromCamera()
        {
            Type commonDialogType = Type.GetTypeFromProgID("WIA.CommonDialog");
            if (commonDialogType == null)
            {
                throw new InvalidOperationException("Windows camera capture is not available on this PC.");
            }

            object dialog = Activator.CreateInstance(commonDialogType);
            object image = commonDialogType.InvokeMember(
                "ShowAcquireImage",
                BindingFlags.InvokeMethod,
                null,
                dialog,
                new object[] { 2, 0, 0, "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}", true, true, false });

            if (image == null)
            {
                return null;
            }

            string captureFolder = Path.Combine(Path.GetTempPath(), "ShopPOSCamera");
            Directory.CreateDirectory(captureFolder);

            string filePath = Path.Combine(captureFolder, "customer_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg");
            image.GetType().InvokeMember("SaveFile", BindingFlags.InvokeMethod, null, image, new object[] { filePath });
            return filePath;
        }

        private static string MakeSafeFileName(string value)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            int index;

            for (index = 0; index < invalidChars.Length; index++)
            {
                value = value.Replace(invalidChars[index], '_');
            }

            return string.IsNullOrWhiteSpace(value) ? "customer" : value;
        }
    }
}
