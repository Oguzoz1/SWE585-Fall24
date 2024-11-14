using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {   [Header("------------------------MOVEMENT------------------------------------------")]
        [Header("Movement Keybinds")]
        [SerializeField] private KeyCode _forwardKey = KeyCode.LeftShift;
        [SerializeField] private KeyCode _backwardKey = KeyCode.LeftControl;
        [SerializeField] private KeyCode _rollLeftKey = KeyCode.A;
        [SerializeField] private KeyCode _rollRightKey = KeyCode.D;
        [SerializeField] private KeyCode _yawLeftKey = KeyCode.Q;
        [SerializeField] private KeyCode _yawRightKey = KeyCode.E;
        [SerializeField] private KeyCode _upwardKey = KeyCode.Space;
        [SerializeField] private KeyCode _downwardKey = KeyCode.C;
        [SerializeField] private KeyCode _pitchUpwardKey = KeyCode.S;
        [SerializeField] private KeyCode _pitchDownwardKey = KeyCode.W;

        [Header("General Speed Settings")]
        [SerializeField] private float _thrustPower = 1000f;
        [SerializeField] private float _rotationSpeed = 60f;
        [SerializeField] private float _maxSpeed = 100f;

        [Header("---ROLL AXIS SETTINGS---")]
        [Header("Roll Axis Spring Effect Settings")]
        [SerializeField] private float _springStrength = 5f; 
        [SerializeField] private float _dampingFactor = 0.3f;
        [SerializeField] private float _rollRotationSpeed = 60f;

        private Rigidbody _playerRb;
        private AudioSource _audio;

        //AUDIO RELATED
        private float _currentAudioPitch = 0f;

        //SPRING AND DAMPING
        private Vector3 _rotationVelocity = Vector3.zero;
        public static bool MovementActivated = false;
        private void Awake()
        {
            _playerRb = GetComponent<Rigidbody>();
            _playerRb.useGravity = false;

            _audio = GetComponent<AudioSource>();
        }
        private void FixedUpdate() => ApplyMovement();

        private void ApplyMovement()
        {
            ApplyRotation();
            Movement();
            ClampSpeedAtLimit();
        }
        private void ApplyRotation()
        {
            HandleMovementSound();
            Rotate();
        }
        private void HandleMovementSound()
        {
            float targetPitch = Input.GetKey(_forwardKey) ? 1f : 0f;

            _currentAudioPitch = Mathf.MoveTowards(_currentAudioPitch, targetPitch, 0.75f * Time.deltaTime);
            _audio.pitch = _currentAudioPitch;
        }
        private void Movement()
        {
            //Forward Movement
            Vector3 forwardForce = Vector3.zero;

            if (Input.GetKey(_forwardKey))
            {
                forwardForce = transform.forward * _thrustPower * Time.fixedDeltaTime;
            }
            else if (Input.GetKey(_backwardKey))
            {
                forwardForce = -transform.forward * _thrustPower * Time.fixedDeltaTime;
            }

            //Upward Movement
            Vector3 verticalForce = Vector3.zero;
            if (Input.GetKey(_upwardKey))
            {
                verticalForce = transform.up * _thrustPower * Time.fixedDeltaTime;
            }
            else if (Input.GetKey(_downwardKey))
            {
                verticalForce = -transform.up * _thrustPower * Time.fixedDeltaTime;
            }

            _playerRb.AddForce(forwardForce + verticalForce);
        }

        private void Rotate()
        {
            // Use fixedDeltaTime since FixedUpdate is using fixed intervals.
            float yaw = _rotationSpeed * Time.deltaTime;
            float roll = _rollRotationSpeed * Time.deltaTime;
            float pitch = _rotationSpeed * Time.deltaTime;

            bool isYawing = false, isRolling = false, isPitching = false;
            MovementActivated = false;
            // Yaw Rotation
            if (Input.GetKey(_yawLeftKey))
            {
                transform.Rotate(0, -yaw, 0);
                isYawing = true;
                MovementActivated = true;
            }
            else if (Input.GetKey(_yawRightKey))
            {
                transform.Rotate(0, yaw, 0);
                isYawing = true;
                MovementActivated = true;

            }

            // Roll Rotation
            if (Input.GetKey(_rollLeftKey))
            {
                transform.Rotate(0, 0, roll);
                isRolling = true;
                MovementActivated = true;

            }
            else if (Input.GetKey(_rollRightKey))
            {
                transform.Rotate(0, 0, -roll);
                isRolling = true;
                MovementActivated = true;

            }

            // Pitch Rotation (set target pitch based on input)
            if (Input.GetKey(_pitchDownwardKey))
            {
                transform.Rotate(pitch, 0, 0);
                // Increment target pitch
                isPitching = true;
                MovementActivated = true;

            }
            else if (Input.GetKey(_pitchUpwardKey))
            {
                transform.Rotate(-pitch, 0, 0);
                isPitching = true;
                MovementActivated = true;

            }

            // Rotating Original
            if (!isRolling && !isPitching && !isYawing)
            {
                RollSpringDampRotationToTarget();
            }
        }

        // Normalize angles to ensure they stay within -180 to 180 degrees
        private float NormalizeAngle(float angle)
        {
            angle %= 360;
            if (angle > 180) angle -= 360;
            else if (angle < -180) angle += 360;
            return angle;
        }

        private void RollSpringDampRotationToTarget()
        {
            // Use a fixed target rotation for stabilization
            Vector3 targetRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0); // Keep the yaw the same
            Vector3 rotationError = targetRotation - transform.localEulerAngles;
            // Ensure the rotation values are within a sensible range to avoid wrapping issues
            rotationError.x = NormalizeAngle(rotationError.x);
            rotationError.z = NormalizeAngle(rotationError.z);

            // Spring and Damping effect calculation
            _rotationVelocity += (rotationError * _springStrength - _rotationVelocity * _dampingFactor) * Time.deltaTime;

            transform.localEulerAngles += _rotationVelocity * Time.deltaTime;

        }
        private void ClampSpeedAtLimit()
        {
            if (_playerRb.velocity.magnitude > _maxSpeed)
            {
                _playerRb.velocity = _playerRb.velocity.normalized * _maxSpeed;
            }
        }
    }

}
