using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Pattern.DependencyInjection
{
    public class ExampleClassA : MonoBehaviour
    {
        [Inject]
        private ServiceExampleA _serviceA;

        [Inject]
        private ServiceExampleB _serviceB;

        //The system should be either and abstract class or interface since this system 
        //-> can be of type anything. (Generic)
        [Inject]
        private IExampleSystem _exampleSystem;

        [Inject]
        public ServiceExampleC _propertyC { get; private set; }

        private FactoryA _factoryA;

        //factoryA is injected through a parameter.
        [Inject]
        public void Init(FactoryA factoryA)
        {
            this._factoryA = factoryA;
        }

        private void Start()
        {
            //We injected dependencies with attributes.
            _serviceA.Initialize("ServiceA initialized from: " + GetType().FullName);
            _propertyC.Init("Property init");
            _exampleSystem.Init();

            _factoryA.CreateServiceA().Initialize();
        }
    }


}
