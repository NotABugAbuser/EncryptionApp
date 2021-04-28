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
                    
                }
            }
        }
        public override byte[] Decrypt(byte[] data, string key, string iv) {
            throw new NotImplementedException();
        }
    }
}
