using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public static class AppUrls
    {
        public static readonly string BASE_URL = "http://127.0.0.1:5071";
        public static readonly string API_BASE_URL = $"{BASE_URL}/api";
        public static readonly string API_PLAYER_URL = $"{API_BASE_URL}/player";
        public static readonly string API_AUTH_URL = $"{API_BASE_URL}/auth";
    }

}
