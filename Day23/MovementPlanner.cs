using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day23
{
    public class MovementPlanner
    {
        const int PARKING_BOTTOM_ROW = 5;

        /// <summary>
        /// Entry point for recursive method
        /// </summary>
        public List<List<FullMoveStep>> PlanMoves(Map m, Amphipod amph)
        {
            List<List<FullMoveStep>> paths = new List<List<FullMoveStep>>();

            int r = amph.Row;
            int c = amph.Column;

            paths = PlanMoves(m, amph, r, c, paths, new List<FullMoveStep>());

            return paths;
        }

        /// <summary>
        ///  This generates allowed single (exceptionally double - to enter or leave doors) moves from a single position
        ///  It does not select paths
        /// </summary>
        List<List<FullMoveStep>> PlanMoves(Map m, Amphipod a, int r, int c, List<List<FullMoveStep>> paths, List<FullMoveStep> path)
        {
            if(path.Count > 0 && m.CanStopHere(r,c))
                paths.Add(path.DeepCopy());

            // process the end conditions
            //if (m.AtEndPosition(r, c, a.Type))
            //{
            //    // this gentlement can't move anymore, and the path is already recorded, as per above, so bail out
            //    return paths;
            //}

            // not over yet for this player, continue

            
            // can move up?
            if(a.Steps == 0 && m.WhatsInPosition(r-1, c) == SquareType.Free && !JustCameFromThere(r-1,c, path))
            {
                // this is a up movement, we should only go up if we're not already parked "at home"
                // note that we check the current position, not the new one, we don't want to let it move out
                if (!m.AtEndPosition(r, c, a.Type))
                {
                    // to be able to move up, the entire colum has to be Free
                    bool isColumnAllFree = true;
                    for(int pointerRow = r-1; pointerRow >= 1; pointerRow--)
                    {
                        if (m.WhatsInPosition(pointerRow, c) != SquareType.Free)
                            isColumnAllFree = false;
                    }

                    if (isColumnAllFree)
                    {
                        int nMovesUp = r - 1;
                        int rowForStep = r;

                        for (int j = 0; j < nMovesUp; j++)
                        {
                            path.Add(new FullMoveStep() { direction = MoveDirection.Up, previousRow = rowForStep, previousColumn = c, newRow = rowForStep - 1, newColumn = c, player = a });
                            rowForStep--; // we're moving up
                        }

                        PlanMoves(m, a, 1 /*rowForStep*/, c, paths, path);

                        for (int j = 0; j < nMovesUp; j++)
                        {
                            path.RemoveAt(path.Count() - 1);
                        }
                    }

                    // Code for part 1 below, above is generic code for part 2
                    //// we're at the bottom of the door, so if we move at all, we have to move 2 positions, we can't stay halfway through the door
                    //if (r == 3)
                    //{
                    //    path.Add(new FullMoveStep() { direction = MoveDirection.Up, previousRow = r, previousColumn = c, newRow = r - 1, newColumn = c, player = a });
                    //    path.Add(new FullMoveStep() { direction = MoveDirection.Up, previousRow = r-1, previousColumn = c, newRow = r - 2, newColumn = c, player = a });
                    //    PlanMoves(m, a, r - 2, c, paths, path);
                    //    path.RemoveAt(path.Count() - 1);
                    //    path.RemoveAt(path.Count() - 1);
                    //}
                    //else 
                    //{
                    //    path.Add(new FullMoveStep() { direction = MoveDirection.Up, previousRow = r, previousColumn = c, newRow = r - 1, newColumn = c, player = a });
                    //    PlanMoves(m, a, r - 1, c, paths, path);
                    //    path.RemoveAt(path.Count() - 1);
                    //}
                }
            }

            // down
            if (a.Steps <= 1 && m.WhatsInPosition(r + 1, c) == SquareType.Free && !JustCameFromThere(r + 1, c, path))
            {
                // this is a down movement. we can only go down if we're going down into our door
                if(m.DoorType(r+1, c) == a.Type)
                {
                    // Prior test -- can only move down if there's only Free spaces or amphipods of the same species
                    bool canMoveDown = true;
                    for(int j=1; j< PARKING_BOTTOM_ROW; j++)
                    {
                        if(m.WhatsInPosition(r+j,c) != SquareType.Free && m.WhatsInPosition(r+j, c).GetAmphipodType() != a.Type)
                            canMoveDown = false;
                    }

                    if (canMoveDown)
                    {
                        int currentPos = r;

                        while (m.WhatsInPosition(currentPos + 1, c) == SquareType.Free)
                        {
                            path.Add(new FullMoveStep() { direction = MoveDirection.Down, previousRow = currentPos, previousColumn = c, newRow = currentPos + 1, newColumn = c, player = a });
                            currentPos++;
                        }
                        PlanMoves(m, a, currentPos, c, paths, path);

                        while (currentPos != r)
                        {
                            path.RemoveAt(path.Count() - 1);
                            currentPos--;
                        }
                    }

                    //// NOTAJOTA: on the code below -- what if there's something there of a different letter?!
                    //// can move down twice?
                    //if (m.WhatsInPosition(r + 2, c) == SquareType.Free)
                    //{
                    //    path.Add(new FullMoveStep() { direction = MoveDirection.Down, previousRow = r, previousColumn = c, newRow = r + 1, newColumn = c, player = a });
                    //    path.Add(new FullMoveStep() { direction = MoveDirection.Down, previousRow = r + 1, previousColumn = c, newRow = r + 2, newColumn = c, player = a });
                    //    PlanMoves(m, a, r + 2, c, paths, path);
                    //    path.RemoveAt(path.Count() - 1);
                    //    path.RemoveAt(path.Count() - 1);
                    //}
                    //else
                    //{
                    //    path.Add(new FullMoveStep() { direction = MoveDirection.Down, previousRow = r, previousColumn = c, newRow = r + 1, newColumn = c, player = a });
                    //    PlanMoves(m, a, r + 1, c, paths, path);
                    //    path.RemoveAt(path.Count() - 1);
                    //}
                }
            }

            // left
            if (a.Steps <2 && m.WhatsInPosition(r, c-1) == SquareType.Free && !JustCameFromThere(r, c-1, path))
            {
                path.Add(new FullMoveStep() { direction = MoveDirection.Left, previousRow = r, previousColumn = c, newRow = r, newColumn = c - 1, player = a });
                PlanMoves(m, a, r, c - 1, paths, path);
                path.RemoveAt(path.Count() - 1);
            }

            // right
            if (a.Steps <2 && m.WhatsInPosition(r, c + 1) == SquareType.Free && !JustCameFromThere(r, c + 1, path))
            {
                path.Add(new FullMoveStep() { direction = MoveDirection.Right, previousRow = r, previousColumn = c, newRow = r, newColumn = c + 1, player = a });
                PlanMoves(m, a, r, c + 1, paths, path);
                path.RemoveAt(path.Count() - 1);
            }

            return paths;
        }

        bool JustCameFromThere(int r, int c, List<FullMoveStep> moves)
        {
            if (moves.Count>0 && moves[^1].previousRow == r && moves[^1].previousColumn == c)
                return true;
            
            return false;
        }
    }
}
