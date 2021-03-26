using UnityEngine;

namespace Unit
{
    public abstract class UnitStateChangePositionLogic
    {
        public UnitStateChangeArg Do(Vector2Int curDir, Vector2Int dir, bool run)
        {
            return CheckChangeCondition(curDir, dir, run) ? GetChangeArg(curDir, dir, run) : null;
        }

        protected abstract bool CheckChangeCondition(Vector2Int curDir, Vector2Int dir, bool run);
        protected abstract UnitStateChangeArg GetChangeArg(Vector2Int curDir, Vector2Int dir, bool run);
    }

    public class UnitStateChangePositionLogic_Idle : UnitStateChangePositionLogic
    {
        protected override bool CheckChangeCondition(Vector2Int curDir, Vector2Int dir, bool run)
            => dir.x == 0 && dir.y == 0;

        protected override UnitStateChangeArg GetChangeArg(Vector2Int curDir, Vector2Int dir, bool run)
            => new UnitStateChangeArg()
            {
                State = UnitState.Idle,
                ChangeDir = false,
            };
    }

    public class UnitStateChangePositionLogic_Run : UnitStateChangePositionLogic
    {
        protected override bool CheckChangeCondition(Vector2Int curDir, Vector2Int dir, bool run)
            => run && dir.x != 0 && dir.y == 0;

        protected override UnitStateChangeArg GetChangeArg(Vector2Int curDir, Vector2Int dir, bool run)
            => new UnitStateChangeArg()
            {
                State = UnitState.Run,
            };
    }

    public class UnitStateChangePositionLogic_ContinueRun : UnitStateChangePositionLogic
    {
        protected override bool CheckChangeCondition(Vector2Int curDir, Vector2Int dir, bool run)
            => dir.x != 0 && dir.x == curDir.x && dir.y != 0;

        protected override UnitStateChangeArg GetChangeArg(Vector2Int curDir, Vector2Int dir, bool run)
            => new UnitStateChangeArg()
            {
                State = UnitState.Run,
            };
    }

    public class UnitStateChangePositionLogic_Dash : UnitStateChangePositionLogic
    {
        protected override bool CheckChangeCondition(Vector2Int curDir, Vector2Int dir, bool run)
            => run && dir.x == 0 && dir.y != 0;
        protected override UnitStateChangeArg GetChangeArg(Vector2Int curDir, Vector2Int dir, bool run)
            => new UnitStateChangeArg()
            {
                State = UnitState.Dash,
            };
    }

    public class UnitStateChangePositionLogic_Walk : UnitStateChangePositionLogic
    {
        protected override bool CheckChangeCondition(Vector2Int curDir, Vector2Int dir, bool run)
            => dir.x != 0 || dir.y != 0;
        protected override UnitStateChangeArg GetChangeArg(Vector2Int curDir, Vector2Int dir, bool run)
            => new UnitStateChangeArg()
            {
                State = UnitState.Walk,
            };
    }
}