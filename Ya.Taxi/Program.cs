using System;
using System.Linq;

namespace Ya.Taxi
{
    class Program
    {
        static void Main(string[] args)
        {
            int width, height;
            try
            {
                Console.Write("Enter the field parameter W >\t");
                width = Convert.ToInt16(Console.ReadLine());
                Console.Write("Enter the field parameter H >\t");
                height = Convert.ToInt16(Console.ReadLine());
            }
            catch (FormatException e)
            {
                Console.WriteLine("Invalid user input data. Changing to (0, 0)");
                width = 0;
                height = 0;
            }
            Console.WriteLine();
            BruteForceSearch bruteForceSearch = new BruteForceSearch(width, height);
            int maxKey = bruteForceSearch.SuccededMovementDictionary.Keys.Max();
            Console.WriteLine($"Unique routes count is [{bruteForceSearch.SuccededMovementDictionary.Count}]");
            Console.WriteLine($"\n\nMax weight is [{maxKey}] @ [{bruteForceSearch.SuccededMovementDictionary[maxKey]}]");
            Console.ReadKey();
        }
    }
}
