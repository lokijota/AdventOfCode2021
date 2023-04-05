// See https://aka.ms/new-console-template for more information
Console.WriteLine("Day 16");

string[] rows = File.ReadAllLines("data.txt");

int SIZE = rows.Length;
int[,] energyLevels = new int[SIZE, SIZE];
bool[,] hasFlashed = new bool[SIZE, SIZE];


// convert to integers
for (int row=0; row<SIZE; row++)
    for(int col=0; col<SIZE; col++)
    {
        energyLevels[row, col] = (int)rows[row][col] - (int)'0';
    }

// now start the loop

int flashCount = 0;
for (int step = 0; step < 1000; step++)
{
    ResetFlashes();
    EnergyLevelRise();

    bool areThereFlashes;
    do
    {
        areThereFlashes = false;
        
        for (int row = 0; row < SIZE; row++)
            for (int col = 0; col < SIZE; col++)
                if (energyLevels[row, col] > 9 && !hasFlashed[row, col])
                {
                    areThereFlashes = true;
                    flashCount++;
                    hasFlashed[row, col] = true;
                    energyLevels[row, col] = 0;

                    EnergyLevelRiseAdjacent(row, col);

                }
    } while(areThereFlashes);

    //for (int row = 0; row < SIZE; row++)
    //    for (int col = 0; col < SIZE; col++)
    //        if(energyLevels[row, col] > 9)
    //            energyLevels[row, col] = 0;

    if(SimultaneousFlash())
    {
        Console.WriteLine("Simultaneous flash, step={0}", step+1); // part 2 - 354
        break;
    }
}

Console.WriteLine("Flash count: {0}", flashCount); // part 1 - 1785
Print();
Console.ReadLine();

bool SimultaneousFlash()
{
    for (int row = 0; row < SIZE; row++)
        for (int col = 0; col < SIZE; col++)
            if (hasFlashed[row, col] == false)
                return false;

    return true;
}

void EnergyLevelRiseAdjacent(int row, int col)
{
    List<Tuple<int, int>> allAdjacent = new List<Tuple<int, int>>();

    // add all adjacent even if they fall outside the grid
    allAdjacent.Add(new Tuple<int, int>(row-1, col-1));
    allAdjacent.Add(new Tuple<int, int>(row-1, col));
    allAdjacent.Add(new Tuple<int, int>(row-1, col+1));

    allAdjacent.Add(new Tuple<int, int>(row, col-1));
    allAdjacent.Add(new Tuple<int, int>(row, col+1));

    allAdjacent.Add(new Tuple<int, int>(row+1, col-1));
    allAdjacent.Add(new Tuple<int, int>(row+1, col));
    allAdjacent.Add(new Tuple<int, int>(row+1, col+1));

    // process just the coordinates that are valid
    foreach(Tuple<int,int> coords in allAdjacent)
    {
        if (coords.Item1 >= 0 && coords.Item1 < SIZE && coords.Item2 >= 0 && coords.Item2 < SIZE && !hasFlashed[coords.Item1, coords.Item2])
            energyLevels[coords.Item1, coords.Item2]++;
    }
}


void EnergyLevelRise()
{
    for (int row = 0; row < SIZE; row++)
        for (int col = 0; col < SIZE; col++)
        {
            energyLevels[row, col]++;
        }
}

void ResetFlashes()
{
    for (int row = 0; row < SIZE; row++)
        for (int col = 0; col < SIZE; col++)
            hasFlashed[row,col] = false;
}

void Print()
{
    for (int row = 0; row < SIZE; row++)
    {
        for (int col = 0; col < SIZE; col++)
            Console.Write(energyLevels[row, col]);
        Console.WriteLine();
    }
}