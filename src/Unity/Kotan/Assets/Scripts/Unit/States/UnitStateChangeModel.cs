using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Unit
{
    public abstract class UnitStateChangeModel
    {
        public abstract UnitState State { get; }

        protected virtual List<UnitStateChangePositionLogic> changeDirectionStrategies { get; }
        protected virtual List<UnitStateActionLogic> actionStrategies { get; }

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

        public UnitStateChangeArg Action(Unit unit, InputAction action)
        {
            if (actionStrategies == null)
                return null;

            foreach (var strategy in actionStrategies)
            {
                var res = strategy.Do(unit, action);
                if (res != null) {
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

        protected override List<UnitStateActionLogic> actionStrategies => new List<UnitStateActionLogic>() {
            new UnitStateActionLogic_Poke(),
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

    public class UnitStatePoke : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Poke;

        protected override List<UnitStateChangePositionLogic> changeDirectionStrategies => new List<UnitStateChangePositionLogic>() {
            new UnitStateChangePositionLogic_Walk(),
        };

        protected override List<UnitStateActionLogic> actionStrategies => new List<UnitStateActionLogic>() {
            new UnitStateActionLogic_Poke(),
            new UnitStateActionLogic_Combo1(),
        };
    }

    public class UnitStateCombo1 : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Combo1;

        protected override List<UnitStateChangePositionLogic> changeDirectionStrategies => new List<UnitStateChangePositionLogic>() {
            new UnitStateChangePositionLogic_Walk(),
        };
    }
    #endregion
}