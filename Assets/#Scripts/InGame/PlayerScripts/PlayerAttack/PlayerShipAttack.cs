using Mirror;
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

        private void Start()
        {
            _fireSound = _projectileSpawnPoint.GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!isLocalPlayer) return; // Only handle input on the local player.

            bool canShoot = (_lastAttackElapsed += Time.deltaTime) >= _attackSpeed;
            if (Input.GetKey(_attackInput) && canShoot)
            {
                FireProjectile();
                _lastAttackElapsed = 0f;
            }

        }
        //First intantiate locally
        void FireProjectile()
        {
            // Create projectile on the server.
            Quaternion projectileOrientation = Quaternion.LookRotation(_projectileSpawnPoint.forward);
            GameObject pj = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, projectileOrientation);

            CmdFireProjectile(pj.transform.position, pj.transform.rotation);

            TargetReconcileProjectile(pj);
        }
        //Send command to server
        [Command]
        void CmdFireProjectile(Vector3 spawnPos, Quaternion rot)
        {
            GameObject projectile = Instantiate(_projectilePrefab, spawnPos, rot);
            NetworkServer.Spawn(projectile, connectionToClient);

            RpcPlayFireSound(); 

            Destroy(projectile, 10f); 
        }


        void TargetReconcileProjectile(GameObject obj)
        {
            if (obj != null)
                Destroy(obj);
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
