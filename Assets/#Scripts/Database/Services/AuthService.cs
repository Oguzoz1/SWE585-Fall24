using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Database.Services
{
    using Payload;
    using System;
    using System.Text;
    using UnityEngine.Networking;
    using Utility;

    internal class AuthService : IDatabaseService
    {
        private string apiUrl = AppUrls.API_AUTH_URL;
        public IEnumerator ClientLogin(UserLoginPayload userLogin, Action<string> onError)
        {
            Debug.Log("Login started");

            yield return ApiUtility.SendRequestNoToken<UserLoginPayload, string>($"{apiUrl}/login", "POST", userLogin,
                onSuccess: (response) =>
                {
                    Debug.Log("User logged in successfully");
                    TokenManager.SetUserToken(response.data);
                },
                onError: (error) =>
                {
                    Debug.LogError($"Login failed: {error}");
                    onError?.Invoke(error);
                });
        }

        public IEnumerator ClientRegisterUser(RegisterPayload newRegistration, Action<bool> onSucceed)
        {

            Debug.Log("Registration Started");


            yield return ApiUtility.SendRequestNoToken<RegisterPayload, string>($"{apiUrl}/register", "POST", newRegistration,
                onSuccess: (response) =>
                {
                    Debug.Log("User created successfully");
                    TokenManager.SetUserToken(response.data);
                    onSucceed?.Invoke(true);
                },
                onError: (error) =>
                {
                    Debug.LogError($"Registration failed: {error}");
                    onSucceed?.Invoke(false);
                });
        }

#if UNITY_SERVER
        public IEnumerator ServerCheckServer()
        {
            yield return ApiUtility.SendGetRequestWithToken<string>($"{apiUrl}/server-check",
                onSuccess: (response) =>
                {
                    Debug.Log(response.data);
                },
                onError: (error) =>
                {
                    Debug.Log(error);
                });
        }
#endif
    }
}
