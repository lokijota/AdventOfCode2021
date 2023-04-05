// See https://aka.ms/new-console-template for more information
Console.WriteLine("Day 17");

// target area: x = 241..273, y = -97..-63

int targetXmin = 241;
int targetXmax = 273;
int targetYmin = -97;
int targetYmax = -63;

//int targetXmin = 20;
//int targetXmax = 30;
//int targetYmin = -10;
//int targetYmax = -5;


var startPosition = Tuple.Create(0, 0);

int maxHeight = Int32.MinValue;
int recordMaxHeight = Int32.MinValue;
int countTotalHits = 0;

for(int xVelocityStart = 1; xVelocityStart < 274; xVelocityStart++)
{
    for(int yVelocityStart=-97; yVelocityStart < 1000; yVelocityStart++)
    {

        // this tries the initial velocity
        int currentXvelocity = xVelocityStart;
        int currentYvelocity = yVelocityStart;
        var position = Tuple.Create(0, 0);
        maxHeight = Int32.MinValue;
        int steps = 0;

        do
        {
            position = RunStep(position, ref currentXvelocity, ref currentYvelocity);
            if(position.Item2 > maxHeight)
                maxHeight = position.Item2;
            
            steps++;

        } while (!IsInTargetArea(position) && !IsPastTargetArea(position));

        if (IsInTargetArea(position))
        {
            Console.WriteLine("For initial velocity ({0},{1}) hit max height of {2} and reached final box at ({3},{4}) after {5} steps", xVelocityStart, yVelocityStart, maxHeight, position.Item1, position.Item2, steps);
            countTotalHits++;

            if (maxHeight > recordMaxHeight)
            {
                recordMaxHeight = maxHeight;
                Console.WriteLine("  *** NEW RECORD HEIGHT IS NOW {0} ***", recordMaxHeight);
            }
        }
    }
}

Console.WriteLine("Num of shots that hit target: {0}", countTotalHits);

/*
- The probe's x position increases by its x velocity.
- The probe's y position increases by its y velocity.
- Due to drag, the probe's x velocity changes by 1 toward the value 0; that is, it decreases by 1 if it is greater than 0,
  increases by 1 if it is less than 0, or does not change if it is already 0.
- Due to gravity, the probe's y velocity decreases by 1.
*/
Tuple<int, int> RunStep(Tuple<int,int> position, ref int xVelocity, ref int yVelocity)
{
    var newPosition = Tuple.Create(position.Item1 + xVelocity, position.Item2 + yVelocity);

    if (xVelocity > 0)
        xVelocity--;
    else if (xVelocity < 0)
        xVelocity++;

    yVelocity--;

    return newPosition;
}

bool IsInTargetArea(Tuple<int,int> position)
{
    return position.Item1 >= targetXmin && position.Item1 <= targetXmax && position.Item2 >= targetYmin && position.Item2 <= targetYmax;
}

bool IsPastTargetArea(Tuple<int,int> position)
{
    return position.Item1 > targetXmax || position.Item2 < targetYmin;
}
