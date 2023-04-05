using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day24
{
    public abstract class Instruction
    {
        protected Variable _param1;

        public Instruction(Variable param)
        {
            _param1 = param;
        }

        abstract public void Run(Queue<int> inputParams);
    }
}
