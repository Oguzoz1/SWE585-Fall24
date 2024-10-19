using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationManager : MonoBehaviour
{
    private void Start()
    {
        PlayerDocker.OnPlayerDocking += HandlePlayerDocking;
    }

    private void OnDestroy()
    {
        PlayerDocker.OnPlayerDocking -= HandlePlayerDocking;
    }
    private void HandlePlayerDocking(PlayerDocker docker)
    {
        //Run animations and sounds
        Debug.Log("Player Docked");
    }
}
