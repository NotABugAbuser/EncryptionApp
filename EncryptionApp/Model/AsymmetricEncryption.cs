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
        RSAParameters publicKeyInfo = new RSAParameters();
        RSAParameters privateKeyInfo = new RSAParameters();
        public AsymmetricEncryption() {
            this.publicKeyInfo = rsa.ExportParameters(false);
            this.privateKeyInfo = rsa.ExportParameters(true);
        }
        public override byte[] Encrypt(byte[] data, string _, string privateKeyInfo) {
            string privateKey = $"-----BEGIN RSA PRIVATE KEY-----{privateKeyInfo}-----END RSA PRIVATE KEY-----";
            rsa = RSAKeys.ImportPrivateKey(privateKey);
            return data;
        }
        public override byte[] Decrypt(byte[] data, string _, string privateKey) {
            throw new NotImplementedException();
        }
        public override string[] CreateKeys() {
            RsaKeyPairGenerator rsa = new RsaKeyPairGenerator();
            rsa.Init(new KeyGenerationParameters(new Org.BouncyCastle.Security.SecureRandom(), 1024));
            AsymmetricCipherKeyPair pair = rsa.GenerateKeyPair();
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(pair.Private);
            
            SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(pair.Public);
            byte[] privateBytes = privateKeyInfo.PrivateKeyData.ToAsn1Object().GetDerEncoded();
            byte[] publicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();
            return new string[] {
                Convert.ToBase64String(publicBytes),
                Convert.ToBase64String(privateBytes)
            };
        } 
    }
}
