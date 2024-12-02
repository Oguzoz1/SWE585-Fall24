using UnityEngine;

namespace AeriaUtil.Pattern
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name + "AutoCreated";
                        _instance = obj.AddComponent<T>();
                        DontDestroyOnLoad(obj);
                        Debug.LogError($"{typeof(T)} instance not found in the scene. Therefore, {obj.name} is created!");
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake() => InitialiseSingleton();
        protected virtual void InitialiseSingleton()
        {
            if (!Application.isPlaying)
                return;

            if (_instance != null && _instance != this)
            {
                Destroy(this);
                return;
            }
            _instance = this as T;

        }
    }

}

