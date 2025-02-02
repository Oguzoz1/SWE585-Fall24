using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerShipMovementEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _engineEffects;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }
        void Update()
        {
            if (Input.GetKey(KeyCode.LeftShift))
                ChangeEffectForwardBackward();
            else
            {
                foreach (var effect in _engineEffects)
                {
                    var main = effect.main;
                    main.startSpeed = 0;
                }
            }
        }

        void ChangeEffectForwardBackward()
        {
            foreach (var effect in _engineEffects)
            {
                var main = effect.main;
                main.startSpeed = _rb.velocity.magnitude * 0.1f;
            }
        }
    }

}
