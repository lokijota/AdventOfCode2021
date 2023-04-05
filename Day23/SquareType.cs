using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day23
{
    public enum SquareType { Wall, Free, PlayerA, PlayerB, PlayerC, PlayerD };

    public static class SquareTypeExtensions
    {
        public static char GetMapRepresentation(this SquareType st)
        {
            switch (st)
            {
                case SquareType.Wall: return '#';
                case SquareType.Free: return '.';
                case SquareType.PlayerA: return 'A';
                case SquareType.PlayerB: return 'B';
                case SquareType.PlayerC: return 'C';
                case SquareType.PlayerD: return 'D';
            }
            return ' ';
        }

        public static AmphipodType GetAmphipodType(this SquareType st)
        {
            switch (st)
            {
                case SquareType.PlayerA: return AmphipodType.Amber;
                case SquareType.PlayerB: return AmphipodType.Bronze;
                case SquareType.PlayerC: return AmphipodType.Copper;
                case SquareType.PlayerD: return AmphipodType.Desert;
            }

            throw new ApplicationException("Invalid conversion");
        }

        public static SquareType ConvertFromChar(char letter)
        {
            switch (letter)
            {
                // not really needed, as it's in the constructor, but leaving it just in case
                case 'A':
                    return SquareType.PlayerA;
                case 'B':
                    return SquareType.PlayerB;
                case 'C':
                    return SquareType.PlayerC;
                default:
                    return SquareType.PlayerD;
            }
        }
    }
}
