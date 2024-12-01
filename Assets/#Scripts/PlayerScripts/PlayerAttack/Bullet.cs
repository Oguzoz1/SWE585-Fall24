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
        [SerializeField] private float _bulletThrust = 2000f;
        [SerializeField] private ParticleSystem _explosion;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private float _damage = 10f;

        private int _damageableLayer;
        private int _playerLayer;

        private void Start()
        {
            _rb = GetComponentInChildren<Rigidbody>();
            _rb.velocity = transform.forward * _bulletThrust;

            _damageableLayer = LayerMask.NameToLayer("Damageable");
            _playerLayer = LayerMask.NameToLayer("Player");
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Handle Client Code
            if (isClientOnly)
                HandleExplosionEffect(collision);

            //Handle Server/Client
            Invoke(nameof(DestroyItself), 1f);
            gameObject.SetActive(false);

            //Handle Server Code
            if (!isServerOnly) return;

            //Check if objects on certain layers have health component to avoid excessive tryget
            int collidedLayer = collision.gameObject.layer;

            if (collidedLayer == _damageableLayer || collidedLayer == _playerLayer)
            {
                if (collision.gameObject.TryGetComponent<Health>(out Health health))
                {
                    health.TakeDamage(_damage, connectionToClient);
                }
            }
        }

        [Server]
        public void DestroyItself()
        {
            NetworkServer.Destroy(gameObject);
        }

        private void HandleExplosionEffect(Collision collision)
        {
            Instantiate(_explosion.gameObject, collision.GetContact(0).point, Quaternion.identity);
        }
    }
}
