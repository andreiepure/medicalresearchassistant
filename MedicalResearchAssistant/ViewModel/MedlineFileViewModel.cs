using MedicalResearchAssistant.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearchAssistant.ViewModel
{
    public class MedlineFileViewModel : ViewModelBase
    {
        private readonly CitationRepository citationRepository;

        public ObservableCollection<CitationViewModel> Citations { get; private set; }

        public string Folder { get; private set; }
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
            Folder = Path.GetFileName(filePath);
            Name = Path.GetPathRoot(filePath);
            citationRepository = new CitationRepository();
        }

        public override void  Dispose()
        {
        }
    }
}
