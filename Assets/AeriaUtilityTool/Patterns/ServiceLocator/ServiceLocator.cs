using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Linq;
using System;
using UnityEditor;

namespace AeriaUtil.Pattern.ServiceLocator
{
    public class ServiceLocator : MonoBehaviour
    {
        private static ServiceLocator _global;
        private static Dictionary<Scene, ServiceLocator> _sceneContainers;
        private static List<GameObject> _tmpSceneGameObjects;

        private readonly ServiceManager _services = new ServiceManager();

        private const string GLOBALSERVICELOCATORNAME = "ServiceLocator [Global]";
        private const string SCENESERVICELOCATORNAME = "ServiceLocator [Scene]";

        internal void ConfigureAsGlobal(bool dontDestroyOnLoad)
        {
            if (_global == this) Debug.LogWarning("ServiceLocator.ConfigureAsGlobal: Already configured as global", this);
            else if (_global != null) Debug.LogError("ServiceLocator.ConfigureAsGlobal: Another ServiceLocator is already configured as global", this);
            else
            {
                _global = this;
                if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            }
        }

        internal void ConfigureForScene()
        {
            Scene scene = gameObject.scene;

            if (_sceneContainers.ContainsKey(scene))
            {
                Debug.LogError("ServiceLocator.ConfigureForScene: Another ServiceLocator is already configured for this scene", this);
                return;
            }

            _sceneContainers.Add(scene, this);
        }

        public static ServiceLocator Global
        {
            get
            {
                if (_global != null) return _global;

                // bootstrap or initalise the new instance of global as necessary.
                if (FindFirstObjectByType<ServiceLocatorGlobalBootStrapper>() is { } found)
                {
                    found.BootstrapOnDemand();
                    return _global;
                }

                GameObject container = new GameObject(GLOBALSERVICELOCATORNAME, typeof(ServiceLocator));
                container.AddComponent<ServiceLocatorGlobalBootStrapper>().BootstrapOnDemand();

                return _global;
            }
        }

        public static ServiceLocator For(MonoBehaviour mb)
        {
            //Check if the object itself have it, if its not check scene, if its not check Global and return.
            return mb.GetComponentInParent<ServiceLocator>().OrNull() ?? ForSceneOf(mb) ?? Global;
        }
        public static ServiceLocator ForSceneOf(MonoBehaviour mb)
        {
            Scene scene = mb.gameObject.scene;

            //If can, return a servicelocator other than "mb"
            if (_sceneContainers.TryGetValue(scene, out ServiceLocator container) && container != mb) return container;

            //If there is none, try to see if there is one or its forgotten to register.
            _tmpSceneGameObjects.Clear();
            scene.GetRootGameObjects(_tmpSceneGameObjects);

            //Other than the current ServiceLocatorScenebootstapper, look for another one.
            foreach (GameObject go in _tmpSceneGameObjects.Where(go => go.GetComponent<ServiceLocatorSceneBootStrapper>() != null))
            {
                if (go.TryGetComponent(out ServiceLocatorSceneBootStrapper bootstrapper) && bootstrapper.Container != mb)
                {
                    bootstrapper.BootstrapOnDemand();
                    return bootstrapper.Container;
                }
            }

            //If anyhow we can not find it, we use Global.
            return Global;
        }

        //Adding and Getting Services stored in Service Manager.
        public ServiceLocator Register<T>(T service)
        {
            //Return servicelocator after registering the service into servicemanager.
            _services.Register(service);
            return this;
        }
        public ServiceLocator Register(Type type, object service)
        {
            //Return servicelocator after registering the service into servicemanager.
            _services.Register(type, service);
            return this;
        }
        public ServiceLocator Get<T>(out T service) where T : class
        {
            if (TryGetService(out service)) return this;

            if (TryGetNextInHierarchy(out ServiceLocator container))
            {
                container.Get(out service);
                return this;
            }

            throw new ArgumentException($"ServiceLocator.Get: Service of type {typeof(T).FullName} is NOT registered");
        }
        private bool TryGetService<T>(out T service) where T : class
        {
            return _services.TryGet(out service);
        }

        private bool TryGetNextInHierarchy(out ServiceLocator container)
        {
            //If we are at the global level and if we couldn't find registered service then there is no service.
            if (this == _global)
            {
                container = null;
                return false;
            }

            container = transform.parent.OrNull()?.GetComponentInParent<ServiceLocator>().OrNull() ?? ForSceneOf(this);
            return container != null;
        }

        private void OnDestroy()
        {
            if (this == _global)
                _global = null;
            else if (_sceneContainers.ContainsValue(this))
                _sceneContainers.Remove(gameObject.scene);
        }

        //CLEAR STATICS TO MAKE SURE
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _global = null;
            _sceneContainers = new Dictionary<Scene, ServiceLocator>();
            _tmpSceneGameObjects = new List<GameObject>();
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/ServiceLocator/Add Global")]
        private static void AddGlobal()
        {
            GameObject go = new GameObject(GLOBALSERVICELOCATORNAME, typeof(ServiceLocatorGlobalBootStrapper));
        }

        [MenuItem("GameObject/ServiceLocator/Add Scene")]
        private static void AddScene()
        {
            GameObject go = new GameObject(SCENESERVICELOCATORNAME, typeof(ServiceLocatorSceneBootStrapper));
        }
#endif
    }
}
