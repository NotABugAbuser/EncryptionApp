using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EncryptionApp.Model
{
    class HashEncryption : Encryption
    {
        public override byte[] Encrypt(byte[] data, string key, string _) {
            //без using эта конструкция начинала потреблять непомерно много памяти
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider()) {
                using (TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider()) {
                    tripleDES.Padding = PaddingMode.PKCS7;
                    tripleDES.Mode = CipherMode.ECB;
                    tripleDES.Key = md5.ComputeHash(Convert.FromBase64String(key));
                    using (var encryptor = tripleDES.CreateEncryptor()) {

                    }
                }
            }

                return data;
        }
        public override byte[] Decrypt(byte[] data, string key, string _) {
            MessageBox.Show("hi");
            return data;
        }
    }
}
