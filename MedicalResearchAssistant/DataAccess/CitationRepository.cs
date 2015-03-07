using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalResearchAssistant.Model;

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
                new Citation("1", "First citation", "first abstract"),
                new Citation("2", "Second citation", "second abstract"),
                new Citation("3", "Third citation", "third abstract"),
                new Citation("4", "Fourth citation", "fourth abstract")
            };
        }
    }
}
