using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EncryptionApp.Model
{
    class HashEncryption : Encryption
    {
        ///<summary>
        ///Извлекает байты файла, вычисляет их хеш-сумму и записывает её в lastHash.txt, находящийся в корневой папке
        ///</summary>
        public override byte[] Encrypt(byte[] data, string key = null, string skey = null) {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider()) {
                string path = Path.GetFullPath("lastHash.txt");
                byte[] md5Hash = md5.ComputeHash(data);
                string hexHash = BitConverter.ToString(md5Hash).Replace("-","");
                File.WriteAllText(path, hexHash);
                return data;
            }
        }
        public override byte[] Decrypt(byte[] data, string key, string _) {
            MessageBox.Show("Хеш-сумму невозможно дешифровать");
            return data;
        }
    }
}
