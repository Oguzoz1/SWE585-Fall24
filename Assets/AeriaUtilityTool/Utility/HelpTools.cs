using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AeriaUtil
{
    public static class RenderUtility
    {
        #region Transparency
        public static void SetTransparency(this Renderer renderer, float alpha)
        {
            if (renderer != null)
            {
                Material material = renderer.material;
                Color color = material.color;
                color.a = Mathf.Clamp01(alpha); // Ensure alpha is within valid range (0 to 1)
                material.color = color;
            }
            else Debug.LogError($"{renderer.name} is NULL!");
        }

        public static void SetTransparency(this GameObject gameObject, float alpha)
        {
            if (gameObject.TryGetComponent<Renderer>(out Renderer renderer))
            {
                if (renderer != null)
                {
                    SetAlpha(renderer, alpha);
                }
            }
            else
            {
                renderer = gameObject.GetComponentInChildren<Renderer>();

                if (renderer != null)
                {
                    SetAlpha(renderer, alpha);
                }
            }
        }

        private static void SetAlpha(Renderer renderer, float alpha)
        {
            Material material = renderer.material;
            Color color = material.color;
            color.a = Mathf.Clamp01(alpha); // Ensure alpha is within valid range (0 to 1)
            material.color = color;
        }
        #endregion
    }

    public static class DebugExt
    {
        public static void LogCheckObjExist(UnityEngine.Object obj)
        {
            Debug.Log(obj != null ? $"{obj.name} Found" : $"{obj.name} Not Found");
        }

        public static void LogErrorCheckObjExist(UnityEngine.Object obj)
        {
            Debug.LogError(obj != null ? $"{obj.name} Found" : $"{obj.name} Not Found");
        }
    }
#if UNITY_EDITOR
    public static class ScriptableObjectFinder
    {
        public static T FindScriptableObject<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name); // Find all assets of type T
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                T asset = AssetDatabase.LoadAssetAtPath<T>(path);
                if (asset != null)
                {
                    return asset; // Return the first one found
                }
            }
            return null; // Return null if none found
        }
    }
#endif

    public static class IterationExtension
    {
        /// <summary>
        /// Returns the next element by feeding parameter with an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Array to traverse.</param>
        /// <param name="currentElement">Find next element after this</param>
        /// <returns>T: Next Element</returns>
        public static T NextArrayElement<T>(this T[] array, T currentElement)
        {
            int index = Array.IndexOf(array, currentElement);
            if (index != -1 && index + 1 < array.Length)
            {
                return array[index + 1];
            }
            else
            {
                Debug.LogError("It's out of boundaries");
                return default(T); // Return the default value for the type T
            }
        }
        /// <summary>
        /// Returns the next element by feeding parameter with an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">List to traverse</param>
        /// <param name="currentElement">Find next element after this.</param>
        /// <returns>T: next element</returns>
        public static T NextListElement<T>(this List<T> list, T currentElement)
        {
            int index = list.IndexOf(currentElement);
            if (index != -1 && index + 1 < list.Count)
            {
                return list[index + 1];
            }
            else
            {
                return default(T); // Return the default value for the type T
            }
        }

        public static T OrNull<T>(this T obj) where T : UnityEngine.Object => (bool)obj ? obj : null;
    }

    public static class AnimationExtension
    {
        /// <summary>
        /// Moving a transform towars an object with Vector3.Movetowards with Time.Deltatime
        /// </summary>
        /// <param name="obj">Object to be moved</param>
        /// <param name="target">Target Transform</param>
        /// <param name="speed">Speed of movement</param>
        public static void MoveWithMoveTowards(this Transform obj, Transform target, float speed)
        {
            obj.transform.position = Vector3.MoveTowards(obj.position, target.position, speed * Time.deltaTime);
        }
        /// <summary>
        /// Moving a transform towars an object with Vector3.Movetowards with Time.Deltatime
        /// </summary>
        /// <param name="obj">Object to be moved</param>
        /// <param name="target">Target Vector</param>
        /// <param name="speed">Speed of movement</param>
        public static void MoveWithMoveTowards(this Transform obj, Vector3 target, float speed)
        {
            obj.transform.position = Vector3.MoveTowards(obj.position, target, speed * Time.deltaTime);
        }
        /// <summary>
        /// Rotation the object with slerp by using LookRotation by using time.deltatime.
        /// </summary>
        /// <param name="obj">Object to be rotated</param>
        /// <param name="target">Target transform to to turn.</param>
        /// <param name="AngularSpeed">Angular speed.</param>
        public static void RotateToTargetWithSlerp(this Transform obj, Transform target, float AngularSpeed)
        {
            if (target.position != Vector3.zero)
            {
                Vector3 dir = (target.position - obj.position).normalized;

                // Create a rotation that looks at the target
                Quaternion targetRotation = Quaternion.LookRotation(dir);

                // Smoothly interpolate the rotation
                obj.rotation = Quaternion.Slerp(obj.rotation, targetRotation, AngularSpeed * Time.deltaTime);
            }
        }
        /// <summary>
        /// Rotation the object with slerp by using LookRotation by using time.deltatime.
        /// </summary>
        /// <param name="obj">Object to be rotated</param>
        /// <param name="target">Target vector to to turn towards.</param>
        /// <param name="AngularSpeed">Angular speed.</param>
        public static void RotateToTargetWithSlerp(this Transform obj, Vector3 target, float AngularSpeed)
        {
            if (target != Vector3.zero)
            {
                Vector3 dir = (target - obj.position).normalized;

                // Create a rotation that looks at the target
                Quaternion targetRotation = Quaternion.LookRotation(dir);

                // Smoothly interpolate the rotation
                obj.rotation = Quaternion.Slerp(obj.rotation, targetRotation, AngularSpeed * Time.deltaTime);
            }
        }
    }
}

