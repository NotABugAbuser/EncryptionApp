using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionApp.Model
{
    class Encryption
    {
        public virtual byte[] Encrypt(byte[] data, string firstKey, string secondKey) {
            throw new NotImplementedException();
        }
        public virtual byte[] Decrypt(byte[] data, string firstKey, string secondKey) {
            throw new NotImplementedException();
        }
        public virtual void CreateKeys() {
            throw new NotImplementedException();
        }
    }
}
