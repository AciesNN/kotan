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
            new Parry(),
            new Magic(),

            new Idle(), //TODO ??? -> None ?
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
            new DashAttack(),
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

    public class UnitStateDamage : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Damage;
    }

    public class UnitStateKnockout : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Knockout;
    }

    public class UnitStateParry : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Parry;
    }

    public class UnitStateMagic : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Magic;
    }

    public class UnitStateDashAttack : UnitStateChangeModel
    {
        public override UnitState State => UnitState.DashAttack;
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
            new HitBack() {CheckAmimationComplete = true},

            new Combo() {CheckAmimationComplete = true, N = 1},
            new Poke {CheckAmimationComplete = true},

            new Walk() {CheckAmimationComplete = true},
            new BufferInput() {BufferAction = InputAction.Slash},
        };
    }

    public class UnitStateHitBack : UnitStateChangeModel
    {
        public override UnitState State => UnitState.HitBack;

        protected override List<BaseUnitStateInputLogic> unitStateInputLogic => new List<BaseUnitStateInputLogic>() {
            new Poke {CheckAmimationComplete = true}, //FIX ME no need check input

            new BufferInput() {BufferAction = InputAction.Slash},
        };
    }

    public abstract class UnitStateCombo : UnitStateChangeModel
    {
        static readonly List<UnitState> States = new List<UnitState> { UnitState.Combo1, UnitState.Combo2, UnitState.Combo3 };

        public override UnitState State => States[ComboN - 1];

        public abstract int ComboN { get; }

        public virtual BaseUnitStateInputLogic NextHit
            => new Combo() { CheckAmimationComplete = true, N = ComboN + 1 };

        protected override List<BaseUnitStateInputLogic> unitStateInputLogic => new List<BaseUnitStateInputLogic>() {
            new HitBack() {CheckAmimationComplete = true},
            NextHit,

            new Walk() {CheckAmimationComplete = true},

            new BufferInput() {BufferAction = InputAction.Slash},
        };
    }

    public class UnitStateCombo1 : UnitStateCombo
    {
        public override int ComboN => 1;
    }
    public class UnitStateCombo2 : UnitStateCombo
    {
        public override int ComboN => 2;
    }
    public class UnitStateCombo3 : UnitStateCombo
    {
        public override int ComboN => 3;

        public override BaseUnitStateInputLogic NextHit
            => new Poke { CheckAmimationComplete = true };
    }
}