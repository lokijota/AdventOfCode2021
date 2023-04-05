﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day24
{
    public class Div : Instruction
    {
        Variable _param2;

        public Div(Variable param1, Variable param2) : base(param1)
        {
            _param2 = param2;
        }

        public override void Run(Queue<int> inputParams)
        {
            if (_param2.Value == 0)
                throw new ApplicationException(String.Format("Division by zero with null or negative number: {0}", _param2.Value));

            long result = _param1.Value / _param2.Value;

            //Console.WriteLine("Inp.Div() {0} / {1} = {2}", _param1.Value, _param2.Value, result);

            _param1.Value = result;
        }
    }
}
