// See https://aka.ms/new-console-template for more information
using Day19;
using MathNet.Numerics.LinearAlgebra;
using System.Diagnostics;

Console.WriteLine("Day 19");

Stopwatch sw = Stopwatch.StartNew();

// read the data
string[] rows = File.ReadAllLines("data.txt");

List<Scanner> scanners = Parser.Parse(rows);

MatrixOperations mo = new MatrixOperations();
mo.ApplyStandardRotations(scanners);

ScannerToScannerConnection[,] connections = mo.CalculateDistancesBetweenScanners(scanners);

var uniqueBeacons = mo.CalculateScanner0ReferecenDistances(scanners, connections);

int maxman = mo.CalculateMaximumManhatanDistance(scanners, connections);

sw.Stop();
Console.WriteLine("Number of unique beacons: {0} in {1} ms", uniqueBeacons.Count(), sw.ElapsedMilliseconds);
Console.WriteLine("Max Manhattan distance: {0}", maxman);


Console.WriteLine("Done. Press enter to end.");
Console.ReadLine();