using Attributes;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Ship
{
    public class Bullet : NetworkBehaviour
    {
        private Rigidbody _rb;
        [SerializeField] private float _bulletThrust = 10f;
        [SerializeField] private ParticleSystem _explosion;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private float _damage = 10f;

        private int _damageableLayer;
        private int _playerLayer;

        private void Start()
        {
            _rb = GetComponentInChildren<Rigidbody>();

            _damageableLayer = LayerMask.NameToLayer("Damageable");
            _playerLayer = LayerMask.NameToLayer("Player");
        }

        private void Update()
        {
            transform.position += transform.forward * _bulletThrust * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider collision)
        {
            //Handle Client Code
            if (isClientOnly)
                HandleExplosionEffect(collision);

            // Handle Server/Client
            Invoke(nameof(DestroyItself), 1f);
            gameObject.SetActive(false);

            // Handle Server Code
            if (!isServerOnly) return;

            // Check if objects on certain layers have health component to avoid excessive TryGetComponent
            int collidedLayer = collision.gameObject.layer;

            if (collidedLayer == _damageableLayer || collidedLayer == _playerLayer)
            {
                if (collision.gameObject.TryGetComponent<Health>(out Health health))
                {
                    health.TakeDamage(_damage, connectionToClient);
                }
                else
                {
                    Debug.Log("No Health Component found on: " + collision.gameObject.name);
                }
            }
        }

        [Command]
        public void DestroyItself()
        {
            NetworkServer.Destroy(gameObject);
        }

        private void HandleExplosionEffect(Collider other)
        {
            var go = Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(go, 0.5f);
        }
    }
}
