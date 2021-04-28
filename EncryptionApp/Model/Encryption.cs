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
        public virtual string[] CreateKeys() {
            byte[] firstKeySequence = new byte[16];
            byte[] secondKeySequence = new byte[16];
            Random random = new Random();
            random.NextBytes(firstKeySequence);
            random.NextBytes(secondKeySequence);
            return new string[] {
                Convert.ToBase64String(firstKeySequence),
                Convert.ToBase64String(secondKeySequence)
            };
        }
    }
}
