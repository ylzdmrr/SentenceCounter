using SentenceCounter.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SentenceCounter
{
    class Program
    {
        const int MAX_THREAD_COUNT = 5;
        static ConcurrentDictionary<string, int> _wordList;
        static void Main(string[] args)
        {
            int totalWordCount = 0;
            _wordList = new ConcurrentDictionary<string, int>();

            try
            {
                List<string> sentenceList = GetSentencesFromFile(@"Documents/SampleText.txt");
                sentenceList.ToList()
                    .AsParallel()
                    .WithDegreeOfParallelism(MAX_THREAD_COUNT)
                    .ForAll(sentence =>
                    {
                        var words = GetWordsFromSentence(sentence);
                        totalWordCount += words.Length;
                        words.ToList()
                            .AsParallel()
                            .ForAll(word =>
                            {
                                _wordList.AddOrUpdate(word, 1, (key, oldValue) => oldValue + 1);
                            });
                    });

                Console.WriteLine($"Sentence Count: {sentenceList?.Count}");
                Console.WriteLine($"Avg. Word Count: {totalWordCount / sentenceList?.Count}");
                _wordList.OrderByDescending(x => x.Value).ToDictionary(k => k.Key, v => v.Value).AsEnumerable().ToList().ForEach(x => { Console.WriteLine(x); });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured! Exception: {ex}");
            }
        }

        /// <summary>
        /// It returns sentence list from file given path
        /// </summary>
        /// <returns></returns>
        static List<string> GetSentencesFromFile(string path)
        {
            List<string> sentences = new List<string>();
            string text = Utility.ReadTextFromFile(path);
            sentences.AddRange(Regex.Split(text, @"(?<=[\.!\?])\s+"));

            return sentences;
        }

        /// <summary>
        /// It returns word array given sentence
        /// </summary>
        /// <returns></returns>
        static string[] GetWordsFromSentence(string sentence)
        {
            Regex reg_exp = new Regex("[^a-zA-Z0-9]");
            sentence = reg_exp.Replace(sentence, " ");

            string[] words = sentence.Split(
                new char[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries);
            return words;
        }
    }
}
