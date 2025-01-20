using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding.Octrees
{
    [System.Serializable]
    public class Octree
    {
        public OctreeNode Root;

        //Axis Alligned bounding box
        public Bounds Bounds;
        public Graph Graph;

        public List<OctreeNode> _emptyLeaves = new List<OctreeNode>();

        
        public Octree(GameObject[] sceneObjects, float minNodeSize, Graph waypoints)
        {
            CalculateBounds(sceneObjects);
            CreateTree(sceneObjects, minNodeSize);
            Graph = waypoints;

            GetEmptyLeaves(Root);
            GetEdges();
            Debug.Log(Graph.Edges.Count);
        }

        public OctreeNode FindClosestNode(Vector3 position) => FindClosestNode(Root, position);

        public OctreeNode FindClosestNode(OctreeNode node, Vector3 position)
        {
            OctreeNode found = null;
            for (int i = 0; i < node.Children.Length; i++)
            {
                if (node.Children[i].bounds.Contains(position))
                {
                    if (node.Children[i].IsLeaf)
                    {
                        found = node.Children[i];
                        break;
                    }
                    found = FindClosestNode(node.Children[i], position);
                }
            }
            return found;
        }


        private void GetEdges()
        {
            foreach(OctreeNode leaf in _emptyLeaves)
            {
                foreach(OctreeNode otherLeaf in _emptyLeaves)
                {
                    if (leaf.bounds.Intersects(otherLeaf.bounds))
                    {
                        Graph.AddEdge(leaf, otherLeaf);
                    }
                }
            }
        }

        //Maybe scene objects can be obstacle type?
        private void CalculateBounds(GameObject[] sceneObjects)
        {
            //Assuming all gameobjects have colliders on them.
            foreach (var obj in sceneObjects)
            {
                if (obj.TryGetComponent<Collider>(out Collider c))
                {
                    Bounds.Encapsulate(c.bounds);
                }
                else
                {
                    Debug.LogError($"{obj.name} does not have a component of type {typeof(Collider)}");
                }
            }

            //Make the bounds equal in size by making it cube
            //Get the largest value and create a vector with equal components out of it. 
            //Divide it into two, to make the cube from the center
            Vector3 size = Vector3.one * Mathf.Max(Bounds.size.x, Bounds.size.y, Bounds.size.z) * 0.6f;
            Bounds.SetMinMax(Bounds.center - size, Bounds.center + size);

        }

        private void GetEmptyLeaves(OctreeNode node)
        {
            if (node.IsLeaf && node._octreeObjects.Count == 0)
            {
                _emptyLeaves.Add(node);
                Graph.AddNode(node);
            }

            if (node.Children == null) return;

            foreach(OctreeNode child in node.Children)
            {
                GetEmptyLeaves(child);
            }

            for (int i = 0; i < node.Children.Length; i++)
            {
                for (int j = 0; j < node.Children.Length; j++)
                {
                    Graph.AddEdge(node.Children[i], node.Children[j]);
                }
            }
        }

        private void CreateTree(GameObject[] sceneObjects, float minNodeSize)
        {
            Root = new OctreeNode(Bounds, minNodeSize);

            foreach(var so in sceneObjects)
            {
                Root.Divide(so);
            }
        }
    }

}
