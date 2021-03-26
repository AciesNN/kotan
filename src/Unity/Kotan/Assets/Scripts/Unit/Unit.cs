using System;
using UnityEngine;

namespace Unit
{
    public class Unit : MonoBehaviour
    {
        public event Action OnAnimationComplete;

        [SerializeField] UnitSettings settings;
        [SerializeField] UnitPhysics unitPhysics;
        [SerializeField] UnitAnimator unitAnimator;

        public UnitState UnitState { get; private set; }

        private Vector2Int curDir;
        public Vector2Int CurDir {
            get => curDir;
            set {
                curDir = value;
                unitAnimator.SetDir(CurDir);
            }
        }

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
                State = newState,
            });
        }
        
        public void SetState(UnitStateChangeArg newState)
        {
            if (newState.ChangeDir) {
                CurDir = newState.Dir;
            }
            unitAnimator.SetState(newState, changeAnim: (UnitState != newState.State || newState.ReplayAnimation));
            unitPhysics.SetState(newState);
            UnitState = newState.State;
        }

        public void AnimationComplete()
        {
            OnAnimationComplete?.Invoke();
        }
        #endregion

        #region Impl
        #endregion
    }
}