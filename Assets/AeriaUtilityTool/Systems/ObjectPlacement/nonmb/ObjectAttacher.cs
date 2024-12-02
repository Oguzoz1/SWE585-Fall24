using UnityEngine;

namespace AeriaUtil.Systems.ObjectPlacement
{
    /// <summary>
    /// A service responsible of changing position of an object to a specified position. 
    /// </summary>
    public class ObjectAttacher : IPlacementService
    {
        /// <summary>
        /// Making the object child of the target, and resetting position.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="target"></param>
        public void AttachObjectToTargetByChild(GameObject obj, GameObject target)
        {
            obj.transform.SetParent(target.transform);
            obj.transform.localPosition = Vector3.zero;
        }
        /// <summary>
        /// Set GameObject to target's pos.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="target"></param>
        public void AttachObjectToTargetByPos(Transform obj, Transform target)
        {
            obj.position = target.position;
        }
        /// <summary>
        /// Set GameObject to target's pos.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="target"></param>
        public void AttachObjectToTargetByPos(Transform obj, Vector3 target)
        {
            obj.position = target;
        }
        /// <summary>
        /// Set GameObject to target's pos.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="target"></param>
        public void AttachObjectToTargetByPos(Transform obj, Vector2 target)
        {
            obj.position = target;
        }
    }
}

