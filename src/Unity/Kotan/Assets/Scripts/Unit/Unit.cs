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
        [SerializeField] UnitWeapon unitWeapon;

        public UnitState UnitState { get; private set; }

        private Vector2Int curDir;
        public Vector2Int CurDir
        {
            get => curDir;
            set
            {
                curDir = value;
                unitAnimator.SetDir(CurDir);
            }
        }

        public bool IsAnimationComplete => unitAnimator.IsAnimationComplete;

        public bool HitDetected => unitWeapon.HitDetected;

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
            if (newState == null) {
                return;
            }

            if (newState.ChangeDir) {
                CurDir = newState.Dir;
            }
            unitAnimator.SetState(newState, changeAnim: (UnitState != newState.State || newState.ReplayAnimation));
            unitPhysics.SetState(newState);
            unitWeapon.Reset();
            UnitState = newState.State;
        }

        public void AnimationEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentException("Empty animation event name");
            }

            switch (eventName)
            {
                case "AnimationComplete":
                    unitAnimator.AnimationComplete();
                    OnAnimationComplete?.Invoke();
                    break;
                case "WeaponOn":
                    unitWeapon.SetActive(true);
                    break;
                case "WeaponOff":
                    unitWeapon.SetActive(false);
                    break;
            }
        }
        #endregion

        #region Impl
        #endregion
    }
}