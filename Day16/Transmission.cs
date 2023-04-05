using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    public class Transmission
    {
        private int _numPackets = 0;
        private int _versionSum = 0;

        public int NumPacketsRead
        {
            get { return _numPackets; }
        }

        public int VersionSum
        {
            get
            {
                return _versionSum;
            }
        }

        public Int64 Read(string binaryString)
        {
            Tokenizer t = new Tokenizer(binaryString);
            return ReadPacket(t);
        }

        private Int64 ReadPacket(Tokenizer t)
        {
            int version = t.ReadPacketVersion();
            int typeId = t.ReadPacketTypeID();
            _numPackets++;
            _versionSum += version;

            Console.WriteLine("PACKET {0}, Version: {1}, TypeID: {2}", _numPackets, version, typeId);

            if (typeId == 4)
            {
                // read literal
                Int64 lit = t.ReadLiteral();
                Console.WriteLine(" - Lit: {0}", lit);
                return lit;
            }
            else
            {
                int lengthTypeId = t.ReadLengthTypeID();
                int subPacketLength = t.ReadSubpacketLength(lengthTypeId);

                if (lengthTypeId == 0)
                {
                    Console.WriteLine(" - Operator(s) in : {0} bits", subPacketLength);

                    // If the length type ID is 0, then the next 15 bits are a number that represents the total length in bits of the sub-packets
                    // contained by this packet.
                    int startingPosition = t.CurrentPosition;

                    // different operator types
                    if (typeId == 0)
                    {
                        Int64 sum = 0;
                        while (t.CurrentPosition - startingPosition < subPacketLength)
                        {
                            sum += ReadPacket(t);
                        }
                        return sum;
                    }
                    else if(typeId == 1)
                    {
                        Int64 prod = 1;
                        while (t.CurrentPosition - startingPosition < subPacketLength)
                        {
                            prod *= ReadPacket(t);
                        }
                        return prod;
                    }
                    else if(typeId == 2)
                    {
                        Int64 min = Int64.MaxValue;
                        while (t.CurrentPosition - startingPosition < subPacketLength)
                        {
                            Int64 newValue = ReadPacket(t);
                            if (newValue < min)
                                min = newValue;
                        }
                        return min;
                    }
                    else if (typeId == 3)
                    {
                        Int64 max = Int64.MinValue;
                        while (t.CurrentPosition - startingPosition < subPacketLength)
                        {
                            Int64 newValue = ReadPacket(t);
                            if (newValue> max)
                                max = newValue;
                        }
                        return max;
                    }
                    else if (typeId == 5) // greater than
                    {
                        if (ReadPacket(t) > ReadPacket(t))
                            return 1;
                        else
                            return 0;
                    }
                    else if (typeId == 6) // less than
                    {
                        if (ReadPacket(t) < ReadPacket(t))
                            return 1;
                        else
                            return 0;
                    }
                    else if (typeId == 7) // equal to 
                    {
                        if (ReadPacket(t) == ReadPacket(t))
                            return 1;
                        else
                            return 0;
                    }
                }
                else if (lengthTypeId == 1)
                {
                    Console.WriteLine(" - Operator(s) in number: {0}", subPacketLength);

                    // If the length type ID is 1, then the next 11 bits are a number that represents the number of sub-packets
                    // immediately contained by this packet
                    // different operator types
                    if (typeId == 0)
                    {
                        Int64 sum = 0;

                        for (int subpacket = 0; subpacket < subPacketLength; subpacket++)
                        {
                            sum += ReadPacket(t);
                        }

                        return sum;
                    }
                    else if (typeId == 1)
                    {
                        Int64 prod = 1;

                        for (int subpacket = 0; subpacket < subPacketLength; subpacket++)
                        {
                            prod *= ReadPacket(t);
                        }

                        return prod;
                    }
                    else if (typeId == 2)
                    {
                        Int64 min = Int64.MaxValue;
                        for (int subpacket = 0; subpacket < subPacketLength; subpacket++)
                        {
                            Int64 newValue = ReadPacket(t);
                            if (newValue < min)
                                min = newValue;
                        }

                        return min;
                    }
                    else if (typeId == 3)
                    {
                        Int64 max = Int64.MinValue;
                        for (int subpacket = 0; subpacket < subPacketLength; subpacket++)
                        {
                            Int64 newValue = ReadPacket(t);
                            if (newValue > max)
                                max = newValue;
                        }

                        return max;
                    }
                    else if (typeId == 5) // greater than
                    {
                        if (ReadPacket(t) > ReadPacket(t))
                            return 1;
                        else
                            return 0;
                    }
                    else if (typeId == 6) // less than
                    {
                        if (ReadPacket(t) < ReadPacket(t))
                            return 1;
                        else
                            return 0;
                    }
                    else if (typeId == 7) // equal to 
                    {
                        if (ReadPacket(t) == ReadPacket(t))
                            return 1;
                        else
                            return 0;
                    }
                }
            }
            return 1;
        }
    }
}
