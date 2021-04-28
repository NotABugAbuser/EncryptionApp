using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionApp.Model
{
    class SymmetricEncryption : Encryption
    {
        public override byte[] Encrypt(byte[] data) {
            throw new NotImplementedException();
        }
        public override byte[] Decrypt(byte[] data) {
            throw new NotImplementedException();
        }
    }
}
