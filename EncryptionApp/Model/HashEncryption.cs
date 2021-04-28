using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EncryptionApp.Model
{
    class HashEncryption : Encryption
    {
        public override byte[] Encrypt(byte[] data, string key, string _) {
            throw new NotImplementedException();
        }
        public override byte[] Decrypt(byte[] data, string key, string _) {
            throw new NotImplementedException();
        }
    }
}
