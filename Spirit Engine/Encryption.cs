using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit
{
    public class Encryption
    {
        static byte[] GenerateKeyFromPassword(string password)
        {
            byte[] salt = new byte[16];
            byte[] keyBytes;

            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                keyBytes = pbkdf2.GetBytes(32); // 256-bit key
            }

            return keyBytes;
        }

        public static byte[] Encrypt(byte[] data, string key)
        {
            if (key.Length > 256) throw new("Key must be shorter than 256 symbols");
            while (key.Length < 256) key += "T";
            byte[] keyBytes = GenerateKeyFromPassword(key);
            byte[] iv = new byte[]
            {
                132, 208, 210, 158, 101, 167, 101, 219, 29, 94, 187, 50, 91, 80, 192, 150
            };
            byte[] encryptedData;

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = keyBytes;
                aes.IV = iv;

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    encryptedData = encryptor.TransformFinalBlock(data, 0, data.Length);
                }
            }

            byte[] encryptedDataWithIV = new byte[iv.Length + encryptedData.Length];
            Array.Copy(iv, 0, encryptedDataWithIV, 0, iv.Length);
            Array.Copy(encryptedData, 0, encryptedDataWithIV, iv.Length, encryptedData.Length);

            return encryptedDataWithIV;
        }

        public static byte[] Decrypt(byte[] encrypted, string key)
        {
            if (key.Length > 256) throw new("Key must be shorter than 256 symbols");

            while (key.Length < 256) key += "T";

            byte[] keyBytes = GenerateKeyFromPassword(key);
            byte[] iv = new byte[]
            {
                132, 208, 210, 158, 101, 167, 101, 219, 29, 94, 187, 50, 91, 80, 192, 150
            };
            byte[] encryptedData = new byte[encrypted.Length - 16];

            Array.Copy(encrypted, 0, iv, 0, iv.Length);
            Array.Copy(encrypted, iv.Length, encryptedData, 0, encryptedData.Length);

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = keyBytes;
                aes.IV = iv;

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    byte[] decryptedData = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                    return decryptedData;
                }
            }
        }
    }
}
