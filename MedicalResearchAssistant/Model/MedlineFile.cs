using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalResearchAssistant.DataAccess;

namespace MedicalResearchAssistant.Model
{
    /// <summary>
    /// Model for a Medline file (the NBIB format)
    /// </summary>
    class MedlineFile
    {
        /// <summary>
        /// Name of the file
        /// </summary>
        public string Name { get; private set; }


        /// <summary>
        /// Number of articles in file
        /// </summary>
        public int NumberOfCitations { get; private set; }


        public IEnumerable<Citation> Citations { get; private set; }


        public MedlineFile(CitationRepository repo)
        {
            if (repo == null)
            {
                throw new ArgumentNullException("repo");
            }

            Name = "default";
            Citations = repo.Citations;
            NumberOfCitations = Citations.Count();
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="articles"></param>
        public MedlineFile(string name, IEnumerable<Citation> articles)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is null or whitespace");
            }

            if (articles == null || articles.Any(article => article == null))
            {
                throw new ArgumentException("Article list is null or contains null");
            }

            Name = name;
            Citations = articles;
            NumberOfCitations = articles.Count();
        }
    }
}
