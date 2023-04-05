// See https://aka.ms/new-console-template for more information
using Day18;

Console.WriteLine("Day 18");

string[] rows = File.ReadAllLines("data.txt");

//// [[[[[9,8],1],2],3],4] becomes [[[[0,9],2],3],4] (the 9 has no regular number to its left, so it is not added to any regular number).
//SnailNumber sn = SnailNumber.Parse("[[[[[9,8],1],2],3],4]");
//sn.Print(); Console.WriteLine();
//SnailNumber snReduced = SnailNumber.Reduce(sn);
//snReduced.Print();
//Console.WriteLine("\n---------------------------");

////[7,[6,[5,[4,[3,2]]]]] becomes[7,[6,[5,[7, 0]]]](the 2 has no regular number to its right, and so it is not added to any regular number).
//sn = SnailNumber.Parse("[7,[6,[5,[4,[3,2]]]]]");
//sn.Print(); Console.WriteLine();
//snReduced = SnailNumber.Reduce(sn);
//snReduced.Print();
//Console.WriteLine("\n---------------------------");

////[[6,[5,[4,[3,2]]]],1] becomes[[6,[5,[7,0]]],3].
//sn = SnailNumber.Parse("[[6,[5,[4,[3,2]]]],1]");
//sn.Print(); Console.WriteLine();
//snReduced = SnailNumber.Reduce(sn);
//snReduced.Print();
//Console.WriteLine("\n---------------------------");

////[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]] becomes [[3,[2,[8, 0]]],[9,[5,[4,[3,2]]]]]
////(the pair[3, 2] is unaffected because the pair[7, 3] is further to the left;[3,2] would explode on the next action).
//sn = SnailNumber.Parse("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]");
//sn.Print(); Console.WriteLine();
//snReduced = SnailNumber.Reduce(sn);
//snReduced.Print();
//Console.WriteLine("\n---------------------------");

////[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]] becomes[[3,[2,[8,0]]],[9,[5,[7,0]]]].
//sn = SnailNumber.Parse("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]");
//sn.Print(); Console.WriteLine();
//snReduced = SnailNumber.Reduce(sn);
//snReduced.Print();
//Console.WriteLine("\n---------------------------");

//SnailNumber snLeft = SnailNumber.Parse("[[[[4,3],4],4],[7,[[8,4],9]]]");
//SnailNumber snRight = SnailNumber.Parse("[1,1]");
//SnailNumber sum = SnailNumber.Add(snLeft, snRight);
//snReduced.Print();
//Console.WriteLine("\n---------------------------");



//SnailNumber unittest = SnailNumber.Parse("[[[[9,0],10],[[0,7],[7,13]]],[[[0,2],[13,0]],[[[16,0],[9,8]],5]]]");
//unittest.Print(); Console.WriteLine();
//SnailNumber.Reduce(unittest);

//SnailNumber unittest = SnailNumber.Parse("[ [[[7,7],[7,8]],[[9,5],[8,0]]] , [[[9,10],20],[8,[9,0]]] ]");
//unittest.Print(); Console.WriteLine();
//SnailNumber.Reduce(unittest);



//SnailNumber magnitudeTest = SnailNumber.Parse("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]");
//magnitudeTest.Print(); Console.WriteLine();
//Console.WriteLine("Magnitude is: {0}", magnitudeTest.CalculateMagnitude());



Console.WriteLine("\n------------------------------------------------------");
Console.WriteLine("\nPARSNAILS");

// Exercise 1
//SnailNumber exercise1 = SnailNumber.Parse(rows[0]);

//for (int j = 1; j < rows.Length; j++)
//{
//    SnailNumber toAdd = SnailNumber.Parse(rows[j]);

//    Console.WriteLine("------ ADDING SNAILNUMBER {0} ------", j);
//    exercise1 = SnailNumber.Add(exercise1, toAdd);
//    exercise1.Print(); Console.WriteLine();
//}

//Console.WriteLine("Final Sum:");
//exercise1.Print(); Console.WriteLine();

//Console.WriteLine("Magnitude: {0}", exercise1.CalculateMagnitude());

int maxMagnitude = 0;
for (int left = 0; left < rows.Length; left++)
{
    for (int right = 0; right < rows.Length; right++)
    {
        SnailNumber leftNumber = SnailNumber.Parse(rows[left]);
        SnailNumber rightNumber = SnailNumber.Parse(rows[right]);
        var sum = SnailNumber.Add(leftNumber, rightNumber);
        var magnitude = sum.CalculateMagnitude();

        if (magnitude > maxMagnitude)
            maxMagnitude = magnitude;
    }
    Console.Write(".");
}

Console.WriteLine("\nMax Magnitude: {0}", maxMagnitude); // 4731 for part 2


Console.ReadLine();
