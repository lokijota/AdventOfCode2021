using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day24
{
    public class Inp : Instruction
    {
        public Inp(Variable param) : base(param)
        {
        }

        public override void Run(Queue<int> inputParams)
        {
            _param1.Value = inputParams.Dequeue();

            //Console.WriteLine("Inp.Run() read value {0}", _param1.Value);
        }
    }
}
