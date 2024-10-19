using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _thrustPower = 10f;
        [SerializeField] private float _rotationSpeed = 100f;
        [SerializeField] private float _maxSpeed = 20f;

        private Rigidbody _playerRb;
        private AudioSource _audio;

        private float _currentPitch = 0f;
        private void Awake()
        {
            _playerRb = GetComponent<Rigidbody>();
            _playerRb.useGravity = false;

            _audio = GetComponent<AudioSource>();
        }

        private void FixedUpdate() => ApplyMovement();

        private void ApplyMovement()
        {
            HandleMovementSound();
            Movement();
            Rotate();
            ClampSpeedAtLimit();
        }
        private void HandleMovementSound()
        {
            float targetPitch = Input.GetKey(KeyCode.W) ? 1f : 0f;

            _currentPitch = Mathf.MoveTowards(_currentPitch, targetPitch, 0.75f * Time.deltaTime);
            _audio.pitch = _currentPitch;
        }
        private void Movement()
        {
            //Forward Movement
            float thrust = Input.GetAxis("Vertical") * _thrustPower;
            Vector3 forwardForce = transform.forward * thrust * Time.fixedDeltaTime; //Force direction and thrust giving vector for us to move into.

            //Upward Movement
            Vector3 verticalForce = Vector3.zero;
            if (Input.GetKey(KeyCode.Space))
            {
                verticalForce = transform.up * _thrustPower * Time.fixedDeltaTime;
            }
            else if (Input.GetKey(KeyCode.C))
            {
                verticalForce = -transform.up * _thrustPower * Time.fixedDeltaTime;
            }

            _playerRb.AddForce(forwardForce + verticalForce);
        }

        private void Rotate()
        {
            //Use fixedDeltaTime since fixedupdate is using fixed intervals.
            float yaw = Input.GetAxis("Horizontal") * _rotationSpeed * Time.fixedDeltaTime;
            transform.Rotate(0, yaw, 0);

            float rot = _rotationSpeed * Time.fixedDeltaTime;

            if (Input.GetKey(KeyCode.Q))
            {
                transform.Rotate(0, 0, rot);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                transform.Rotate(0, 0, -rot);
            }

            if (Input.GetKey(KeyCode.R))
            {
                transform.Rotate(rot, 0, 0);
            }
            else if (Input.GetKey(KeyCode.F))
            {
                transform.Rotate(-rot, 0, 0);
            }
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
