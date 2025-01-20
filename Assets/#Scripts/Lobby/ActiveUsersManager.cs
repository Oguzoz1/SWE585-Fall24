using Database.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Authentication
{
    public class ActiveUsersManager : MonoBehaviour
    {
        public IEnumerator RemovePlayer(int playerId)
        {
            ActiveUserService aus = new();
            yield return StartCoroutine(aus.RemovePlayer(playerId));
        }

        public IEnumerator SendHeartbeatRoutine(int playerId)
        {
            ActiveUserService aus = new();
            while (true)
            {
                yield return StartCoroutine(aus.SendHeartbeat(playerId));
                yield return new WaitForSeconds(20f);
            }
        }
    }

}

