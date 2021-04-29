using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle;
using System.Windows;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using System.Diagnostics;

namespace EncryptionApp.Model
{
    class AsymmetricEncryption : Encryption
    {

        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
        ///<summary>
        ///Преобразует входной текстовый ключ в приватный ключ, который впоследствии передается в 
        ///RsaCryptoProviderService, производящий шифрование
        ///</summary>
        public override byte[] Encrypt(byte[] data, string _, string privateKeyInfo) {
            try {
                rsa = RSAKeys.ImportPrivateKey(privateKeyInfo);
                data = rsa.Encrypt(data, false);
            } catch (FormatException) {
                MessageBox.Show("RSA 2048 не поддерживает шифрование данных такого объема");
            } catch (NullReferenceException) {
                rsa = new RSACryptoServiceProvider();
            }
            return data;
        }
        ///<summary>
        ///Преобразует входной текстовый ключ в приватный ключ, который впоследствии передается в 
        ///RsaCryptoProviderService, производящий дешифрование
        ///</summary>
        public override byte[] Decrypt(byte[] data, string _, string privateKeyInfo) {
            try {
                rsa = RSAKeys.ImportPrivateKey(privateKeyInfo);
                data = rsa.Decrypt(data, false);
            } catch (Exception) {
                MessageBox.Show("RSA 2048 не поддерживает шифрование данных такого объема");
            }
            return data;
        }
        public override string[] CreateKeys() {
            return new string[] {
                RSAKeys.ExportPublicKey(rsa),
                RSAKeys.ExportPrivateKey(rsa)
            };
        }
    }
}
