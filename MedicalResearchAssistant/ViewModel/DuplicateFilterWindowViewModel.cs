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
using System.Globalization;

namespace MedicalResearchAssistant.ViewModel
{
    public class DuplicateFilterWindowViewModel : ViewModelBase
    {
        //public MedlineFileViewModel SelectedFiles { get; set; }

        public ObservableCollection<IListableFileViewModel> SelectedFiles { get; private set; }

        public ICommand SaveToFilesCommand { get; private set; }

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
                OnPropertyChanged("ChosenFolder");
            }
        }

        private int citationsPerFile;
        public int CitationsPerFile
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

        private int totalCitationNumber;
        public int TotalCitationNumber
        {
            get
            {
                return totalCitationNumber;
            }
            set
            {
                totalCitationNumber = value;
                OnPropertyChanged("TotalCitationNumber");
            }
        }

        private Dictionary<string, CitationViewModel> UniqueCitations;
        private int uniqueCitationNumber;
        public int UniqueCitationNumber
        {
            get
            {
                return uniqueCitationNumber;
            }
            set
            {
                uniqueCitationNumber = value;
                OnPropertyChanged("UniqueCitationNumber");
            }
        }

        public DuplicateFilterWindowViewModel()
        {
            SelectedFiles = new ObservableCollection<IListableFileViewModel>();
            SaveToFilesCommand = new RelayCommand(new Action<object>(SaveToFiles));
            ChooseOutputFolderCommand = new RelayCommand(new Action<object>(ChooseOutputFolder));
            AddFileToListCommand = new RelayCommand(new Action<object>(AddFileToList));
            ChooseFolderLabel = "Add a file";
            ChosenFolder = string.Empty;
            TotalCitationNumber = 0;
            UniqueCitationNumber = 0;
            UniqueCitations = new Dictionary<string, CitationViewModel>();
        }

        public void SaveToFiles(object message)
        {
            if (SelectedFiles.Count < 2)
            {
                // handle
                ShowError("You might want to select at least two files...");
                return;
            }

            if (CitationsPerFile < 1)
            {
                ShowError("Citations per file should be positive (> 0)");
                return;
            }


            if (string.IsNullOrWhiteSpace(ChosenFolder))
            {
                ShowError("Please select an output folder");
                return;
            }

            if (string.IsNullOrWhiteSpace(OutName))
            {
                ShowError("Please enter an output file name");
                return;
            }

            Debug.WriteLine(OutName + " " + CitationsPerFile + " " + ChosenFolder);
            int numberOfFiles;
            if (CitationsPerFile >= UniqueCitationNumber)
            {
                numberOfFiles = 1;
            }
            else
            {
                numberOfFiles = (int)(UniqueCitationNumber / CitationsPerFile) + 1;
            }

            if (numberOfFiles >= 20)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("This is going to create " + numberOfFiles + " files, are you sure?",
                    "Confirmation",
                    MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
                else
                {
                    if (numberOfFiles > 100)
                    {
                        ShowError("" + numberOfFiles + " is greater than 100, you'll ruin your computer, I won't let you do that");
                        return;
                    }
                }
            }

            if (Directory.Exists(ChosenFolder))
            {
                // TODO check has right to open and write file http://stackoverflow.com/questions/130617/how-do-you-check-for-permissions-to-write-to-a-directory-or-file

                List<string> uniqueIds = UniqueCitations.Keys.ToList();

                for (int fileNo = 0; fileNo < numberOfFiles; fileNo++)
                {
                    // TODO this should be in own method
                    string newFilePath = Path.Combine(ChosenFolder,
                        string.Concat(OutName, fileNo.ToString(CultureInfo.InvariantCulture), ".nbib"));
                    try
                    {
                        using (FileStream newStream = new FileStream(newFilePath, FileMode.Create))
                        using (TextWriter writer = new StreamWriter(newStream))
                        {
                            int startIndex = fileNo * CitationsPerFile;
                            int endIndex = (fileNo + 1) * CitationsPerFile;
                            if (endIndex > uniqueIds.Count)
                            {
                                endIndex = uniqueIds.Count;
                            }

                            for (int citationNo = startIndex; citationNo < endIndex; citationNo++)
                            {
                                string citationId = uniqueIds[citationNo];
                                string fullText = UniqueCitations[citationId].FullText;
                                writer.WriteLine(fullText);
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        // TODO better exception hadnling
                        ShowError(UnknowErrorMessage);
                        return;
                    }
                }
            }
            else
            {
                Debug.WriteLine(ChosenFolder + " does not exist");
            }

            ShowSuccess("Success - files have been created");
        }

        public async void AddFileToList(object message)
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
                    if (!SelectedFiles.Where(file => file.FullPath.Equals(dialog.FileName)).Any())
                    {
                        IListableFileViewModel loadingMessage = new LoadingMessage { FullPath = " - LOADING - PLEASE WAIT -", NumberOfCitations = 0 };
                        try
                        {

                            var slowTask = Task<MedlineFileViewModel>.Factory.StartNew(() => CreateMedlineFileViewModel(dialog.FileName));

                            SelectedFiles.Add(loadingMessage);

                            await slowTask;

                            MedlineFileViewModel fileViewModel = slowTask.Result;

                            SelectedFiles.Remove(loadingMessage);

                            SelectedFiles.Add(fileViewModel);
                            TotalCitationNumber += fileViewModel.NumberOfCitations;
                            foreach (CitationViewModel citation in fileViewModel.Citations)
                            {
                                if (!UniqueCitations.ContainsKey(citation.Id))
                                {
                                    UniqueCitations[citation.Id] = citation;
                                }
                            }

                            UniqueCitationNumber = UniqueCitations.Count;
                        }
                        catch (Exception ex)
                        {
                            ShowError(WrongFileFormatMessage);
                            SelectedFiles.Remove(loadingMessage);
                        }
                    }
                    else
                    {
                        ShowError("Already added that file...");
                    }
                }
            }
        }

        private MedlineFileViewModel CreateMedlineFileViewModel(string fileName)
        {
            return new MedlineFileViewModel(fileName);
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

        private const string SuccessTitle = "Success";
        private const string ErrorTitle = "Error";
        private const string WrongFileFormatMessage = "Error occurred - are you sure you are adding a PUBMED/MEDLINE file?";
        private const string UnknowErrorMessage = "Error occurred - unkown error";
        private const string FilesSavedMessage = "Files have been saved";


        private void ShowError(string message)
        {
            System.Windows.MessageBox.Show(
    message,
    ErrorTitle,
    MessageBoxButton.OK);
        }

        private void ShowSuccess(string message)
        {
            System.Windows.MessageBox.Show(
    message,
    SuccessTitle,
    MessageBoxButton.OK);
        }

        private class LoadingMessage : IListableFileViewModel
        {
            public string FullPath { get; set; }

            public int NumberOfCitations { get; set; }
        }

        public override void Dispose()
        {
        }
    }
}
