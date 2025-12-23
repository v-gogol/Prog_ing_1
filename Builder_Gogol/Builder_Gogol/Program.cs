using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Text.Encodings.Web;

namespace Builder_Gogol
{
    internal class Program
    {
        public class PublicationRecord
        {
            public string Name { get; set; }
            public List<string> Contributors { get; set; }
            public string Content { get; set; }
            public string Checksum { get; set; }
            public bool IsChecksumCorrect { get; set; }
        }

        public class PublicationDirector
        {
            private PublicationRecord Record { get; set; }

            public PublicationDirector()
            {
                Record = new PublicationRecord();
            }

            public PublicationDirector AddName(string name)
            {
                Record.Name = name.Trim();
                return this;
            }

            public PublicationDirector AddContributors(string contributorsInput)
            {
                var contributorList = new List<string>();
                foreach (var person in contributorsInput.Split(','))
                {
                    contributorList.Add(person.Trim());
                }
                Record.Contributors = contributorList;
                return this;
            }

            public PublicationDirector AddMainText(string mainText)
            {
                Record.Content = mainText.Trim();
                return this;
            }

            public PublicationDirector AddChecksum(string checksum)
            {
                Record.Checksum = checksum.Trim();
                return this;
            }

            public PublicationRecord Create()
            {
                return Record;
            }
        }

        public class PublicationFileHandler
        {
            public PublicationRecord ProcessFileContent(string fileLocation)
            {
                string[] fileLines = File.ReadAllLines(fileLocation, Encoding.UTF8);
                PublicationDirector director = new PublicationDirector();

                int currentLine = 0;
                string accumulatedText = string.Empty;

                foreach (string singleLine in fileLines)
                {
                    currentLine++;
                    if (currentLine == 1)
                    {
                        director.AddName(singleLine);
                    }
                    else if (currentLine == 2)
                    {
                        director.AddContributors(singleLine);
                    }
                    else if (currentLine == fileLines.Length)
                    {
                        director.AddChecksum(singleLine);
                        director.AddMainText(accumulatedText);
                    }
                    else
                    {
                        accumulatedText = accumulatedText + singleLine + "\n";
                    }
                }

                return director.Create();
            }
        }

        public static class IntegrityVerifier
        {
            public static bool CheckIntegrity(PublicationRecord record)
            {
                using (var md5Algorithm = MD5.Create())
                {
                    byte[] textBytes = Encoding.UTF8.GetBytes(record.Content);
                    byte[] hashBytes = md5Algorithm.ComputeHash(textBytes);
                    string calculatedHash = BitConverter.ToString(hashBytes);
                    return calculatedHash == record.Checksum;
                }
            }
        }

        static void Main(string[] args)
        {
            string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));
            string storageFolder = Path.Combine(projectRoot, "Data");

            string sourceFile = Path.Combine(storageFolder, "article.txt");
            string destinationFile = Path.Combine(storageFolder, "article.json");

            var fileProcessor = new PublicationFileHandler();
            var publication = fileProcessor.ProcessFileContent(sourceFile);
            publication.IsChecksumCorrect = IntegrityVerifier.CheckIntegrity(publication);

            var serializationSettings = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string jsonOutput = JsonSerializer.Serialize(publication, serializationSettings);

            File.WriteAllText(destinationFile, jsonOutput, Encoding.UTF8);

            Console.WriteLine("Статья успешно сохранена в JSON формате!");
        }
    }
}
