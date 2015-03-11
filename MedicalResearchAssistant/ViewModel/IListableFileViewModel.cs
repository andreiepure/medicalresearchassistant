using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearchAssistant.ViewModel
{
    public interface IListableFileViewModel
    {
        string FullPath { get; }
        int NumberOfCitations { get; }
    }
}
