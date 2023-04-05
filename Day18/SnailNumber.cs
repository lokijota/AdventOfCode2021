using Day18;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Day18
{
    public class SnailNumber
    {
        #region Variables and Setters
        int? leftElementary = null;
        SnailNumber? leftComplex = null;

        int? rightElementary = null;
        SnailNumber? rightComplex = null;

        SnailNumber? _parent = null;

        public SnailNumber()
        {
            leftElementary = 0;
            leftComplex = null;
            rightElementary = 0;
            rightComplex = null;
        }

        public void SetLeftElementary(int i)
        {
            leftElementary = i;
            leftComplex = null;
        }

        public void SetLeftComplex(SnailNumber sn)
        {
            leftElementary = null;
            leftComplex = sn;
        }

        public void SetRightElementary(int i)
        {
            rightElementary = i;
            rightComplex = null;
        }

        public void SetRightComplex(SnailNumber sn)
        {
            rightElementary = null;
            rightComplex = sn;
        }

        public void SetParent(SnailNumber parent)
        {
            this._parent = parent;
        }

        #endregion

        public void Print()
        {
            Console.Write('[');
            if(leftElementary!=null)
                Console.Write(leftElementary.ToString());
            else if (leftComplex != null)
                leftComplex.Print();

            Console.Write(",");

            if (rightElementary != null)
                Console.Write(rightElementary.ToString());
            else if (rightComplex != null)
                rightComplex.Print();

            Console.Write(']');
        }

        public static SnailNumber Parse(string s)
        {
            s = s.Replace(" ", ""); // just to avoid some errors when there are spaces

            SnailNumber sn = new SnailNumber();

            Debug.Assert(s[0] == '[', "Expected open parenthesis");

            int readPosition = 1;
            //string left = s.Substring(readPosition, 1);

            int digitCount = 0;

            int leftIntPart = ReadInt(s.Substring(readPosition), ref digitCount);
            if (digitCount > 0)
            {
                sn.SetLeftElementary(leftIntPart);
                readPosition += digitCount;
            }
            else
            {
                // it's an open parenthesis
                // so look for the closing one, and extract the substring
                // and call parse again
                // and assign that to the right side

                string leftComplexExpression = FindEnclosedNumber(s.Substring(1));
                sn.SetLeftComplex(SnailNumber.Parse(leftComplexExpression));
                sn.leftComplex._parent = sn;
                readPosition += leftComplexExpression.Length;
            }

            Debug.Assert(s[readPosition] == ',', "Expected comma");

            // now the right side
            readPosition++;

            int rightIntPart = ReadInt(s.Substring(readPosition), ref digitCount);
            if (digitCount > 0)
            {
                sn.SetRightElementary(rightIntPart);
                readPosition += digitCount;
            }
            else
            {
                // same logic as above

                string rightComplexExpression = FindEnclosedNumber(s.Substring(readPosition));
                sn.SetRightComplex(SnailNumber.Parse(rightComplexExpression));
                sn.rightComplex._parent = sn;
                readPosition += rightComplexExpression.Length;
            }

            return sn;
        }

        private static string FindEnclosedNumber(string s)
        {
            int OpenParentesisCount = 0;
            int CloseParentesisCount = 0;

            // assume the first is an opening parenthesis
            int pos = 0;
            for (; pos < s.Length; pos++)
            {
                if (s[pos] == '[')
                    OpenParentesisCount++;

                if (s[pos] == ']')
                    CloseParentesisCount++;

                if (OpenParentesisCount == CloseParentesisCount) // found end of block
                    break;
            }

            return s.Substring(0, pos+1);
        }

        public static SnailNumber Add(SnailNumber sn1, SnailNumber sn2)
        {
            SnailNumber sn = new SnailNumber();
            sn.SetLeftComplex(sn1);
            sn1._parent = sn;

            sn.SetRightComplex(sn2);
            sn2._parent = sn;

            //Console.Write("Added: ");
            //sn.Print();
            //Console.WriteLine();

            return Reduce(sn);
        }

        /*
         * You must repeatedly do the first action in this list that applies to the snailfish number:
         * - If any pair is nested inside four pairs, the leftmost such pair explodes.
         * - If any regular number is 10 or greater, the leftmost such regular number splits.
         */
        public static SnailNumber Reduce(SnailNumber sn)
        {
            bool exploded = false, split = false;

            do
            {
                exploded = false;
                split = false;

               TryExplode(sn, ref exploded);

                if (!exploded)
                {
                    TrySplit(sn, ref split);
                }

                //if (exploded)
                //    Console.Write("After explode: ");
                //else if (split)
                //    Console.Write("After split: ");
                //sn.Print();
                //Console.WriteLine();
            }
            while (exploded || split);
            
            return sn;
        }

        public int CalculateMagnitude()
        {
            // kickoff a recursive call
            return CalculateMagnitude(this);
        }

        private int CalculateMagnitude(SnailNumber sn)
        {
            if (sn.leftElementary != null && sn.rightElementary != null)
                return sn.leftElementary.Value * 3 + sn.rightElementary.Value * 2;
            else if (sn.leftElementary != null)
                return sn.leftElementary.Value * 3 + CalculateMagnitude(sn.rightComplex) * 2;
            else if (sn.rightElementary != null)
                return CalculateMagnitude(sn.leftComplex) * 3 + sn.rightElementary.Value * 2;
            else
                return CalculateMagnitude(sn.leftComplex) * 3 + CalculateMagnitude(sn.rightComplex) * 2;
        }

        // always applies to pairs
        private static SnailNumber TryExplode(SnailNumber sn, ref bool exploded)
        {
            Stack<SnailNumber> snStack = new Stack<SnailNumber>();

            snStack.Push(sn);
            while(snStack.Any())
            {
                var currentSN = snStack.Pop();

                if (currentSN.rightComplex != null)
                    snStack.Push(currentSN.rightComplex);

                if (currentSN.leftComplex != null)
                    snStack.Push(currentSN.leftComplex);

                if(currentSN.leftElementary != null && currentSN.rightElementary != null && currentSN.Depth() >= 5)
                {
                    // need to explode this node
                    exploded = true;

                    // find locations where we explode, before doing any change
                    var leftSnailExplosionAbsorver = FindParentNumberWithElementaryValue(currentSN, true);
                    var rightSnailExplosionAbsorver = FindParentNumberWithElementaryValue(currentSN, false);

                    if (leftSnailExplosionAbsorver != null)
                    {
                        if (leftSnailExplosionAbsorver.rightElementary != null)
                        {
                            leftSnailExplosionAbsorver.rightElementary += currentSN.leftElementary;
                        }
                        else if (leftSnailExplosionAbsorver.leftElementary != null)
                        {
                            leftSnailExplosionAbsorver.leftElementary += currentSN.leftElementary;

                            // setting to null what pointed at the current means there's no more references to currentSN => it's removed from the tree
                            if (leftSnailExplosionAbsorver.rightComplex == currentSN)
                            {
                                leftSnailExplosionAbsorver.rightElementary = 0;
                                leftSnailExplosionAbsorver.rightComplex = null;
                            }
                        }
                    }

                    if (rightSnailExplosionAbsorver != null)
                    {
                        if (rightSnailExplosionAbsorver.leftElementary != null)
                        {
                            rightSnailExplosionAbsorver.leftElementary += currentSN.rightElementary;
                        }
                        else if (rightSnailExplosionAbsorver.rightElementary != null)
                        {
                            rightSnailExplosionAbsorver.rightElementary += currentSN.rightElementary;

                            // setting to null what pointed at the current means there's no more references to currentSN => it's removed from the tree
                            if (rightSnailExplosionAbsorver.leftComplex == currentSN)
                            {
                                rightSnailExplosionAbsorver.leftElementary = 0;
                                rightSnailExplosionAbsorver.leftComplex = null;
                            }
                        }
                    }

                    // delete the reference to this node
                    if (currentSN._parent.leftComplex == currentSN)
                    {
                        currentSN._parent.leftElementary = 0;
                        currentSN._parent.leftComplex = null;
                    }
                    else if (currentSN._parent.rightComplex == currentSN)
                    {
                        currentSN._parent.rightElementary = 0;
                        currentSN._parent.rightComplex = null;
                    }

                    // exit immediately -- only one explosion per round
                    return sn;
                }
            }

            return sn;
        }

        private static SnailNumber FindParentNumberWithElementaryValue(SnailNumber sn, bool leftOperand)
        {
            SnailNumber prevNodeVisited = sn;
            SnailNumber nextNodeToSearch = sn._parent;

            if (leftOperand)
            {
                // go up - look for a split to the left side
                while (nextNodeToSearch != null) // && (nextNodeToSearch.leftComplex == null || nextNodeToSearch.leftElementary != null))
                {
                    if(nextNodeToSearch.leftComplex != null && nextNodeToSearch.leftComplex != prevNodeVisited)
                    {
                        nextNodeToSearch = nextNodeToSearch.leftComplex;
                        break; // let's search downwards now, we found a path
                    }

                    if (nextNodeToSearch.leftElementary != null)
                        return nextNodeToSearch;

                    prevNodeVisited = nextNodeToSearch;
                    nextNodeToSearch = nextNodeToSearch._parent;
                }

                // go down -- loook for the rightmost elementary
                while(nextNodeToSearch != null)
                {
                    if(nextNodeToSearch.rightElementary != null)
                    {
                        return nextNodeToSearch;
                    }

                    nextNodeToSearch = nextNodeToSearch.rightComplex;
                }
            }
            else // look on the right side
            {
                // go up - look for a split to the right side
                while (nextNodeToSearch != null)
                {
                    if (nextNodeToSearch.rightComplex != null && nextNodeToSearch.rightComplex != prevNodeVisited)
                    {
                        nextNodeToSearch = nextNodeToSearch.rightComplex;
                        break; // let's search downwards now, we found a path
                    }

                    if (nextNodeToSearch.rightElementary != null)
                        return nextNodeToSearch;

                    prevNodeVisited = nextNodeToSearch;
                    nextNodeToSearch = nextNodeToSearch._parent;
                }

                // go down -- loook for the leftmost elementary
                while (nextNodeToSearch != null)
                {
                    if (nextNodeToSearch.leftElementary != null)
                    {
                        return nextNodeToSearch;
                    }

                    nextNodeToSearch = nextNodeToSearch.leftComplex;
                }
            }

            return null;
        }

        // Just generating a simple tree won't work as the nodes get out of order when there are complex+values in the same node, so have to switch the orders
        private static void BuildStack(Stack<SnailNumber> stack, SnailNumber sn)
        {
            if(sn.leftElementary!= null && sn.rightElementary != null)
                stack.Push(sn);

            if (sn.leftElementary != null && sn.rightComplex != null)
            {
                BuildStack(stack, sn.rightComplex);
                stack.Push(sn);
            }
            else if (sn.rightElementary != null && sn.leftComplex != null)
            {
                stack.Push(sn);
                BuildStack(stack, sn.leftComplex);
            }
            else if (sn.rightComplex != null && sn.leftComplex != null)
            {
                BuildStack(stack, sn.rightComplex);
                BuildStack(stack, sn.leftComplex);
            }
        }

        // If any regular number is 10 or greater, the leftmost such regular number splits.
        private static SnailNumber TrySplit(SnailNumber sn, ref bool split)
        {
            Stack<SnailNumber> snStack = new Stack<SnailNumber>();
            BuildStack(snStack, sn);

            while(snStack.Any())
            {
                SnailNumber currentSN = snStack.Pop();

                if(currentSN.leftElementary != null && currentSN.leftElementary.Value >= 10)
                {
                    // do a split on the leftElementary
                    SnailNumber newSN = new SnailNumber();
                    newSN.SetLeftElementary(currentSN.leftElementary.Value / 2);
                    newSN.SetRightElementary(currentSN.leftElementary.Value / 2 + currentSN.leftElementary.Value % 2);
                    newSN._parent = currentSN;
                    currentSN.leftElementary = null;
                    currentSN.SetLeftComplex(newSN);
                    split = true;
                    return sn;
                }
                if(currentSN.rightElementary != null && currentSN.rightElementary.Value >= 10)
                {
                    // do a split on the rightElementary
                    SnailNumber newSN = new SnailNumber();
                    newSN.SetLeftElementary(currentSN.rightElementary.Value / 2);
                    newSN.SetRightElementary(currentSN.rightElementary.Value / 2 + currentSN.rightElementary.Value % 2);
                    newSN._parent = currentSN;

                    currentSN.rightElementary = null;
                    currentSN.SetRightComplex(newSN);

                    split = true;

                    return sn;
                }
            }

            return sn;
        }

        private int Depth()
        {
            if (_parent == null)
                return 1;
            else
                return _parent.Depth()+1;
        }

        private static int ReadInt(string s, ref int countDigits)
        {
            string intAsString = string.Empty;
            countDigits = 0;

            for (int j = 0; j < s.Length; j++)
            {
                if (char.IsDigit(s[j]))
                {
                    intAsString += s[j];
                    countDigits++;
                }
                else
                    break;
            }

            if (intAsString.Length == 0) // no int was found
                return -1;
            else
                return Int32.Parse(intAsString);
        }
    }
}
