using MedicalResearchAssistant.DataAccess;
using MedicalResearchAssistant.FileParser.Medline;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearchAssistant.ViewModel
{
    public class MedlineFileViewModel : ViewModelBase, IListableFileViewModel
    {
        public ObservableCollection<CitationViewModel> Citations { get; private set; }

        private MedlineFile medlineFile;

        public string FullPath { get; private set; }
        public string Name { get; private set; }
        public int NumberOfCitations
        {
            get
            {
                if (Citations == null)
                {
                    return 0;
                }

                return Citations.Count;
            }
        }

        public MedlineFileViewModel(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException("name");
            }
            Name = Path.GetFileName(filePath);
            FullPath = Path.GetFullPath(filePath);
            string absolutePath = Path.GetFullPath(filePath);
            try
            {
                medlineFile = MedlineParser.Parse(absolutePath);
                Citations = new ObservableCollection<CitationViewModel>();
                foreach (Citation citation in medlineFile.Citations)
                {
                    CitationViewModel citationModel = new CitationViewModel(citation);
                    Citations.Add(citationModel);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public override void  Dispose()
        {
        }
    }
}
