using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceStation
{
    public class StationManager : MonoBehaviour
    {
        private Animator _animator;
        private void Start()
        {
            _animator = GetComponent<Animator>();

            PlayerDocker.OnPlayerDocking += HandlePlayerDocking;
            PlayerDocker.OnPlayerTakeOff += HandlePlayerTakeOff;

        }

        private void OnDestroy()
        {
            PlayerDocker.OnPlayerDocking -= HandlePlayerDocking;
            PlayerDocker.OnPlayerTakeOff -= HandlePlayerTakeOff;
        }
        private void HandlePlayerDocking(PlayerDocker docker)
        {
            //Run animations and sounds
            _animator.SetBool("shipDocked", true);
            Debug.Log("Player Docked");
        }
        private void HandlePlayerTakeOff(PlayerDocker docker)
        {
            _animator.SetBool("shipDocked", false);
            Debug.Log("Player Took Off");
        }
    }
}

