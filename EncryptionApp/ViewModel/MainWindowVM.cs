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
        private string firstKeySequenceName = "Ключ (16 байт)";
        private string firstKeySequence = "";
        private string secondKeySequenceName = "Вектор инициализации (16 байт)";
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
        private CustomCommand signFile;
        private CustomCommand checkFile;
        private CustomCommand verifySignatures;
        private CustomCommand changeSignatureInterfaceVisibility;
        private CustomCommand createSignatureKey;
        private string signaturePhrase = "";
        private string signatureKey = "";
        private Visibility signatureInterfaceVisibility = Visibility.Collapsed;
        public MainWindowVM() {
            CreateKeysMethod();
            SignaturePhrase = "Someone";
            byte[] bytes = new byte[16];
            new Random().NextBytes(bytes);
            SignatureKey = Convert.ToBase64String(bytes);
        }
        private void SetMetaInfo(string firstKeySequenceName, string secondKeySequenceName, string currentEncryptionMethod, Visibility keyVisibilities, double keyFontSize) {
            this.FirstKeySequenceName = firstKeySequenceName;
            this.SecondKeySequenceName = secondKeySequenceName;
            this.CurrentEncryptionMethod = currentEncryptionMethod;
            this.KeyVisibilities = keyVisibilities;
            this.KeyFontSize = keyFontSize;
        }
        ///<summary>
        ///Считывает все байты файла, подписывает их и записывает в этот же файл.
        ///</summary>
        public CustomCommand SignFile => signFile ?? (signFile = new CustomCommand(obj => {
            byte[] data = File.ReadAllBytes(currentFilePath);
            data = DataSigning.SignData(data, SignaturePhrase, SignatureKey);
            File.WriteAllBytes(currentFilePath + ".sign", data);
        }));
        public CustomCommand CheckFile => checkFile ?? (checkFile = new CustomCommand(obj => {
            DataSigning.CheckFile(currentFilePath, SignatureKey);
        }));
        public CustomCommand VerifySignatures => verifySignatures ?? (verifySignatures = new CustomCommand(obj => {
            DataSigning.VerifySign(SignatureKey);
        }));
        public CustomCommand SetAsymmetricEncryption => setAsymmetricEncryption ?? (setAsymmetricEncryption = new CustomCommand(obj => {
            currentEncryptor = new AsymmetricEncryption();
            SetMetaInfo("Открытый ключ (PEM)", "Закрытый ключ (PEM)", "Асимметричный", Visibility.Visible, 10);
        }));
        public CustomCommand SetSymmetricEncryption => setSymmetricEncryption ?? (setSymmetricEncryption = new CustomCommand(obj => {
            currentEncryptor = new SymmetricEncryption();
            SetMetaInfo("Ключ (16 байт)", "Вектор инициализации (16 байт)", "Симметричный", Visibility.Visible, 20);
        }));
        public CustomCommand SetHashEncryption => setHashEncryption ?? (setHashEncryption = new CustomCommand(obj => {
            currentEncryptor = new HashEncryption();
            SetMetaInfo("", "", "Необратимый", Visibility.Collapsed, 20);
        }));
        ///<summary>
        ///Считывает байты файла, шифрует их текущим шифратором и записывает в этот же файл
        ///</summary>
        public CustomCommand EncryptFile => encryptFile ?? (encryptFile = new CustomCommand(obj => {
            if (!String.IsNullOrEmpty(currentFilePath)) {
                byte[] data = File.ReadAllBytes(currentFilePath);
                data = currentEncryptor.Encrypt(data, FirstKeySequence, SecondKeySequence);
                File.WriteAllBytes(currentFilePath, data);
            }
        }));
        ///<summary>
        ///Считывает байты файла, дешифрует их текущим шифратором и записывает в этот же файл
        ///</summary>
        public CustomCommand DecryptFile => decryptFile ?? (decryptFile = new CustomCommand(obj => {
            if (!String.IsNullOrEmpty(currentFilePath)) {
                byte[] data = File.ReadAllBytes(currentFilePath);
                data = currentEncryptor.Decrypt(data, FirstKeySequence, SecondKeySequence);
                File.WriteAllBytes(currentFilePath, data);
            }
        }));
        ///<summary>
        ///Открывает проводник для выбора одного текстового файла или word-документа, устанавливая его в качестве текущего файла
        ///</summary>
        public CustomCommand OpenFile => openFile ?? (openFile = new CustomCommand(obj => {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Текстовые файлы (*.txt)|*.txt|Файлы MS Word(*.doc; *.docx)|*.doc; *.docx" };
            // это nullable bool, поэтому значение приходится указывать напрямую
            if (openFileDialog.ShowDialog() == true) {
                CurrentFileName = openFileDialog.SafeFileName;
                currentFilePath = openFileDialog.FileName;
            }
        }));
        public CustomCommand CreateKeys => createKeys ?? (createKeys = new CustomCommand(obj => {
            CreateKeysMethod();
        }));
        public CustomCommand ChangeSignatureInterfaceVisibility => changeSignatureInterfaceVisibility ?? (changeSignatureInterfaceVisibility = new CustomCommand(obj => {
            if (SignatureInterfaceVisibility == Visibility.Visible) {
                SignatureInterfaceVisibility = Visibility.Collapsed;
            } else {
                SignatureInterfaceVisibility = Visibility.Visible;
            }
        }));
        public CustomCommand CreateSignatureKey => createSignatureKey ?? (createSignatureKey = new CustomCommand(obj => {
            byte[] bytes = new byte[16];
            new Random().NextBytes(bytes);
            SignatureKey = Convert.ToBase64String(bytes);
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
        public string SignaturePhrase {
            get => signaturePhrase;
            set {
                signaturePhrase = value;
                OnPropertyChanged();
            }
        }

        public Visibility SignatureInterfaceVisibility {
            get => signatureInterfaceVisibility;
            set {
                signatureInterfaceVisibility = value;
                OnPropertyChanged();
            }
        }

        public string SignatureKey {
            get => signatureKey;
            set {
                signatureKey = value;
                OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string property = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
