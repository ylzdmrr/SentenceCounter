using System;
using System.IO;

namespace SentenceCounter.Core
{
    public class Utility
    {
        public static string ReadTextFromFile(string path) 
        {
            var directory = System.IO.Directory.GetCurrentDirectory();
            return File.ReadAllText(Path.Combine(directory,path));
        }
    }
}
