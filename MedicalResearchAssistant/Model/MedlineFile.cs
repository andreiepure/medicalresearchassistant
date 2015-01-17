using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearchAssistant.Model
{
    /// <summary>
    /// Model for a Medline file (the NBIB format)
    /// </summary>
    class MedlineFile
    {
        /// <summary>
        /// Name of the files
        /// </summary>
        public string Name { get; private set; }


        /// <summary>
        /// Number of articles in file
        /// </summary>
        public int NumberOfArticles
        {
            get
            {
                return m_articles.Count;
            }
        }


        /// <summary>
        /// List of articles
        /// </summary>
        private readonly List<Article> m_articles = new List<Article>();
        public IEnumerable<Article> Articles
        {
            get
            {
                return new List<Article>(m_articles);
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="articles"></param>
        public MedlineFile(string name, IEnumerable<Article> articles)
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
            m_articles.AddRange(articles);
        }
    }
}
