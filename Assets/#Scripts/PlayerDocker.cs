using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDocker : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private bool isLanded = true;

    [SerializeField] private Vector3 _lastPointOfDock;

    private bool _dockingInvoked = false;
    private bool _tookOffInvoked = false;

    public static event Action<PlayerDocker> OnPlayerDocking;
    public static event Action<PlayerDocker> OnPlayerTakeOff;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (CheckTakingOff())
        {
            if (!_tookOffInvoked)
            {
                OnPlayerTakeOff?.Invoke(this);
                _tookOffInvoked = true;
            }
            isLanded = false;
            _dockingInvoked = false;
        }
            

        if (CheckLanded())
        {
            if (!_dockingInvoked)
            {
                OnPlayerDocking?.Invoke(this);
                _dockingInvoked = true;
            }
            _tookOffInvoked = false;
        }
    }

    bool CheckLanded()
    {
        if (isLanded && _rb.velocity.magnitude < 1f)
            return true;
        else return false;
    }
    bool CheckTakingOff()
    {
        if (Vector3.Distance(transform.position, _lastPointOfDock) > 10f && _rb.velocity.magnitude > 1f)
            return true;
        else return false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("dock"))
        {
            Debug.Log("Colided with dock");
            isLanded = true;
            _dockingInvoked = false;
            _lastPointOfDock = collision.contacts[0].point;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("dock"))
        {
            Debug.Log("Colliding! with dock");
            isLanded = true;
            _lastPointOfDock = collision.contacts[0].point;
        }
    }
}
