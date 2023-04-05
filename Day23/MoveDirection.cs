using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day23
{
    public enum MoveDirection
    {
        Up = 0,
        Down,
        Left,
        Right
    }

    public static class MoveDirectionExtensions
    {
        public static MoveDirection Reverse(this MoveDirection md)
        {
            if (md == MoveDirection.Left) return MoveDirection.Right;
            if (md == MoveDirection.Right) return MoveDirection.Left;
            if (md == MoveDirection.Up) return MoveDirection.Down;
            /*if (md == MoveDirection.Down) */
            return MoveDirection.Up;
        }
    }
}
