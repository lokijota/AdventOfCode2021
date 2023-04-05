using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using MathNet.Numerics.LinearAlgebra;
using QuikGraph;
using QuikGraph.Algorithms;

namespace Day19
{
    public class MatrixOperations
    {
        private List<Matrix<double>> _rightAngleRotations = new List<Matrix<double>>();

        /// <summary>
        /// Class constructor
        /// </summary>
        public MatrixOperations()
        {
            // generate all the different 24 rotation matrixes
            // with info from https://en.wikipedia.org/wiki/Rotation_matrix (basic 3D Rotations + multiplying the conmpabinations and keewping the unique)
            var M = Matrix<double>.Build;

            List<Matrix<double>> _RxRightAngleRotations = new List<Matrix<double>>();
            List<Matrix<double>> _RyRightAngleRotations = new List<Matrix<double>>();
            List<Matrix<double>> _RzRightAngleRotations = new List<Matrix<double>>();

            for (double angle = 0; angle < 2 * Math.PI; angle += Math.PI / 2.0)
            {
                // Rx(angle)
                var m = M.DenseOfArray(new[,] {{ 1.0, 0.0, 0.0},
                                               { 0.0, Math.Round(Math.Cos(angle)), -Math.Round(Math.Sin(angle)) },
                                               { 0.0, Math.Round(Math.Sin(angle)), Math.Round(Math.Cos(angle))}});
                _RxRightAngleRotations.Add(m);
                if (!_rightAngleRotations.Contains(m))
                    _rightAngleRotations.Add(m);

                // Ry(angle)
                m = M.DenseOfArray(new[,] {{ Math.Round(Math.Cos(angle)), 0.0, Math.Round(Math.Sin(angle))},
                                           { 0.0, 1.0, 0.0 },
                                           { -Math.Round(Math.Sin(angle)), 0, Math.Round(Math.Cos(angle))}});
                _RyRightAngleRotations.Add(m);
                if (!_rightAngleRotations.Contains(m))
                    _rightAngleRotations.Add(m);

                // Ry(angle)
                m = M.DenseOfArray(new[,] {{ Math.Round(Math.Cos(angle)), -Math.Round(Math.Sin(angle)), 0},
                                           { Math.Round(Math.Sin(angle)), Math.Round(Math.Cos(angle)), 0.0 },
                                           { 0.0, 0.0, 1.0 }});
                _RzRightAngleRotations.Add(m);
                if (!_rightAngleRotations.Contains(m))
                    _rightAngleRotations.Add(m);
            }

            // also add the multiplications: yaw * pitch * roll, keeping only the uniques
            foreach (var rxRotation in _RxRightAngleRotations)
                foreach (var ryRotation in _RyRightAngleRotations)
                    foreach (var rzRotation in _RzRightAngleRotations)
                    {
                        var multMat = rxRotation * ryRotation * rzRotation;

                        if(!_rightAngleRotations.Contains(multMat))
                            _rightAngleRotations.Add(multMat);
                    }

            Debug.Assert(_rightAngleRotations.Count == 24);

            //foreach (Matrix<double> m in _rightAngleRotations)
            //    Console.WriteLine(m.ToString());
        }

        public int CalculateMaximumManhatanDistance(List<Scanner> scanners, ScannerToScannerConnection[,] connections)
        {
            double max = 0;
            foreach (Scanner scannerLeft in scanners)
            {
                foreach (Scanner scannerRight in scanners)
                {
                    double distance = Math.Abs(scannerLeft.Ref0Position[0] - scannerRight.Ref0Position[0]) +
                        Math.Abs(scannerLeft.Ref0Position[1] - scannerRight.Ref0Position[1]) +
                        Math.Abs(scannerLeft.Ref0Position[2] - scannerRight.Ref0Position[2]);

                    if(distance > max)
                        max = distance;

                }
            }
            return Convert.ToInt32(max);
        }

        /// <summary>
        /// Apply all the standard rotations to a set of beacons positions detected by a set of scanners
        /// </summary>
        /// <param name="scannerReading"></param>
        public void ApplyStandardRotations(List<Scanner> scannerReading)
        {
            //var M = Matrix<double>.Build;
            //var V = Vector<double>.Build;

            foreach(Scanner scanner in scannerReading)
                foreach (Beacon beacon in scanner.Beacons)
                    // start at 1 not as 0, as the first rotation is the identity one
                    for(int j=1; j < _rightAngleRotations.Count; j++)
                    {
                        beacon.BeaconPositionsInCoordinateSystem[j] = _rightAngleRotations[j] * beacon.BeaconPositionsInCoordinateSystem[0];
                    }
        }

        /// <summary>
        ///  For each pair of scanners, compare the distances between the respective beacons in all the coordinate systems
        ///  If a same distance appears more than 12 times, that's considered to be the "right" distance / translation
        /// </summary>
        /// <param name="scanners"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ScannerToScannerConnection[,] CalculateDistancesBetweenScanners(List<Scanner> scanners)
        {
            Vector<double>[,] translations = new Vector<double>[scanners.Count, scanners.Count];
            ScannerToScannerConnection[,] scannerConnections = new ScannerToScannerConnection[scanners.Count, scanners.Count];

            var V = Vector<double>.Build;

            for (int j=0; j<scanners.Count; j++)
                for(int i=0; i<scanners.Count; i++)
                {
                    // self-comparison, skipping
                    if (scanners[j] == scanners[i])
                    {
                        translations[j, i] = V.DenseOfArray(new double[] { 0.0, 0.0, 0.0 });
                        continue;
                    }

                    // for all the other cases: for each distance vector, how many times was it seen in the beacon distances
                    Dictionary<Vector<double>, int> distances = new Dictionary<Vector<double>, int>();

                    for (int matrixTransformation = 0; matrixTransformation < 24; matrixTransformation++)
                    {
                        distances.Clear();

                        foreach (Beacon leftBeacon in scanners[j].Beacons)
                        {
                            foreach (Beacon rightBeacon in scanners[i].Beacons)
                            {
                                Vector<double> distance = leftBeacon.BeaconPositionsInCoordinateSystem[0]
                                    - rightBeacon.BeaconPositionsInCoordinateSystem[matrixTransformation];

                                if (!distances.ContainsKey(distance))
                                {
                                    distances[distance] = 1; // found a yet-unseen translation vector
                                }
                                else
                                {
                                    distances[distance]++; // saw a translation vector one more time
                                }
                            }
                        }

                        var mostCommonDistance = distances.OrderByDescending(x => x.Value).ToArray()[0];
                        if (mostCommonDistance.Value >= 12)
                        {
                            scannerConnections[j, i] = new ScannerToScannerConnection(scanners[j], scanners[i], matrixTransformation, mostCommonDistance.Key);

                            translations[j, i] = mostCommonDistance.Key;
                            Console.WriteLine("Scanner {0} to scanner {1} overlap found with transformation #{2}", j, i, matrixTransformation);
                            break;
                        }
                    }
                }

            return scannerConnections;
        }

        public List<Vector<double>> CalculateScanner0ReferecenDistances(List<Scanner> scanners, ScannerToScannerConnection[,] scannerDistances)
        {
            List<Vector<double>> translatedVectorCoordinates = new List<Vector<double>>();
            var V = Vector<double>.Build;

            // Add beacons of scanner 0
            List<Vector<double>> beaconsInScanner0 = new List<Vector<double>>();

            // Add everything from scanner 0
            foreach(Beacon b in scanners[0].Beacons)
            {
                if (!beaconsInScanner0.Contains(b.BeaconPositionsInCoordinateSystem[0]))
                    beaconsInScanner0.Add(b.BeaconPositionsInCoordinateSystem[0]);
            }

            // for all the other scanners
            AdjacencyGraph<string, Edge<string>> connectionsGraph = GetScannerConnectionGraph(scannerDistances);
            var pathsFromScanner0 = connectionsGraph.ShortestPathsDijkstra(x => 1, "0"); // calculate all paths from scanner "0"
            IEnumerable<Edge<string>> path;

            for (int j = 1; j < scannerDistances.GetLength(0); j++)
            {
                // is it directly connected to scanner0?
                if (scannerDistances[0, j] != null)
                {
                    foreach (Beacon b in scannerDistances[0, j].DestinationScanner.Beacons)
                    {
                        var v = b.BeaconPositionsInCoordinateSystem[scannerDistances[0, j].MatrixNumber];

                        var translatedBeaconLocation = v + scannerDistances[0, j].Translation;

                        if (!beaconsInScanner0.Contains(translatedBeaconLocation))
                            beaconsInScanner0.Add(translatedBeaconLocation);
                    }

                    // This is needed for part 2 -- coordinates in referential 0
                    scanners[j].Ref0Position = scannerDistances[0, j].Translation;
                }
                else
                {
                    // this is the more complicated case -- we'll have to find a path from 0 to this scanner,
                    // and apply the different retations + translations

                    // find a connection to scanner 0
                    if (pathsFromScanner0(j.ToString(), out path))
                    {
                        Vector<double>[] beaconPositions = scanners[j].Beacons.Select(x => x.BeaconPositionsInCoordinateSystem[0]).ToArray();

                        // go over the path, backwards towards scanner 0
                        foreach (var segment in path.Reverse())
                        {
                            List<Vector<double>> beaconPositionsTransformed = new List<Vector<double>>();

                            // we have a set of beacon positions. let's apply a matrix and translation
                            int from = Int32.Parse(segment.Source);
                            int to = Int32.Parse(segment.Target);

                            var matrix = _rightAngleRotations[scannerDistances[from, to].MatrixNumber];
                            var translation = scannerDistances[from, to].Translation;

                            // TODO
                            scanners[j].Ref0Position = matrix * scanners[j].Ref0Position + translation;

                            foreach (Vector<double> beaconPosition in beaconPositions)
                            {
                                beaconPositionsTransformed.Add(matrix * beaconPosition + translation);
                            }

                            beaconPositions = beaconPositionsTransformed.ToArray();
                        }

                        // add unique beacon coordinates found
                        foreach(Vector<double> beaconInRef0 in beaconPositions)
                            if (!beaconsInScanner0.Contains(beaconInRef0))
                                beaconsInScanner0.Add(beaconInRef0);
                    }
                    else
                    {
                        throw new ApplicationException(string.Format("No path found from 0 to {0}", j.ToString()));
                    }
                }
            }

            return beaconsInScanner0;
        }

        private AdjacencyGraph<string, Edge<string>> GetScannerConnectionGraph(ScannerToScannerConnection[,] connections)
        {
            var graph = new AdjacencyGraph<string, Edge<string>>();

            // add all the nodes
            for (int i = 0; i < connections.GetLength(0); i++)
                graph.AddVertex(i.ToString());

            // add all the edges
            for (int i = 0; i < connections.GetLength(0); i++)
                for (int j = 0; j < connections.GetLength(0); j++)
                {
                    if (i == j)
                        continue;

                    if (connections[i, j] != null)
                    {
                        Edge<string> newEdge = new Edge<string>(i.ToString(), j.ToString());
                        graph.AddEdge(newEdge);
                    }
                }

            return graph;
        }
    }
}
