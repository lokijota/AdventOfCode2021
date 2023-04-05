// See https://aka.ms/new-console-template for more information
using Day25;

Console.WriteLine("Day 25");

// read map
string[] mapStrings = File.ReadAllLines("data.txt");
char[,] map = new char[mapStrings.Length, mapStrings[0].Length];

for(int row=0; row<mapStrings.Length; row++)
    for(int col=0; col<mapStrings[row].Length; col++)
    {
        map[row, col] = mapStrings[row][col];
    }

int nSteps = 0;
int nMoves = 0;

do
{
    char[,] newMap = (char[,])map.Clone();
    nMoves = map.MoveEast(newMap);

    map = (char[,])newMap.Clone();
    nMoves += map.MoveSouth(newMap);

    map = newMap;

    nSteps++;
    Console.SetCursorPosition(0, 1);
    Console.WriteLine(nSteps);
}
while (nMoves > 0);

// print map
for(int row=0; row<map.GetLength(0); row++)
    for (int col= 0; col < map.GetLength(1); col++)
    {
        Console.Write(map[row, col]);
        if (col == map.GetLength(1) - 1)
            Console.WriteLine();
    }




        Console.WriteLine("NSteps = {0}", nSteps);
Console.ReadLine();

// part 1 - 295



