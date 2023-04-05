// See https://aka.ms/new-console-template for more information
Console.WriteLine("Day 6");

string[] initialAges = File.ReadAllText("data.txt").Split(',');

// enter records for all ages
decimal[] countPerAges = new decimal[9];

foreach(string age in initialAges)
{
    countPerAges[int.Parse(age)]++;
}

for(int days=0; days<256; days++)
{
    decimal[] newAges = new decimal[9];

    for(int age=0; age<9; age++)
    {
        // reproduction
        if (age == 0) {
            newAges[8] = countPerAges[0];
            newAges[6] = countPerAges[0];
        }
        else
        {
            newAges[age - 1] += countPerAges[age];
        }
    }

    countPerAges = newAges;
}

decimal totalFish = 0;
for(int age=0; age<9; age++)
{
    totalFish += countPerAges[age];
}


Console.WriteLine("Total fish: {0}", totalFish);

Console.ReadLine();

// 1632146183902 for 256 days