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

    public class UnitStateInputLogic_Jump : UnitStateInputLogic
    {
        protected override bool CheckChangeCondition(Unit unit, InputAction action, Vector2Int dir, bool force)
            => action == InputAction.Jump && dir.y == 0;

        protected override UnitStateChangeArg GetChangeArg(Unit unit, InputAction action, Vector2Int dir, bool force)
            => new UnitStateChangeArg()
            {
                State = UnitState.Jump,
                ProcessJump = true,
            };
    }

    public class UnitStateInputLogic_Poke : UnitStateInputLogic
    {
        private bool onAmimationComplete;
        public UnitStateInputLogic_Poke(bool onAmimationComplete = false)
        {
            this.onAmimationComplete = onAmimationComplete;
        }

        protected override bool CheckChangeCondition(Unit unit, InputAction action, Vector2Int dir, bool force)
            => action == InputAction.Slash 
            && (onAmimationComplete && unit.IsAnimationComplete || !unit.HitDetected);

        protected override UnitStateChangeArg GetChangeArg(Unit unit, InputAction action, Vector2Int dir, bool force)
            => new UnitStateChangeArg()
            {
                State = UnitState.Poke,
                ReplayAnimation = !onAmimationComplete,
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
    
    public class UnitStateInputLogic_Combo2 : UnitStateInputLogic
    {
        protected override bool CheckChangeCondition(Unit unit, InputAction action, Vector2Int dir, bool force)
            => action == InputAction.Slash && unit.HitDetected && unit.IsAnimationComplete;

        protected override UnitStateChangeArg GetChangeArg(Unit unit, InputAction action, Vector2Int dir, bool force)
            => new UnitStateChangeArg()
            {
                State = UnitState.Combo2,
            };
    }    
    
    public class UnitStateInputLogic_Combo3 : UnitStateInputLogic
    {
        protected override bool CheckChangeCondition(Unit unit, InputAction action, Vector2Int dir, bool force)
            => action == InputAction.Slash && unit.HitDetected && unit.IsAnimationComplete;

        protected override UnitStateChangeArg GetChangeArg(Unit unit, InputAction action, Vector2Int dir, bool force)
            => new UnitStateChangeArg()
            {
                State = UnitState.Combo3,
            };
    }
}