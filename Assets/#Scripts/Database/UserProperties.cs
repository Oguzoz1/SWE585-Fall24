using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public static class UserProperties
    {
        public static string USER_TOKEN { get; private set; }
        public static string SetUserToken(string token)
        {
            USER_TOKEN = token;
            return USER_TOKEN;
        }

    }
}
