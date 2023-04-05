using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day20
{
    public class TrenchMap
    {
        bool[,] _trenchMap = new bool[1000, 1000];
        int _startX = 400, _startY = 400, _squareSize = 100;

        public TrenchMap(string[] rows)
        {
            for (int row = 0; row < 1000; row++)
                for (int col=0; col < 1000; col++)
                {
                    _trenchMap[row, col] = false; // equivalent to '.'/dark
                }

            _squareSize = rows[0].Length;
            for(int row = 0; row < _squareSize; row++)
                for(int col= 0; col< _squareSize; col++)
                {
                    _trenchMap[row + _startX, col + _startY] = rows[row][col] == '.' ? false : true;
                }
        }

        public void EnhanceImage(ConversionMap settings)
        {
            bool[,] newTrenchMap = new bool[1000, 1000];
            int startX = _startX-1, startY = _startY-1, squareSize = _squareSize + 2;

            bool fillBit = !_trenchMap[0, 0];
            for (int row = 0; row < 1000; row++)
                for (int col = 0; col < 1000; col++)
                {
                    newTrenchMap[row, col] = fillBit; // equivalent to '.'/dark
                }

            for (int row = _startX-1; row < _startX+1 + _squareSize; row++)
                for (int col = _startY-1; col < _startY+1 + _squareSize; col++)
                {
                    bool[] nineBits = new bool[9];

                    nineBits[8] = _trenchMap[row - 1, col - 1];
                    nineBits[7] = _trenchMap[row - 1, col];
                    nineBits[6] = _trenchMap[row - 1, col + 1];
                    nineBits[5] = _trenchMap[row, col - 1];
                    nineBits[4] = _trenchMap[row, col];
                    nineBits[3] = _trenchMap[row, col + 1];
                    nineBits[2] = _trenchMap[row + 1, col - 1];
                    nineBits[1] = _trenchMap[row + 1, col];
                    nineBits[0] = _trenchMap[row + 1, col + 1];

                    // convert to int
                    int convertedToInt = 0;
                    for (int i = 0; i < 9; i++) if (nineBits[i]) convertedToInt |= 1 << i;

                    bool newBit = settings.Convert(convertedToInt);
                    newTrenchMap[row, col] = newBit;
                }

            _trenchMap = newTrenchMap;
            _startX = startX;
            _startY = startY;
            _squareSize = squareSize;
        }

        public int CountLitPixels()
        {
            int count = 0;

            for (int row = _startX; row < _startX + _squareSize; row++)
                for (int col = _startY; col < _startY + _squareSize; col++)
                {
                    count += _trenchMap[row, col] ? 1 : 0;
                }

            return count;
        }
    }
}
