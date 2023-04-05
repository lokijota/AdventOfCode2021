using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day24
{
    public class Eql : Instruction
    {
        Variable _param2;

        public Eql(Variable param1, Variable param2) : base(param1)
        {
            _param2 = param2;
        }

        public override void Run(Queue<int> inputParams)
        {
            int result = _param1.Value == _param2.Value ? 1 : 0;

            //Console.WriteLine("Inp.Eql() {0} == {1} ?? {2}", _param1.Value, _param2.Value, result);

            _param1.Value = result;
        }
    }
}
