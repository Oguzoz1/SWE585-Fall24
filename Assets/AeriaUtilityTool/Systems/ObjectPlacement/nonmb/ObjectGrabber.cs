using AeriaUtil.Pattern.DependencyInjection;
using AeriaUtil.Systems.GameTemplate;
using System;
using UnityEngine;

namespace AeriaUtil.Systems.ObjectPlacement
{
    /// <summary>
    /// Responsible of grabbing any object, changing its values to grabbed state and attaching it to a position.
    /// ServiceProvider for dependency injection is required.
    /// </summary>

    public class ObjectGrabber : IPlacementService
    {
        private ObjectAttacher _objectAttacher = new ObjectAttacher();

        public void GrabbingObject(MovableObject currentHeldObject, Transform targetTransform)
        {
            currentHeldObject.transform.rotation = Quaternion.Euler(Vector3.zero);

            //Grab State Settings
            currentHeldObject.SetGrabbedStateSettings();
            GrabObjectbyChilding(currentHeldObject.transform, targetTransform);
        }
        public void GrabObjectbyChilding(Transform grabbedObject, Transform hand)
        {
            _objectAttacher.AttachObjectToTargetByChild(grabbedObject.gameObject, hand.gameObject);
            grabbedObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
        public void GrabObjectToPos(Transform grabbedObject, Vector3 grabbedPos)
        {
            _objectAttacher.AttachObjectToTargetByPos(grabbedObject, grabbedPos);
        }

    }
}
