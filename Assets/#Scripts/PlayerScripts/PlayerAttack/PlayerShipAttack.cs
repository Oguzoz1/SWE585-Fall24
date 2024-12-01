using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Ship
{
    public class PlayerShipAttack : NetworkBehaviour
    {
        [SerializeField] private KeyCode _attackInput = KeyCode.LeftControl;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private Transform _projectileSpawnPoint;
        [SerializeField] private float _attackSpeed = 0.1f;

        private float _lastAttackElapsed = 0f;
        private AudioSource _fireSound;

        LODGroup[] grps;

        private void Start()
        {
            _fireSound = _projectileSpawnPoint.GetComponent<AudioSource>();
            grps = FindObjectsOfType<LODGroup>();
        }

        private void Update()
        {
            if (!isLocalPlayer) return; // Only handle input on the local player.

            bool canShoot = (_lastAttackElapsed += Time.deltaTime) >= _attackSpeed;
            if (Input.GetKey(_attackInput) && canShoot)
            {
                CmdFireProjectile();
                _lastAttackElapsed = 0f;
            }

        }

        [Command]
        void CmdFireProjectile()
        {
            // Create projectile on the server.
            Quaternion projectileOrientation = Quaternion.LookRotation(_projectileSpawnPoint.forward);
            GameObject projectile = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, projectileOrientation);

            NetworkServer.Spawn(projectile, connectionToClient); // Spawn the projectile on all clients.

            RpcPlayFireSound(); // Play fire sound on clients.

            Destroy(projectile, 10f); // Destroy the projectile after 10 seconds.
        }

        [ClientRpc]
        void RpcPlayFireSound()
        {
            if (_fireSound != null)
            {
                _fireSound.Play();
            }
        }
    }
}
