using Database.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Authentication
{
    public class ActiveUsersManager : MonoBehaviour
    {
        public IEnumerator RemovePlayer(int userCredId)
        {
            ActiveUserService aus = new();
            yield return StartCoroutine(aus.RemovePlayer(userCredId));
        }

        public IEnumerator SendHeartbeatRoutine(int userCredId)
        {
            ActiveUserService aus = new();
            while (true)
            {
                yield return StartCoroutine(aus.SendHeartbeat(userCredId));
                yield return new WaitForSeconds(20f);
            }
        }
    }

}

