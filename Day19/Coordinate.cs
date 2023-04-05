using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day19
{
    public class Coordinate
    {
        private int _x, _y, _z;

        public Coordinate(int x = 0, int y = 0, int z = 0)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public int X { get { return _x; } set { _x = value;  } }
        public int Y { get { return _y; } set { _y = value;  } }
        public int Z { get { return _z; } set { _z = value;  } }

        public double[] AsDoubleArray()
        {
            return new double[] { Convert.ToDouble(_x), Convert.ToDouble(_y), Convert.ToDouble(_z) };
        }
    }
}
