using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding.Octrees
{
    public class OctreeNode
    {
        //Game Objects that are inside or partially inside of the node.
        public List<OctreeObject> _octreeObjects = new List<OctreeObject>();

        //Node ID
        private static int s_nextId;
        public readonly int Id;

        public Bounds bounds;
        private Bounds[] _childBounds = new Bounds[8];

        public OctreeNode[] Children;
        public bool IsLeaf => Children == null;

        private float _minNodeSize;

        public OctreeNode(Bounds bounds, float minNodeSize)
        {
            //IncrementID
            Id = s_nextId++;

            this.bounds = bounds;
            this._minNodeSize = minNodeSize;

            CreateOctreeNode();

        }

        public void Divide(GameObject so) => Divide(new OctreeObject(so));

        private void Divide(OctreeObject octreeObject)
        {
            if (bounds.size.x <= _minNodeSize)
            {
                AddObject(octreeObject);
                return;
            }

            //if children array is not initailized yet.
            Children ??= new OctreeNode[8];

            //We want to divide untill we make sure the gameobject is entirely covered by the nodes
            bool intersectedChild = false;

            //Initialize Children and if octree object intersects the childbound, we will keep dividing to make sure we cover all.
            //if no intersection is found, object does not fit in any child node.
            for (int i = 0; i < 8; i++)
            {
                Children[i] ??= new OctreeNode(_childBounds[i], _minNodeSize);

                if (octreeObject.Intersects(_childBounds[i]))
                {
                    Children[i].Divide(octreeObject);
                    intersectedChild = true;
                }

                if (!intersectedChild)
                {
                    AddObject(octreeObject);
                }
            }
        }
        private void AddObject(OctreeObject octreeObject) => _octreeObjects.Add(octreeObject);
        public void DrawNode()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(bounds.center, bounds.size);

            foreach (OctreeObject obj in _octreeObjects)
            {
                if (obj.Intersects(bounds))
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireCube(bounds.center, bounds.size);
                }
            }

            if (Children != null)
            {
                foreach (OctreeNode child in Children)
                {
                    if (child != null) child.DrawNode();
                }
            }
        }
        private void CreateOctreeNode()
        {
            Vector3 newSize = bounds.size * 0.5f; //Half the size
            Vector3 centerOffset = bounds.size * 0.25f; //Quarter the offset gives the center
            Vector3 parentCenter = bounds.center; //Determine the parent center

            for (int i = 0; i < 8; i++)
            {
                //Assuming child center is parent center
                Vector3 childCenter = parentCenter;
                //Check the bits of given value of i to determine to which orientation center will be placed
                //Example of i = 5
                //We are compaing each bit, if 1 meets 1 we return 1 else 0.
                //5 & 1, Decimal: 5, Binary: 101, Binary 1: 001, 5 & 1 != 0, result: 1
                childCenter.x += centerOffset.x * ((i & 1) == 0 ? -1 : 1);
                //5 & 2, Decimal: 5, Binary: 101, Binary 2: 010, 5 & 2 == 0, result: -1
                childCenter.y += centerOffset.y * ((i & 2) == 0 ? -1 : 1);
                //5 & 1, Decimal: 5, Binary: 101, Binart 4: 100, 5 & 4 != 0, result: 1
                childCenter.z += centerOffset.z * ((i & 4) == 0 ? -1 : 1);

                //But why not i & 3 but i & 4? 
                //Because up to 4, there is no binary value for the bit on the far left. It is always 0. (000, 001,010,011,100(4))
                //Therefore to be able to enable that value, we have to start from 4 which is "100"
                //And we are checking on z value and z value is represented with the most significant bit.

                //Creates a bounding box from center to the newsize
                _childBounds[i] = new Bounds(childCenter, newSize);
            }
        }
    }
}