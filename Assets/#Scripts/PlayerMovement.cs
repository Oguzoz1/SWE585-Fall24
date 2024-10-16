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

        private void Awake()
        {
            _playerRb = GetComponent<Rigidbody>();
            _playerRb.useGravity = false;
        }

        private void FixedUpdate() => ApplyMovement();

        private void ApplyMovement()
        {
            Movement();
            Rotate();
            ClampSpeedAtLimit();
        }
        private void Movement()
        {
            float thrust = Input.GetAxis("Vertical") * _thrustPower;
            Vector3 force = transform.forward * thrust; //Force direction and thrust giving vector for us to move into.

            _playerRb.AddForce(force);
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
