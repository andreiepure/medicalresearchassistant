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
            List<Citation> citationList = new List<Citation>();
            try
            {
                string currentId = null;
                string currentTitle = null;
                string currentAbstract = "dummy";
                StringBuilder contentBuilder = null;

                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        string line;
                        bool firstFoundId = true;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (string.IsNullOrWhiteSpace(line))
                            {
                                continue;
                            }

                            line = line.Trim();

                            if (line.Length > ArticleIdentifier.Length &&
                                line.Contains(IdentifierSeparator) &&
                                line.IndexOf(ArticleIdentifier) == 0)
                            {
                                if (!firstFoundId)
                                {
                                    if (currentId != null)
                                    {
                                        if (currentTitle != null)
                                        {
                                            if (currentAbstract != null)
                                            {
                                                if (contentBuilder != null)
                                                {
                                                    Citation justParsedCitation =
                                                        new Citation(currentId,
                                                            currentTitle,
                                                            currentAbstract,
                                                            contentBuilder.ToString());
                                                    citationList.Add(justParsedCitation);
                                                }
                                                else
                                                {
                                                    Debug.WriteLine("Content builder is null");
                                                }
                                            }
                                            else
                                            {
                                                Debug.WriteLine("Current abstract is null");
                                            }
                                        }
                                        else
                                        {
                                            Debug.WriteLine("Current title is null");
                                        }
                                    }
                                    else
                                    {
                                        Debug.WriteLine("Current id is null");
                                    }
                                }

                                firstFoundId = false;
                                currentTitle = null;
                                contentBuilder = new StringBuilder();
                                string[] idLineContents = line.Split(IdentifierSeparator);
                                if (idLineContents.Length == 2 && !string.IsNullOrWhiteSpace(idLineContents[1]))
                                {
                                    currentId = idLineContents[1].Trim();
                                }
                                else
                                {
                                    Debug.WriteLine("Could not get PMID");
                                }
                            }
                            else if (line.Length > TitleIdentifier.Length &&
                                line.Contains(IdentifierSeparator) &&
                                line.Substring(0, TitleIdentifier.Length).Equals(TitleIdentifier))
                            {
                                string[] titleLineContents = line.Split(IdentifierSeparator);
                                if (titleLineContents.Length >= 2 && !string.IsNullOrWhiteSpace(titleLineContents[1]))
                                {
                                    currentTitle = string.Concat(titleLineContents.Skip(1));
                                }
                                else
                                {
                                    Debug.WriteLine("Could not get title");
                                }
                            }

                            contentBuilder.AppendLine(line);
                        }
                    }
                }

                // last article
                if (!string.IsNullOrWhiteSpace(currentId))
                {
                    if (!string.IsNullOrWhiteSpace(currentTitle))
                    {
                        if (!string.IsNullOrWhiteSpace(currentAbstract))
                        {
                            if (contentBuilder != null)
                            {
                                Citation justParsedCitation =
                                    new Citation(currentId,
                                        currentTitle,
                                        currentAbstract,
                                        contentBuilder.ToString());
                                citationList.Add(justParsedCitation);
                            }
                            else
                            {
                                Debug.WriteLine("Content builder is null");
                            }
                        }
                        else
                        {
                            Debug.WriteLine("Current abstract is null");
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Current title is null");
                    }
                }
                else
                {
                    Debug.WriteLine("Current id is null");
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
    }
}
