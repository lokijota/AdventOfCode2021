// See https://aka.ms/new-console-template for more information
using Day16;

Console.WriteLine("Day 16");

string hexstring = File.ReadAllText("data.txt");

string binarystring = String.Join(String.Empty,
  hexstring.Select(
    c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
  )
);

Console.WriteLine(binarystring.Substring(0, 30));

Transmission t = new Transmission();
Console.WriteLine("Result: {0}", t.Read(binarystring));

Console.WriteLine("Version sum: {0}", t.VersionSum);

Console.ReadLine();


