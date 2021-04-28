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
using System.Text;
using System.Threading.Tasks;

namespace EncryptionApp.ViewModel
{
    class MainWindowVM : INotifyPropertyChanged
    {
        private Encryption currentEncryptor = new SymmetricEncryption();
        private string firstKeySequenceName = "Ключ";
        private string firstKeySequence = "";
        private string secondKeySequenceName = "Вектор инициализации";
        private string secondKeySequence = "";
        private string currentFilePath = "";
        private string currentFileName = "Не выбран";
        private string currentEncryptionMethod = "Симметричный";
        private CustomCommand encryptFile;
        private CustomCommand decryptFile;
        private CustomCommand openFile;
        private CustomCommand createKeys;
        private CustomCommand setAsymmetricEncryption;
        private CustomCommand setSymmetricEncryption;
        private CustomCommand setHashEncryption;
        private void SetMetaInfo(string firstKeySequenceName, string secondKeySequenceName, string currentEncryptionMethod) {
            FirstKeySequenceName = firstKeySequenceName;
            SecondKeySequenceName = secondKeySequenceName;
            CurrentEncryptionMethod = currentEncryptionMethod;
        }
        public CustomCommand SetAsymmetricEncryption => setAsymmetricEncryption ?? (setAsymmetricEncryption = new CustomCommand(obj => {
            currentEncryptor = new AsymmetricEncryption();
            SetMetaInfo("Открытый ключ", "Закрытый ключ", "Асимметричный");
        }));
        public CustomCommand SetSymmetricEncryption => setSymmetricEncryption ?? (setSymmetricEncryption = new CustomCommand(obj => {
            currentEncryptor = new SymmetricEncryption();
            SetMetaInfo("Ключ", "Вектор инициализации", "Симметричный");
        }));
        public CustomCommand SetHashEncryption => setHashEncryption ?? (setHashEncryption = new CustomCommand(obj => {
            currentEncryptor = new HashEncryption();
            SetMetaInfo("No name", "No name", "Хеширование");
        }));
        public CustomCommand EncryptFile => encryptFile ?? (encryptFile = new CustomCommand(obj => {
            byte[] data = File.ReadAllBytes(currentFilePath);
            data = currentEncryptor.Encrypt(data);
            File.WriteAllBytes(currentFilePath, data);
        }));
        public CustomCommand DecryptFile => decryptFile ?? (decryptFile = new CustomCommand(obj => {
            byte[] data = File.ReadAllBytes(currentFilePath);
            data = currentEncryptor.Decrypt(data);
            File.WriteAllBytes(currentFilePath, data);
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

        }));
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

        public void OnPropertyChanged([CallerMemberName] string property = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
