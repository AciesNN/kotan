using System;
using UI;
using UnityEngine;

namespace Unit
{
    public abstract class UnitStateInputLogic
    {
        public bool DoOnlyOnAmimationComplete;

        public virtual bool ProcessInput()
        {
            if (DoOnlyOnAmimationComplete && !unit.IsAnimationComplete) {
                return false;
            }

            if (CheckCondition()) {
                ProcessImpl();
                return true;
            }

            return false;
        }

        protected abstract bool CheckCondition();
        protected abstract void ProcessImpl();

        //weird DI pattern? TODO?
        protected BufferedDirectonInput input;
        protected Unit unit;
        protected Vector2Int dir;
        protected bool force;
        public void SetCurrentData(BufferedDirectonInput input, Unit unit)
        {
            this.unit = unit;
            this.input = input;

            dir = input.CurrentDir;
            force = input.CurrentForce;
        }

        #region Checks - move to strategy classes
        protected bool CheckInputAction(InputAction desireableAction)
        {
            return input.CurrentAction.Equals(desireableAction);
        }

        protected bool CheckInputForce(bool desireableForce = true)
        {
            return desireableForce == force;
        }

        protected bool CheckInputDirecton(bool? xNotZero = null, bool? yNotZero = null, bool andCheck = true)
        {
            bool checkDirX = xNotZero.HasValue ? (xNotZero.Value ? dir.x != 0: dir.x == 0) : true;
            bool checkDirY = yNotZero.HasValue ? (yNotZero.Value ? dir.y != 0 : dir.y == 0) : true;
            bool checkDir = andCheck ? checkDirX && checkDirY : checkDirX || checkDirY;
            return checkDir;
        }
        #endregion

        #region Actions - move to strategy classes
        protected void SetUnitState(UnitState state)
        {
            unit.SetState(state);
        }
        
        protected void SetUnitDirection()
        {
            unit.SetDirection(dir);
        }
        
        protected void StopUnit()
        {
            unit.StopMove();
        }

        protected void UnitMove()
        {
            unit.Move(input.CurrentDir);
        }
        
        protected void UnitJump()
        {
            unit.Jump(dir);
        }

        protected void ResetForce()
        {
            input.ResetForce();
        }
        #endregion
    }
}