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
        private Encryption currentEncryptor = new SymmetricEncryption();
        private string firstKeySequenceName = "Ключ (BASE64)";
        private string firstKeySequence = "";
        private string secondKeySequenceName = "Вектор инициализации (BASE64)";
        private string secondKeySequence = "";
        private string currentFilePath = "";
        private string currentFileName = "Не выбран";
        private string currentEncryptionMethod = "Симметричный";
        private double keyFontSize = 20;
        private Visibility keyVisibilities = Visibility.Visible;
        private CustomCommand encryptFile;
        private CustomCommand decryptFile;
        private CustomCommand openFile;
        private CustomCommand createKeys;
        private CustomCommand setAsymmetricEncryption;
        private CustomCommand setSymmetricEncryption;
        private CustomCommand setHashEncryption;
        public MainWindowVM() {
            CreateKeysMethod();
        }
        private void SetMetaInfo(string firstKeySequenceName, string secondKeySequenceName, string currentEncryptionMethod, Visibility keyVisibilities, double keyFontSize) {
            this.FirstKeySequenceName = firstKeySequenceName;
            this.SecondKeySequenceName = secondKeySequenceName;
            this.CurrentEncryptionMethod = currentEncryptionMethod;
            this.KeyVisibilities = keyVisibilities;
            this.KeyFontSize = keyFontSize;
        }
        public CustomCommand SetAsymmetricEncryption => setAsymmetricEncryption ?? (setAsymmetricEncryption = new CustomCommand(obj => {
            currentEncryptor = new AsymmetricEncryption();
            SetMetaInfo("Открытый ключ (PEM)", "Закрытый ключ (PEM)", "Асимметричный", Visibility.Visible, 10);
        }));
        public CustomCommand SetSymmetricEncryption => setSymmetricEncryption ?? (setSymmetricEncryption = new CustomCommand(obj => {
            currentEncryptor = new SymmetricEncryption();
            SetMetaInfo("Ключ (BASE64)", "Вектор инициализации (BASE64)", "Симметричный", Visibility.Visible, 20);
        }));
        public CustomCommand SetHashEncryption => setHashEncryption ?? (setHashEncryption = new CustomCommand(obj => {
            currentEncryptor = new HashEncryption();
            SetMetaInfo("", "", "Необратимый", Visibility.Collapsed, 20);
        }));
        public void TestMethod() {
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

        public double KeyFontSize { 
            get => keyFontSize;
            set {
                keyFontSize = value;
                OnPropertyChanged();
            } 
        }

        public void OnPropertyChanged([CallerMemberName] string property = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
