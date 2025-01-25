using UnityEngine;

namespace Game.Authentication
{
    public class PlayerAuthenticator : IAuthenticatorService
    {
        public bool Authenticate(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                Debug.LogError("Player Token is null or empty");
                return false;
            }

            Debug.Log("Authenticating player...");
            //SEND REQUEST HERE
            return true;
        }
    }
}