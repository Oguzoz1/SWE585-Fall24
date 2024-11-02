using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private float _bulletThrust = 2000f;
    private void Start()
    {
        _rb = GetComponentInChildren<Rigidbody>();
        _rb.velocity = transform.forward * _bulletThrust;
    }

}
