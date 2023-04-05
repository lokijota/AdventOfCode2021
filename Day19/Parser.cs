using MathNet.Numerics.LinearAlgebra.Complex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace Day19
{
    public class Parser
    {
        public static List<Scanner> Parse(string[] rows)
        {
            List<Scanner> scanners = new List<Scanner>();
            var V = Vector<double>.Build;

            Scanner scanner = null;
            foreach (string row in rows)
            {
                if (row.Length == 0) // skip empty rows
                    continue;
                else if (row.StartsWith("--- scanner "))
                {
                    scanner = new Scanner(row.Replace("-", "").Trim());
                    scanners.Add(scanner);
                }
                else
                {
                    double[] coordParts = row.Split(',').Select(x => Double.Parse(x)).ToArray();
                    var newVector = V.DenseOfArray(coordParts);

                    scanner.Beacons.Add(new Beacon(newVector));
                }
            }

            return scanners;
        }
    }
}
