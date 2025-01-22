using Player.Ship;
using UnityEngine;

namespace SpaceStation
{
    public class StationManager : MonoBehaviour
    {
        private Animator _animator;
        private void Start()
        {
            _animator = GetComponent<Animator>();

            PlayerShipDocker.OnPlayerDocking += HandlePlayerDocking;
            PlayerShipDocker.OnPlayerTakeOff += HandlePlayerTakeOff;

        }

        private void OnDestroy()
        {
            PlayerShipDocker.OnPlayerDocking -= HandlePlayerDocking;
            PlayerShipDocker.OnPlayerTakeOff -= HandlePlayerTakeOff;
        }
        private void HandlePlayerDocking(PlayerShipDocker docker)
        {
            //Run animations and sounds
            _animator.SetBool("shipDocked", true);
        }
        private void HandlePlayerTakeOff(PlayerShipDocker docker)
        {
            _animator.SetBool("shipDocked", false);
        }
    }
}

