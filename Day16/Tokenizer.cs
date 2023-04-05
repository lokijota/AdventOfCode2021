using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    public class Tokenizer
    {
        private string _binaryString;
        int _pointerPosition;

        public Tokenizer(string binaryString)
        {
            _binaryString = binaryString;
            _pointerPosition = 0;
        }

        public void ResetPointer()
        {
            _pointerPosition = 0;
        }
        public int CurrentPosition   // property
        {
            get { return _pointerPosition; }   // get method
        }

        public int ReadPacketVersion()
        {
            string s = _binaryString.Substring(_pointerPosition, 3);
            _pointerPosition += 3;

            return Convert.ToInt32(s, 2);
        }

        public int ReadPacketTypeID()
        {
            return ReadPacketVersion();
        }

        public Int64 ReadLiteral()
        {
            string s = string.Empty;
            char leftmost = ' ';
            int bitsRead = 0;

            do
            {
                string block = _binaryString.Substring(_pointerPosition, 5);
                leftmost = block[0];
                bitsRead += 5;
                _pointerPosition += 5;

                s += block.Substring(1); // add all but first bit
            } while (leftmost != '0');

            return Convert.ToInt64(s, 2);
        }

        public int ReadLengthTypeID()
        {
            string s = _binaryString.Substring(_pointerPosition, 1);
            _pointerPosition += 1;

            return Convert.ToInt32(s, 2);
        }

        public int ReadSubpacketLength(int lengthTypeID)
        {
            if (lengthTypeID == 0)
            {
                // If the length type ID is 0, then the next 15 bits are a number that represents the total length in bits of the sub-packets
                // contained by this packet.
                string s = _binaryString.Substring(_pointerPosition, 15);
                _pointerPosition += 15;
                return Convert.ToInt32(s, 2);
            }
            else
            {
                // If the length type ID is 1, then the next 11 bits are a number that represents the number of sub-packets
                // immediately contained by this packet.
                string s = _binaryString.Substring(_pointerPosition, 11);
                _pointerPosition += 11;
                return Convert.ToInt32(s, 2);
            }
        }
    }
}