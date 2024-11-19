using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Database.Services
{
    using Payload;
    using System.Text;
    using UnityEngine.Networking;
    using Utility;

    internal class AuthService : IDatabaseService
    {
        private string apiUrl = AppUrls.API_AUTH_URL;

        public IEnumerator Login(UserLoginPayload userLogin)
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
                    Debug.LogError($"Error: {request.error}");
                    if (!string.IsNullOrEmpty(request.downloadHandler.text))
                    {
                        Debug.LogError($"Server Response: {request.downloadHandler.text}");
                    }
                }
            }
        }

        public IEnumerator RegisterUser(RegisterPayload newRegistration)
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
                }
                else
                {
                    Debug.LogError($"Error: {request.error}");
                    if (!string.IsNullOrEmpty(request.downloadHandler.text))
                    {
                        Debug.LogError($"Server Response: {request.downloadHandler.text}");
                    }
                }
            }
        }
    }
}
