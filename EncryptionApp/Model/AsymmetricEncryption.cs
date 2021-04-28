using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionApp.Model
{
    class AsymmetricEncryption : Encryption
    {
        public override byte[] Encrypt(byte[] data, string publicKey, string privateKey) {
            throw new NotImplementedException();
        }
        public override byte[] Decrypt(byte[] data, string publicKey, string privateKey) {
            throw new NotImplementedException();
        }
    }
}
