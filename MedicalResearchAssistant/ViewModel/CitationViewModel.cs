using MedicalResearchAssistant.FileParser.Medline;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MedicalResearchAssistant.ViewModel
{
    public class CitationViewModel : ViewModelBase
    {
        private Citation citation;

        public string Id
        {
            get
            {
                return citation.Pmid;
            }
        }

        public string Title
        {
            get
            {
                return citation.Title;
            }
        }

        public CitationViewModel(Citation citation)
        {
            if (citation == null)
            {
                throw new ArgumentNullException("citation");
            }

            this.citation = citation;
        }

        public override void Dispose()
        {
        }
    }
}
