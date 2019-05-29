using System;
using System.Collections.Generic;

namespace LzwStringCompression
{
    class Program
    {
        static void Main(string[] args)
        {
            var compressed = Compress("string to be compressed");
            Console.WriteLine(string.Join(",", compressed));
            var decompressed = Decompress(compressed);
            Console.WriteLine(decompressed);
        }
        public static List<int> Compress(string source)
        {
            var dictionary = new Dictionary<string, int>();
            for (byte i = 0; i < byte.MaxValue; i++)
            {
                dictionary.Add(((char)i).ToString(), i);
            }

            var c = string.Empty;
            var result = new List<int>();

            foreach (var s in source)
            {
                string cs = c + s;
                if (dictionary.ContainsKey(cs))
                {
                    c = cs;
                }
                else
                {
                    result.Add(dictionary[c]);
                    dictionary.Add(cs, dictionary.Count);
                    c = s.ToString();
                }
            }

            if (!string.IsNullOrEmpty(c))
            {
                result.Add(dictionary[c]);
            }

            return result;
        }

        public static string Decompress(List<int> source)
        {
            var dictionary = new Dictionary<int, string>();
            for (byte i = 0; i < byte.MaxValue; i++)
            {
                dictionary.Add(i, ((char)i).ToString());
            }

            string fc = dictionary[source[0]];
            source.RemoveAt(0);

            var result = fc;

            foreach (var c in source)
            {
                var entry = string.Empty;
                if (dictionary.ContainsKey(c))
                {
                    entry = dictionary[c];
                }
                else if (c == dictionary.Count)
                {
                    entry = fc + fc[0];
                }

                result += entry;

                dictionary.Add(dictionary.Count, result + entry[0]);

                fc = entry;
            }
            return result;
        }
    }

}
