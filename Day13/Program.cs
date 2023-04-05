Console.WriteLine("***** DAY 12 *****");

// eg, fold along x=40
string[] foldRows = File.ReadAllText("folds.txt").Replace("\r", "").Replace("fold along ", "").Split('\n');
List<string[]> folds = foldRows.Select(foldRow => foldRow.Split('=')).ToList();
Console.WriteLine("Folds: {0}", folds.Count());

// read the dots and put in the map
string[] dotRows = File.ReadAllLines("dots.txt");
bool[,] thermalManual = new bool[900, 1320];

int maxCol = 0, maxRow = 0;

foreach (string dotRow in dotRows)
{
    string[] coords = dotRow.Split(',');
    int col = Convert.ToInt32(coords[0]);
    int row = Convert.ToInt32(coords[1]);

    thermalManual[row, col] = true;

    if (col > maxCol) maxCol = col;
    if (row > maxRow) maxRow = row;
}

// Data read, dots=799, maxX=1310, maxY=894
Console.WriteLine("Data read, dots={0}, maxX={1}, maxY={2}", dotRows.Length, maxCol, maxRow);

Console.WriteLine("Dots before one fold: {0}", CountDots());
// DoFold("x", 655);
// Console.WriteLine("Dots after one fold: {0}", CountDots());

for (int j = 0; j < folds.Count(); j++)
{
    DoFold(folds[j][0], Convert.ToInt32(folds[j][1]));
}

Console.Write("maxRow={0}, maxCol={1}", maxRow, maxCol);
PrintThermalManual();


void DoFold(string axis, int axisPosition)
{
    // x = columns, y = rows

    if (axis == "x")
    {
        int colsToCopy = axisPosition;
        if (colsToCopy > maxCol - axisPosition) colsToCopy = maxCol - axisPosition;

        for (int j = 1; j <= colsToCopy; j++)
        {
            for (int y = 0; y <= maxRow; y++)
            {
                thermalManual[y, axisPosition - j] |= thermalManual[y, axisPosition + j];
                thermalManual[y, axisPosition + j] = false; // just to allow later counting

            }
        }
        maxCol = axisPosition;
    }
    else
    {
        int rowsToCopy = axisPosition;
        if (rowsToCopy > maxRow - axisPosition) rowsToCopy = maxRow - axisPosition;

        for (int j = 1; j <= rowsToCopy; j++)
        {
            for (int x = 0; x <= maxCol; x++)
            {
                thermalManual[axisPosition - j, x] |= thermalManual[axisPosition + j, x];
                thermalManual[axisPosition + j, x] = false; // just to allow later counting
            }
        }
        maxRow = axisPosition;
    }
}

int CountDots()
{
    int count = 0;
    for (int x = 0; x <= maxCol; x++)
        for (int y = 0; y <= maxRow; y++)
            if (thermalManual[y,x])
                count++;

    return count;
}

// EFJKZLBL
// mas falta a top row por algum motivo
void PrintThermalManual()
{
    Console.WriteLine(System.Environment.NewLine);
    for (int row = 0; row < maxRow; row++)
    {
        for (int col = 0; col < maxCol; col++)
            if (thermalManual[row, col])
                Console.Write("x");
            else
                Console.Write(" ");
        Console.WriteLine();
    }
}