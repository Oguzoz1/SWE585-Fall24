using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Systems.ObjectPlacement
{
    /// <summary>
    /// Responsible of declaring state functions.
    /// </summary>
    public interface IPlacementState
    {
        void EnterState(PlayerObjectHolder playerContext);
        void UpdateState(PlayerObjectHolder playerContext);
        void ExitState(PlayerObjectHolder playerContext);
    }

    /// <summary>
    /// Responsible of usage of states. Acts as a recipie.
    /// </summary>

    /// <summary>
    /// Service Mark
    /// </summary>
    public interface IPlacementService
    {

    }
    public class PlacementStateManager
    {

        private IPlacementState _currentState;
        private Dictionary<Type, IPlacementState> _stateMap = new Dictionary<Type, IPlacementState>();

        public IPlacementState CurrentState { get => _currentState; set => _currentState = value; }
        private PlayerObjectHolder _objectHolder;
        public PlacementStateManager(PlayerObjectHolder objHolder)
        {
            _objectHolder = objHolder;
            InitStates();
            SetState<IdleState>();
        }
        private void InitStates()
        {
            //Violation of IoC
            _stateMap[typeof(IdleState)] = new IdleState();
            _stateMap[typeof(GrabbingState)] = new GrabbingState();
            _stateMap[typeof(PlacingState)] = new PlacingState();
        }
        public void SetState<T>() where T : IPlacementState
        {
            Type stateType = typeof(T);
            // If the state is already the same, return.
            if (_currentState != null && _currentState.GetType() == stateType) return;

            if (_stateMap.TryGetValue(stateType, out var newState))
            {
                _currentState?.ExitState(_objectHolder);
                _currentState = newState;
                _currentState.EnterState(_objectHolder);
            }
            else Debug.LogError($"State {stateType} not found in state map.");
        }
        public void Update() => _currentState?.UpdateState(_objectHolder);
    }

    public class GrabbingState : IPlacementState
    {
        public void EnterState(PlayerObjectHolder playerContext) => playerContext.EnterGrabbingState();

        public void ExitState(PlayerObjectHolder playerContext) { }

        public void UpdateState(PlayerObjectHolder playerContext) => playerContext.UpdateGrabbing();
    }

    public class IdleState : IPlacementState
    {
        public void EnterState(PlayerObjectHolder playerContext) => playerContext.EnterIdleState();
        public void ExitState(PlayerObjectHolder playerContext) { }
        public void UpdateState(PlayerObjectHolder playerContext) => playerContext.UpdateIdling();
    }

    public class PlacingState : IPlacementState
    {
        public void EnterState(PlayerObjectHolder playerContext) => playerContext.EnterPlacingState();
        public void ExitState(PlayerObjectHolder playerContext) => playerContext.ExitPlacingState();
        public void UpdateState(PlayerObjectHolder playerContext) => playerContext.UpdatePlacing();

    }
}
