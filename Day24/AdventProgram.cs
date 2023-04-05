using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day24
{
    public class AdventProgram
    {
        List<Instruction> _program = new List<Instruction>();
        Variable _registerX = new Variable(0);
        Variable _registerY = new Variable(0);
        Variable _registerZ = new Variable(0);
        Variable _registerW = new Variable(0);

        public AdventProgram() { }

        public int Load(string[] instructions)
        {
            foreach(string instruction in instructions)
            {
                if (instruction.StartsWith("inp w"))
                {
                    Instruction instruction1 = new Inp(_registerW);
                    _program.Add(instruction1);
                }
                else // if (instruction.StartsWith("mul"))
                {
                    // extract parameters
                    Variable p1 = null;
                    Variable p2 = null;

                    // first param
                    switch (instruction[4])
                    {
                        case 'x':
                            p1 = _registerX;
                            break;
                        case 'y':
                            p1 = _registerY;
                            break;
                        case 'z':
                            p1 = _registerZ;
                            break;
                        case 'w':
                            p1 = _registerW;
                            break;
                    }

                    // second param
                    switch (instruction[6])
                    {
                        case 'x':
                            p2 = _registerX;
                            break;
                        case 'y':
                            p2 = _registerY;
                            break;
                        case 'z':
                            p2 = _registerZ;
                            break;
                        case 'w':
                            p2 = _registerW;
                            break;
                        default:
                            // it's not a variable name but a number, let's parse it and put in a variable
                            p2 = new Variable(int.Parse(instruction.Substring(6)));
                            break;
                    }

                    // now add the right instruction
                    if (instruction.StartsWith("mul"))
                        _program.Add(new Mul(p1, p2));
                    else if (instruction.StartsWith("add"))
                        _program.Add(new Add(p1, p2));
                    else if (instruction.StartsWith("div"))
                        _program.Add(new Div(p1, p2));
                    else if (instruction.StartsWith("mod"))
                        _program.Add(new Mod(p1, p2));
                    else if (instruction.StartsWith("eql"))
                        _program.Add(new Eql(p1, p2));
                }
            }

            return instructions.Length;
        }

        public Int64 Run(Queue<int> inputs)
        {
            _registerX.Value = 0;
            _registerY.Value = 0;
            _registerZ.Value = 0;
            _registerW.Value = 0;

            foreach(Instruction i in _program)
            {
                i.Run(inputs);
            }

            return 0;
        }

        public long X { get { return _registerX.Value; } }
        public long Y { get { return _registerY.Value; } }
        public long Z { get { return _registerZ.Value; } }
        public long W { get { return _registerW.Value; } }
    }
}
