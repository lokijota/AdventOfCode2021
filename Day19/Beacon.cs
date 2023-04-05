using MathNet.Numerics.LinearAlgebra.Complex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace Day19
{
    public class Beacon
    {
        public Beacon(Vector<double> detectionCoordinates)
        {
            BeaconPositionsInCoordinateSystem = new Vector<double>[24];
            BeaconPositionsInCoordinateSystem[0] = detectionCoordinates;
        }

        public Vector<double>[] BeaconPositionsInCoordinateSystem { get; set; }
    }
}
