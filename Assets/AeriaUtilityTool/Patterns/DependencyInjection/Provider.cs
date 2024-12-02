using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Pattern.DependencyInjection
{
    public class Provider : MonoBehaviour 
    {
        [Provide]
        public ServiceExampleA ProvideServiceA()
        {
            return new ServiceExampleA();
        }

        [Provide]
        public ServiceExampleB ProvideServiceB()
        {
            return new ServiceExampleB();
        }

        [Provide]
        public  ServiceExampleC ProvideServiceC()
        {
            return new ServiceExampleC();
        }

        [Provide]
        public FactoryA ProvideFactoryA()
        {
            return new FactoryA();
        }
    }
    public class ServiceExampleA
    {
        //dummy implementation
        public void Initialize(string message = null)
        {
            Debug.Log($"ServiceExampleA.Initialize: {message}");
        }
    }

    public class ServiceExampleB
    {
        //dummy implementation
        public void Initialize(string message = null)
        {
            Debug.Log($"ServiceExampleB.Initialize: {message}");
        }
    }

    public class ServiceExampleC
    {

        public void Init(string message = null)
        {
            Debug.Log($"ServiceExampleC.Init: {message}");
        }
    }
    //Factory as a dependency
    public class FactoryA
    {
        private ServiceExampleA _cachedServiceA;

        public ServiceExampleA CreateServiceA()
        {
            if (_cachedServiceA == null)
                _cachedServiceA = new ServiceExampleA();
            return _cachedServiceA;
        }
    }
}
