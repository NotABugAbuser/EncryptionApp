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
            byte[] signaturePhraseBlob = ToByteArray<string>(signaturePhrase);
            HashEncryption md5Encryptor = new HashEncryption();
            byte[] hashedData = md5Encryptor.Encrypt(data);
            byte[] hashedSignaturePhrase = md5Encryptor.Encrypt(signaturePhraseBlob);
            List<byte> resultHash = new List<byte>();
            foreach (byte hd in hashedData) {
                resultHash.Add(hd);
            }
            foreach(byte hsp in hashedSignaturePhrase) {
                resultHash.Add(hsp);
            }
            Signature signature = new Signature();
            signature.SignedData = resultHash.ToArray();
            signature.Length = hashedSignaturePhrase.Length;
            SymmetricEncryption aes128Encryptor = new SymmetricEncryption();
            byte[] encryptedSignature = aes128Encryptor.Encrypt(ToByteArray(signature), "uDxCCkipuqM03WhfkLDgzw==", "uDxCCkipuqM03WhfkLDgzw==");
            return encryptedSignature;
        }
        public void VerifySign(byte[] originalData, byte[] fakeData) {

        }
        private Signature GetSignature(byte[] encryptedSignature, string key) {
            SymmetricEncryption aes128Encryptor = new SymmetricEncryption();
            Signature signature = FromByteArray<Signature>(aes128Encryptor.Decrypt(encryptedSignature, key, key));
            return signature;
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
