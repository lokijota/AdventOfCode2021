// See https://aka.ms/new-console-template for more information


using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

Console.WriteLine("Day 4 - Part 1");

// read the called numbers
string lineOfNumbers = File.ReadAllText("numbers.txt");
string[] calledNumbersAsStrings = lineOfNumbers.Split(',');
int[] calledNumbers = calledNumbersAsStrings.Select(x => Convert.ToInt32(x)).ToArray();

// read the boards
string[] allBoardNumbers = File.ReadAllText("boards.txt").Replace("\n", " ").Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

List<int[,]> boards = new List<int[,]>();

int[,] current = new int[5, 5];
int rowNumber = 0;
int columnNumber = 0;

foreach (string boardNumber in allBoardNumbers)
{
    current[rowNumber, columnNumber] = Convert.ToInt32(boardNumber);

    if(columnNumber == 4)
    {
        rowNumber++;
        columnNumber = 0;

        // board complete
        if(rowNumber==5)
        {
            boards.Add(current);
            current = new int[5, 5]; // create a new empty board
            rowNumber = 0;
        }

    }
    else
    {
        columnNumber++;
    }
}

//Console.WriteLine(boards.Count());
//PrintBoard(boards[0]);
//PrintBoard(boards[99]);


// call the numbers
bool firstWinnerFound = false;
int[,] lastWinningBoard = null;
int lastWinCalledNumber = -1;

foreach(int calledNumber in calledNumbers)
{
    CallNumber(calledNumber);
    int[,]? winningBoard = BoardWon();

    // Console.WriteLine("Remaining boards: {0}", boards.Count());

    if (winningBoard!=null && !firstWinnerFound)
    {
        int firstResult = SumWinningBoard(winningBoard);
        Console.WriteLine("First Winner found - last number {0}, sum board {1}, multiplication = {2}", calledNumber, firstResult, firstResult * calledNumber);
        firstWinnerFound = true;
    }

    if(winningBoard!=null)
    {
        lastWinningBoard = winningBoard;
        lastWinCalledNumber = calledNumber;

        // remove other boards that also won
        while (BoardWon() != null) ;
    }
}


int result = SumWinningBoard(lastWinningBoard);
Console.WriteLine("Last Winner found - last number {0}, sum board {1}, multiplication = {2}", lastWinCalledNumber, result, result * lastWinCalledNumber);




// Auxiliary Methods
void CallNumber(int number)
{
    foreach(int[,] board in boards)
    {
        for (int row = 0; row < 5; row++)
            for (int column = 0; column < 5; column++)
                if (number == board[row, column])
                    board[row, column] = -1;
    }
}

int[,]? BoardWon()
{
    for(int boardNb = 0; boardNb < boards.Count(); boardNb++)
    {
        int[,] board = boards[boardNb];

        // check for win in rows
        for (int row = 0; row < 5; row++)
        {
            int sumRow = 0;
            for (int column = 0; column < 5; column++)
            {
                sumRow += board[row, column];
            }

            if (sumRow == -5)
            {  // row won
                boards.RemoveAt(boardNb);
                return board;
            }
        }

        // check for win in columns
        for (int column = 0; column< 5; column++)
        {
            int sumColumn= 0;
            for (int row = 0; row < 5; row++)
            {
                sumColumn += board[row, column];
            }

            if (sumColumn == -5)
            {  // column won
                boards.RemoveAt(boardNb);
                return board;
            }

        }
    }

    return null; // no win
}

int SumWinningBoard(int[,] board)
{
    int sum = 0;
    for (int row = 0; row < 5; row++)
    {
        for (int column = 0; column < 5; column++)
        {
            if(board[row, column] > 0)
                sum += board[row, column];
        }
    }

    PrintBoard(board);
    Console.WriteLine("Sum: {0}", sum);
    return sum;
}

void PrintBoard(int[,] board)
{
    Console.WriteLine("--------------");
    for (int row = 0; row < 5; row++)
    {
        Console.WriteLine("{0,2} {1,2} {2,2} {3,2} {4,2}", board[row, 0], board[row, 1], board[row, 2], board[row, 3], board[row, 4]);
    }
    Console.WriteLine("--------------");
}