using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding.Octrees
{
    public class Octree
    {
        public OctreeNode root;

        //Axis Alligned bounding box
        public Bounds bounds;

        public Octree(GameObject[] sceneObjects, float minNodeSize)
        {
            CalculateBounds(sceneObjects);
            CreateTree(sceneObjects, minNodeSize);
        }


        //Maybe scene objects can be obstacle type?
        private void CalculateBounds(GameObject[] sceneObjects)
        {
            //Assuming all gameobjects have colliders on them.
            foreach (var obj in sceneObjects)
            {
                if (obj.TryGetComponent<Collider>(out Collider c))
                {
                    bounds.Encapsulate(c.bounds);
                }
                else
                {
                    Debug.LogError($"{obj.name} does not have a component of type {typeof(Collider)}");
                }
            }

            //Make the bounds equal in size by making it cube
            //Get the largest value and create a vector with equal components out of it. 
            //Divide it into two, to make the cube from the center
            Vector3 size = Vector3.one * Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z) * 0.5f;
            bounds.SetMinMax(bounds.center - size, bounds.center + size);

        }

        private void CreateTree(GameObject[] sceneObjects, float minNodeSize)
        {
            root = new OctreeNode(bounds, minNodeSize);

            foreach(var so in sceneObjects)
            {
                root.Divide(so);
            }
        }
    }

}
