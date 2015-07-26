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

        public string FullText
        {
            get
            {
                return citation.Content;
            }
        }

        /// <summary>
        /// Constructor - called when the first instance of the citation is found
        /// </summary>
        /// <param name="citation">The citation backing the viewmodel</param>
        /// <param name="medlineFile">The file containing the citation</param>
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
