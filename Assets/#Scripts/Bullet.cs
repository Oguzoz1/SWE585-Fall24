using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponentInChildren<Rigidbody>();
        _rb.velocity = transform.forward * 100f;
    }
}
