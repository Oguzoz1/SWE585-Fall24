
using UnityEngine;

namespace PathFinding.Octrees
{
    public class OctreeGenerator : MonoBehaviour
    {
        [Header("Octree Settings")]
        [SerializeField] private GameObject[] _sceneObjects;
        [SerializeField] private float _minNodeSize = 1f;

        public Octree Octree;
        public Graph Waypoints = new Graph();
        public OctreeSO OctreeData;


        public void InitializeOctree()
        {
            Octree = new Octree(_sceneObjects, _minNodeSize, Waypoints);

            SaveOctree();
        }
        private void SaveOctree()
        {
            OctreeData.Octree = Octree;
            //Not serializable
            OctreeData.Octree.Graph.Nodes = Octree.Graph.Nodes;
            OctreeData.Octree.Graph.Edges = Octree.Graph.Edges;
            OctreeData.Octree.Graph._pathList = Octree.Graph._pathList;
        }
        private void LoadOctree()
        {
            Octree = OctreeData.Octree;
            Waypoints = OctreeData.Octree.Graph;
            Octree.Graph.Nodes = OctreeData.Octree.Graph.Nodes;
            Octree.Graph.Edges = OctreeData.Octree.Graph.Edges;
            Octree.Graph._pathList = OctreeData.Octree.Graph._pathList;
        }
        private void Awake()
        {
            Octree = new Octree(_sceneObjects, _minNodeSize, Waypoints);
            //LoadOctree();
            Debug.Log(Waypoints.Edges.Count);
            Debug.Log(Waypoints.Nodes.Count);
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Octree.Bounds.center, Octree.Bounds.size);

            Octree.Root.DrawNode();
        }
    }
}
