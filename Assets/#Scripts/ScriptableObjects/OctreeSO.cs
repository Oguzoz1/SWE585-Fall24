using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding.Octrees
{
    [CreateAssetMenu(fileName = "OctreeData", menuName = "ScriptableObjects/OctreeData")]
    public class OctreeSO : ScriptableObject
    {
        public Octree Octree;
    }
}
