using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearchAssistant.Model
{
    class Article
    {
        /// <summary>
        /// The PMID of the article
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// The title of the article
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        public Article(string id, string title)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException();
            }

            Id = id;
            Title = title;
        }
    }
}
