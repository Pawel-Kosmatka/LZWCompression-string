using System;
using System.Collections.Generic;

namespace LzwStringCompression
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Nisl purus in mollis nunc. Faucibus nisl tincidunt eget nullam non nisi est. Ipsum suspendisse ultrices gravida dictum fusce ut. A cras semper auctor neque vitae tempus quam. Consectetur adipiscing elit ut aliquam purus. Bibendum enim facilisis gravida neque. Netus et malesuada fames ac turpis. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Quam elementum pulvinar etiam non quam lacus. Nisl tincidunt eget nullam non nisi est. Mauris in aliquam sem fringilla ut morbi tincidunt. Maecenas volutpat blandit aliquam etiam erat velit scelerisque. Aliquam eleifend mi in nulla posuere. Id aliquet risus feugiat in. Dictum non consectetur a erat nam at lectus urna. Sit amet commodo nulla facilisi. Vitae ultricies leo integer malesuada. Enim nec dui nunc mattis enim ut tellus. Sit amet volutpat consequat mauris nunc congue.";
            var compressed = Compress(source);
            Console.WriteLine("Compression result:");
            Console.WriteLine(string.Join(",", compressed));
            Console.WriteLine("Decompressed string:");
            var decompressed = Decompress(compressed);
            Console.WriteLine(decompressed);

            Console.WriteLine("Source string length:");
            Console.WriteLine(source.Length);
            Console.WriteLine("Compressed data length:");
            Console.WriteLine(compressed.Count);
            Console.WriteLine("Decompressed string length:");
            Console.WriteLine(decompressed.Length);

            Console.WriteLine("Compression ratio:");
            Console.WriteLine(((double)source.Length / compressed.Count).ToString("F"));

            Console.ReadLine();
        }
        public static List<int> Compress(string source)
        {
            var dictionary = new Dictionary<string, int>();
            for (byte i = 0; i < byte.MaxValue; i++)
            {
                dictionary.Add(((char)i).ToString(), i);
            }

            var result = new List<int>();

            var c = string.Empty;
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

            var fc = source[0];
            source.RemoveAt(0);

            var result = dictionary[fc];

            foreach (var c in source)
            {
                var pc = dictionary[fc];
                if (dictionary.ContainsKey(c))
                {
                    dictionary.Add(dictionary.Count, pc + dictionary[c][0]);
                    result += dictionary[c];
                }
                else
                {
                    dictionary.Add(dictionary.Count, pc + pc[0]);
                    result += pc + pc[0];
                }

                fc = c;
            }
            return result.ToString();
        }
    }

}
