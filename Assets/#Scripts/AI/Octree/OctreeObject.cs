using UnityEngine;

namespace PathFinding.Octrees
{
    //All the objects that exists within the tree will have their own bounds.
    //This is to understand exactly where the incoming object around the octreeobject.
    [System.Serializable]
    public class OctreeObject
    {
        Bounds bounds;

        public OctreeObject(GameObject gameObject)
        {
            if(gameObject.TryGetComponent<Collider>(out Collider c))
            {
                bounds = c.bounds;
            }
            else
            {
                Debug.LogError($"{gameObject.name} does not have a component of type {typeof(Collider)}");
            }
        }

        public bool Intersects(Bounds boundsToCheck) => bounds.Intersects(boundsToCheck);
    }
}