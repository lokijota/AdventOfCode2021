// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;
using QuikGraph;
using QuikGraph.Algorithms;

Console.WriteLine("Day 15");
string[] map = File.ReadAllLines("data.txt");

const int MAPSIZE = 500; // 100

char[,] largeMap = GenerateV2Map(map);

char[,] GenerateV2Map(string[] map)
{
    char[,] largerMap = new char[MAPSIZE, MAPSIZE];

    // copy the initial values
    for(int r = 0;r < map.Length; r++)
    {
        for (int c = 0; c < map.Length; c++)
            largerMap[r, c] = map[r][c];
    }

    // copy the first 100 element row
    for(int c= map.Length; c<5*map.Length; c++) // 100-500
        for (int r = 0; r < map.Length; r++) // 0-100
        {
            int val = (int)largerMap[r, c-map.Length]  - (int)'0';
            int newval = val + 1 == 10 ? 1 : val + 1;


            largerMap[r, c] = (char)(newval + (int)'0');
        }

    // now copy downwards
    for(int r=map.Length; r<MAPSIZE; r++)
        for (int c=0; c < MAPSIZE; c++)
        {
            int val = (int)largerMap[r-map.Length, c] - (int)'0';

            largerMap[r, c] = (char)(val + 1 == 10 ? '1' : (char)(val + 1 + '0'));
        }

    return largerMap;
}

// Add all the Vertices/Nodes
// is it a bidirectional? or adjacenty
var graph = new AdjacencyGraph<string, Edge<string>>();

for (int r= 0; r<MAPSIZE; r++)
    for (int c = 0; c < MAPSIZE; c++)
    {
        string nodeName = string.Format("{0:000}:{1:000}", r, c);
        graph.AddVertex(nodeName);
    }

// Add all the Edges
for (int r = 0; r < MAPSIZE; r++)
    for (int c = 0; c < MAPSIZE; c++)
    {
        string currentNode = string.Format("{0:000}:{1:000}", r, c);
        
        // add edge to right
        if (c < MAPSIZE-1)
        {
            string rightNode = string.Format("{0:000}:{1:000}", r, c+1);
            Edge<string> toRight = new Edge<string>(currentNode, rightNode);
            graph.AddEdge(toRight);
        }

        // add edge to left
        if(c>0)
        {
            string leftNode = string.Format("{0:000}:{1:000}", r, c - 1);
            Edge<string> toLeft = new Edge<string>(currentNode, leftNode);
            graph.AddEdge(toLeft);
        }

        // add edge up
        if(r>0)
        {
            string upNode = string.Format("{0:000}:{1:000}", r-1, c);
            Edge<string> toUp = new Edge<string>(currentNode, upNode);
            graph.AddEdge(toUp);
        }

        // add edge down
        if (r < MAPSIZE-1)
        {
            string downNode = string.Format("{0:000}:{1:000}", r + 1, c);
            Edge<string> toDown= new Edge<string>(currentNode, downNode);
            graph.AddEdge(toDown);
        }
    }

double EdgeCost(Edge<string> edge)
{
    // the weight is always the weight of the target node
    string[] targetNodeCoords = edge.Target.Split(':');

    char c = largeMap[int.Parse(targetNodeCoords[0]), int.Parse(targetNodeCoords[1])];

    return (int)c - (int)'0';
}

Func<Edge<string>, double> edgeCostReal = EdgeCost;

string root = "000:000";

//TryFunc<string, IEnumerable<string>> 
var tryGetPaths = graph.ShortestPathsDijkstra(edgeCostReal, root);


IEnumerable<Edge<string>> path;
int pathCost = 0, countSteps = 0;
if (tryGetPaths("499:499", out path))
    foreach (var e in path)
    {
        countSteps++;

        pathCost += (int) EdgeCost(e);

        Console.WriteLine(e);
    }

Console.WriteLine("Cost: {0}, Steps={1}", pathCost, countSteps);

// output:
// Cost: 2842, Steps=1006