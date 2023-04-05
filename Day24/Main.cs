// See https://aka.ms/new-console-template for more information
using Day24;
using System.Diagnostics;

Console.WriteLine("Day 24");
string[] instructions = File.ReadAllLines("data.txt");

AdventProgram p = new AdventProgram();
Console.WriteLine("{0} instructions read.", p.Load(instructions));

Queue<int> queue = new Queue<int>();
long modelNumber = 0;

Stopwatch sw = Stopwatch.StartNew();
var csv = new StreamWriter("output.csv");

long min = long.MaxValue;
Random rnd = new Random(DateTime.Now.Second);

for (modelNumber= 99999999999999; modelNumber> 11111111111111; modelNumber--) 
{
    // change to random execution
    modelNumber = rnd.NextInt64(11111111111111, 99998995286291);

    string input = string.Format("{0:00000000000000}", modelNumber);

    input = input.Remove(7, 2);
    input = input.Insert(7, "31");

    //input = input.Remove(3, 2);
    //input = input.Insert(3, "15");

    if (input.Contains("0"))
        continue;

    queue.Clear();

    Console.SetCursorPosition(0, 0);
    Console.Write(input);

    for (int i=0; i < 14/*input.Length*/; i++)
    {
        queue.Enqueue(input[i] - '0'); // int.Parse(input[i].ToString())); // THIS CAN BE OPTIMIZED 
    }

    try
    {
        p.Run(queue);
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex.Message);
        continue;
    }

    if (p.Z < 100 /*min*/)
    {
        csv.WriteLine(string.Format("{0}, {1}", input, p.Z));
        csv.Flush();
        min = p.Z;
    }

    if (p.Z == 0)
    {
        Console.WriteLine();
        Console.WriteLine("Found model number: {0} with Z=0", input);

        Console.WriteLine("Value of Z = {0}", p.Z);
        Console.WriteLine("Value of X = {0}", p.X);
        Console.WriteLine("Value of Y = {0}", p.Y);
        Console.WriteLine("Value of W = {0}", p.W);

        break;
    }

}

csv.Close();

sw.Stop();
Console.WriteLine("took {0} ms", sw.ElapsedMilliseconds);

//Console.WriteLine("Value of W = {0} for modelNumber={0}", p.W, modelNumber);

Console.ReadLine();

