using MedicalResearchAssistant.FileParser.Medline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MedicalResearchAssistant.DataAccess
{
    /// <summary>
    /// Mock repository holding mock citations
    /// </summary>
    public class CitationRepository
    {
        /// <summary>
        /// Internal list of citations
        /// </summary>
        private IList<Citation> citations;

        /// <summary>
        /// External accessible citations enumeration
        /// </summary>
        public IEnumerable<Citation> Citations
        {
            get
            {
                return citations;
            }
        }

        /// <summary>
        /// The constructor - initializes the mock citation list
        /// </summary>
        public CitationRepository()
        {
            citations = new List<Citation>
            {
            };
        }
    }
}
