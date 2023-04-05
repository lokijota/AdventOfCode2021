using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day21
{
    public class Game
    {
        public int P1Position { get; set; }
        public int P2Position { get; set; }

        public int P1Score { get; set; }
        public int P2Score { get; set; }

        public int NextPlayer { get; set; }

        public Game(int p1, int p2)
        {
            P1Position = p1;
            P2Position = p2;
            NextPlayer = 1;
        }

        public Game[] QuantumSplit(int n)
        {
            Game[] newGames = new Game[n];

            for(int i = 0;i<n; i++)
            {
                newGames[i] = new Game(P1Position, P2Position);
                newGames[i].P1Score = P1Score;
                newGames[i].P2Score = P2Score;
                newGames[i].NextPlayer = NextPlayer;
            }

            return newGames;
        }

        public int Play(int diceRoll)
        {

            int newPosition = NextPlayer == 1 ? P1Position + diceRoll : P2Position + diceRoll;

            if (newPosition % 10 == 0) 
                newPosition = 10;
            else
                newPosition = newPosition % 10;

            if(NextPlayer == 1)
            {
                P1Position = newPosition;
                P1Score += newPosition;
                if (P1Score >= 21)
                    return 1;

                NextPlayer = 2;
            }
            else
            {
                P2Position = newPosition;
                P2Score += newPosition;

                if (P2Score >= 21)
                    return 2;

                NextPlayer = 1;
            }
            return 0; // game continuing, no winner
        }
    }
}
