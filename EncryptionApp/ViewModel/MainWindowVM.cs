using EncryptionApp.Model;
using Microsoft.Win32;
using RealtyModel.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace EncryptionApp.ViewModel
{
    class MainWindowVM : INotifyPropertyChanged
    {
        private Encryption currentEncryptor = new HashEncryption();
        private string firstKeySequenceName = "Ключ";
        private string firstKeySequence = "";
        private string secondKeySequenceName = "Вектор инициализации";
        private string secondKeySequence = "";
        private string currentFilePath = "";
        private string currentFileName = "Не выбран";
        private string currentEncryptionMethod = "Симметричный";
        private Visibility keyVisibilities = Visibility.Visible;
        private CustomCommand encryptFile;
        private CustomCommand decryptFile;
        private CustomCommand openFile;
        private CustomCommand createKeys;
        private CustomCommand setAsymmetricEncryption;
        private CustomCommand setSymmetricEncryption;
        private CustomCommand setHashEncryption;
        private void SetMetaInfo(string firstKeySequenceName, string secondKeySequenceName, string currentEncryptionMethod, Visibility keyVisibilities) {
            this.FirstKeySequenceName = firstKeySequenceName;
            this.SecondKeySequenceName = secondKeySequenceName;
            this.CurrentEncryptionMethod = currentEncryptionMethod;
            this.KeyVisibilities = keyVisibilities;
        }
        public CustomCommand SetAsymmetricEncryption => setAsymmetricEncryption ?? (setAsymmetricEncryption = new CustomCommand(obj => {
            currentEncryptor = new AsymmetricEncryption();
            SetMetaInfo("Открытый ключ", "Закрытый ключ", "Асимметричный", Visibility.Visible);
        }));
        public CustomCommand SetSymmetricEncryption => setSymmetricEncryption ?? (setSymmetricEncryption = new CustomCommand(obj => {
            currentEncryptor = new SymmetricEncryption();
            SetMetaInfo("Ключ", "Вектор инициализации", "Симметричный", Visibility.Visible);
        }));
        public CustomCommand SetHashEncryption => setHashEncryption ?? (setHashEncryption = new CustomCommand(obj => {
            currentEncryptor = new HashEncryption();
            SetMetaInfo("Ключ", "No name", "Необратимый", Visibility.Collapsed);
        }));
        public void TestMethod() {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024);
            string privateKey = $"-----BEGIN RSA PRIVATE KEY-----\nBIICYTCCAl0CAQACgYEAo/2cuhl0t/fcMQmLE6f6anPnJkoDmr4VRQ73zdkC2tNOG8qlaTXfqzIxWZspBODdEO9lNtviEQ53U/eHqZCEnGiaNZgfoECFXHwqwI2epvjlsET8QyRmSERx/8I+Xoc/Jqh1GxsE7Um4GEeZ21hP+5yaaxFzrgLxnBLpc7Kjre8CAwEAAQKBgBSr0kavEMRjzPCteEd5BBrJE1kDOWMXFM1IrrnW4gI9YnokWCdj8Ba/U/MsmMYRpiwNUR/SJbPqs+X0rLgWDHEKxwkeojo7Kzs89Dw3c3h0TYqWU8Sz9pfYNpRufwL95CXu6Wh7A+bjD7rYKbYiGcXAyj2XSoNe1i+tfDYPVzIxAkEA5n0nu799qlGHLD15eSBD3gyhx8MgCTKKDzAixueTcBD5J3n2lT/eXceycvzsGnFCz18Jj4Asw1K89N0XoxNEqQJBALYkPyywLxAhOiz05Undzh3qec7ePtmG/EEFGtVdIYWW/OlHhLMAd5qDCmKmONiptEp1aVTT9/TboADEYuRrZNcCQGd4that/48vbHxq2JaM6orLpvET4tTeMGZjGKmsml7L795N/WnBM2VsWesPKjswr2qC4rreMro48YUHoC3gX4kCQQCwaHbPX2yVoLjppd8VJcBl9R04oMQahsR2bO1KTUMUUeJuRhheDkvI2LYMSZWxMtwtaX407H+xJa3YUFy/gsxNAkEAnigw6SR0CXIbdo8x7HQ9CldiNWI6sf/D2dds8Da1r6nQTuvrmTqtojI9Xvisf0M/GqeP9hVxNzIYowX3dEuCrg==\n-----END RSA PRIVATE KEY-----";
            Debug.WriteLine(SecondKeySequence);
            string pem = RSAKeys.ExportPrivateKey(rsa);
            Debug.WriteLine(pem);
            //rsa = RSAKeys.ImportPrivateKey(privateKey);
        }
        public CustomCommand EncryptFile => encryptFile ?? (encryptFile = new CustomCommand(obj => {
            if (!String.IsNullOrEmpty(currentFilePath)) {
                byte[] data = File.ReadAllBytes(currentFilePath);
                data = currentEncryptor.Encrypt(data, FirstKeySequence, SecondKeySequence);
                File.WriteAllBytes(currentFilePath, data);
            }
        }));
        public CustomCommand DecryptFile => decryptFile ?? (decryptFile = new CustomCommand(obj => {
            if (!String.IsNullOrEmpty(currentFilePath)) {
                byte[] data = File.ReadAllBytes(currentFilePath);
                data = currentEncryptor.Decrypt(data, FirstKeySequence, SecondKeySequence);
                File.WriteAllBytes(currentFilePath, data);
            }
        }));
        public CustomCommand OpenFile => openFile ?? (openFile = new CustomCommand(obj => {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // это nullable bool, поэтому значение приходится указывать напрямую
            if (openFileDialog.ShowDialog() == true) {
                CurrentFileName = openFileDialog.SafeFileName;
                currentFilePath = openFileDialog.FileName;
            }
        }));
        public CustomCommand CreateKeys => createKeys ?? (createKeys = new CustomCommand(obj => {
            CreateKeysMethod();
            TestMethod();
        }));

        private void CreateKeysMethod() {
            string[] keys = currentEncryptor.CreateKeys();
            FirstKeySequence = keys[0];
            SecondKeySequence = keys[1];
        }

        public string CurrentFileName {
            get => currentFileName;
            set {
                currentFileName = value;
                OnPropertyChanged();
            }
        }
        public string FirstKeySequence {
            get => firstKeySequence;
            set {
                firstKeySequence = value;
                OnPropertyChanged();
            }
        }
        public string SecondKeySequence {
            get => secondKeySequence;
            set {
                secondKeySequence = value;
                OnPropertyChanged();
            }
        }
        public string FirstKeySequenceName {
            get => firstKeySequenceName;
            set {
                firstKeySequenceName = value;
                OnPropertyChanged();
            }
        }
        public string SecondKeySequenceName {
            get => secondKeySequenceName;
            set {
                secondKeySequenceName = value;
                OnPropertyChanged();
            }
        }

        public string CurrentEncryptionMethod {
            get => currentEncryptionMethod;
            set {
                currentEncryptionMethod = value;
                OnPropertyChanged();
            }
        }

      

        public Visibility KeyVisibilities {
            get => keyVisibilities;
            set {
                keyVisibilities = value;
                OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string property = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
