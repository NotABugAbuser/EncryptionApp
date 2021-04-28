using EncryptionApp.Model;
using Microsoft.Win32;
using RealtyModel.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionApp.ViewModel
{
    class MainWindowVM : INotifyPropertyChanged
    {
        private Encryption currentEncryptor = new HashEncryption();
        private string firstKeySequence = "";
        private string secondKeySequence = "";
        private string currentFilePath = "";
        private string currentFileName = "";
        private CustomCommand setCurrentEncryptor;
        private CustomCommand encryptFile;
        private CustomCommand decryptFile;
        private CustomCommand openFile;
        private CustomCommand setAsymmetricEncryption;
        private CustomCommand setSymmetricEncryption;
        private CustomCommand setHashEncryption;
        public CustomCommand SetCurrentEncryptor => setCurrentEncryptor ?? (setCurrentEncryptor = new CustomCommand(obj => {
            Type type = Type.GetType((string)obj);
            currentEncryptor = (Encryption)Activator.CreateInstance(type);
            Debug.WriteLine(currentEncryptor is HashEncryption);
        }));
        public CustomCommand SetAsymmetricEncryption => setAsymmetricEncryption ?? (setAsymmetricEncryption = new CustomCommand(obj => {
            currentEncryptor = new AsymmetricEncryption();
        }));
        public CustomCommand SetSymmetricEncryption => setSymmetricEncryption ?? (setSymmetricEncryption = new CustomCommand(obj => {
            currentEncryptor = new SymmetricEncryption();
        }));
        public CustomCommand SetHashEncryption => setHashEncryption ?? (setHashEncryption = new CustomCommand(obj => {
            currentEncryptor = new HashEncryption();
        }));
        public CustomCommand EncryptFile => encryptFile ?? (encryptFile = new CustomCommand(obj => {
        
        }));
        public CustomCommand DecryptFile => decryptFile ?? (decryptFile = new CustomCommand(obj => {

        }));
        public CustomCommand OpenFile => openFile ?? (openFile = new CustomCommand(obj => {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // это nullable bool, поэтому значение приходится указывать напрямую
            if (openFileDialog.ShowDialog() == true) {
                CurrentFileName = openFileDialog.SafeFileName;
                currentFilePath = openFileDialog.FileName;
            }
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

        public void OnPropertyChanged([CallerMemberName] string property = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
