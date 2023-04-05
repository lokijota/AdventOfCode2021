using System.Text;

// read the data
string[] dataRows = File.ReadAllLines("data.txt");
string polymerTemplate = dataRows[0];

Dictionary<string, char> insertionRules = new Dictionary<string, char>();

// start in row 2
for (int j = 2; j < dataRows.Length; j++)
{
    string[] insertionRule = dataRows[j].Replace(" -> ", "-").Split('-');
    insertionRules.Add(insertionRule[0], insertionRule[1][0]);
}

// this code assumes no pair repeat, which is the case
Dictionary<string, decimal> pairCounts = new Dictionary<string, decimal>();
for(int j=0; j<polymerTemplate.Length-1; j++)
{
    pairCounts.Add(string.Concat(polymerTemplate[j], polymerTemplate[j + 1]), 1);
}

Console.WriteLine("Template is: {0}", polymerTemplate);
Console.WriteLine("# of Insertion Rules: {0}", insertionRules.Count());
Console.WriteLine("Initial pair count: {0}", pairCounts.Count());

// do the generation loop
for(int step=0; step<40; step++)
{
    pairCounts = DoGeneration(pairCounts);

    decimal totalCount = 0;
    foreach (KeyValuePair<string, decimal> somePairCount in pairCounts)
    {
        totalCount += (decimal)somePairCount.Value;
    }

    Console.WriteLine("Step: {0}, entries: {1}, total count of pairs: {2}", step, pairCounts.Count(), totalCount);
}


decimal[] frequencyCounts = new decimal[26];
foreach (KeyValuePair<string, decimal> somePairCount in pairCounts)
{
    frequencyCounts[(int)somePairCount.Key[0] - (int)'A'] += somePairCount.Value;
    //frequencyCounts[(int)somePairCount.Key[1] - (int)'A'] += somePairCount.Value; 
}

frequencyCounts[(int)'V' - (int)'A']++;

for (int theChar = 0; theChar < 26; theChar++)
{
    Console.WriteLine("Char '{0}' appears {1} times", ((char)(theChar + (int)'A')).ToString(), frequencyCounts[theChar]);
}

decimal max = 0;
int maxChar = ' ';

decimal min = decimal.MaxValue;
int minChar = ' ';

for(char c = 'A'; c<= 'Z'; c++)
{
    int index = (int)c - (int)'A';

    if (frequencyCounts[index] == 0)
        continue;

    if (frequencyCounts[index] > max)
    {
        max = frequencyCounts[index];
        maxChar = c;
    }

    if (frequencyCounts[index] < min)
    {
        min = frequencyCounts[index];
        minChar = c;
    }
}

Console.WriteLine("Most frequent is '{0}', {1} times", (char)maxChar, max);
Console.WriteLine("Less frequent is '{0}', {1} times", (char)minChar, min);
Console.WriteLine("Difference is {0}", max - min);

// 2422238515435 is too low -- 39 steps
// 4844889522565 is too high - 40 steps

Dictionary<string, decimal> DoGeneration(Dictionary<string, decimal> pairCounts)
{
    Dictionary<string, decimal> newPairCounts = new Dictionary<string, decimal>();

    foreach(KeyValuePair<string, decimal> somePairCount in pairCounts)
    {

        // is there a generation rule for the pair?
        //if (insertionRules.ContainsKey(somePairCount.Key))
        //{
            // get the replacement character
            char c = insertionRules[somePairCount.Key];

            // AB -> C => AC + 
            // this is about AB
            string newFirstPair = string.Concat(somePairCount.Key[0], c);
            if (newPairCounts.ContainsKey(newFirstPair))
            {
                newPairCounts[newFirstPair] += somePairCount.Value;
            }
            else
            {
                newPairCounts[newFirstPair] = somePairCount.Value;
            }

            // this is for CD
            string newSecondPair = string.Concat(c, somePairCount.Key[1]);
            if (newPairCounts.ContainsKey(newSecondPair))
            {
                newPairCounts[newSecondPair] += somePairCount.Value;
            }
            else
            {
                newPairCounts[newSecondPair] = somePairCount.Value;
            }
        //}
        //else
        //{
        //    // pair is staying unchanged, AB leads to AB, so just have to add quantities
        //    if (newPairCounts.ContainsKey(somePairCount.Key))
        //    {
        //        newPairCounts[somePairCount.Key] += somePairCount.Value;
        //    }
        //    else
        //    {
        //        newPairCounts[somePairCount.Key] = somePairCount.Value;
        //    }
        //}
    }

    return newPairCounts;
}
