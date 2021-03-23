using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    #region Args
    public class UnitStateChangeArg
    {
        public UnitState NewState;
        public bool ChangeDir = true;
        public Vector2Int Dir;
        public bool ReplayAnimation;
    }
    #endregion

    public abstract class UnitStateChangeModel
    {
        public abstract UnitState State { get; }

        protected virtual List<UnitStateChangePositionLogic> changeDirectionStrategies { get; }

        public UnitStateChangeArg ChangeDirection(Vector2Int dir, bool run = false)
        {
            if (changeDirectionStrategies == null)
                return null;

            foreach (var strategy in changeDirectionStrategies)
            {
                var res = strategy.Do(dir, run);
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

    public class UnitStateWalk : UnitStateIdle
    {
        public override UnitState State => UnitState.Walk;
    }

    public class UnitStateRun: UnitStateIdle
    {
        public override UnitState State => UnitState.Run;
    }

    public class UnitStateDash: UnitStateIdle
    {
        public override UnitState State => UnitState.Dash;
    }
    #endregion
}