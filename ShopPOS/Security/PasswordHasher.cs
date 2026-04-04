using System;
using System.Security.Cryptography;
using System.Text;

namespace ShopPOS.Security
{
    public static class PasswordHasher
    {
        public static bool Verify(string enteredPassword, string storedPassword)
        {
            if (string.IsNullOrEmpty(storedPassword))
            {
                return false;
            }

            if (string.Equals(enteredPassword, storedPassword, StringComparison.Ordinal))
            {
                return true;
            }

            string sha256Hex = ComputeSha256Hex(enteredPassword);
            if (string.Equals(sha256Hex, storedPassword, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            string sha256Base64 = ComputeSha256Base64(enteredPassword);
            return string.Equals(sha256Base64, storedPassword, StringComparison.Ordinal);
        }

        private static string ComputeSha256Hex(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input ?? string.Empty));
                StringBuilder builder = new StringBuilder(hash.Length * 2);

                foreach (byte value in hash)
                {
                    builder.Append(value.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        private static string ComputeSha256Base64(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input ?? string.Empty));
                return Convert.ToBase64String(hash);
            }
        }
    }
}
