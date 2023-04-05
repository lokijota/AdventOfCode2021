// See https://aka.ms/new-console-template for more information


// read data and fill in the map
using System.Reflection.PortableExecutable;

string[] allCoordinates= File.ReadAllText("data.txt").Split('\n');

int[,] map = new int[1000, 1000];


int dots = 0;
foreach (string coordinate in allCoordinates)
{
    // parse coordinates
    int[] coordinatePair = coordinate.Replace(" -> ", ",").Split(',').Select(x => Convert.ToInt32(x)).ToArray();
    int x1 = coordinatePair[0];
    int y1 = coordinatePair[1];
    int x2 = coordinatePair[2];
    int y2 = coordinatePair[3];

    // draw horizontal and vertical lines
    if ( (x1 == x2) || (y1 == y2))
    {
        if (x1 > x2)
            (x1, x2) = (x2, x1);

        if (y1 > y2)
            (y1, y2) = (y2, y1);

        for (int x = x1; x <= x2; x++)
        {
            for (int y = y1; y <= y2; y++)
            {
                map[x, y]++;
            }
        }
    }
    else
    {
        // process diagonals
        while(x1 != x2 && y1 != y2)
        {
            map[x1, y1]++;
            
            if (x1 < x2) x1++;
            else x1--;

            if (y1 < y2) y1++;
            else y1--;
        }
        map[x1, y1]++;
    }
}

// count overlaps

int count = 0;
int countZeros = 0;
int countOnes = 0;

for (int x = 0; x < 1000; x++)
    for (int y = 0; y < 1000; y++)
        if (map[x, y] > 1)
            count++;
        else if (map[x, y] == 0)
            countZeros++;
        else if (map[x, y] == 1)
            countOnes++;

Console.WriteLine("Count overlaps: {0}, zeros {1}, ones {2}, sum={3}", count, countZeros, countOnes, count+countZeros+countOnes);
