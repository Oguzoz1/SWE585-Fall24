using Mirror;
using System;
using UnityEngine;

namespace Attributes
{
    [RequireComponent(typeof(Collider))]
    public class Health : NetworkBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private float _maxHealthPoint = 100f;

        [SyncVar(hook = nameof(HandleHealthUpdated))]
        private float _currentHealthPoint;


        public event Action<float, float> ClientOnHealthUpdated;
        public event Action ServerOnObjectDie;

        #region Server
        public override void OnStartServer()
        {
            _currentHealthPoint = _maxHealthPoint;
            ServerOnObjectDie += HandleDie;
        }

        public override void OnStopServer()
        {
            ServerOnObjectDie -= HandleDie;
        }
        private void OnDestroy()
        {
            ServerOnObjectDie -= HandleDie;
        }

        [Server]
        public void TakeDamage(float damage, NetworkConnectionToClient sourceClient)
        {
            if (_currentHealthPoint <= 0) return;

            if (sourceClient == null)
            {
                Debug.LogError("Source of fire is null");
                return;
            }

            if (sourceClient == connectionToClient) return;

            _currentHealthPoint = Mathf.Max(_currentHealthPoint - damage, 0);

            Debug.Log($"Damage Taken: {damage}, current health: {_currentHealthPoint}");

            if (_currentHealthPoint == 0)
                ServerOnObjectDie?.Invoke();
        }

        [Server]
        private void HandleDie()
        {
            NetworkServer.Destroy(this.gameObject);
        }

        [Server]
        public void ResetHealth()
        {
            _currentHealthPoint = _maxHealthPoint;
        }
        #endregion

        #region Client
        private void HandleHealthUpdated(float oldHealth, float newHealth)
        {
            //Handling Client Display Update. Passing newHealth and maxHealth
            ClientOnHealthUpdated?.Invoke(newHealth, _maxHealthPoint);
        }

        #endregion
    }
}

