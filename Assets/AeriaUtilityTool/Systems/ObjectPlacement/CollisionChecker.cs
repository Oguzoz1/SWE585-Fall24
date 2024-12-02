using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Systems.ObjectPlacement
{
    /// <summary>
    /// Responsible of returning a bool if the object is colliding or not.
    /// </summary>
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class CollisionChecker : MonoBehaviour
    {
        public bool IsColliding { get { return _isColliding; } }
        private bool _isColliding;

        public Collider Collider
        {
            get
            {
                _collider = _collider == null ? GetComponent<Collider>() : _collider;
                return _collider;
            }
        }
        private Collider _collider;

        private void OnTriggerStay(Collider other)
        {
            _isColliding = true;
            Debug.Log(IsColliding);
        }

        private void OnTriggerExit(Collider other)
        {
            _isColliding = false;
            Debug.Log(IsColliding);
        }
    }
}
