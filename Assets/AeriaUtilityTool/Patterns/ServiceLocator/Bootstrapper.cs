using System;
using UnityEngine;

namespace AeriaUtil.Pattern.ServiceLocator
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ServiceLocator))]
    public abstract class Bootstrapper : MonoBehaviour
    {
        ServiceLocator container;

        //if container is not null just return container. If container is null, assign container GetComponent<T>. Always returns non-null
        internal ServiceLocator Container => container.OrNull() ?? (container = GetComponent<ServiceLocator>());
        private bool hasBeenBootsrapped;

        void Awake() => BootstrapOnDemand();

        public void BootstrapOnDemand()
        {
            if (hasBeenBootsrapped) return;
            hasBeenBootsrapped = true;
            Bootstrap();
        }

        protected abstract void Bootstrap();
    }

    [AddComponentMenu("ServiceLocator/ServiceLocator Global")]
    public class ServiceLocatorGlobalBootStrapper : Bootstrapper
    {
        [SerializeField] private bool _dontDestroyOnLoad = true;
        protected override void Bootstrap()
        {
            //Configure as global here:
            Container.ConfigureAsGlobal(_dontDestroyOnLoad);
        }
    }

    [AddComponentMenu("ServiceLocator/ServiceLocator Global")]
    public class ServiceLocatorSceneBootStrapper : Bootstrapper
    {
        protected override void Bootstrap()
        {
            //Configure as scene here:
            Container.ConfigureForScene();
        }
    }

}