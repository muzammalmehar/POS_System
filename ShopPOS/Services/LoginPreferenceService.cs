using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace ShopPOS.Services
{
    public class LoginPreferenceService
    {
        private readonly string _filePath;

        public LoginPreferenceService()
        {
            string folderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ShopPOS");
            Directory.CreateDirectory(folderPath);
            _filePath = Path.Combine(folderPath, "login-preferences.xml");
        }

        public LoginPreference Load()
        {
            LoginPreference preference = new LoginPreference();
            if (!File.Exists(_filePath))
            {
                return preference;
            }

            XmlDocument document = new XmlDocument();
            document.Load(_filePath);

            XmlNode root = document.SelectSingleNode("/loginPreference");
            if (root == null)
            {
                return preference;
            }

            preference.RememberMe = ReadBoolean(root, "rememberMe");
            preference.Username = ReadString(root, "username");
            preference.Password = Decrypt(ReadString(root, "password"));
            return preference;
        }

        public void Save(string username, string password, bool rememberMe)
        {
            if (!rememberMe)
            {
                Clear();
                return;
            }

            XmlDocument document = new XmlDocument();
            XmlElement root = document.CreateElement("loginPreference");
            document.AppendChild(root);
            AppendElement(document, root, "rememberMe", "true");
            AppendElement(document, root, "username", username == null ? string.Empty : username.Trim());
            AppendElement(document, root, "password", Encrypt(password));
            document.Save(_filePath);
        }

        public void Clear()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }

        private static void AppendElement(XmlDocument document, XmlElement parent, string name, string value)
        {
            XmlElement element = document.CreateElement(name);
            element.InnerText = value ?? string.Empty;
            parent.AppendChild(element);
        }

        private static string ReadString(XmlNode root, string name)
        {
            XmlNode node = root.SelectSingleNode(name);
            return node == null ? string.Empty : node.InnerText;
        }

        private static bool ReadBoolean(XmlNode root, string name)
        {
            bool result;
            return bool.TryParse(ReadString(root, name), out result) && result;
        }

        private static string Encrypt(string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText))
            {
                return string.Empty;
            }

            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] protectedBytes = ProtectedData.Protect(plainBytes, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(protectedBytes);
        }

        private static string Decrypt(string cipherText)
        {
            if (string.IsNullOrWhiteSpace(cipherText))
            {
                return string.Empty;
            }

            try
            {
                byte[] protectedBytes = Convert.FromBase64String(cipherText);
                byte[] plainBytes = ProtectedData.Unprotect(protectedBytes, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(plainBytes);
            }
            catch
            {
                return string.Empty;
            }
        }
    }

    public class LoginPreference
    {
        public bool RememberMe { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
