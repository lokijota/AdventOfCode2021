// See https://aka.ms/new-console-template for more information

using QuikGraph;

// Load data
string[] edgesAsString = File.ReadAllText("data.txt").Replace("\r", "").Split('\n');

var graph = new BidirectionalGraph<string, Edge<string>>();

foreach (string edgeAsString in edgesAsString)
{
    string [] vertexPair = edgeAsString.Split('-');

    if (!graph.ContainsVertex(vertexPair[0]))
        graph.AddVertex(vertexPair[0]);

    if (!graph.ContainsVertex(vertexPair[1]))
        graph.AddVertex(vertexPair[1]);

    graph.AddEdge(new Edge<string>(vertexPair[0], vertexPair[1]));
    graph.AddEdge(new Edge<string>(vertexPair[1], vertexPair[0]));
}

// small optimization
graph.ClearInEdges("start");
graph.ClearOutEdges("end");

Console.WriteLine("Vertex count: {0}, Edge Count: {1}", graph.VertexCount, graph.EdgeCount);

PrintAllPaths(graph, "start", "end");
Console.WriteLine("done");


void PrintAllPaths(BidirectionalGraph<string, Edge<string>> graph, string start, string end)
{
    List<string> pathList = new List<string>();
    List<string> smallCavesVisitedOnce = new List<string>();
    List<string> smallCavesVisitedTwice = new List<string>();

    pathList.Add(start);

    PrintAllPathsRecursive(graph, start, end, pathList, smallCavesVisitedOnce, smallCavesVisitedTwice);
    Console.ReadLine();
}

// Recursive method
void PrintAllPathsRecursive(BidirectionalGraph<string, Edge<string>> graph, string current, string end, List<string> pathList, List<string> smallCavesVisitedOnce, List<string>  smallCavesVisitedTwice)
{
    if (current == end)
    {
        Console.WriteLine(string.Join(" ", pathList));
        return;
    }

    // process small caves
    if (current.Any(char.IsLower))
    {
        if (smallCavesVisitedOnce.Contains(current)) // visited once
            if (smallCavesVisitedTwice.Count() > 0)
                return; // abort path
            else
                smallCavesVisitedTwice.Add(current);
        else
            smallCavesVisitedOnce.Add(current);
    } 

    IEnumerable<Edge<string>> outEdges;
    bool findOutEdges = graph.TryGetOutEdges(current, out outEdges);

    foreach (Edge<string> edge in outEdges)
    {
        // skip if already visited
        if (smallCavesVisitedOnce.Contains(edge.Target) && smallCavesVisitedTwice.Count() > 0) // it only passes the first condition if it's lowercase
            continue;

        pathList.Add(edge.Target);
        PrintAllPathsRecursive(graph, edge.Target, end, pathList, smallCavesVisitedOnce, smallCavesVisitedTwice);
        pathList.RemoveAt(pathList.Count - 1);
    }

    // the removal code has to be the logical inverse of the one at the top to process small caves:
    // if it's on the Twice collection, remove it
    bool removedFromTwice = smallCavesVisitedTwice.Remove(current);

    // if not emovedFromTwice, remove from Once
    int lastFoundPos = smallCavesVisitedOnce.FindLastIndex(x => (x.CompareTo(current) == 0));
    if (!removedFromTwice && lastFoundPos > -1)
        smallCavesVisitedOnce.RemoveAt(lastFoundPos);

    // For part 1 of exercise:
    // smallCavesVisitedOnce.Remove(current); // has to remove the last only
    // Console.WriteLine(lastFoundPos);

}