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
    class SymmetricEncryption : Encryption
    {
        ///<summary>
        ///Преобразует входные ключи в их битовое представление, которое впоследствии использует для настройки шифратора
        ///После настройки протоколирует поток шифрования, возвращая результат оного 
        ///При неправильной длине ключа выдает ошибку
        ///</summary>
        public override byte[] Encrypt(byte[] data, string key, string iv) {
            try {
                byte[] keyBlob = Convert.FromBase64String(key);
                byte[] ivBlob = Convert.FromBase64String(iv);
                using (Aes aes = Aes.Create()) {
                    aes.KeySize = 128;
                    aes.Key = keyBlob;
                    aes.BlockSize = 128;
                    aes.IV = ivBlob;
                    using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV)) {
                        using (MemoryStream memoryStream = new MemoryStream()) {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)) {
                                cryptoStream.Write(data, 0, data.Length);
                                cryptoStream.FlushFinalBlock();
                                return memoryStream.ToArray();
                            }
                        }
                    }
                }
            } catch (FormatException ex) {
                MessageBox.Show("Неверная длина ключа");
                return data;
            }
        }
        ///<summary>
        ///Преобразует входные ключи в их битовое представление, которое впоследствии использует для настройки дешифратора.
        ///После настройки протоколирует поток шифрования, возвращая результат оного.
        ///При неправильной длине ключа выдает ошибку
        ///</summary>
        public override byte[] Decrypt(byte[] data, string key, string iv) {
            try {
                byte[] keyBlob = Convert.FromBase64String(key);
                byte[] ivBlob = Convert.FromBase64String(iv);
                using (Aes aes = Aes.Create()) {
                    aes.KeySize = 128;
                    aes.Key = keyBlob;
                    aes.BlockSize = 128;
                    aes.IV = ivBlob;
                    using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV)) {
                        using (MemoryStream memoryStream = new MemoryStream()) {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write)) {
                                cryptoStream.Write(data, 0, data.Length);
                                cryptoStream.FlushFinalBlock();
                                return memoryStream.ToArray();
                            }
                        }
                    }
                }
            } catch (FormatException ex) {
                MessageBox.Show("Неверная длина ключа");
                return data;
            }
        }
    }
}
