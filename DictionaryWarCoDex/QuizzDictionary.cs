using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryWarCoDex
{
    public static class QuizzDictionary
    {
        public static Word word = new Word();

        public static Dictionary<string, string> nova = new Dictionary<string, string>();
        public static Dictionary<int, bool> rezultati = new Dictionary<int, bool>();

        public static int BrojTacno = 0;
        public static int BrojNetacno = 0;

        public static void EmptyDictionaryQuiz()
        {
            nova.Clear();
            BrojTacno = 0;
            BrojNetacno = 0;
        }
        public static void EmptyResults()
        {
            rezultati.Clear();
        }

        public static void EmptyWord()
        {
            word.WordMain = null;
        }
    }
}
