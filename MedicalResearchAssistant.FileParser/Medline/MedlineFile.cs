using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearchAssistant.FileParser.Medline
{
    /// <summary>
    /// Model for a Medline file (the NBIB format)
    /// </summary>
    public class MedlineFile
    {
        /// <summary>
        /// Name of the file
        /// </summary>
        public string Name { get; private set; }


        /// <summary>
        /// Number of articles in file
        /// </summary>
        public int NumberOfCitations { get; private set; }


        /// <summary>
        /// The enumaration of citations in this medline file
        /// </summary>
        public IEnumerable<Citation> Citations { get; private set; }


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
