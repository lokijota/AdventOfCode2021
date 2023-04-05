// See https://aka.ms/new-console-template for more information
using Day20;

Console.WriteLine("Day 20");

string[] rows = File.ReadAllLines("data.txt");

ConversionMap map = new ConversionMap(rows[0]);

TrenchMap tm = new TrenchMap(rows[2..]); // c# ranges --- woo-hooo! :-)

Console.WriteLine("Initial lit pixels: {0}", tm.CountLitPixels());

for(int j=0; j< 50; j++)
{
    tm.EnhanceImage(map);
    Console.WriteLine("Lit pixels after {0} enhancement: {1}", j+1, tm.CountLitPixels());
}

Console.ReadLine();

