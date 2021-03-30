using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Unit
{
    public abstract class UnitStateChangeModel
    {
        public abstract UnitState State { get; }

        protected virtual List<UnitStateInputLogic> unitStateInputLogic { get; }

        public UnitStateChangeArg ChangeDirection(Unit unit, InputAction action, Vector2Int dir, bool run = false)
        {
            if (unitStateInputLogic == null)
                return null;

            foreach (var strategy in unitStateInputLogic)
            {
                var res = strategy.Do(unit, action, dir, run);
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

        protected override List<UnitStateInputLogic> unitStateInputLogic => new List<UnitStateInputLogic>() {
            new UnitStateInputLogic_Poke(),

            new UnitStateInputLogic_Idle(),
            new UnitStateInputLogic_Run(),
            new UnitStateInputLogic_Dash(),
            new UnitStateInputLogic_Walk(),
        };
    }

    public class UnitStateWalk : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Walk;

        protected override List<UnitStateInputLogic> unitStateInputLogic => new List<UnitStateInputLogic>() {
            new UnitStateInputLogic_Poke(),

            new UnitStateInputLogic_Idle(),
            new UnitStateInputLogic_Run(),
            new UnitStateInputLogic_Dash(),
            new UnitStateInputLogic_Walk(),
        };
    }

    public class UnitStateRun: UnitStateChangeModel
    {
        public override UnitState State => UnitState.Run;

        protected override List<UnitStateInputLogic> unitStateInputLogic => new List<UnitStateInputLogic>() {
            new UnitStateInputLogic_Idle(),
            new UnitStateInputLogic_ContinueRun(),
            new UnitStateInputLogic_Dash(),
            new UnitStateInputLogic_Walk(),
        };
    }

    public class UnitStateDash : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Dash;
    }

    public class UnitStatePoke : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Poke;

        protected override List<UnitStateInputLogic> unitStateInputLogic => new List<UnitStateInputLogic>() {
            new UnitStateInputLogic_Walk(),

            new UnitStateInputLogic_Poke(),
            new UnitStateInputLogic_Combo1(),
       };
    }

    public class UnitStateCombo1 : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Combo1;

        protected override List<UnitStateInputLogic> unitStateInputLogic => new List<UnitStateInputLogic>() {
            new UnitStateInputLogic_Walk(),
        };
    }
    #endregion
}