using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day23
{
    public class Amphipod
    {
        // properties
        int _startingRow = 0;
        int _startingColumn = 0;

        public AmphipodType Type { get; private set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int Steps { get; set; }

        // class constructors
        public Amphipod(AmphipodType type, int positionRow, int positionColumn)
        {
            Type = type;
            Row = positionRow;
            Column = positionColumn;
            Steps = 0;

            _startingRow = positionRow;
            _startingColumn = positionColumn;
        }

        public Amphipod(char letter, int positionRow, int positionColumn) : this(AmphipodType.Amber, positionRow, positionColumn)
        {
            switch (letter)
            {
                // not really needed, as it's in the constructor, but leaving it just in case
                case 'A':
                    Type = AmphipodType.Amber;
                    break;
                case 'B':
                    Type = AmphipodType.Bronze;
                    break;
                case 'C':
                    Type = AmphipodType.Copper;
                    break;
                case 'D':
                    Type = AmphipodType.Desert;
                    break;
            }
        }

        // movement
        public void Move(MoveDirection move)
        {
            switch (move)
            {
                case MoveDirection.Up:
                    Row--;
                    break;
                case MoveDirection.Down:
                    Row++;
                    break;
                case MoveDirection.Right:
                    Column++;
                    break;
                case MoveDirection.Left:
                    Column--;
                    break;
            }  

            if(Row == 0 || Column == 0)
            {
                throw new ApplicationException("Invalid move, Row or Column = 0!");
            }
        }

        public void Move(List<MoveDirection> moves)
        {
            foreach (MoveDirection move in moves)
                Move(move);
        }

        public void Reset()
        {
            Row = _startingRow;
            Column = _startingColumn;
            Steps = 0;
        }

        public int EnergySpendPerMove { 
            get
            {
                switch(Type)
                {
                    case AmphipodType.Amber:
                        return 1;
                    case AmphipodType.Bronze:
                        return 10;
                    case AmphipodType.Copper:
                        return 100;
                    default: //  AmphipodType.Desert:
                        return 1000;
                }
            }
        }
    }
}
