using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Pattern.DependencyInjection
{
    public interface IExampleSystem
    {
        IExampleSystem ProvidedExampleSystem();

        public void Init();

    }

    public class ExampleSpecificSystem : MonoBehaviour, IDependencyProvider, IExampleSystem
    {
        public void Init()
        {
            Debug.Log("EXAMPLE SPECIFIC SYSTEM IS INITIATED IN EXAMPLECLASSA");
        }

        [Provide]
        public IExampleSystem ProvidedExampleSystem()
        {
            return this;
        }
    }
}

