using System;
using System.Linq;

namespace Ya.Taxi
{
    class Program
    {
        static void Main(string[] args)
        {
            BruteForceSearch bruteForceSearch = new BruteForceSearch(6, 6);
            int maxKey = bruteForceSearch.SuccededMovementDictionary.Keys.Max();
            Console.WriteLine($"\n\nMax weight is [{maxKey}] @ [{bruteForceSearch.SuccededMovementDictionary[maxKey]}]");
            Console.ReadKey();
        }
    }
}
