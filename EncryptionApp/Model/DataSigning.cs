using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EncryptionApp.Model
{
    class DataSigning
    {
        public static byte[] SignData(byte[] data, string signaturePhrase, string key = "uDxCCkipuqM03WhfkLDgzw==") {
            byte[] signaturePhraseBlob = ToByteArray<string>(signaturePhrase);
            HashEncryption md5Encryptor = new HashEncryption();
            byte[] hashedData = md5Encryptor.Encrypt(data);
            byte[] hashedSignaturePhrase = md5Encryptor.Encrypt(signaturePhraseBlob);
            List<byte> resultHash = new List<byte>();
            foreach (byte hd in hashedData) {
                resultHash.Add(hd);
            }
            foreach (byte hsp in hashedSignaturePhrase) {
                resultHash.Add(hsp);
            }
            Signature signature = new Signature();
            signature.SignedData = resultHash.ToArray();
            signature.Length = hashedSignaturePhrase.Length;
            SymmetricEncryption aes128Encryptor = new SymmetricEncryption();
            byte[] encryptedSignature = aes128Encryptor.Encrypt(ToByteArray(signature), key, key);
            return encryptedSignature;
        }
        public static void CheckFile(string filePath, string key) {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Файлы ЭЦП (*.sign)|*.sign"};
            if (openFileDialog.ShowDialog() == true) {
                string signaturePath = openFileDialog.FileName;
                Signature signature = GetSignature(File.ReadAllBytes(signaturePath), key);
                byte[] data = File.ReadAllBytes(filePath);
                bool isEqual = true;
                for (int i = 0; i < data.Length; i++) {
                    isEqual = data[i] == signature.SignedData[i];
                    if (!isEqual) {
                        MessageBox.Show("Файл не соответствует подписи");
                        break;
                    }
                }
                if (isEqual) {
                    MessageBox.Show("Файл соответствует подписи");
                }
            }
        }
        public static void VerifySign(string key) {
            string firstPath = "";
            string secondPath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Файлы ЭЦП (*.sign)|*.sign" };
            if (openFileDialog.ShowDialog() == true) {
                firstPath = openFileDialog.FileName;
                if (openFileDialog.ShowDialog() == true) {
                    secondPath = openFileDialog.FileName;
                    Signature sign1 = DataSigning.GetSignature(File.ReadAllBytes(firstPath), key);
                    Signature sign2 = DataSigning.GetSignature(File.ReadAllBytes(secondPath), key);
                    bool isEqual = true;
                    for (int i = 0; i < sign1.Length; i++) {
                        isEqual = sign1.SignedData[sign1.Length - 1 - i] == sign2.SignedData[sign2.Length - 1 - i];
                        if (!isEqual) {
                            MessageBox.Show("Подпись недействительна");
                            break;
                        }
                    }
                    if (isEqual) {
                        MessageBox.Show("Подпись действительна");
                    }
                }
            }
        }
        static public Signature GetSignature(byte[] encryptedSignature, string key = "uDxCCkipuqM03WhfkLDgzw==") {
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
