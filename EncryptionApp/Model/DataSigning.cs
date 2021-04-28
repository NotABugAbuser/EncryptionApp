using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionApp.Model
{
    class DataSigning
    {
        public byte[] SignData(byte[] data, string signaturePhrase, byte[] key) {
            Signature signature = new Signature();
            HashEncryption md5Encryptor = new HashEncryption();
            SymmetricEncryption aes128Encryptor = new SymmetricEncryption();
            byte[] signaturePhraseBlob = ToByteArray<string>(signaturePhrase);
            byte[] hashedData = md5Encryptor.Encrypt(data);
            byte[] hashedSignaturePhrase = md5Encryptor.Encrypt(signaturePhraseBlob);
            List<byte> resultHash = new List<byte>();
            foreach (byte hd in hashedData) {
                resultHash.Add(hd);
            }
            foreach(byte hsp in hashedSignaturePhrase) {
                resultHash.Add(hsp);
            }
            return ToByteArray<Signature>(signature);
        }
        public void VerifySign(byte[] originalData, byte[] fakeData) {

        }
        public static byte[] ToByteArray<T>(T obj) {
            if (obj == null) {
                return null;
            }
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream()) {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T FromByteArray<T>(byte[] data) {
            if (data == null) {
                return default(T);
            }
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data)) {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }
    }
}
