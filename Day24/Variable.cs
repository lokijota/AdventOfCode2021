using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day24
{
    public class Variable
    {
        long _value = 0;

        public Variable(long value =0)
        {
            _value = value;
        }

        public long Value { get { return _value; } set { _value = value; } }
    }
}
