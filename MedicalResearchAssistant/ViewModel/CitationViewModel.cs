using MedicalResearchAssistant.Model;
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

        public CitationViewModel(Citation citation)
        {
            this.citation = citation;
        }

        public override void Dispose()
        {
        }
    }
}
