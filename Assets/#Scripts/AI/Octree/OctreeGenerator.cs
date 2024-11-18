
using UnityEngine;

namespace PathFinding.Octrees
{
    public class OctreeGenerator : MonoBehaviour
    {
        [Header("Octree Settings")]
        [SerializeField] private GameObject[] _sceneObjects;
        [SerializeField] private float _minNodeSize = 1f;

        private Octree _octree;

        private void Awake() => InitializeOctree();
        private void InitializeOctree()
        {
            _octree = new Octree(_sceneObjects, _minNodeSize);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_octree.bounds.center, _octree.bounds.size);

            _octree.root.DrawNode();
        }
    }
}
