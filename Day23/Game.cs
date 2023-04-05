using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Day23
{
    public class Game
    {
        Map _map = null;
        List<Amphipod> _players = null;
        const int PARKING_BOTTOM_ROW = 5;

        public Game(string[] map, string[] mapLayer)
        {
            _map = new Map(map, mapLayer, out _players);
            _map.Print();
        }

        public void GeneratePossibleMoves()
        {
            MovementPlanner mp = new MovementPlanner();

            // Generate all possible first moves
            List<List<FullMoveStep>> alternativePaths = new List<List<FullMoveStep>>();

            foreach(Amphipod player in _players)
            {
                alternativePaths.AddRange(mp.PlanMoves(_map, player));
            }

            // first generation paths, put the cheaper first
            //alternativePaths = alternativePaths.OrderBy(x => x.Count).ToList();

            //List<int> endingScores = new List<int>();
            int spentEnergy= 0;
            int bestScore = Int32.MaxValue;
            int pathsExplored = 0;

            while(alternativePaths.Count > 0)
            {
                //Console.WriteLine("Number of paths to explore: {0}, len first: {1}, pathsEplored: {2}", alternativePaths.Count, alternativePaths[0].Count, pathsExplored++);

                // 01. Reset the map and players to the initial position
                spentEnergy = 0;
                _map.Reset();
                foreach(Amphipod amph in _players)
                {
                    amph.Reset();
                }

                // 02.Process the first path in the list - update the map and make the moves
                List<FullMoveStep> currentPath = alternativePaths[0];
                Amphipod currentPlayer = currentPath[0].player;
                currentPlayer.Steps++;

                foreach (FullMoveStep move in currentPath)
                {
                    // 02.01 move the player
                    
                    // increase the # of steps, which influences the subsequent planning 
                    if(move.player != currentPlayer)
                    {
                        currentPlayer = move.player;
                        currentPlayer.Steps++;
                    }
                    
                    move.player.Move(move.direction);
                    spentEnergy += move.player.EnergySpendPerMove;

                    // 02.02 update the map
                    _map.Move(move.previousRow, move.previousColumn, move.newRow, move.newColumn);
                }

                //_map.Print();
                //Console.ReadLine();

                // 03. Check if we're at the end of the game
                if (_map.GameOver())
                {
                    //Console.WriteLine("Game Over, score = {0}!", spentEnergy);

                    //_map.Print();
                    //Console.ReadLine();

                    // 03.01. Yes we are, so just record the best score if appropriate
                    if (spentEnergy < bestScore)
                    {
                        //endingScores.Add(spentEnergy);
                        bestScore = spentEnergy;
                        Console.WriteLine("Found new best score: {0}", bestScore);
                        //Console.ReadLine();
                    }

                    // we're done with this one, remove it
                    alternativePaths.RemoveAt(0);
                }
                else
                {
                    // 03.02. No we are not, so generate new paths to add to the ones in the current queue
                    List<List<FullMoveStep>> nextPossibleMovements = new List<List<FullMoveStep>>();

                    foreach (Amphipod aPlayer in _players)
                    {
                        // don't allow two movements in a row by the same player -- this messes up the Steps count, btw
                        if(aPlayer != currentPlayer)
                            nextPossibleMovements.AddRange(mp.PlanMoves(_map, aPlayer));
                    }

                    // if the movement is the 2nd and doesn't end in a finishing position, remove it
                    int pathNum = 0;
                    while (pathNum < nextPossibleMovements.Count())
                    {
                        if ((nextPossibleMovements[pathNum][0].player.Steps == 1) && (nextPossibleMovements[pathNum][^1].newRow == 1/*< PARKING_BOTTOM_ROW - 3*/)) // TODOJOTA -- number of rows
                            nextPossibleMovements.RemoveAt(pathNum);
                        else
                            pathNum++;
                    }

                    // if morePaths.Count == 0 means we can't find a way out but we didn't win, so we discard this path and go for the next
                    if (nextPossibleMovements.Count == 0)
                    {
                        //_map.Print();
                        //Console.ReadLine();

                        alternativePaths.RemoveAt(0);
                    }
                    else
                    {
                        // Go over each of the previous paths, and add the new moves -- this causes an explosion of paths

                        List<List<FullMoveStep>> alternativePathsWithNewMoves = new List<List<FullMoveStep>>();
                        List<FullMoveStep> pathPrefix = alternativePaths[0];
                        
                        foreach (List<FullMoveStep> nextAmphipodMovement in nextPossibleMovements)
                        {
                            List<FullMoveStep> newFullMove = pathPrefix.DeepCopy();
                            newFullMove.AddRange(nextAmphipodMovement);
                            alternativePathsWithNewMoves.Add(newFullMove);
                        }

                        alternativePaths.RemoveAt(0); // remove this path, as we're adding new variations of it with new moves appended
                        alternativePathsWithNewMoves.AddRange(alternativePaths); // add the previous paths at the end, to minimize explosion
                        alternativePaths = alternativePathsWithNewMoves; // replace old paths
                    }
                }
            }

            Console.WriteLine("Final lowest score: {0}", bestScore);
            // part a: 10607
            // part b: 59071

            return;
        }
    }
}
