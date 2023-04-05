using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day23
{
    public struct FullMoveStep
    {
        public Amphipod player;

        public int previousRow, previousColumn;
        public MoveDirection direction;
        public int newRow, newColumn;
    }
}
