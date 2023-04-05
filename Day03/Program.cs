// See https://aka.ms/new-console-template for more information
using System.ComponentModel;
using System.Runtime.InteropServices;

Console.WriteLine("Hello, World!");

int gamma = 0;
int epsilon = 0;

string[] rows = File.ReadAllLines("data.txt");

// Console.WriteLine(rows[0]);
// Console.WriteLine(Convert.ToInt32(rows[0], 2));

for(int bit=0; bit<12; bit++)
{
    int count0 = 0;
    int count1 = 0;

    for(int i=0; i<rows.Length; i++)
    {
        if (rows[i][bit] == '0')
            count0++;
        else
            count1++;
    }

    gamma <<= 1;
    epsilon <<= 1;

    if (count0 > count1) // most common is 0
    {
        // add 0 at rightmost position of gamma
        // add 1 at rightmost position of epsilon
        epsilon |= 1;

    }
    else // most common is 1
    {
        // add 1 at rightmost position of gamma
        gamma |= 1;
        // add 0 at rightmost position of epsilon
    }

}

Console.WriteLine("Part 1 - gamma={0}, epsilon={1}, gamma x epsilon={2}", gamma, epsilon, gamma * epsilon);
// Console.WriteLine(Convert.ToString(gamma, 2));
// Console.WriteLine(Convert.ToString(epsilon, 2));

List<string> currentList = rows.ToList<string>();

// oxygen generator
for (int bit = 0; bit < 12; bit++)
{
    List<string> zerosAtBitPosition = new List<string>();
    List<string> onesAtBitPosition = new List<string>();

    for (int i = 0; i < currentList.Count(); i++)
    {
        if (currentList[i][bit] == '0')
            zerosAtBitPosition.Add(currentList[i]);
        else
            onesAtBitPosition.Add(currentList[i]);
    }

    if(onesAtBitPosition.Count() >= zerosAtBitPosition.Count())
    {
        currentList = onesAtBitPosition;
    }
    else
    {
        currentList = zerosAtBitPosition;
    }

    if (currentList.Count() == 1)
        break;
}

int ox = Convert.ToInt32(currentList[0], 2);

// co2
currentList = rows.ToList<string>();

for (int bit = 0; bit < 12; bit++)
{
    List<string> zerosAtBitPosition = new List<string>();
    List<string> onesAtBitPosition = new List<string>();

    for (int i = 0; i < currentList.Count(); i++)
    {
        if (currentList[i][bit] == '0')
            zerosAtBitPosition.Add(currentList[i]);
        else
            onesAtBitPosition.Add(currentList[i]);
    }

    if (onesAtBitPosition.Count() >= zerosAtBitPosition.Count())
    {
        currentList = zerosAtBitPosition;
    }
    else
    {
        currentList = onesAtBitPosition;
    }

    if (currentList.Count() == 1)
        break;
}

int co2 = Convert.ToInt32(currentList[0], 2);

Console.WriteLine("Oxygen generator rating = {0}, CO2 = {1}, ox * co2 = {2}", ox, co2, ox * co2);

