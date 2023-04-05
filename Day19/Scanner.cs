using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace Day19
{
    public class Scanner
    {
        public Scanner(string scannerName)
        {
            ScannerName = scannerName;
            Beacons = new List<Beacon>();
            Ref0Position = Vector<double>.Build.DenseOfArray(new double[] { 0.0, 0.0, 0.0 });
        }

        public string ScannerName { get; set; }
        public List<Beacon> Beacons { get; set; }
        public Vector<double> Ref0Position { get; set; }
    }
}
