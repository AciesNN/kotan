using UI;
using UnityEngine;

namespace Unit
{
    public abstract class UnitStateInputLogic
    {
        public UnitStateChangeArg Do(Unit unit, InputAction action, Vector2Int dir, bool force)
        {
            return CheckChangeCondition(unit, action, dir, force) ? GetChangeArg(unit, action, dir, force) : null;
        }

        protected abstract bool CheckChangeCondition(Unit unit, InputAction action, Vector2Int dir, bool force);
        protected abstract UnitStateChangeArg GetChangeArg(Unit unit, InputAction action, Vector2Int dir, bool force);
    }

    public class UnitStateInputLogic_Idle : UnitStateInputLogic
    {
        protected override bool CheckChangeCondition(Unit unit, InputAction action, Vector2Int dir, bool force)
            => dir.x == 0 && dir.y == 0;

        protected override UnitStateChangeArg GetChangeArg(Unit unit, InputAction action, Vector2Int dir, bool force)
            => new UnitStateChangeArg()
            {
                State = UnitState.Idle,
                ChangeDir = false,
            };
    }

    public class UnitStateInputLogic_Run : UnitStateInputLogic
    {
        protected override bool CheckChangeCondition(Unit unit, InputAction action, Vector2Int dir, bool force)
            => force && dir.x != 0 && dir.y == 0;

        protected override UnitStateChangeArg GetChangeArg(Unit unit, InputAction action, Vector2Int dir, bool force)
            => new UnitStateChangeArg()
            {
                State = UnitState.Run,
            };
    }

    public class UnitStateInputLogic_ContinueRun : UnitStateInputLogic
    {
        protected override bool CheckChangeCondition(Unit unit, InputAction action, Vector2Int dir, bool force)
            => dir.x != 0 && dir.x == unit.CurDir.x;

        protected override UnitStateChangeArg GetChangeArg(Unit unit, InputAction action, Vector2Int dir, bool force)
            => new UnitStateChangeArg()
            {
                State = UnitState.Run,
            };
    }

    public class UnitStateInputLogic_Dash : UnitStateInputLogic
    {
        protected override bool CheckChangeCondition(Unit unit, InputAction action, Vector2Int dir, bool force)
            => force && dir.x == 0 && dir.y != 0;

        protected override UnitStateChangeArg GetChangeArg(Unit unit, InputAction action, Vector2Int dir, bool force)
            => new UnitStateChangeArg()
            {
                State = UnitState.Dash,
            };
    }

    public class UnitStateInputLogic_Walk : UnitStateInputLogic
    {
        protected override bool CheckChangeCondition(Unit unit, InputAction action, Vector2Int dir, bool force)
            => dir.x != 0 || dir.y != 0;

        protected override UnitStateChangeArg GetChangeArg(Unit unit, InputAction action, Vector2Int dir, bool force)
            => new UnitStateChangeArg()
            {
                State = UnitState.Walk,
            };
    }

    public class UnitStateInputLogic_Poke : UnitStateInputLogic
    {
        protected override bool CheckChangeCondition(Unit unit, InputAction action, Vector2Int dir, bool force)
            => action == InputAction.Slash && !unit.HitDetected;

        protected override UnitStateChangeArg GetChangeArg(Unit unit, InputAction action, Vector2Int dir, bool force)
            => new UnitStateChangeArg()
            {
                State = UnitState.Poke,
                ReplayAnimation = true,
            };
    }

    public class UnitStateInputLogic_Combo1 : UnitStateInputLogic
    {
        protected override bool CheckChangeCondition(Unit unit, InputAction action, Vector2Int dir, bool force)
            => action == InputAction.Slash && unit.HitDetected && unit.IsAnimationComplete;

        protected override UnitStateChangeArg GetChangeArg(Unit unit, InputAction action, Vector2Int dir, bool force)
            => new UnitStateChangeArg()
            {
                State = UnitState.Combo1,
            };
    }
}