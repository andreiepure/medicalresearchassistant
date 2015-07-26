using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Diagnostics;

namespace MedicalResearchAssistant.FileParser.Medline
{
    public class MedlineParser
    {
        private const string ArticleIdentifier = "PMID";
        private const string TitleIdentifier = "TI";
        private const string AbstractIdentifier = "AB";
        private const char IdentifierSeparator = '-';

        public static MedlineFile Parse(string filePath)
        {
            List<Citation> citationList = null;
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        citationList = AddCitations(reader);
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            //catch(FileNotFoundException ex)
            //{
            //    D
            //}
            //catch (DirectoryNotFoundException ex)
            //{
            //}
            //catch (PathTooLongException ex)
            //{
            //}
            //catch (OutOfMemoryException ex)
            //{
            //}
            //catch (SecurityException ex)
            //{
            //}
            //catch (NotSupportedException ex)
            //{
            //}
            MedlineFile result = new MedlineFile(filePath, citationList);
            return result;
        }

        /// <summary>
        /// Reads the lines from the reader, constructs citations and returns them in a list
        /// </summary>
        /// <remarks>
        /// First it finds the PMID, then the title. The abstract is currently not used.
        /// When it finds the next PMID, it creates the previous found citation and adds it to the list.
        /// At the end of reading the file, the last citation is stored in the variables and must be added
        /// outside the loop.
        /// </remarks>
        /// <param name="reader">Stream reader initialized with the contents of the file</param>
        private static List<Citation> AddCitations(StreamReader reader)
        {
            List<Citation> citationList = new List<Citation>();
            string line;
            string currentId = null;
            string currentTitle = null;
            string currentAbstract = "dummy";
            StringBuilder currentContentBuilder = null;

            // it must first create a citation before adding it
            bool notFirstIdentifier = false;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                line = line.Trim();

                // TODO move this check in a method - IsIdentifierLine
                if (line.Length > ArticleIdentifier.Length &&
                    line.Contains(IdentifierSeparator) &&
                    line.IndexOf(ArticleIdentifier) == 0)
                {
                    if (notFirstIdentifier)
                    {
                        Citation previousCitation = CreateCitation(currentId, currentTitle, currentAbstract, currentContentBuilder);
                        citationList.Add(previousCitation);
                    }

                    // get the article id
                    // TODO move in method TryParseIdentifier
                    string[] idLineContents = line.Split(IdentifierSeparator);
                    if (idLineContents.Length == 2 && !string.IsNullOrWhiteSpace(idLineContents[1]))
                    {
                        currentId = idLineContents[1].Trim();
                        notFirstIdentifier = true;
                        // reset the title and the content builder because we found a new citation
                        currentTitle = null;
                        currentContentBuilder = new StringBuilder();
                    }
                    else
                    {
                        throw new FormatException("Could not get PMID");
                    }
                }
                //TODO move this check in a method - IsTitleLine
                else if (line.Length > TitleIdentifier.Length &&
                    line.Contains(IdentifierSeparator) &&
                    line.Substring(0, TitleIdentifier.Length).Equals(TitleIdentifier))
                {
                    // TODO move in method TryParseTitle - take into account the Concat problem
                    string[] titleLineContents = line.Split(IdentifierSeparator);
                    if (titleLineContents.Length >= 2 && !string.IsNullOrWhiteSpace(titleLineContents[1]))
                    {
                        // the title might contain the IdentifierSeparator
                        currentTitle = string.Concat(titleLineContents.Skip(1));
                    }
                    else
                    {
                        throw new FormatException("Could not get title");
                    }
                }

                currentContentBuilder.AppendLine(line);
            }

            // last article
            Citation lastCitation = CreateCitation(currentId, currentTitle, currentAbstract, currentContentBuilder);
            citationList.Add(lastCitation);

            return citationList;
        }

        /// <summary>
        /// Tries to create a citation if the given fields are valid
        /// </summary>
        /// <param name="currentId">Id of the citation</param>
        /// <param name="currentTitle">Title of the citation</param>
        /// <param name="currentAbstract">Abstract of the article</param>
        /// <param name="contentBuilder">The content builder holding the whole file</param>
        /// <param name="justParsedCitation">The result - the citation</param>
        /// <returns>True if the citation was created, false otherwise</returns>
        private static Citation CreateCitation(string currentId, string currentTitle, string currentAbstract, StringBuilder contentBuilder)
        {
            Citation citation = null;
            if (!string.IsNullOrWhiteSpace(currentId))
            {
                if (!string.IsNullOrWhiteSpace(currentTitle))
                {
                    if (!string.IsNullOrWhiteSpace(currentAbstract))
                    {
                        if (contentBuilder != null)
                        {
                            citation =
                                new Citation(currentId,
                                    currentTitle,
                                    currentAbstract,
                                    contentBuilder.ToString());
                            return true;
                        }
                        else
                        {
                            throw new ArgumentNullException("Content builder is null");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Abstract is null or whitespace");
                    }
                }
                else
                {
                    throw new ArgumentException("Title is null or whitespace");
                }
            }
            else
            {
                throw new ArgumentException("Id is null or whitespace");
            }

            return citation;
        }
    }
}
