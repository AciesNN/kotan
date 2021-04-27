using System.Collections.Generic;
using UI;
using Unit.UnitStateInputLogic;

namespace Unit
{
    public class UnitStateIdle : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Idle;

        protected override List<BaseUnitStateInputLogic> unitStateInputLogic => new List<BaseUnitStateInputLogic>() {
            new Poke(),
            new Jump(),

            new Idle(),
            new Run(),
            new Dash(),
            new Walk(),
        };
    }

    public class UnitStateWalk : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Walk;

        protected override List<BaseUnitStateInputLogic> unitStateInputLogic => new List<BaseUnitStateInputLogic>() {
            new Poke(),
            new Jump(),

            new Idle(),
            new Run(),
            new Dash(),
            new Walk(),
        };
    }

    public class UnitStateRun: UnitStateChangeModel
    {
        public override UnitState State => UnitState.Run;

        protected override List<BaseUnitStateInputLogic> unitStateInputLogic => new List<BaseUnitStateInputLogic>() {
            new Idle(),
            new JumpRun(),

            new ContinueRun(),
            new Dash(),
            new Walk(),
        };
    }

    public class UnitStateDash : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Dash;
    }

    public class UnitStateJump : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Jump;

        protected override List<BaseUnitStateInputLogic> unitStateInputLogic => new List<BaseUnitStateInputLogic>() {
            new JumpMove(),
        };
    }

    public class UnitStateFall : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Fall;

        protected override List<BaseUnitStateInputLogic> unitStateInputLogic => new List<BaseUnitStateInputLogic>() {
            new JumpMove() {CheckAmimationComplete = false},

            new Jump {CheckAmimationComplete = true},
            new Poke {CheckAmimationComplete = true},

            new Walk() {CheckAmimationComplete = true},
        };
    }

    public class UnitStatePoke : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Poke;

        protected override List<BaseUnitStateInputLogic> unitStateInputLogic => new List<BaseUnitStateInputLogic>() {
            new Walk() {CheckAmimationComplete = true},

            new Combo() {CheckAmimationComplete = true, N = 1},
            new Poke {CheckAmimationComplete = true},

            new BufferInput() {BufferAction = InputAction.Slash},
        };
    }

    public class UnitStateCombo1 : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Combo1;

        protected override List<BaseUnitStateInputLogic> unitStateInputLogic => new List<BaseUnitStateInputLogic>() {
            new Walk() {CheckAmimationComplete = true},

            new Combo() {CheckAmimationComplete = true, N = 2}
        };
    }    
    
    public class UnitStateCombo2 : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Combo2;

        protected override List<BaseUnitStateInputLogic> unitStateInputLogic => new List<BaseUnitStateInputLogic>() {
            new Walk() {CheckAmimationComplete = true},

            new Combo() {CheckAmimationComplete = true, N = 3},
        };
    }   
    
    public class UnitStateCombo3 : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Combo3;

        protected override List<BaseUnitStateInputLogic> unitStateInputLogic => new List<BaseUnitStateInputLogic>() {
            new Walk() {CheckAmimationComplete = true},

            new Poke() {CheckAmimationComplete = true},
        };
    }
}