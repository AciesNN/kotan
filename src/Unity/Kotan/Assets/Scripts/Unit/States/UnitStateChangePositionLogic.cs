using UnityEngine;

namespace Unit
{
    public abstract class UnitStateChangePositionLogic
    {
        public UnitStateChangeArg Do(Vector2Int dir, bool run)
        {
            return CheckChangeCondition(dir, run) ? GetChangeArg(dir, run) : null;
        }

        protected abstract bool CheckChangeCondition(Vector2Int dir, bool run);
        protected abstract UnitStateChangeArg GetChangeArg(Vector2Int dir, bool run);
    }

    public class UnitStateChangePositionLogic_Idle : UnitStateChangePositionLogic
    {
        protected override bool CheckChangeCondition(Vector2Int dir, bool run)
            => dir.x == 0 && dir.y == 0;

        protected override UnitStateChangeArg GetChangeArg(Vector2Int dir, bool run)
            => new UnitStateChangeArg()
            {
                NewState = UnitState.Idle,
                ChangeDir = false,
            };
    }

    public class UnitStateChangePositionLogic_Run : UnitStateChangePositionLogic
    {
        protected override bool CheckChangeCondition(Vector2Int dir, bool run)
            => run && dir.x != 0;
        protected override UnitStateChangeArg GetChangeArg(Vector2Int dir, bool run)
            => new UnitStateChangeArg()
            {
                NewState = UnitState.Run,
            };
    }

    public class UnitStateChangePositionLogic_Dash : UnitStateChangePositionLogic
    {
        protected override bool CheckChangeCondition(Vector2Int dir, bool run)
            => run && dir.x == 0;
        protected override UnitStateChangeArg GetChangeArg(Vector2Int dir, bool run)
            => new UnitStateChangeArg()
            {
                NewState = UnitState.Dash,
            };
    }

    public class UnitStateChangePositionLogic_Walk : UnitStateChangePositionLogic
    {
        protected override bool CheckChangeCondition(Vector2Int dir, bool run)
            => !run;
        protected override UnitStateChangeArg GetChangeArg(Vector2Int dir, bool run)
            => new UnitStateChangeArg()
            {
                NewState = UnitState.Walk,
            };
    }
}