using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Unit
{
    public class UnitStateIdle : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Idle;

        protected override List<UnitStateInputLogic> unitStateInputLogic => new List<UnitStateInputLogic>() {
            new UnitStateInputLogic_Poke(),
            new UnitStateInputLogic_Jump(),

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
            new UnitStateInputLogic_Jump(),

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
    
    public class UnitStateJump : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Jump;
    }

    public class UnitStateFall : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Fall;

        protected override List<UnitStateInputLogic> unitStateInputLogic => new List<UnitStateInputLogic>() {
            new UnitStateInputLogic_Jump {DoOnlyOnAmimationComplete = true},
            new UnitStateInputLogic_Poke {DoOnlyOnAmimationComplete = true},

            new UnitStateInputLogic_Walk(),
        };
    }

    public class UnitStatePoke : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Poke;

        protected override List<UnitStateInputLogic> unitStateInputLogic => new List<UnitStateInputLogic>() {
            new UnitStateInputLogic_Walk(),

            new UnitStateInputLogic_Combo1() {DoOnlyOnAmimationComplete = true},
            new UnitStateInputLogic_Poke {DoOnlyOnAmimationComplete = true},
        };

        protected override InputAction ActionToLockBuffer => InputAction.Slash;
    }

    public class UnitStateCombo1 : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Combo1;

        protected override List<UnitStateInputLogic> unitStateInputLogic => new List<UnitStateInputLogic>() {
            new UnitStateInputLogic_Walk(),

            new UnitStateInputLogic_Combo2() {DoOnlyOnAmimationComplete = true}
        };
    }    
    
    public class UnitStateCombo2 : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Combo2;

        protected override List<UnitStateInputLogic> unitStateInputLogic => new List<UnitStateInputLogic>() {
            new UnitStateInputLogic_Walk(),

            new UnitStateInputLogic_Combo3() {DoOnlyOnAmimationComplete = true},
        };
    }   
    
    public class UnitStateCombo3 : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Combo3;

        protected override List<UnitStateInputLogic> unitStateInputLogic => new List<UnitStateInputLogic>() {
            new UnitStateInputLogic_Walk(),

            new UnitStateInputLogic_Poke() {DoOnlyOnAmimationComplete = true},
        };
    }
}