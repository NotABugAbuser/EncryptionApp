using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EncryptionApp.Model
{
    class HashEncryption : Encryption
    {
        public override byte[] Encrypt(byte[] data, string key, string _) {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider()) {                                 //объекты подобного рода обращаются напрямую к API windows и закрывают не все соединения                                 
                using (TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider()) {           //поэтому без using сборщик мусора очистит их не до конца      
                    tripleDES.Padding = PaddingMode.PKCS7;                                                          
                    tripleDES.Mode = CipherMode.ECB;                                                                //эти режимы были выбраны, потому что они единственные, что дали верный результат при шифровании/дешифровании
                    tripleDES.Key = md5.ComputeHash(Convert.FromBase64String(key));
                    using (ICryptoTransform encryptor = tripleDES.CreateEncryptor()) {
                        return encryptor.TransformFinalBlock(data, 0, data.Length);
                    }
                }
            }
        }
        public override byte[] Decrypt(byte[] data, string key, string _) {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider()) {
                using (TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider()) {
                    tripleDES.Padding = PaddingMode.PKCS7;
                    tripleDES.Mode = CipherMode.ECB;
                    tripleDES.Key = md5.ComputeHash(Convert.FromBase64String(key));
                    using (ICryptoTransform decryptor = tripleDES.CreateDecryptor()) {
                        return decryptor.TransformFinalBlock(data, 0, data.Length);
                    }
                }
            }
        }
    }
}
