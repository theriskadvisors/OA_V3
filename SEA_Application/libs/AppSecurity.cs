using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;

namespace SEA_Application.libs
{
    public class AppSecurity
    {
        public static string Encrypt(string plainText)
        {
            string PasswordHash = WebConfigurationManager.AppSettings["PasswordHash"];
            string SaltKey = WebConfigurationManager.AppSettings["SaltKey"];
            string VIKey = WebConfigurationManager.AppSettings["VIKey"];
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }

            string temp = Convert.ToBase64String(cipherTextBytes);
            string temp1 = temp.Replace('/', '_');
            string temp2 = temp1.Replace('+', '-');
            return temp2;
        }

        public static string Decrypt(string temp)
        {
            string PasswordHash = WebConfigurationManager.AppSettings["PasswordHash"];
            string SaltKey = WebConfigurationManager.AppSettings["SaltKey"];
            string VIKey = WebConfigurationManager.AppSettings["VIKey"];
            string temp1 = temp.Replace('-', '+');
            string encryptedText = temp1.Replace('_', '/');

            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }
    }

}