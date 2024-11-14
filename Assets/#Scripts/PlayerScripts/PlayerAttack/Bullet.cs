using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Attack
{
    public class Bullet : MonoBehaviour
    {
        private Rigidbody _rb;
        [SerializeField] private float _bulletThrust = 2000f;
        [SerializeField] private ParticleSystem _explosion;
        [SerializeField] private MeshRenderer _meshRenderer;
        private void Start()
        {
            _rb = GetComponentInChildren<Rigidbody>();
            _rb.velocity = transform.forward * _bulletThrust;
        }

        private void OnCollisionEnter(Collision collision)
        {
            GameObject explosion = Instantiate(_explosion.gameObject, collision.GetContact(0).point, Quaternion.identity);
            Destroy(explosion, 1f);
            gameObject.SetActive(false);
        }
    }
}
