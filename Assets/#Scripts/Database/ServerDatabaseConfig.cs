using Database;
using System.IO;
using UnityEngine;

namespace Database
{
    public class ServerDatabaseConfig
    {
#if UNITY_SERVER
        string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJNaXJyb3JTZXJ2ZXIiLCJqdGkiOiI2OGNiYzM5My0zNWI0LTRmODEtYjgzMi0yY2I3Njg2NDVjNTYiLCJyb2xlIjoiU2VydmVyIiwibmJmIjoxNzM2MzUxMDY4LCJleHAiOjE3Mzg5NDMwNjgsImlhdCI6MTczNjM1MTA2OCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdCIsImF1ZCI6IlVuaXR5Q2xpZW50QXBwIn0.ccrjhtFwFEUQCblOth8UfGTqsMW1WTfSin7FEyHdFuM";
        public ServerDatabaseConfig()
        {
            //string path = Path.Combine(Application.dataPath, "dbconfig.json");
            //Debug.Log("path: " + path);
            //if (File.Exists(path))
            //{
            //    var json = File.ReadAllText(path);
            //    var config = JsonUtility.FromJson<ConfigData>(json);
            //    TokenManager.SetUserToken(config.BackendApiKey);
            //    Debug.Log("TOKEN SET INITIATED");
            //}
            //else
            //{
            //    Debug.LogWarning("CONFIG FILE IS NOT FOUND. The application is now paused. Please provide the config file and restart the application.");

            //    while (true)
            //    {
            //        Debug.Log("Application is on hold. Close the application manually or resolve the issue.");
            //        System.Threading.Thread.Sleep(30000); 
            //    }
            //}

            TokenManager.SetUserToken(token);
            Debug.Log("TOKEN SET INITIATED");
        }
        //Datablock
        private class ConfigData
        {
            public string BackendApiKey;
        }

#endif
    }

}
