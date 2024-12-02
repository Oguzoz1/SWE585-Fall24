using AeriaUtil.Pattern.DependencyInjection;
using UnityEngine;

namespace AeriaUtil.Systems.ObjectPlacement
{
    public class ObjectPlacer : IPlacementService
    {
        private ObjectAttacher _objectAttacher = new ObjectAttacher();

        /// <summary>
        /// Place Object to raycast hit point.
        /// </summary>
        /// <param name="currentHeldObject"></param>
        /// <param name="raycastHit"></param>
        public void PlaceObjectToPos(MovableObject currentHeldObject, RaycastHit raycastHit)
        {
            //Free from parent
            currentHeldObject.transform.parent = null;

            //Set offset
            Vector3 desiredPoint = raycastHit.point + raycastHit.normal * OffSet(currentHeldObject.transform, raycastHit);

            _objectAttacher.AttachObjectToTargetByPos(currentHeldObject.transform, desiredPoint);
        }

        /// <summary>
        /// Place Object to target vector position.
        /// </summary>
        /// <param name="currentHeldObject"></param>
        /// <param name="target"></param>
        public void PlaceObjectToPos(MovableObject currentHeldObject, Vector3 target, Vector3 offset)
        {
            //Free from parent
            currentHeldObject.transform.parent = null;

            //Set offset
            float yOffset = (float)(currentHeldObject.transform.localScale.y * 0.5);
            Vector3 desiredPoint = new Vector3(target.x + offset.x, target.y + offset.y, target.z + offset.z);

            //Fix Orientation
            currentHeldObject.transform.rotation = Quaternion.Euler(Vector3.zero);

            _objectAttacher.AttachObjectToTargetByPos(currentHeldObject.transform, desiredPoint);
        }


        public bool IsPlacable(CollisionChecker checker)
        {
            if (checker.IsColliding)
            {
                Debug.Log("IS COLLIDING");
                return false;
            }
                
            else return true;
        }

        /// <summary>
        /// Return offset relative to surface normal.
        /// </summary>
        /// <param name="hit"></param>
        /// <returns></returns>
        private float OffSet(Transform transform, RaycastHit hit)
        {
            if (hit.normal == Vector3.down)
            {
                return transform.localScale.z * 0.5f;
            }
            else if (hit.normal == Vector3.right ||
                hit.normal == Vector3.left ||
                hit.normal == Vector3.forward ||
                hit.normal == Vector3.back)
            {
                return transform.localScale.z * 0.5f;
            }
            else
            {
                return transform.localScale.y * 0.5f;
            }
        }
    }
}
