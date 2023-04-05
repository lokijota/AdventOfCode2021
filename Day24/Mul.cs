using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day24
{
    public class Mul : Instruction
    {
        Variable _param2;

        public Mul(Variable param1, Variable param2) : base(param1)
        {
            _param2 = param2;
        }

        public override void Run(Queue<int> inputParams)
        {
            long result = _param1.Value * _param2.Value;

            //Console.WriteLine("Inp.Mul() {0} x {1} = {2}", _param1.Value, _param2.Value, result);

            _param1.Value = result;
        }
    }
}
