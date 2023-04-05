// See https://aka.ms/new-console-template for more information
using Day23;

Console.WriteLine("Day 24");

string[] rowsMap = File.ReadAllLines("data.txt");
string[] rowsMapConfig = File.ReadAllLines("dataLayer.txt");

Game g = new Game(rowsMap, rowsMapConfig);

g.GeneratePossibleMoves();

Console.ReadLine();
