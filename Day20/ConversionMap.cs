using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day20
{
    public class ConversionMap
    {
        bool[] _map = null;

        public ConversionMap(string values)
        {
            values = values.TrimEnd();
            _map = new bool[values.Length];

            for (int i = 0; i < values.Length; i++)
                _map[i] = values[i] == '.' ? false : true;
        }

        public bool Convert(int position)
        {
            return _map[position];
        }
    }
}
