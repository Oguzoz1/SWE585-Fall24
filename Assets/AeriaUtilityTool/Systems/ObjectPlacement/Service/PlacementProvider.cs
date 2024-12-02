using AeriaUtil.Pattern.DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Systems.ObjectPlacement
{
    /// <summary>
    /// Object Placement System's Dependency Injection Provider 
    /// </summary>
    public class PlacementProvider : MonoBehaviour, IDependencyProvider
    {
        [Provide]
        public PlayerObjectHolder PlayerObjectHolder
        {
            get
            {
                return _playerObjectHolder;
            }
            set
            {
                _playerObjectHolder = _playerObjectHolder == null ?
                    FindObjectOfType<PlayerObjectHolder>() : _playerObjectHolder;
            }
        }

        private PlayerObjectHolder _playerObjectHolder = null;

        [Provide]
        public ObjectGrabber ObjectGrabber
        {
            get
            {
                return _objectGrabber;
            }
            private set
            {
                _objectGrabber = _objectGrabber 
                    == null ? (ObjectGrabber)PlacementFactory.CreatePlacementService<ObjectGrabber>() : _objectGrabber;
            }
        }
        private ObjectGrabber _objectGrabber = null;

        [Provide]
        public ObjectPlacer ObjectPlacer
        {
            get
            {
                return _objectPlacer;
            }
            private set
            {
                _objectPlacer = _objectPlacer
                         == null ? (ObjectPlacer)PlacementFactory.CreatePlacementService<ObjectPlacer>() : _objectPlacer;
            }
        }
        private ObjectPlacer _objectPlacer = null;

        [Provide]
        public ObjectAttacher ObjectAttacher
        {
            get
            {
                return _objectAttacher;
            }
            private set
            {
                _objectAttacher = _objectAttacher
                         == null ? (ObjectAttacher)PlacementFactory.CreatePlacementService<ObjectAttacher>() : _objectAttacher;
            }
        }
        private ObjectAttacher _objectAttacher = null;

        [Provide]
        public ObjectRotator ObjectRotator
        {
            get
            {
                return _objectRotator;
            }
            private set
            {
                _objectRotator = _objectRotator
                         == null ? (ObjectRotator)PlacementFactory.CreatePlacementService<ObjectRotator>() : _objectRotator;
            }
        }


        private ObjectRotator _objectRotator = null;

        [Provide]
        public PlacementStateManager PlacementStateManager
        {
            get
            {
                return _placementStateManager;
            }
            set 
            {
                _placementStateManager = _placementStateManager
                         == null ? new PlacementStateManager(PlayerObjectHolder) : _placementStateManager;
            }
        }


        private PlacementStateManager _placementStateManager = null;
    }

    public static class PlacementFactory
    {
        public static IPlacementService CreatePlacementService<T>() where T : IPlacementService, new()
        {
            return new T();
        }
    }
}
