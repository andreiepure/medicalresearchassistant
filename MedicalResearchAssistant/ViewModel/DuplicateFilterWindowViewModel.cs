using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;

namespace MedicalResearchAssistant.ViewModel
{
    public class DuplicateFilterWindowViewModel : ViewModelBase
    {
        //public MedlineFileViewModel SelectedFiles { get; set; }

        public ObservableCollection<MedlineFileViewModel> SelectedFiles { get; private set; }

        public ICommand FilterDuplicatesCommand { get; private set; }

        public ICommand ChooseOutputFolderCommand { get; private set; }
        public ICommand AddFileToListCommand { get; private set; }

        /// <summary>
        /// Backing string for ChooseFolderLabel
        /// </summary>
        private string chooseFolderLabel;

        /// <summary>
        /// The label for the choose folder button
        /// </summary>
        public string ChooseFolderLabel
        {
            get
            {
                return chooseFolderLabel;
            }

            private set
            {
               chooseFolderLabel = value;
               OnPropertyChanged("ChooseFolderLabel");
            }
        }

        private string chosenFolder;
        public string ChosenFolder
        {
            get
            {
                return chosenFolder;
            }
            set
            {
               chosenFolder = value;
               OnPropertyChanged("EnteredText");
            }
        }

        private long citationsPerFile;
        public long CitationsPerFile
        {
            get
            {
                return citationsPerFile;
            }
            set
            {
                citationsPerFile = value;
                OnPropertyChanged("CitationsPerFile");
            }
        }

        private string outName;
        public string OutName
        {
            get
            {
                return outName;
            }
            set
            {
                outName = value;
                OnPropertyChanged("OutName");
            }
        }

        public DuplicateFilterWindowViewModel()
        {
            SelectedFiles = new ObservableCollection<MedlineFileViewModel>();
            FilterDuplicatesCommand = new RelayCommand(new Action<object>(FilterDuplicates));
            ChooseOutputFolderCommand = new RelayCommand(new Action<object>(ChooseOutputFolder));
            AddFileToListCommand = new RelayCommand(new Action<object>(AddFileToList));
            ChooseFolderLabel = "Add a file";
            ChosenFolder = "Please enter text";
        }

        public void FilterDuplicates(object message)
        {
            Debug.WriteLine(message.ToString());
            ChooseFolderLabel = "First choose the folder";
            Debug.WriteLine(ChosenFolder);
        }

        public void AddFileToList(object message)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dialog.DefaultExt = ".nbib";
            dialog.Filter = "Text Files (*.txt)|*.txt|Medline Files (*.nbib)|*.nbib";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                if (!string.IsNullOrWhiteSpace(dialog.FileName))
                {
                    MedlineFileViewModel fileViewModel = new MedlineFileViewModel(dialog.FileName);
                    SelectedFiles.Add(fileViewModel);
                }
            }
        }

        public void ChooseOutputFolder(object message)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = true;
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (!string.IsNullOrWhiteSpace(dialog.SelectedPath))
                { 
                    ChosenFolder = dialog.SelectedPath;
                }
            }
        }


        public override void Dispose()
        {
        }
    }
}
