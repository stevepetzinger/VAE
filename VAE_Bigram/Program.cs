using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace VAE_Bigram
{
    /// <summary>
    /// Usage: VAE_Bigram "<full path to file>"
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: VAE_Bigram \"<Full path to file>\"");
                Console.ReadLine();
                return;
            }
            var fullFilePath = args[0];

            // Read in the file contents
            var fileContents = string.Empty;
            using (var sr = new StreamReader(fullFilePath))
            {
                fileContents = sr.ReadToEnd();
            }

            // clean up the string
            fileContents = CleanUpString(fileContents);

            // parse the string
            List<string> words = ParseString(fileContents);

            // perform the counts
            Dictionary<string, int> wordsDict = PerformCounts(words);

            // output the result
            foreach (var phrase in wordsDict)
            {
                Console.WriteLine($"\"{phrase.Key}\" {phrase.Value}");
            }

            Console.ReadLine();
        }

        public static Dictionary<string, int> PerformCounts(List<string> words)
        {
            var wordsDict = new Dictionary<string, int>();

            string prev = string.Empty, current = string.Empty;
            foreach (var word in words)
            {
                if (prev == string.Empty)
                { // continue to next if first
                    prev = word;
                    continue;
                }

                var phrase = $"{prev} {word}";
                if (!wordsDict.Keys.Contains(phrase)) // add if not exists
                    wordsDict.Add(phrase, 1);
                else // increment if exists
                    wordsDict[phrase] += 1;

                prev = word;
            }

            return wordsDict;
        }

        /// <summary>
        /// Splits the string on spaces and removes empty items
        /// </summary>
        /// <param name="fileContents"></param>
        /// <returns></returns>
        public static List<string> ParseString(string fileContents)
        {
            return fileContents.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// Cleans up the input string to conform to requirements.
        /// Assumption: ignore punctuation, convert new lines to spaces, and covert to lower case
        /// </summary>
        /// <param name="fileContents"></param>
        /// <returns></returns>
        public static string CleanUpString(string fileContents)
        {
            fileContents = Regex.Replace(fileContents, "[.;!:?]+", string.Empty);
            fileContents = Regex.Replace(fileContents, $"[{Environment.NewLine}]+", " ");
            fileContents = fileContents.ToLower();
            return fileContents;
        }
    }
}
