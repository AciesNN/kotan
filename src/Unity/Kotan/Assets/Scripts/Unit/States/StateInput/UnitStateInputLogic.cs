using UI;
using UnityEngine;

namespace Unit
{
    public abstract class UnitStateInputLogic
    {
        public bool DoOnlyOnAmimationComplete;

        public virtual bool ProcessInput(Unit unit, InputAction action, Vector2Int dir, bool force)
        {
            if (DoOnlyOnAmimationComplete && !unit.IsAnimationComplete) {
                return false;
            }

            if (CheckCondition(unit, action, dir, force)) {
                ProcessImpl(unit, action, dir, force);
                return true;
            }

            return false;
        }

        protected abstract bool CheckCondition(Unit unit, InputAction action, Vector2Int dir, bool force);
        protected abstract void ProcessImpl(Unit unit, InputAction action, Vector2Int dir, bool force);

        protected void SetUnitState(Unit unit, UnitState state)
        {
            unit.SetState(state);
        }
        
        protected void SetUnitDirection(Unit unit, Vector2Int dir)
        {
            unit.SetDirection(dir);
        }
        
        protected void StopUnit(Unit unit)
        {
            unit.StopMove();
        }

        protected void UnitMove(Unit unit, Vector2Int dir)
        {
            unit.Move(dir);
        }
        
        protected void UnitJump(Unit unit, Vector2Int dir)
        {
            unit.Jump(dir);
        }
    }
}