using System;
using UI;
using UnityEngine;

namespace Unit
{
    public abstract class UnitStateInputLogic
    {
        protected virtual InputAction? checkAction => null;
        protected virtual bool? checkInputForce => null;
        
        public bool? CheckAmimationComplete;
        protected bool? checkAmimationComplete => CheckAmimationComplete;
        
        protected virtual UnitState? newState => null;
        protected virtual bool setDir => false;

        public virtual bool ProcessInput()
        {
            if (!CheckAnimationComplete()) {
                return false;
            }

            if (!CheckInputForce()) {
                return false;
            }
            
            if (!CheckInputAction()) {
                return false;
            }

            if (!CheckCondition()) {
                return false;
            }

            SetUnitState();
            SetUnitDirection();
            ProcessImpl();
            return true;
        }

        protected virtual bool CheckCondition() => true;
        protected virtual void ProcessImpl() { }

        #region DI
        //weird DI pattern? TODO?
        protected BufferedDirectonInput input;
        protected Unit unit;
        protected Vector2Int dir;
        protected bool force;
        protected InputAction action;
        public void SetCurrentData(BufferedDirectonInput input, Unit unit)
        {
            this.unit = unit;
            this.input = input;

            dir = input.CurrentDir;
            force = input.CurrentForce;
            action = input.CurrentAction;
        }
        #endregion

        #region Checks - move to strategy classes
        protected bool CheckAnimationComplete()
        {
            return checkAmimationComplete.HasValue ? checkAmimationComplete.Value == unit.IsAnimationComplete : true;
        }

        protected bool CheckUnitHitDetected()
        {
            return unit.HitDetected;
        }

        protected bool CheckInputAction()
        {
            return checkAction.HasValue ? input.CurrentAction.Equals(checkAction.Value) : true;
        }

        protected bool CheckInputForce()
        {
            return checkInputForce.HasValue ? checkInputForce == force : true;
        }

        protected bool CheckInputDirecton(bool? xNotZero = null, bool? yNotZero = null)
        {
            bool checkDirX = xNotZero.HasValue ? (xNotZero.Value ? dir.x != 0: dir.x == 0) : true;
            bool checkDirY = yNotZero.HasValue ? (yNotZero.Value ? dir.y != 0 : dir.y == 0) : true;
            return checkDirX && checkDirY;
        }
        #endregion

        #region Actions - move to strategy classes
        private void SetUnitState()
        {
            if (newState.HasValue) {
                unit.SetState(newState.Value);
            }
        }
        
        private void SetUnitDirection()
        {
            if (setDir) {
                unit.SetDirection(dir);
            }
        }
        
        protected void StopUnit()
        {
            unit.StopMove();
        }

        protected void UnitMove()
        {
            unit.Move(input.CurrentDir, force);
        }
        
        protected void UnitJump()
        {
            unit.Jump(dir, force);
        }

        protected void ResetForce()
        {
            input.ResetForce();
        }
        #endregion
    }
}