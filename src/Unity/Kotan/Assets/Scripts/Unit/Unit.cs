using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] UnitSettings settings;
        [SerializeField] UnitPhysics unitPhysics;
        [SerializeField] UnitAnimator unitAnimator;

        public UnitState UnitState { get; private set; }

        #region MonoBehaviour
        private void Awake()
        {
            SetState(UnitState.Idle);
        }
        #endregion

        #region Interface
        public void SetState(UnitState newState)
        {
            SetState(new UnitStateChangeArg() {
                NewState = newState,
            });
        }
        
        public void SetState(UnitStateChangeArg newUnitStateChange)
        {
            unitAnimator.SetState(newUnitStateChange, changeAnim: (UnitState != newUnitStateChange.NewState || newUnitStateChange.ReplayAnimation));
            unitPhysics.SetState(newUnitStateChange);
            UnitState = newUnitStateChange.NewState;
        }

        public void Move(Vector2Int dir)
        {
        }

        public void Stop()
        {
        }
        #endregion

        #region Impl
        #endregion
    }
}