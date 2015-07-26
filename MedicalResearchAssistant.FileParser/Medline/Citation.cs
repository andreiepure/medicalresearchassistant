using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearchAssistant.FileParser.Medline
{
    public class Citation
    {
        /// <summary>
        /// The identifier of the article
        /// </summary>
        public string Pmid { get; private set; }

        /// <summary>
        /// The title of the article
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// The abstract of the article
        /// </summary>
        public string Abstract { get; private set; }

        /// <summary>
        /// Whole raw content of Medline citation
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// The files that contain this citation
        /// </summary>
        public HashSet<MedlineFile> ParentContainers { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">The medline identifier of the article</param>
        /// <param name="title">The title of the article</param>
        /// <param name="articleAbstract">The abstract of the article</param>
        /// <param name="content">The raw content of the citation</param>
        public Citation(string id, string title, string articleAbstract, string content)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("id");
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException("title");
            }

            if (string.IsNullOrWhiteSpace(articleAbstract))
            {
                throw new ArgumentNullException("articleAbstract");
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentNullException("content");
            }

            Pmid = id;
            Title = title;
            Abstract = articleAbstract;
            Content = content;
        }
    }
}
