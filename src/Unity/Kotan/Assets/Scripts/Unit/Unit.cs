using System;
using UnityEngine;

namespace Unit
{
    public class Unit : MonoBehaviour
    {
        public event Action OnAnimationComplete;
        public event Action OnStateChanged;

        [SerializeField] UnitSettings settings;
        [SerializeField] UnitPhysics unitPhysics;
        [SerializeField] UnitAnimator unitAnimator;
        [SerializeField] UnitWeapon unitWeapon;

        public UnitState State { get; private set; }

        private Vector2Int curDir;
        public Vector2Int CurDir
        {
            get => curDir;
            set
            {
                curDir = value;
                unitAnimator.SetDirection(CurDir);
            }
        }

        public bool IsAnimationComplete => unitAnimator.IsAnimationComplete;

        public bool HitDetected => unitWeapon.HitDetected;

        #region MonoBehaviour
        private void Awake()
        {
            unitPhysics.Init(this);
            SetState(UnitState.Idle);
        }
        #endregion

        #region Interface
        public void SetState(UnitState newState)
        {
            //Debug.Log($"-> {newState}");

            unitWeapon.Reset();
            State = newState;
            unitAnimator.SetState(newState);
            OnStateChanged?.Invoke();
        }

        public void SetDirection(Vector2Int dir)
        {
            CurDir = dir;
            unitAnimator.SetDirection(dir);
        }

        public void StopMove()
        {
            unitPhysics.StopMove();
        }

        public void Move(Vector2Int dir)
        {
            SetDirection(dir);
            unitPhysics.Move(dir);
        }

        public void Jump(Vector2Int dir)
        {
            SetDirection(dir);
            unitPhysics.Jump(dir);
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