// See https://aka.ms/new-console-template for more information
Console.WriteLine("Day 7");

string[] crabPositionsStr = File.ReadAllText("data.txt").Split(',');
int[] crabPositions = crabPositionsStr.Select(x => int.Parse(x)).ToArray();

int averagePosition = (int) Math.Round(crabPositions.Average());

Console.WriteLine("Average position is {0}", averagePosition);
int min = int.MaxValue;

int minPos = int.MaxValue;
int maxPos = 0;
foreach (int pos in crabPositions)
{
    if (pos < minPos)
        minPos = pos;

    if (pos > maxPos)
        maxPos = pos;
}


for (int position = minPos; position < maxPos; position++)
{
    int costToPosition = CostToPosition(position, crabPositions);
    Console.WriteLine("Cost to position {0} is {1}", position, costToPosition);

    if(costToPosition < min)
    {
        min = costToPosition;
    }
}


Console.WriteLine("Minimum cost is {0}", min);

// Part 1 - 348664
// Part 2 - 100220525

Console.ReadLine();


int CostToPosition(int targetPosition, int[] crabPositions)
{
    int totalDistance = 0;
    foreach (int crabPosition in crabPositions)
    {

        // Solution for part 2:
        int crabDistance = Math.Abs(crabPosition - targetPosition);

        // https://math.stackexchange.com/questions/60578/what-is-the-term-for-a-factorial-type-operation-but-with-summation-instead-of-p
        totalDistance += (crabDistance * crabDistance + crabDistance) / 2;
    }

    return totalDistance;
}