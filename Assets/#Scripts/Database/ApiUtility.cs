using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

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
    }
}
