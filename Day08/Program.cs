// See https://aka.ms/new-console-template for more information
using System.Diagnostics.SymbolStore;

Console.WriteLine("Day 8");

string[] lines = File.ReadAllLines("data.txt");

string[,] elements = new string[lines.Length,14];

int row = 0;
foreach(string line in lines)
{
    string[] parts = line.Split('|', StringSplitOptions.TrimEntries);

    //int pos = 0;
    string[] leftSide = parts[0].Split(' ', StringSplitOptions.TrimEntries);
    string[] rightSide = parts[1].Split(' ', StringSplitOptions.TrimEntries);
    
    for(int i=0; i<10; i++)
        elements[row,i] = SortString(leftSide[i]);

    for (int i = 10; i < 14; i++)
        elements[row, i] = SortString(rightSide[i-10]);

    row++;
}


// part 1
int count = 0;
for(row = 0; row < lines.Length; row++)
{
    for(int col=10; col < 14; col++)
    {
        if (elements[row, col].Length == 2 || elements[row, col].Length == 4 || elements[row, col].Length == 3 || elements[row, col].Length == 7)
        {
            count++;
            //Console.WriteLine("{0} - {1}", count, elements[row, col]);
        }
    }
}


Console.WriteLine("Count 1,4,7,8 is: {0}", count); // 237 for part 1

// for second part
char[,] mappingSegments = new char[200, 7];
char[,] mappingDigits = new char[200, 10];

Dictionary<string, int> segmentToDigit = new Dictionary<string, int>();
segmentToDigit.Add("abcefg", 0);
segmentToDigit.Add("cf", 1);
segmentToDigit.Add("acdeg", 2);
segmentToDigit.Add("acdfg", 3);
segmentToDigit.Add("bcdf", 4);
segmentToDigit.Add("abdfg", 5);
segmentToDigit.Add("abdefg", 6);
segmentToDigit.Add("acf", 7);
segmentToDigit.Add("abcdefg", 8);
segmentToDigit.Add("abcdfg", 9);

int totalSum = 0;

for(row=0; row < 200; row++)
{
    string um = string.Empty, quatro = string.Empty, sete = string.Empty, oito=string.Empty;

    Dictionary<string, int> dict = new Dictionary<string, int>();

    for(int col=0; col<10; col++)
    {

        mappingDigits[row, col] = '.';

        if (elements[row, col].Length == 2)
        {
            mappingDigits[row, col] = '1';
            um = elements[row, col];

            dict[elements[row, col]] = 1;
        }

        if (elements[row, col].Length == 4)
        {
            mappingDigits[row, col] = '4';
            quatro = elements[row, col];

            dict[elements[row, col]] = 4;

        }

        if (elements[row, col].Length == 3)
        {
            mappingDigits[row, col] = '7';
            sete = elements[row, col];

            dict[elements[row, col]] = 7;

        }

        if (elements[row, col].Length == 7)
        {
            mappingDigits[row, col] = '8';
            oito = elements[row, col];

            dict[elements[row, col]] = 8;

        }
    }

    for (int col = 0; col < 10; col++)
    {
        if (elements[row, col].Length == 5 && RemoveAllFrom(elements[row, col], um).Length==3)
        {
            mappingDigits[row, col] = '3';
            dict[elements[row, col]] = 3;

        }

        if (elements[row, col].Length == 6 && RemoveAllFrom(elements[row, col], um).Length == 5)
        {
            mappingDigits[row, col] = '6';
            dict[elements[row, col]] = 6;

        }

        if (elements[row, col].Length == 6 && RemoveAllFrom(elements[row, col], quatro).Length == 2)
        {
            mappingDigits[row, col] = '9';
            dict[elements[row, col]] = 9;

        }

        if (mappingDigits[row, col] == '.' && elements[row, col].Length == 6 && RemoveAllFrom(elements[row, col], quatro).Length == 3)
        {
            mappingDigits[row, col] = '0';
            dict[elements[row, col]] = 0;

        }

        if (mappingDigits[row, col] == '.' && elements[row, col].Length == 5 && RemoveAllFrom(RemoveAllFrom(elements[row, col], sete),quatro).Length == 1)
        {
            mappingDigits[row, col] = '5';
            dict[elements[row, col]] = 5;

        }

        if (mappingDigits[row, col] == '.')
        {
            mappingDigits[row, col] = '2';
            dict[elements[row, col]] = 2;
        }

        Console.Write(mappingDigits[row, col]);
    }

    int number = dict[elements[row, 10]] * 1000 + dict[elements[row, 11]] * 100 + dict[elements[row, 12]] * 10 + dict[elements[row, 13]];

    Console.WriteLine(" - {0}", number);

    totalSum += number;
}


Console.WriteLine("Total sum = {0}", totalSum);




string RemoveAllFrom(string input, string remove)
{
    for (int i = 0; i < remove.Length; i++)
        input=input.Replace(remove[i].ToString(), string.Empty);

    return input;
}

static string SortString(string input)
{
    char[] characters = input.ToArray();
    Array.Sort(characters);
    return new string(characters);
}
