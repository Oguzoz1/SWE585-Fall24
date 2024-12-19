using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PathFinding.Octrees
{
    [System.Serializable]
    public class Node
    {
        private static int s_nextId;
        public readonly int Id;

        public float f, g, h;
        public Node From;

        public List<Edge> Edges = new List<Edge>();
        public OctreeNode OctreeNode;

        public Node(OctreeNode octreeNode)
        {
            this.Id = s_nextId++;
            this.OctreeNode = octreeNode;
        }

        public override bool Equals(object obj) => obj is Node other && Id == other.Id;
        public override int GetHashCode() => Id.GetHashCode();
    }
    [System.Serializable]
    public class Edge
    {
        public readonly Node a, b;

        public Edge(Node a, Node b)
        {
            this.a = b;
            this.b = a;
        }

        public override bool Equals(object obj)
        {
            return obj is Edge other && ((a == other.a && b == other.b)
                || (a == other.b && b == other.a));
        }

        //We want the hashcode for edge ab and edge ba to be same.
        public override int GetHashCode()
        {
            return a.GetHashCode() ^ b.GetHashCode();
        }
    }
    [System.Serializable]
    public class NodeComparer : IComparer<Node>
    {
        //Least expensive node to travel through will always be the first ones to get out of it.
        public int Compare(Node x, Node y)
        {
            if (x == null || y == null) return 0;

            int compare = x.f.CompareTo(y.f);

            if (compare == 0)
            {
                return x.Id.CompareTo(y.Id);
            }

            return compare;
        }
    }

    [System.Serializable]
    public class Graph
    {
        public Dictionary<OctreeNode, Node> Nodes = new Dictionary<OctreeNode, Node>();
        public HashSet<Edge> Edges = new HashSet<Edge>();

        public List<Node> _pathList = new List<Node>();
        const int MAXITERATION = 10000;

        public int GetPathLength() => _pathList.Count;
        public OctreeNode GetPathNode(int index)
        {
            if (_pathList == null) return null;

            if (index < 0 || index >= _pathList.Count)
            {
                Debug.LogError($"Index out of bounds. Path length: {_pathList.Count}, Index: {index}");
                return null;
            }
            return _pathList[index].OctreeNode;
        }

        public bool AStar(OctreeNode startNode, OctreeNode endNode)
        {
            _pathList.Clear();
            Node start = FindNode(startNode);
            Node end = FindNode(endNode);

            if (start == null || end == null)
            {
                Debug.LogError("Start or End node not found in the graph");
                return false;
            }

            SortedSet<Node> openSet = new SortedSet<Node>(new NodeComparer());
            HashSet<Node> closedSet = new HashSet<Node>();

            int iterationCount = 0;

            start.g = 0;
            start.h = Heuristic(start, end);
            start.f = start.g + start.h;
            start.From = null;
            openSet.Add(start)
;           
            while (openSet.Count > 0)
            {
                if (++iterationCount > MAXITERATION)
                {
                    Debug.LogError("A* exceeded maximum iterations");
                    return false;
                }

                Node current = openSet.First();
                openSet.Remove(current);

                if (current.Equals(end))
                {
                    ReconstructPath(current);
                    return true;
                }

                closedSet.Add(current);

                foreach (Edge edge in current.Edges)
                {
                    Node neighbor = Equals(edge.a, current) ? edge.b : edge.a;

                    if (closedSet.Contains(neighbor)) continue;

                    float tentative_gScore = current.g + (current.OctreeNode.bounds.center - neighbor.OctreeNode.bounds.center).sqrMagnitude;

                    if (tentative_gScore < neighbor.g || !openSet.Contains(neighbor))
                    {
                        neighbor.g = tentative_gScore;
                        neighbor.h = Heuristic(neighbor, end);
                        neighbor.f = neighbor.g + neighbor.h;
                        neighbor.From = current;
                        openSet.Add(neighbor);
                    }
                }
            }

            Debug.Log("No path found");
            return false;
        }

        private void ReconstructPath(Node current)
        {
            while (current != null)
            {
                _pathList.Add(current);
                current = current.From;
            }

            _pathList.Reverse();
        }

        //Heuristic Function to calculate estimated cost
        float Heuristic(Node a, Node b) => (a.OctreeNode.bounds.center - b.OctreeNode.bounds.center).sqrMagnitude;

        public void AddNode(OctreeNode octreeNode)
        {
            if (!Nodes.ContainsKey(octreeNode))
            {
                Nodes.Add(octreeNode, new Node(octreeNode)); 
            }
        }

        public void AddEdge(OctreeNode a, OctreeNode b)
        {
            Node nodeA = FindNode(a);
            Node nodeB = FindNode(b);

            if (nodeA == null || nodeB == null) return;

            Edge edge = new Edge(nodeA, nodeB);
            if (Edges.Add(edge))
            {
                nodeA.Edges.Add(edge);
                nodeB.Edges.Add(edge);
            }
        }

        private Node FindNode(OctreeNode octreeNode)
        {
            Nodes.TryGetValue(octreeNode, out Node node);
            return node;
        }

        public void DrawGraph()
        {
            Gizmos.color = Color.red;

            foreach(Edge edge in Edges)
            {
                Gizmos.DrawLine(edge.a.OctreeNode.bounds.center, edge.b.OctreeNode.bounds.center);
            }

            foreach(Node node in Nodes.Values)
            {
                Gizmos.DrawWireSphere(node.OctreeNode.bounds.center, 0.2f);
            }
        }
    }
}