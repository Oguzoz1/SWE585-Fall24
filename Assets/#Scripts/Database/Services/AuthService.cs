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

        public IEnumerator Login(UserLoginPayload userLogin, Action<string> onError)
        {
            Debug.Log("Login started");
            byte[] postData = ApiUtility.PreparePostData(userLogin);

            using (UnityWebRequest request = new UnityWebRequest($"{apiUrl}/login", "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(postData);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("User logged in successfully");
                    Debug.Log(request.downloadHandler.text);

                    ApiResponse<string> response = JsonUtility.FromJson<ApiResponse<string>>(request.downloadHandler.text);

                    UserProperties.SetUserToken(response.data);
                }
                else
                {
                    ApiResponse<string> response = JsonUtility.FromJson<ApiResponse<string>>(request.downloadHandler.text);
                    Debug.LogError($"Error: {response.errors}");
                    if (!string.IsNullOrEmpty(request.downloadHandler.text))
                    {
                        Debug.LogError($"Server Response: {request.downloadHandler.text}");

                        onError?.Invoke(response.message);
                    }
                }
            }
        }

        public IEnumerator RegisterUser(RegisterPayload newRegistration, Action<bool> onSucceed)
        {
            Debug.Log("Registration Started");
            byte[] postData = ApiUtility.PreparePostData(newRegistration);

            using (UnityWebRequest request = new UnityWebRequest($"{apiUrl}/register", "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(postData);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("User created successfully");
                    Debug.Log(request.downloadHandler.text);

                    ApiResponse<string> response = JsonUtility.FromJson<ApiResponse<string>>(request.downloadHandler.text);

                    UserProperties.SetUserToken(response.data);

                    onSucceed?.Invoke(true);
                }
                else
                {
                    Debug.LogError($"Error: {request.error}");
                    if (!string.IsNullOrEmpty(request.downloadHandler.text))
                    {
                        Debug.LogError($"Server Response: {request.downloadHandler.text}");
                    }
                    onSucceed?.Invoke(false);
                }
            }
        }
    }
}
