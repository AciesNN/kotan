using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    public abstract class UnitStateChangeModel
    {
        public abstract UnitState State { get; }

        protected virtual List<UnitStateChangePositionLogic> changeDirectionStrategies { get; }

        public UnitStateChangeArg ChangeDirection(Unit unit, Vector2Int dir, bool run = false)
        {
            if (changeDirectionStrategies == null)
                return null;

            foreach (var strategy in changeDirectionStrategies)
            {
                var res = strategy.Do(unit.CurDir, dir, run);
                if (res != null) {
                    res.Dir = dir;
                    return res;
                }
            }

            return null;
        }
    }

    #region State models
    public class UnitStateIdle : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Idle;

        protected override List<UnitStateChangePositionLogic> changeDirectionStrategies => new List<UnitStateChangePositionLogic>() {
            new UnitStateChangePositionLogic_Idle(),
            new UnitStateChangePositionLogic_Run(),
            new UnitStateChangePositionLogic_Dash(),
            new UnitStateChangePositionLogic_Walk(),
        };
    }

    public class UnitStateWalk : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Walk;

        protected override List<UnitStateChangePositionLogic> changeDirectionStrategies => new List<UnitStateChangePositionLogic>() {
            new UnitStateChangePositionLogic_Idle(),
            new UnitStateChangePositionLogic_Run(),
            new UnitStateChangePositionLogic_Dash(),
            new UnitStateChangePositionLogic_Walk(),
        };
    }

    public class UnitStateRun: UnitStateChangeModel
    {
        public override UnitState State => UnitState.Run;

        protected override List<UnitStateChangePositionLogic> changeDirectionStrategies => new List<UnitStateChangePositionLogic>() {
            new UnitStateChangePositionLogic_Idle(),
            new UnitStateChangePositionLogic_ContinueRun(),
            new UnitStateChangePositionLogic_Dash(),
            new UnitStateChangePositionLogic_Walk(),
        };
    }

    public class UnitStateDash : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Dash;
    }
    #endregion
}