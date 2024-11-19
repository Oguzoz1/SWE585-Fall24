using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Database.Payload
{
    [System.Serializable]
    public class PlayerPayload
    {
        public int id;
        public string playerName;
        public UserCredentialsPayload userCredentials;
    }

    [System.Serializable]
    public class UserCredentialsPayload
    {
        public int id;
        public string loginName;
    }

    [System.Serializable]
    public class RegisterPayload
    {
        public string loginName;
        public string password;
    }

    [System.Serializable]
    public class UserLoginPayload
    {
        public string loginName;
        public string password;
    }

    [System.Serializable]
    public class ApiResponse<T>
    {
        public bool success;
        public T data;
        public string message;
        public List<string> errors;
    }
}
