using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day21
{
    public class Dice
    {
        int _diceValue = 0;

        public Dice() { }

        public int Roll()
        {
            _diceValue++;
            if (_diceValue > 100)
                _diceValue = 1;

            return _diceValue;
        }
    }
}
