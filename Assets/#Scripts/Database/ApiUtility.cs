using Database.Payload;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Database.Utility
{
    public static class ApiUtility
    {
        /// <summary>
        /// Prepares POST data for a UnityWebRequest by serializing the object to JSON and encoding it to bytes.
        /// </summary>
        /// <param name="dataObject">The object to serialize into JSON.</param>
        /// <returns>The prepared byte array for the UnityWebRequest.</returns>
        public static byte[] PreparePostData(object dataObject)
        {
            return dataObject == null ?
                throw new ArgumentNullException(nameof(dataObject), "Data object can not be null.") :
                Encoding.UTF8.GetBytes(JsonUtility.ToJson(dataObject));
        }
        /// <summary>
        /// Generic sending request and receiving response with no token authorization. 
        /// </summary>
        /// <typeparam name="TPayload">Payload to send to the backend</typeparam>
        /// <typeparam name="TResponse">Response type such as a class or a string or a bool</typeparam>
        /// <param name="url"></param>
        /// <param name="payload"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        public static IEnumerator SendRequestNoToken<TPayload, TResponse>(
            string url,
            string requestType,
            TPayload payload,
            Action<ApiResponse<TResponse>> onSuccess,
            Action<string> onError
            )
        {
            Debug.Log($"Sending request to {url}");
            byte[] postData = PreparePostData(payload);

            using (UnityWebRequest request = new UnityWebRequest(url, requestType))
            {
                request.uploadHandler = new UploadHandlerRaw(postData);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        ApiResponse<TResponse> response =
                            JsonUtility.FromJson<ApiResponse<TResponse>>(request.downloadHandler.text);
                        onSuccess?.Invoke(response);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Parsing error: {ex.Message}");
                        onError?.Invoke("Failed to parse server response.");
                    }
                }
                else
                {
                    string errorResponse = request.downloadHandler?.text;
                    Debug.LogError($"Request error: {request.error}");
                    Debug.LogError($"Server response: {errorResponse}");

                    if (!string.IsNullOrEmpty(errorResponse))
                    {
                        try
                        {
                            ApiResponse<string> error = JsonUtility.FromJson<ApiResponse<string>>(errorResponse);
                            onError?.Invoke(error.message);
                        }
                        catch
                        {
                            onError?.Invoke("Failed to parse error response.");
                        }
                    }
                    else
                    {
                        onError?.Invoke(request.error ?? "Unknown error occurred.");
                    }
                }
            }
        }
        public static IEnumerator SendRequestWithToken<TPayload, TResponse>(
        string url,
        string requestType,
        TPayload payload,
        Action<ApiResponse<TResponse>> onSuccess,
        Action<string> onError
        )
        {
            string token = TokenManager.GetToken();
            Debug.Log($"Sending request to {url}");
            byte[] postData = PreparePostData(payload);

            using (UnityWebRequest request = new UnityWebRequest(url, requestType))
            {
                request.uploadHandler = new UploadHandlerRaw(postData);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Authorization", $"Bearer {token}");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        ApiResponse<TResponse> response =
                            JsonUtility.FromJson<ApiResponse<TResponse>>(request.downloadHandler.text);
                        onSuccess?.Invoke(response);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Parsing error: {ex.Message}");
                        onError?.Invoke("Failed to parse server response.");
                    }
                }
                else
                {
                    string errorResponse = request.downloadHandler?.text;
                    Debug.LogError($"Request error: {request.error}");
                    Debug.LogError($"Server response: {errorResponse}");

                    if (!string.IsNullOrEmpty(errorResponse))
                    {
                        try
                        {
                            ApiResponse<string> error = JsonUtility.FromJson<ApiResponse<string>>(errorResponse);
                            onError?.Invoke(error.message);
                        }
                        catch
                        {
                            onError?.Invoke("Failed to parse error response.");
                        }
                    }
                    else
                    {
                        onError?.Invoke(request.error ?? "Unknown error occurred.");
                    }
                }
            }
        }
        public static IEnumerator SendRequestWithToken<TResponse>(
        string url,
        string requestType,
        Action<ApiResponse<TResponse>> onSuccess,
        Action<string> onError
        )
        {
            string token = TokenManager.GetToken();
            Debug.Log($"Sending request to {url}");

            using (UnityWebRequest request = new UnityWebRequest(url, requestType))
            {
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Authorization", $"Bearer {token}");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        ApiResponse<TResponse> response =
                            JsonUtility.FromJson<ApiResponse<TResponse>>(request.downloadHandler.text);
                        onSuccess?.Invoke(response);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Parsing error: {ex.Message}");
                        onError?.Invoke("Failed to parse server response.");
                    }
                }
                else
                {
                    string errorResponse = request.downloadHandler?.text;
                    Debug.LogError($"Request error: {request.error}");
                    Debug.LogError($"Server response: {errorResponse}");

                    if (!string.IsNullOrEmpty(errorResponse))
                    {
                        try
                        {
                            ApiResponse<string> error = JsonUtility.FromJson<ApiResponse<string>>(errorResponse);
                            onError?.Invoke(error.message);
                        }
                        catch
                        {
                            onError?.Invoke("Failed to parse error response.");
                        }
                    }
                    else
                    {
                        onError?.Invoke(request.error ?? "Unknown error occurred.");
                    }
                }
            }
        }

        public static IEnumerator SendGetRequestWithToken<TResponse>(
           string url,
           Action<ApiResponse<TResponse>> onSuccess,
           Action<string> onError
           )
        {
            string token = TokenManager.GetToken();
            Debug.Log($"Sending request to {url}");

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.SetRequestHeader("Authorization", $"Bearer {token}");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        ApiResponse<TResponse> response =
                            JsonUtility.FromJson<ApiResponse<TResponse>>(request.downloadHandler.text);
                        onSuccess?.Invoke(response);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Parsing error: {ex.Message}");
                        onError?.Invoke("Failed to parse server response.");
                    }
                }
                else
                {
                    string errorResponse = request.downloadHandler?.text;
                    Debug.LogError($"Request error: {request.error}");
                    Debug.LogError($"Server response: {errorResponse}");

                    if (!string.IsNullOrEmpty(errorResponse))
                    {
                        try
                        {
                            ApiResponse<string> error = JsonUtility.FromJson<ApiResponse<string>>(errorResponse);
                            onError?.Invoke(error.message);
                        }
                        catch
                        {
                            onError?.Invoke("Failed to parse error response.");
                        }
                    }
                    else
                    {
                        onError?.Invoke(request.error ?? "Unknown error occurred.");
                    }
                }
            }
        }
    }
}
