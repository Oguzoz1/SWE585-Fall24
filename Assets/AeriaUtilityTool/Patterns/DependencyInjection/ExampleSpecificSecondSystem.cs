using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Pattern.DependencyInjection
{
    public class ExampleSpecificSecondSystem : MonoBehaviour, IDependencyProvider, IExampleSystem
    {
        public void Init()
        {
            Debug.Log("SECOND IS INITIATED IN EXAMPLECLASSA");
        }

        [Provide]
        public IExampleSystem ProvidedExampleSystem()
        {
            return this;
        }
    }
}

