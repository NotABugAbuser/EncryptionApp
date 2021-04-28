using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionApp.Model
{
    class Signature
    {
        private byte[] signedData;
        private int length = 0;

        public byte[] SignedData {
            get => signedData; 
            set => signedData = value;
        }
        public int Length {
            get => length; 
            set => length = value;
        }
    }
}
