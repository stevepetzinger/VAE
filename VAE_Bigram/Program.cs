using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace VAE_Bigram
{
    public class Program
    {
        private const string _incomingDirectory = "incoming";
        static void Main(string[] args)
        {
            // TODO: read in file from relative directory

            /*
                 * Create an application that can take as input any text file and output a histogram of the bigrams
                in the text.
                Description:
                A bigram is any two adjacent words in the text. A histogram is the count of how many times that
                particular bigram occurred in the text.
                A well formed submission will be runnable from command line and have accompanying unit
                tests for the bigram parsing and counting code. You may do this in any language you wish and
                use any framework or data structures you wish to handle reading the files, building up the
                output, and running the unit tests. However the bigram parsing and counting code must be
                implemented by yourself.
                Example:
                Given the text: “The quick brown fox and the quick blue hare.” The bigrams with their counts
                would be.
                 “the quick” 2
                 “quick brown” 1
                 “brown fox” 1
                 “fox and” 1
                 “and the” 1
                 “quick blue” 1
                 “blue hare” 1

                Unit tests:
                case-insensitive
                basic functionality, 1,2,3 counts
                punctuation removed
                assert the number of distinct pairs
                assert the total number of pairs
                multi-line
            */

            // get all the text files in the incoming directory
            var incomingPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, _incomingDirectory);
            var files = Directory.GetFiles(incomingPath, "*.txt").ToList();

            foreach (var file in files)
            {
                // Read in the file contents
                var fileContents = string.Empty;
                using (var sr = new StreamReader(file))
                {
                    fileContents = sr.ReadToEnd();
                }

                // clean up the string
                fileContents = CleanUpString(fileContents);

                // parse the string
                List<string> words = ParseString(fileContents);

                // perform the counts
                Dictionary<string,int> wordsDict = PerformCounts(words);

                // output the result
                foreach (var phrase in wordsDict)
                {
                    Console.WriteLine($"{phrase.Key} {phrase.Value}");
                }

                Console.ReadLine();
            }
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
