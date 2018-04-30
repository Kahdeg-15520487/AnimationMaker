using System.Collections.Generic;

namespace Utilities.PathFinding
{
    //actually we need the whole graph after the algorthm calculate the path.

    namespace Dijkstras
    {
        public class Graph
        {
            //vertex list aka 
            //the dictionary of distance from a Point to another Point
            //this thing use string to label the Points
            public Dictionary<string, Dictionary<string, int>> Vertices = new Dictionary<string, Dictionary<string, int>>();

            //the shortest path from one point to another point.
            public Dictionary<string, string> Pathlist = new Dictionary<string, string>();

            //the source, the starting point, the origin of the graph
            //usually the location of a Unit
            public string Source;

            //the Maxinum Travel Cost to any point from the source
            public int MaxCost = int.MaxValue;

            //add a Point with its associated distance to nearby Points
            public void add_vertex(string name, Dictionary<string, int> edges)
            {
                Vertices[name] = edges;
            }

            //TODO make a range calculate

            public List<string> FindShortestPath(string destination)
            {
                List<string> path = null;

                if (Pathlist.ContainsKey(destination))
                {
                    path = new List<string>();
                    string currentNode = destination;
                    while (currentNode.CompareTo(Source) != 0)
                    {
                        path.Add(currentNode);
                        currentNode = Pathlist[currentNode];
                    }
                    path.Add(Source);
                    path.Reverse();
                }

                return path;
            }

            public int CalculateShortestPathCost(string destination)
            {
                int cost = int.MaxValue;

                if (Pathlist.ContainsKey(destination))
                {
                    cost = 0;
                    string currentNode = destination;
                    while (currentNode.CompareTo(Source) != 0)
                    {
                        cost += Vertices[currentNode][Pathlist[currentNode]];
                        currentNode = Pathlist[currentNode];
                    }
                }

                return cost;
            }


            public List<string> FindReachableVertex()
            {
                List<string> reachableVertex = new List<string>();

                foreach (string dest in Pathlist.Keys)
                {
                    int cost = CalculateShortestPathCost(dest);
                    if (cost<=MaxCost)
                    {
                        reachableVertex.Add(dest);
                    }
                }

                return reachableVertex;
            }

            /// <summary>
            /// find the shortest path between 2 Point
            /// </summary>
            /// <param name="source"> the label of the starting Point</param>
            /// <param name="finish">the label of the destination Point</param>
            /// <returns>the List of the Points that is on the ways</returns>
            public void Dijkstra(string source,int maxCost = int.MaxValue)
            {
                Source = source;
                MaxCost = maxCost;
                var previous = new Dictionary<string, string>();
                var distances = new Dictionary<string, int>();
                var nodes = new List<string>();

                //initialize dijktra table
                //set the distances between of the starting Point to 0 cause we"re already here
                //set every distances between every point to horizontal 8
                foreach (var vertex in Vertices)
                {
                    if (vertex.Key == source)
                    {
                        distances[vertex.Key] = 0;
                    }
                    else
                    {
                        distances[vertex.Key] = int.MaxValue;
                    }

                    nodes.Add(vertex.Key);
                }
                
                while (nodes.Count != 0)
                {
                    //sort the Point based on the distances of each Point to the previous Point
                    nodes.Sort((x, y) => distances[x] - distances[y]);

                    //then the smallest node is added to the potential Path
                    var smallest = nodes[0];
                    nodes.Remove(smallest);

                    //if the distances of such smallest point is still horizontal 8
                    //then sadly there is not a viable Path
                    if (distances[smallest] == int.MaxValue)
                    {
                        break;
                    }

                    //loop through the surrounding Point of the smallest
                    //to see if there is a Potential Shortest distance
                    foreach (var neighbor in Vertices[smallest])
                    {
                        var alt = distances[smallest] + neighbor.Value;
                        if (alt < distances[neighbor.Key] && alt < MaxCost)
                        {
                            distances[neighbor.Key] = alt;
                            previous[neighbor.Key] = smallest;
                        }
                    }
                }

                foreach (var kvp in previous)
                {
                    Pathlist.Add(kvp.Key, kvp.Value);
                }
            }
        }
    }
}
