using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Systems.ObjectPlacement
{
    [RequireComponent(typeof(CollisionChecker))]
    public class MovableObject : MonoBehaviour
    {
        public LayerMask ObjectLayer { get { return _objectLayer; } }
        [SerializeField] private LayerMask _objectLayer;
        [SerializeField] private bool _isKinematicAllTheTime;


        private Rigidbody _rb;
        public CollisionChecker CollisionChecker
        {
            get
            {
                _collisionChecker = _collisionChecker == null ? GetComponent<CollisionChecker>() : _collisionChecker;
                return _collisionChecker;
            }
        }
        private CollisionChecker _collisionChecker;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();

            InitMovableObject();
        }

        private void InitMovableObject()
        {
            gameObject.tag = PlacementConstants.MOVABLETAG;
            int layer = LayerMask.NameToLayer(PlacementConstants.MOVEMENTLAYER);
            gameObject.layer = layer;

            _rb.isKinematic = false;
            CollisionChecker.Collider.isTrigger = false;
        }

        public void SetGrabbedStateSettings()
        {
            _rb.isKinematic = true;
            CollisionChecker.Collider.isTrigger = true;
        }

        public void SetPlacedStateSettings()
        {
            _rb.isKinematic = _isKinematicAllTheTime;
            CollisionChecker.Collider.isTrigger = false;
            transform.parent = null;
        }
    }
}
