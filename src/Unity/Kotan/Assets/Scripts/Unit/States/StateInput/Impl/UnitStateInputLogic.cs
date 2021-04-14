using UI;
using UnityEngine;

namespace Unit
{
    public class UnitStateInputLogic_Idle : UnitStateInputLogic
    {
        protected override bool CheckCondition(Unit unit, InputAction action, Vector2Int dir, bool force)
            => dir.x == 0 && dir.y == 0;

        protected override void ProcessImpl(Unit unit, InputAction action, Vector2Int dir, bool force)
        {
            SetUnitState(unit, UnitState.Idle);
            SetUnitDirection(unit, dir);
            StopUnit(unit);
        }
    }

    public class UnitStateInputLogic_Run : UnitStateInputLogic
    {
        protected override bool CheckCondition(Unit unit, InputAction action, Vector2Int dir, bool force)
            => force && dir.x != 0 && dir.y == 0;

        protected override void ProcessImpl(Unit unit, InputAction action, Vector2Int dir, bool force)
        {
            SetUnitState(unit, UnitState.Run);
            UnitMove(unit, dir);
        }
    }

    public class UnitStateInputLogic_ContinueRun : UnitStateInputLogic
    {
        protected override bool CheckCondition(Unit unit, InputAction action, Vector2Int dir, bool force)
            => dir.x != 0 && dir.x == unit.CurDir.x;

        protected override void ProcessImpl(Unit unit, InputAction action, Vector2Int dir, bool force)
        {
            SetUnitState(unit, UnitState.Run);
            UnitMove(unit, dir);
        }
    }

    public class UnitStateInputLogic_Dash : UnitStateInputLogic
    {
        protected override bool CheckCondition(Unit unit, InputAction action, Vector2Int dir, bool force)
            => force && dir.x == 0 && dir.y != 0;

        protected override void ProcessImpl(Unit unit, InputAction action, Vector2Int dir, bool force)
        {
            SetUnitState(unit, UnitState.Dash);
            UnitMove(unit, dir);
        }
    }

    public class UnitStateInputLogic_Walk : UnitStateInputLogic
    {
        protected override bool CheckCondition(Unit unit, InputAction action, Vector2Int dir, bool force)
            => dir.x != 0 || dir.y != 0;

        protected override void ProcessImpl(Unit unit, InputAction action, Vector2Int dir, bool force)
        {
            SetUnitState(unit, UnitState.Walk);
            UnitMove(unit, dir);
        }
    }

    public class UnitStateInputLogic_Jump : UnitStateInputLogic
    {
        protected override bool CheckCondition(Unit unit, InputAction action, Vector2Int dir, bool force)
            => action == InputAction.Jump && dir.y == 0;

        protected override void ProcessImpl(Unit unit, InputAction action, Vector2Int dir, bool force)
        {
            SetUnitState(unit, UnitState.Jump);
            UnitJump(unit, dir);
        }
    }

    public class UnitStateInputLogic_Poke : UnitStateInputLogic
    {
        protected override bool CheckCondition(Unit unit, InputAction action, Vector2Int dir, bool force)
            => action == InputAction.Slash;

        protected override void ProcessImpl(Unit unit, InputAction action, Vector2Int dir, bool force)
        {
            SetUnitState(unit, UnitState.Poke);
            SetUnitDirection(unit, dir);
            StopUnit(unit);
        }
    }

    public class UnitStateInputLogic_Combo1 : UnitStateInputLogic
    {
        protected override bool CheckCondition(Unit unit, InputAction action, Vector2Int dir, bool force)
            => action == InputAction.Slash && unit.HitDetected;

        protected override void ProcessImpl(Unit unit, InputAction action, Vector2Int dir, bool force)
        {
            SetUnitState(unit, UnitState.Combo1);
            SetUnitDirection(unit, dir);
        }
    }    
    
    public class UnitStateInputLogic_Combo2 : UnitStateInputLogic
    {
        protected override bool CheckCondition(Unit unit, InputAction action, Vector2Int dir, bool force)
            => action == InputAction.Slash && unit.HitDetected;

        protected override void ProcessImpl(Unit unit, InputAction action, Vector2Int dir, bool force)
        {
            SetUnitState(unit, UnitState.Combo2);
            SetUnitDirection(unit, dir);
        }
    }    
    
    public class UnitStateInputLogic_Combo3 : UnitStateInputLogic
    {
        protected override bool CheckCondition(Unit unit, InputAction action, Vector2Int dir, bool force)
            => action == InputAction.Slash && unit.HitDetected;

        protected override void ProcessImpl(Unit unit, InputAction action, Vector2Int dir, bool force)
        {
            SetUnitState(unit, UnitState.Combo3);
            SetUnitDirection(unit, dir);
        }
    }
}