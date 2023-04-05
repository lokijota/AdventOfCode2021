// See https://aka.ms/new-console-template for more information
Console.WriteLine("Day 10");

string[] rows = File.ReadAllLines("data.txt");
List<string> nonCorruptRows = new List<string>();

Stack<char> pilha = new Stack<char>();

int errorScore = 0;
bool isCorruptRow = false;

// this is the first part of the exercise
foreach (string row in rows)
{
    pilha.Clear();

    for(int j=0; j<row.Length; j++)
    {
        if(IsOpen(row[j]))
            pilha.Push(row[j]);
        else
        {
            if (MatchOpenClose(pilha.Peek(), row[j]))
                pilha.Pop();
            else {
                errorScore += GetPoints(row[j]);
                isCorruptRow = true;
                break;
            }
        }
    }

    if (!isCorruptRow)
        nonCorruptRows.Add(row);

    isCorruptRow = false;
}

// this is the second part
List<decimal> incompleteRowScores = new List<decimal>();
decimal incompleteRowScore = 0;
foreach (string row in nonCorruptRows)
{
    pilha.Clear();
    incompleteRowScore = 0;

    for (int j = 0; j < row.Length; j++)
    {
        if (IsOpen(row[j]))
            pilha.Push(row[j]);
        else
            pilha.Pop();
    }

    // now we should have in the stack a set of un-closed things, let's process it
    char c;
    while(pilha.TryPeek(out c))
    {
        incompleteRowScore = incompleteRowScore * 5 + GetPointsIncompleteRows(c);
        pilha.Pop();
    }
    incompleteRowScores.Add(incompleteRowScore);
}

incompleteRowScores.Sort();

Console.WriteLine("Total error score: {0}", errorScore);
Console.WriteLine("Total error score incomplete rows: {0}", incompleteRowScores[incompleteRowScores.Count()/2]); // 3049320156
Console.ReadLine();

// part 1 - 339477

int GetPoints(char c)
{
    if (c == ')') return 3;
    if (c == ']') return 57;
    if (c == '}') return 1197;
    if (c == '>') return 25137;

    throw new ApplicationException("Unexpected character received, bam!");
}

int GetPointsIncompleteRows(char c)
{
    if (c == '(') return 1;
    if (c == '[') return 2;
    if (c == '{') return 3;
    if (c == '<') return 4;

    throw new ApplicationException("Unexpected character received, bam!");
}


bool IsOpen(char c)
{
    if (c == '(' || c == '{' || c == '<' || c == '[')
        return true;

    return false;
}

bool MatchOpenClose(char open, char close)
{
    if (open == '(' && close == ')')
        return true;

    if (open == '{' && close == '}')
        return true;

    if (open == '[' && close == ']')
        return true;

    if (open == '<' && close == '>')
        return true;

    return false;
}