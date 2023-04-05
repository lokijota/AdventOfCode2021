using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day25
{
    public static class Extensions
    {
        public static int MoveEast(this char[,] fromMap, char[,] intoMap)
        {
            int nMoves = 0;

            for (int row = 0; row < fromMap.GetLength(0); row++)
                for (int col = 0; col < fromMap.GetLength(1); col++)
                {
                    int nextCol = (col == fromMap.GetLength(1) - 1) ? 0 : col + 1;

                    if (fromMap[row, col] == '>' && fromMap[row, nextCol] == '.')
                    {
                        intoMap[row, col] = '.';
                        intoMap[row, nextCol] = '>';
                        nMoves++;
                    }
                }

            return nMoves;
        }


        public static int MoveSouth(this char[,] fromMap, char[,] intoMap)
        {
            int nMoves = 0;

            for (int row = 0; row < fromMap.GetLength(0); row++)
                for (int col = 0; col < fromMap.GetLength(1); col++)
                {
                    int nextRow = (row == fromMap.GetLength(0) - 1) ? 0 : row + 1;

                    if (fromMap[row, col] == 'v' && fromMap[nextRow, col] == '.')
                    {
                        intoMap[row, col] = '.';
                        intoMap[nextRow, col] = 'v';
                        nMoves++;
                    }
                }

            return nMoves;
        }
    }
}
