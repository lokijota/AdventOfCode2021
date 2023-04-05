using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace Day19
{
    public class ScannerToScannerConnection
    {
        public ScannerToScannerConnection()
        {
            MatrixNumber = -1;
        }

        public ScannerToScannerConnection(Scanner source, Scanner destination, int matrixNumber, Vector<double> translation)
        {
            SourceScanner = source;
            DestinationScanner = destination;
            MatrixNumber = matrixNumber;
            Translation = translation;
        }

        public Scanner? SourceScanner { get; set; }
        public Scanner? DestinationScanner { get; set; }
        public int MatrixNumber { get; set; }
        public Vector<double>? Translation { get; set; }
    }
}
