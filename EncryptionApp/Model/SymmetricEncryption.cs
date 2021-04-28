using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionApp.Model
{
    class SymmetricEncryption : Encryption
    {
        public override byte[] Encrypt(byte[] data, string key, string iv) {
            byte[] keyBlob = Convert.FromBase64String(key);
            byte[] ivBlob = Convert.FromBase64String(iv);
            using (Aes aes = Aes.Create()) {
                aes.KeySize = 128;
                aes.Key = keyBlob;
                aes.BlockSize = 128;
                aes.IV = ivBlob;
                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV)) {
                    using (MemoryStream memoryStream = new MemoryStream()) {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)) {
                            cryptoStream.Write(data, 0, data.Length);
                            cryptoStream.FlushFinalBlock();
                            return memoryStream.ToArray();
                        }
                    }
                }
            }
        }
        public override byte[] Decrypt(byte[] data, string key, string iv) {
            byte[] keyBlob = Convert.FromBase64String(key);
            byte[] ivBlob = Convert.FromBase64String(iv);
            using (Aes aes = Aes.Create()) {
                aes.KeySize = 128;
                aes.Key = keyBlob;
                aes.BlockSize = 128;
                aes.IV = ivBlob;
                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV)) {
                    using (MemoryStream memoryStream = new MemoryStream()) {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write)) {
                            cryptoStream.Write(data, 0, data.Length);
                            cryptoStream.FlushFinalBlock();
                            return memoryStream.ToArray();
                        }
                    }
                }
            }
        }
    }
}
