// See https://aka.ms/new-console-template for more information
using Day21;

Console.WriteLine("Day 21");

// from data.txt
int p1Start = 7;
int p2Start = 4;

Dice d = new Dice();
Game g = new Game(p1Start, p2Start);

//int losingScore = 0;
//bool currentPlayer = true;
//int countRolls = 0;

// part 1
//do
//{
//    int roll = d.Roll();
//    roll += d.Roll();
//    roll += d.Roll();
//    countRolls += 3;

//    losingScore = g.Play(currentPlayer, roll);
//    currentPlayer = !currentPlayer;
//} while (losingScore == 0);

//Console.WriteLine("Rolls: {0}, Losing Score: {1}, Total multiplication: {2} ", countRolls, losingScore, losingScore * countRolls);

Console.WriteLine("*** Starting part 2 ***");

//Queue<Game> bigUniverse = new Queue<Game>();
//bigUniverse.Enqueue(g);

decimal[] wins = new decimal[2]; // 0 - player 1, 1 - player 2


//int[] threeDiceSums = new int[] { 3, 4, 4, 4, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 7, 7, 7, 8, 8, 8, 9 };

//while (bigUniverse.Count > 0)
//{
//    Game[] game = bigUniverse.Dequeue().QuantumSplit(24);

//    for(int i=0; i<24; i++)
//    {
//        int result = game[i].Play(threeDiceSums[i]);
//        if (result > 0)
//            wins[result - 1]++;
//        else
//        {
//            bigUniverse.Enqueue(g);
//        }
//    }

//    //Console.Write("[{0}]", bigUniverse.Count);
//}

//Console.WriteLine("P1 wins: {0}", wins[0]);
//Console.WriteLine("P2 wins: {0}", wins[1]);

// ok, let's try something else: keep an array with the count of configurations

decimal[,,,,] universe = new decimal[10, 10, 31, 31, 2]; 
// why 30? if a player has 20 points, max he can get is 10, so 31 is the max
// note that the loops only process up to <=20

 
// dimensions are:
// - current Position P1
// - current Position P2
// - score P1 (including winning final scores)
// - score P2
// - next player is 1 (value 0) or 2 (value 1)
// >> How many games in this configuration

bool thereAreOpenGames = false;

universe[6, 3, 0, 0, 0] = 1; // 7,4 --> 6,3

int Score(int position)
{
    position++; // because we're using 0-9 in the array, but in the real board it's 1-10 points

    if (position % 10 == 0) return 10;
    else return position % 10;
}

do
{
    for (int currentPlayer = 0; currentPlayer < 2; currentPlayer++)
        for (int posP1 = 0; posP1 < 10; posP1++)
        for (int posP2 = 0; posP2 < 10; posP2++)
            for (int scoreP1 = 0; scoreP1 < 21; scoreP1++)
                for (int scoreP2 = 0; scoreP2 < 21; scoreP2++)
                    {
                        decimal gamesInConfig = universe[posP1, posP2, scoreP1, scoreP2, currentPlayer];

                        if (gamesInConfig == 0)
                            continue;

                        universe[posP1, posP2, scoreP1, scoreP2, currentPlayer] = 0;

                        int nextPlayer = (currentPlayer + 1) % 2;
                        if (currentPlayer == 0)
                        {
                            universe[(posP1 + 3) % 10, posP2, scoreP1 + Score(posP1 + 3), scoreP2, nextPlayer] += 1 * gamesInConfig;
                            universe[(posP1 + 4) % 10, posP2, scoreP1 + Score(posP1 + 4), scoreP2, nextPlayer] += 3 * gamesInConfig;
                            universe[(posP1 + 5) % 10, posP2, scoreP1 + Score(posP1 + 5), scoreP2, nextPlayer] += 6 * gamesInConfig;
                            universe[(posP1 + 6) % 10, posP2, scoreP1 + Score(posP1 + 6), scoreP2, nextPlayer] += 7 * gamesInConfig;
                            universe[(posP1 + 7) % 10, posP2, scoreP1 + Score(posP1 + 7), scoreP2, nextPlayer] += 6 * gamesInConfig;
                            universe[(posP1 + 8) % 10, posP2, scoreP1 + Score(posP1 + 8), scoreP2, nextPlayer] += 3 * gamesInConfig;
                            universe[(posP1 + 9) % 10, posP2, scoreP1 + Score(posP1 + 9), scoreP2, nextPlayer] += 1 * gamesInConfig;
                        }
                        else
                        {
                            universe[posP1, (posP2 + 3) % 10, scoreP1, scoreP2 + Score(posP2 + 3), nextPlayer] += 1 * gamesInConfig;
                            universe[posP1, (posP2 + 4) % 10, scoreP1, scoreP2 + Score(posP2 + 4), nextPlayer] += 3 * gamesInConfig;
                            universe[posP1, (posP2 + 5) % 10, scoreP1, scoreP2 + Score(posP2 + 5), nextPlayer] += 6 * gamesInConfig;
                            universe[posP1, (posP2 + 6) % 10, scoreP1, scoreP2 + Score(posP2 + 6), nextPlayer] += 7 * gamesInConfig;
                            universe[posP1, (posP2 + 7) % 10, scoreP1, scoreP2 + Score(posP2 + 7), nextPlayer] += 6 * gamesInConfig;
                            universe[posP1, (posP2 + 8) % 10, scoreP1, scoreP2 + Score(posP2 + 8), nextPlayer] += 3 * gamesInConfig;
                            universe[posP1, (posP2 + 9) % 10, scoreP1, scoreP2 + Score(posP2 + 9), nextPlayer] += 1 * gamesInConfig;

                        }
                    }

    thereAreOpenGames = false;
    decimal openGames = 0;
    for (int posP1 = 0; posP1 < 10; posP1++)
        for (int posP2 = 0; posP2 < 10; posP2++)
            for (int scoreP1 = 0; scoreP1 <= 20; scoreP1++)
                for (int scoreP2 = 0; scoreP2 <= 20; scoreP2++)
                    for (int nextPlayer = 0; nextPlayer < 2; nextPlayer++)
                    {
                        if (universe[posP1, posP2, scoreP1, scoreP2, nextPlayer] > 0)
                        {
                            openGames += universe[posP1, posP2, scoreP1, scoreP2, nextPlayer];
                            thereAreOpenGames = true;
                            //goto ExitNextedLoops;
                        }
                    }

    Console.WriteLine("# universes: {0}", openGames);
    ExitNextedLoops:
        thereAreOpenGames = thereAreOpenGames;

} while (thereAreOpenGames);

// Count winners -- note that we have to count locations where one won and the other lost
for (int posP1 = 0; posP1 < 10; posP1++)
    for (int posP2 = 0; posP2 < 10; posP2++)
    {
        // count player 0 wins
        for (int scoreP1 = 21; scoreP1 <= 30; scoreP1++) // [21,30]
            for (int scoreP2 = 0; scoreP2 <= 20; scoreP2++) // [0,20]
                wins[0] += universe[posP1, posP2, scoreP1, scoreP2, 1];

        // count player 1 wins
        for (int scoreP1 = 0; scoreP1 <= 20; scoreP1++) // [0-20]
            for (int scoreP2 = 21; scoreP2 <= 30; scoreP2++) // [21-30]
                wins[1] += universe[posP1, posP2, scoreP1, scoreP2, 0];
    }


decimal totalRecords = 0;
for (int posP1 = 0; posP1 < 10; posP1++)
    for (int posP2 = 0; posP2 < 10; posP2++)
        for (int scoreP1 = 0; scoreP1 <= 30; scoreP1++)
            for (int scoreP2 = 0; scoreP2 <= 30; scoreP2++)
                for (int nextPlayer = 0; nextPlayer < 2; nextPlayer++)
                {
                    totalRecords += universe[posP1, posP2, scoreP1, scoreP2, nextPlayer];
                }


Console.WriteLine("P1 wins: {0}", wins[0]);
Console.WriteLine("P2 wins: {0}", wins[1]);
Console.WriteLine("Total wins: {0} = {1} (double check)", wins[0] + wins[1], totalRecords);


Console.ReadLine();

// How many games there are for each outcome
// sum 3: 111 --> 1
// sum 4: 112, 121, 211 --> 3
// sum 5: 122, 212, 221 + 113 131 311 --> 6
// sum 6: 222 + 123 132 213 231 312 321 --> 7
// sum 7: 322 232 223
// sum 8: 332 323 233
// sum 9: 333

// first part: 675024
// second part: 570239341223618