using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private KeyCode _attackInput = KeyCode.LeftControl;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private float _attackSpeed = 0.1f;

    private float _lastAttackElapsed = 0f;
    private AudioSource _fireSound;

    private void Start()
    {
        _fireSound = _projectileSpawnPoint.GetComponent<AudioSource>();
    }
    private void Update()
    {
        bool canShoot = (_lastAttackElapsed += Time.deltaTime) >= _attackSpeed;
        if (Input.GetKey(_attackInput) && canShoot)
            ForwardManualAttack();
    }

    void ForwardManualAttack()
    {
        Quaternion projectileOrientation = Quaternion.LookRotation(_projectileSpawnPoint.forward);
        GameObject projectile = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, projectileOrientation);
        Destroy(projectile, 10f);
        _lastAttackElapsed = 0f;
        _fireSound.Play();
    }
    
}
