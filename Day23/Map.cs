using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day23
{

    public class Map
    {
        const int PARKING_BOTTOM_ROW = 5; 

        SquareType[,] _map = null;
        SquareType[,] _startingMap = null;
        int[,] _mapLayer = null;

        public Map(string[] rowsMap, string[] rowsMapLayer, out List<Amphipod> players)
        {
            _map = new SquareType[rowsMap.Length, rowsMap[0].Length];
            _mapLayer = new int[rowsMapLayer.Length, rowsMapLayer[0].Length];

            List<Amphipod> newPlayers = new List<Amphipod>();

            for (int row=0; row < rowsMap.Count(); row++)
                for(int col=0; col < rowsMap[row].Length; col++)
                {
                    if (rowsMap[row][col] == ' ' || rowsMap[row][col] == '#')
                        _map[row, col] = SquareType.Wall;
                    else if (rowsMap[row][col] == '.')
                        _map[row, col] = SquareType.Free;
                    else
                    {
                        _map[row, col] = SquareTypeExtensions.ConvertFromChar(rowsMap[row][col]);
                        newPlayers.Add(new Amphipod(rowsMap[row][col], row, col));
                    }

                    _mapLayer[row, col] = -1;

                    if (rowsMapLayer[row][col] == '0')
                        _mapLayer[row, col] = 0; 
                    else if (rowsMapLayer[row][col] == '1')
                        _mapLayer[row, col] = 1;
                    else if (rowsMapLayer[row][col] == '2')
                        _mapLayer[row, col] = 2;
                    else if (rowsMapLayer[row][col] == '3')
                        _mapLayer[row, col] = 3;
                    else if (rowsMapLayer[row][col] == '4')
                        _mapLayer[row, col] = 4;

                }

            _startingMap = (SquareType[,]) _map.Clone();
            players = newPlayers;
        }

        public void Reset()
        {
            _map = (SquareType[,])_startingMap.Clone();
        }

        public void Move(int fromRow, int fromCol, int toRow, int toCol)
        {
            // swap values
            (_map[fromRow, fromCol], _map[toRow, toCol]) = (_map[toRow, toCol], _map[fromRow, fromCol]);
        }

        public SquareType WhatsInPosition(int row, int column)
        {
            return _map[row, column];
        }

        public bool CanStopHere(int row, int column)
        {
            if (_mapLayer[row, column] == 0)
                return false;

            return true;
        }

        public bool GameOver()
        {
            // HARDCODED - TODOJOTA
            if (_map[2, 3] == SquareType.PlayerA &&
               _map[3, 3] == SquareType.PlayerA &&
               _map[4, 3] == SquareType.PlayerA &&
               _map[5, 3] == SquareType.PlayerA &&

               _map[2, 5] == SquareType.PlayerB &&
               _map[3, 5] == SquareType.PlayerB &&
               _map[4, 5] == SquareType.PlayerB &&
               _map[5, 5] == SquareType.PlayerB &&

               _map[2, 7] == SquareType.PlayerC &&
               _map[3, 7] == SquareType.PlayerC &&
               _map[4, 7] == SquareType.PlayerC &&
               _map[5, 7] == SquareType.PlayerC &&

               _map[2, 9] == SquareType.PlayerD &&
               _map[3, 9] == SquareType.PlayerD &&
               _map[4, 9] == SquareType.PlayerD &&
               _map[5, 9] == SquareType.PlayerD)
                return true;

            return false;
        }

        public bool AtEndPosition(int row, int column, AmphipodType amphipodType)
        {
            SquareType expectedInBottomOfDoor;
            switch(amphipodType)
            {
                case AmphipodType.Amber:
                    expectedInBottomOfDoor = SquareType.PlayerA;
                    break;
                case AmphipodType.Bronze:
                    expectedInBottomOfDoor = SquareType.PlayerB;
                    break;
                case AmphipodType.Copper:
                    expectedInBottomOfDoor = SquareType.PlayerC;
                    break;
                default:
                    expectedInBottomOfDoor = SquareType.PlayerD;
                    break;
            }

            int amphipodTypeAsInt = (int)amphipodType;

            if (_mapLayer[row, column] != amphipodTypeAsInt)
                return false;

            for(int pointerRow=row+1; pointerRow<=PARKING_BOTTOM_ROW; pointerRow++)
            {
                if (_map[pointerRow, column] != expectedInBottomOfDoor)
                    return false;
            }

            //for(int pointerRow=PARKING_BOTTOM_ROW; pointerRow>row; pointerRow--)
            //{
                
            //}

            return true;

            //    // the amphipod is at the bottom of its door
            //    if (_mapLayer[row, column] == amphipodTypeAsInt && row == PARKING_BOTTOM_ROW)
            //    return true;

            
            //// the amphipod is not at the bottom of its door, but at the bottom there's already what's expected to be there
            //// JOTA -- made this generic for the entire row
            //int firstowOfDifferentSpecies;
            //for (firstowOfDifferentSpecies = PARKING_BOTTOM_ROW; firstowOfDifferentSpecies > 1; firstowOfDifferentSpecies--)
            //    if (_map[firstowOfDifferentSpecies, column] != expectedInBottomOfDoor)
            //        break;

            //if(_mapLayer[row, column] == amphipodTypeAsInt &&
            //   row == firstowOfDifferentSpecies)  // was: && PARKING_BOTTOM_ROW - 1
            //    //_map[row + 1, column] == expectedInBottomOfDoor)
            //    return true;

            return false;
        }

        public void Print()
        {
            Console.WriteLine();
            for (int row = 0; row < _map.GetLength(0); row++)
            {
                for (int col = 0; col < _map.GetLength(1); col++)
                {
                    Console.Write(_map[row, col].GetMapRepresentation());
                }

                Console.WriteLine();
            }
        }

        public AmphipodType DoorType(int row, int column)
        {
            // TODOJOTA -- This crashes if called not on doors
            return (AmphipodType)_mapLayer[row, column];
        }

    }
}
