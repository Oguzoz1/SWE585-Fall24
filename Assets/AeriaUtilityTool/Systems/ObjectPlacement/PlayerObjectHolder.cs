using AeriaUtil.Pattern.DependencyInjection;
using AeriaUtil.Systems.GameTemplate;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Systems.ObjectPlacement
{
    //DO NOT FORGET TO ADD "Movement" AS A LAYER AND SET THE LAYER IN INPSECTOR FOR THIS CLASS.


    /// <summary>
    /// Controlling Object Grabbing and Placing through conditions.
    /// </summary>
    public class PlayerObjectHolder : MonoBehaviour
    {
        #region Inspector Settings
        [Header("Hold Settings")]
        [Tooltip("Preferred to have an empty gameobject as a camera child for the purpose of placing the held Item")]
        [SerializeField] private Transform _hand;
        [SerializeField] private float _rotateSpeed = 100f;

        [Header("Raycast Setting")]
        [Tooltip("Set this to Default")]
        [SerializeField] private LayerMask _defaultMask;
        [Tooltip("Set this to Movement")]
        [SerializeField] private LayerMask _movementLayer;

        #endregion

        [Inject] private ObjectGrabber _objectGrabber;
        [Inject] private ObjectPlacer _objectPlacer;
        [Inject] private ObjectRotator _objectRotator;
        [Inject] private PlacementStateManager _stateManager;

        private MovableObject _currentHeldObject;
        [SerializeField] private Vector3 _movableObjectPickedLocation;
        private RaycastHit _raycastHit;
        private Camera _main;

        private bool _isGrabbing = false;
        private bool _isPlacingState = false;

        //DI POS
        public PlacementEvent GrabEvent
        {
            get
            {
                _grabEvent = _grabEvent == null ? new PlacementEvent() : _grabEvent;
                return _grabEvent;
            }
        }
        public PlacementEvent PlaceEvent
        {
            get
            {
                _placeEvent = _placeEvent == null ? new PlacementEvent() : _placeEvent;
                return _placeEvent;
            }
        }
        [Header("Events")]
        [SerializeField] private PlacementEvent _grabEvent;
        [SerializeField] private PlacementEvent _placeEvent;


        private void Start() => Init();
        private void Update()
        {
            CancelObjectPlacement();
            _stateManager.Update();
        }
        private void Init()
        {
            _main = Camera.main;
            _stateManager.SetState<IdleState>();
        }

        #region Idle State Methods
        public void UpdateIdling()
        {
            if (TargetIsMovable() && Input.GetKeyUp(PlayerInput.Instance.GetKeycode(KeyInput.INTERACT)))
                _stateManager.SetState<GrabbingState>();
        }
        private bool TargetIsMovable()
        {
            if (Physics.Raycast(_main.ScreenPointToRay(Input.mousePosition), out _raycastHit, 10f, _movementLayer))
            {
                if (_raycastHit.collider.tag == PlacementConstants.MOVABLETAG)
                    return true;
            }
            return false;
        }
        public void EnterIdleState()
        {
            _currentHeldObject?.SetPlacedStateSettings();
            _currentHeldObject = null;
            _isGrabbing = false;
            _isPlacingState = false;
        }
        #endregion
        #region Grabbing State Methods
        public void EnterGrabbingState()
        {
            //Set Current Object
            _currentHeldObject = _currentHeldObject == null ? _raycastHit.transform.GetComponent<MovableObject>() : _currentHeldObject;
            //Set Picked Location
            _movableObjectPickedLocation = _currentHeldObject.transform.position;
            //INVOKE EVENT
            _grabEvent?.Invoke();
            //Set Grabbing State
            SetGrabbingState();
        }
        public void UpdateGrabbing()
        {
            CheckDropInput();
        }
        //Check if the DROP key is pressed.
        private void CheckDropInput()
        {
            //During grabbing, if DROP is pressed and if we are looking at a placable location.

            if (Physics.Raycast(_main.ScreenPointToRay(Input.mousePosition), out _raycastHit, 5f, _currentHeldObject.ObjectLayer))
            {
                if (_isGrabbing && Input.GetKeyUp(PlayerInput.Instance.GetKeycode(KeyInput.DROP)))
                    _stateManager.SetState<PlacingState>();
            }
        }
        private void SetGrabbingState()
        {
            if (!_isGrabbing)
            {
                _objectGrabber.GrabbingObject(_currentHeldObject, _hand);
                _isGrabbing = true;
            }
        }
        #endregion
        #region Placing State Methods
        public void EnterPlacingState()
        {
            //If there is no held object return.
            _currentHeldObject.transform.rotation = Quaternion.Euler(Vector3.zero);
            _isPlacingState = true;

        }
        public void UpdatePlacing() => DisplayObjectAtTarget();
        public void ExitPlacingState()
        {
            //INVOKE EVENT
            _placeEvent?.Invoke();
            _objectPlacer.PlaceObjectToPos(_currentHeldObject, _raycastHit);

        }
        //Display the object at target position before placing the object.
        private void DisplayObjectAtTarget()
        {
            //If already grabbed and stating state is initiated
            if (_isGrabbing && _isPlacingState)
            {
                //If the ray is hitting the layer.
                if (_currentHeldObject != null && Physics.Raycast(_main.ScreenPointToRay(Input.mousePosition), out _raycastHit, 5f, _currentHeldObject.ObjectLayer))
                {
                    _objectPlacer.PlaceObjectToPos(_currentHeldObject, _raycastHit);
                    _objectRotator.RotateObjectOnInputAtTarget(_raycastHit, _currentHeldObject.transform, _rotateSpeed);
                    PlaceObjectAtDisplayedTarget();
                }
            }
        }
        //Place the object at the displayed position
        private void PlaceObjectAtDisplayedTarget()
        {
            if (Input.GetKeyUp(PlayerInput.Instance.GetKeycode(KeyInput.INTERACT)))
            {
                if (!_currentHeldObject.CollisionChecker.IsColliding)
                    _stateManager.SetState<IdleState>();
            }
        }
        private void CancelObjectPlacement()
        {
            if (Input.GetKeyUp(PlayerInput.Instance.GetKeycode(KeyInput.CANCEL)))
            {
                //Reset Movable Ojbect
                if (_currentHeldObject == null) return;

                _currentHeldObject.transform.position = _movableObjectPickedLocation;
                _stateManager.SetState<IdleState>();
            }
        }
        #endregion
    }

    public class PlacementConstants
    {
        public const string MOVABLETAG = "movable";
        public const string MOVEMENTLAYER = "Movement";
    }
}