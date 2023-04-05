// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Day 1 - a");

            List<Int32> list = new List<Int32>();
            StreamReader sr = new StreamReader("data.txt");

            Int32 lastValue = Int32.MaxValue;
            Int32 count = 0;

            while(!sr.EndOfStream)
            {
                Int32 val = Int32.Parse(sr.ReadLine());
                list.Add(val); // keep for part 2

                if (val > lastValue)
                    count++;
                lastValue = val;

            }

            Console.WriteLine("Total: " + count);

            // ***************************************************

            Console.WriteLine("Day 1 - b");

            List<int> sumGroupsOf3 = new List<int>();
            
            for(int j=0; j<list.Count-2; j++)
            {
                sumGroupsOf3.Add(list[j] + list[j + 1] + list[j + 2]);
            }

            lastValue = Int32.MaxValue;
            count = 0;
            for (int i=0; i<sumGroupsOf3.Count; i++)
            {
                if (sumGroupsOf3[i] > lastValue)
                    count++;
                lastValue = sumGroupsOf3[i];
            }

            Console.WriteLine("Total: " + count);
            Console.ReadLine();
        }
    }
}
