// See https://aka.ms/new-console-template for more information
Console.WriteLine("Day 9");

// Read the data

string[] mapAsChar = File.ReadAllLines("data.txt");

int[,] map = new int[mapAsChar.Length, mapAsChar.Length];
char[,] basin = new char[mapAsChar.Length, mapAsChar.Length];


for (int r = 0; r < mapAsChar.Length; r++)
{
    for (int c = 0; c < mapAsChar.Length; c++)
    {
        map[r, c] = (int)mapAsChar[r][c] - (int)'0';
        basin[r,c] = ' ';
    }
}

// look for the minima

int sum = 0;
List<int> basins = new List<int>();

for(int r=0; r<mapAsChar.Length; r++)
{
    for(int c = 0; c< mapAsChar.Length; c++)
    {
        if (IsLocalMinima(r, c))
        {
            sum += map[r, c] + 1; // for part 1

            int basinSize = FindBasin(r, c);
            basins.Add(basinSize);
        }
    }
}

DrawBasinMap();

basins = basins.OrderByDescending(i => i).ToList();

Console.WriteLine("Mult of 3 largest: {0} x {1} x {2} = {3}", basins[0], basins[1], basins[2], basins[0] * basins[1] * basins[2]);


Console.WriteLine("Sum of minima = {0}", sum);
Console.ReadLine();

void DrawBasinMap()
{
    for (int r = 0; r < mapAsChar.Length; r++)
    {
        for (int c = 0; c < mapAsChar.Length; c++)
        {
            Console.Write(basin[r, c]);
        }
        Console.WriteLine();
    }
}


int FindBasin(int centerRow, int centerColumn)
{
    // check if point was already visited
    if (basin[centerRow, centerColumn] == 'x')
        return 0;

    // found a wall, stop
    if (map[centerRow, centerColumn] == 9)
        return 0;

    // else, continue recursion

    // mark as visited
    basin[centerRow, centerColumn] = 'x';

    // now generate adjacent points
    List<Tuple<int, int>> adjacentPoints = new List<Tuple<int, int>>();

    if(centerRow > 0)
        adjacentPoints.Add(new Tuple<int, int>(centerRow - 1, centerColumn));

    if(centerRow < mapAsChar.Length-1)
        adjacentPoints.Add(new Tuple<int, int>(centerRow + 1, centerColumn));

    if(centerColumn > 0)
        adjacentPoints.Add(new Tuple<int, int>(centerRow, centerColumn - 1));

    if(centerColumn < mapAsChar.Length-1)
        adjacentPoints.Add(new Tuple<int, int>(centerRow, centerColumn + 1));

    // check if the points a local minima excluding the original point

    int sum = 1;
    foreach(Tuple<int,int> point in adjacentPoints)
    {
        sum += FindBasin(point.Item1, point.Item2);
    }

    return sum;

}






bool IsLocalMinima(int row, int col)
{
    // top left corner
    if(row==0 && col == 0)
    {
        if (map[0, 0] < map[1, 0] &&
            map[0, 0] < map[0, 1])
            return true;
        else
            return false;
    }

    // top right corner
    if (row == 0 && col == mapAsChar.Length-1)
    {
        if (map[row, col] < map[row,  col-1] &&
            map[row, col] < map[row+1, col])
            return true;
        else
            return false;
    }

    // bottom left corner
    if (row == mapAsChar.Length - 1 && col == 0)
    {
        if (map[row, col] < map[row-1, col] &&
            map[row, col] < map[row, col+1])
            return true;
        else
            return false;
    }

    // botton right corner
    if (row == mapAsChar.Length - 1 && col == mapAsChar.Length - 1)
    {
        if (map[row, col] < map[row-1, col] &&
            map[row, col] < map[row, col-1])
            return true;
        else
            return false;
    }

    // top edge
    if (row == 0)
        if (map[row, col] < map[row, col-1] &&
            map[row, col] < map[row, col+1] &&
            map[row, col] < map[row+1, col])
            return true;
        else
            return false;

    // left edge
    if (col == 0)
        if (map[row, col] < map[row-1, col] &&
            map[row, col] < map[row+1, col] &&
            map[row, col] < map[row, col+1])
            return true;
        else
            return false;

    // botttom edge
    if (row == mapAsChar.Length - 1)
        if (map[row, col] < map[row, col - 1] &&
            map[row, col] < map[row, col + 1] &&
            map[row, col] < map[row-1, col])
            return true;
        else
            return false;

    // right edge
    if (col == mapAsChar.Length - 1)
        if (map[row, col] < map[row - 1, col] &&
            map[row, col] < map[row + 1, col] &&
            map[row, col] < map[row, col -1])
            return true;
        else
            return false;


    if (map[row, col] < map[row+1, col] &&
        map[row, col] < map[row-1, col] &&
        map[row, col] < map[row, col-1] &&
        map[row, col] < map[row, col+1])
        return true;


    return false;
}

