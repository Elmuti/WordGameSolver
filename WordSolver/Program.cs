using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace WordSolver
{
    class Program
    {
        private static List<string> words = new List<string>();


        private static StreamReader GenerateStreamFromString(string s)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(s);
            MemoryStream stream = new MemoryStream(byteArray);
            StreamReader reader = new StreamReader(stream);
            return reader;
        }

        public static List<string> GetWordsFromList()
        {
            List<string> list = new List<string>();
            foreach (XElement ele in XElement.Load(GenerateStreamFromString(WordSolver.Properties.Resources.wordlist)).Elements("st"))
            {
                list.Add(ele.Element("s").Value);
            }
            return list;
        }

        private static void GetStringPermutations(string word, string currentWord, int minLength, int maxLength)
        {
            string stat = currentWord;
            for (int i = 0; i < word.Length; i++)
            {
                currentWord = stat + word.ElementAt(i);

                if (currentWord.Length >= minLength && currentWord.Length <= maxLength)
                {
                    words.Add(currentWord);
                }

                GetStringPermutations(word.Remove(i, 1), currentWord, minLength, maxLength);
            }
        }


        public static Dictionary<string, int> GetWordsMatchingDictionary(List<string> words, List<string> dict)
        {
            Dictionary<string, int> list = new Dictionary<string, int>();

            foreach(string s in words)
            {
                foreach(string m in dict)
                {
                    if (s == m)
                    {
                        if (list.ContainsKey(s))
                            list[s]++;
                        else
                            list.Add(s, 1);
                    }
                }
            }

            return list;
        }


        static void Main()
        {
            List<string> wordlist = GetWordsFromList();

            while (true)
            {
                Console.Clear();
                Console.Write("All the letters to search for:\n>");
                string str = Console.ReadLine().ToLower();
                Console.Clear();
                Console.Write("The number of letters to find in a word\n>");
                int nChars = Int32.Parse(Console.ReadLine());
                Console.Clear();
                Console.Write("Finding words, please wait...\n");


                GetStringPermutations(str, "", 3, nChars);


                Dictionary<string, int> matches = GetWordsMatchingDictionary(words, wordlist);

                foreach (KeyValuePair<string, int> match in matches)
                {
                    Console.WriteLine(match.Key);
                }

                Console.Write("\n" + matches.Count + " words found.\nPress ENTER to try again");
                Console.ReadLine();
                words = new List<string>();
            }
        }
    }
}
