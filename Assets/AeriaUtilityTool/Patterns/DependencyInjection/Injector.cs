using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace AeriaUtil.Pattern.DependencyInjection
{
    //INJECTOR NEEDS TO BE INITIALISED AS A MONOBEHAVIOUR IN UNITY. 
    //WE CAN HAVE AS MANY PROVIDERS AS WE WANT.
    //INJECTOR INTENDED TO BE ONE, THUS CREATE ONLY ONE.

    //To make this work, get an injector and providers in the scene. ExampleClassA or classes with InjectAttributes must be instantiated as well.
    //This pattern will automatically find the attributes that has the attribute called Inject and Providers. Then Inject dependencies.

    [DefaultExecutionOrder(-1000)]
    public class Injector : Singleton<Injector>
    {
        const BindingFlags BINDINGFLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        //Issue is that since we have one type, we find the marker that carries that type and save it. We look for that Type
        //and inject what is provided.


        // INSTEAD OF RECORDING OBJECTS, RECORD THE METHOD AND INVOKE IT RIGHT BEFORE YOU INJECT IT
        readonly Dictionary<Type, object> registery = new Dictionary<Type, object>();

        protected override void Awake()
        {
            base.Awake();

            //Find all modules implementing IDependencyProvider
            IEnumerable<IDependencyProvider> providers = FindMonoBehaviours().OfType<IDependencyProvider>();
            foreach (IDependencyProvider provider in providers)
            {
                RegisterProviders(provider);
            }

            //Find all injectable objects and inject their dependencies
            //Services can be non-monobehaviour BUT classes that are being injected MUST BE MONOBEHAVIOUR in this logic.
            IEnumerable<MonoBehaviour> injectables = FindMonoBehaviours().Where(IsInjectable);
            foreach (MonoBehaviour injectable in injectables)
            {
                Inject(injectable);
            }
        }

        #region Inject Logic
        private void Inject(MonoBehaviour injectable)
        {
            //Get type of injectable
            Type type = injectable.GetType();

            InjectFields(type, injectable);
            InjectMethods(type, injectable);
            InjectProperty(type, injectable);

        }
        private void InjectFields(Type type, MonoBehaviour injectable)
        {
            //Injecting Fields:
            //Get all fields according to bindingflags and attribute have Inject.
            IEnumerable<FieldInfo> injectableFields = type.GetFields(BINDINGFLAGS).Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            //Loop all the fields with Inject attributes to get their type and check if they have a provider in registery
            //if they don't throw exception, if they do set their value.
            foreach (FieldInfo injectableField in injectableFields)
            {
                Type fieldType = injectableField.FieldType;
                object resolvedInstance = Resolve(fieldType);

                if (resolvedInstance == null) throw new Exception($"Field Failed to resolve {fieldType.Name} for {type.Name}");

                injectableField.SetValue(injectable, resolvedInstance);
                //Injected ServiceExampleA into ExampleClassA 
                Debug.Log($"Field Injected {fieldType.Name} into {type.Name}");
            }
        }
        private void InjectMethods(Type type, MonoBehaviour injectable)
        {
            //Injecting Methods:
            //Get all methods with BINDINGFLAGS constraint where they have inject attribute.
            IEnumerable<MethodInfo> injectableMethods = type.GetMethods(BINDINGFLAGS).Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
            foreach (MethodInfo injectableMethod in injectableMethods)
            {
                Type[] requiredParameters = injectableMethod.GetParameters()
                    .Select(parameter => parameter.ParameterType)
                    .ToArray();

                object[] resolvedInstances = requiredParameters.Select(Resolve).ToArray();
                if (resolvedInstances.Any(resolvedInstance => resolvedInstance == null))
                {
                    throw new Exception($"Method Failed to inject {type.Name} into {injectableMethod.Name}");
                }

                injectableMethod.Invoke(injectable, resolvedInstances);
                Debug.Log($"Method Injected {type.Name} into {injectableMethod.Name}");
            }

        }
        private void InjectProperty(Type type, MonoBehaviour injectable)
        {
            //Injecting Properties
            IEnumerable<PropertyInfo> injectableProperties = type.GetProperties(BINDINGFLAGS).Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
            foreach (PropertyInfo injectableProperty in injectableProperties)
            {
                Type propertyType = injectableProperty.PropertyType;
                object resolvedInstance = Resolve(propertyType);

                if (resolvedInstance == null)
                    throw new Exception($"Property Failed to resolve {propertyType.Name} for {type.Name}");
                injectableProperty.SetValue(injectable, resolvedInstance);
                Debug.Log($"Property Injected {type.Name} into {injectableProperty.Name}");
            }
        }
        #endregion
        #region Register Provider Logic
        //For now, this registers only one object. If we want different new objects
        //it will not affect, instead it will refer to the same object that is created.
        //A good way of referring to an object that is not a singleton but only appears once in the game scene.
        
        private void RegisterProviders(IDependencyProvider provider)
        {
            RegisterMethodProvider(provider);
            RegisterPropertyProvider(provider);
        }
        private void RegisterPropertyProvider(IDependencyProvider provider)
        {
            //get all PROPERTY info from the provider class.
            IEnumerable<PropertyInfo> providedProperties = provider.GetType().GetProperties(BINDINGFLAGS).Where(member => Attribute.IsDefined(member, typeof(ProvideAttribute)));

            foreach (PropertyInfo property in providedProperties)
            {
                if (property.GetGetMethod() == null)
                    throw new Exception($"Property {property.Name} does not have a public getter and cannot be provided");

                Type returnType = property.PropertyType;
                // Ensure the property has a setter and is not read-only
                if (property.GetSetMethod(true) != null)
                {
                    // Get the provided value after invoking the setter
                    object providedInstance = property.GetGetMethod().Invoke(provider, null);

                    // Invoke the setter to set the value before registering
                    property.GetSetMethod(true).Invoke(provider, new object[] { providedInstance });

                    //Try to set providedInstance again since the object will be created after setting.
                    providedInstance = property.GetGetMethod().Invoke(provider, null);

                    if (providedInstance != null)
                    {
                        // Add the provided object to the registry with its associated type
                        registery.Add(property.PropertyType, providedInstance);
                        Debug.Log($"Registered {property.PropertyType.Name} from {provider.GetType().Name}");
                    }
                    else throw new Exception($"Provider {provider.GetType().Name} returned null for {returnType.Name}");
                }
                else
                {
                    throw new Exception($"Property {property.Name} does not have a accessible setter and cannot be provided");
                }
            }
        }
        private void RegisterMethodProvider(IDependencyProvider provider)
        {
            //get all methods' info from the provider class.
            MethodInfo[] methods = provider.GetType().GetMethods(BINDINGFLAGS);

            //Loop all methods that is flagged with Provide Attribute
            foreach (MethodInfo method in methods)
            {
                if (!Attribute.IsDefined(method, typeof(ProvideAttribute))) continue;

                //If it is flagged, get type of the flagged method.
                Type returnType = method.ReturnType;

                //Invoke and get the method.
                object providedInstance = method.Invoke(provider, null);

                if (providedInstance != null)
                {
                    //If its an interface get the type of the returned and save the type like that.
                    returnType = returnType.IsInterface ? (Type)providedInstance.GetType() : returnType;

                    //Add the provided object to registery with its associated type.
                    registery.Add(returnType, providedInstance);
                    //Ex: Registered ServiceExampleA from Provider
                    Debug.Log($"Registered {returnType.Name} from {provider.GetType().Name}");
                }
                else throw new Exception($"Provider {provider.GetType().Name} returned null for {returnType.Name}");

            }
        }
        #endregion
        #region Helper Logic
        private object Resolve(Type type)
        {
            registery.TryGetValue(type, out object resolvedInstance);
            return resolvedInstance;
        }
        private static bool IsInjectable(MonoBehaviour obj)
        {
            MemberInfo[] members = obj.GetType().GetMembers(BINDINGFLAGS);
            return members.Any(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
        }

        private static MonoBehaviour[] FindMonoBehaviours()
        {
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID);
        }
        #endregion


#if UNITY_EDITOR
        [MenuItem("GameObject/Dependency Injection/Add Injector")]
        private static void AddDI()
        {
            GameObject go = new GameObject("Injector", typeof(Injector));
        }
#endif
    }

    //By adding <T> Generic to attribute brackets, we might specify which class that depends on the interface.
    //This would help us to use Interface based providers.
    //Problem is, if we create a Factory that implements abstract class, registerer won't know to inject which type
    //Since there could only be one Key in the dictionary.
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
    public sealed class InjectAttribute : Attribute
    {
        public InjectAttribute() { }
    }


    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ProvideAttribute : Attribute
    {
        public ProvideAttribute() { }
    }


    //Marker
    public interface IDependencyProvider { }
}
