using System.Collections.Generic;
using UI;

namespace Unit.UnitStateInputLogic
{    
    public class BufferInput: BaseUnitStateInputLogic
    {
        public InputAction? BufferAction;

        protected override bool? checkAmimationComplete => false;
        protected override InputAction? checkAction => BufferAction;

        protected override void ProcessImpl()
        {
            BufferInputState();
        }
    }

    public class Idle : BaseUnitStateInputLogic
    {
        protected override bool setDir => true;
        protected override UnitState? newState => UnitState.Idle;

        protected override bool CheckCondition()
            => CheckInputDirecton(xNotZero: false, yNotZero: false);

        protected override void ProcessImpl()
        {
            StopUnit();
        }
    }

    public class Run : BaseUnitStateInputLogic
    {
        protected override bool? checkInputForce => true;
        protected override bool setDir => true;
        protected override UnitState? newState => UnitState.Run;

        protected override bool CheckCondition()
            => CheckInputDirecton(xNotZero: true, yNotZero: false);

        protected override void ProcessImpl()
        {
            UnitMove();
        }
    }

    public class ContinueRun : BaseUnitStateInputLogic
    {
        //protected override bool? checkInputForce => true;
        protected override bool setDir => true;
        protected override UnitState? newState => UnitState.Run;

        protected override bool CheckCondition()
            => CheckInputDirecton(xNotZero: true)
            && CheckInputToCurrentDirecton(xEqual: true);

        protected override void ProcessImpl()
        {
            UnitMove();
        }
    }

    public class Dash : BaseUnitStateInputLogic
    {
        protected override bool? checkInputForce => true;
        protected override bool setDir => true;
        protected override UnitState? newState => UnitState.Dash;

        protected override bool CheckCondition()
            => CheckInputDirecton(xNotZero: false, yNotZero: true);

        protected override void ProcessImpl()
        {
            UnitMove();
            ResetForce();
        }
    }

    public class Walk : BaseUnitStateInputLogic
    {
        protected override bool? checkInputForce => false;
        protected override bool setDir => true;
        protected override UnitState? newState => UnitState.Walk;

        protected override bool CheckCondition()
            => !CheckInputDirecton(xNotZero: false, yNotZero: false);

        protected override void ProcessImpl()
        {
            UnitMove();
        }
    }

    public class JumpMove : BaseUnitStateInputLogic
    {
        protected override bool setDir => true;

        protected override bool CheckCondition()
            => CheckInputDirecton(xNotZero: true, yNotZero: false);

        protected override void ProcessImpl()
        {
            UnitMove();
        }
    }

    public class JumpStopMove : BaseUnitStateInputLogic
    {
        protected override bool CheckCondition()
            => CheckInputDirecton(xNotZero: false, yNotZero: false);

        protected override void ProcessImpl()
        {
            StopUnit();
        }
    }

    public class Jump : BaseUnitStateInputLogic
    {
        protected override InputAction? checkAction => InputAction.Jump;
        protected override bool setDir => true;
        protected override UnitState? newState => UnitState.Jump;

        protected override void ProcessImpl()
        {
            UnitJump();
        }
    }

    public class JumpRun : BaseUnitStateInputLogic
    {
        //protected override bool? checkInputForce => true;
        protected override InputAction? checkAction => InputAction.Jump;
        protected override bool setDir => true;
        protected override UnitState? newState => UnitState.Jump;

        protected override bool CheckCondition()
            => CheckInputDirecton(xNotZero: true, yNotZero: false)
            && CheckInputToCurrentDirecton(xEqual: true);

        protected override void ProcessImpl()
        {
            UnitJump();
        }
    }

    public class Magic : BaseUnitStateInputLogic
    {
        protected override InputAction? checkAction => InputAction.Magic;
        protected override UnitState? newState => UnitState.Magic;
    }

    public class Parry : BaseUnitStateInputLogic
    {
        protected override InputAction? checkAction => InputAction.Parry;
        protected override UnitState? newState => UnitState.Parry;
    }

    public class DashAttack : BaseUnitStateInputLogic
    {
        protected override InputAction? checkAction => InputAction.Slash;
        protected override UnitState? newState => UnitState.DashAttack;

        protected override bool CheckCondition()
            => CheckInputDirecton(xNotZero: true, yNotZero: false)
            && CheckInputToCurrentDirecton(xEqual: true);
    }
    
    public class Poke : BaseUnitStateInputLogic
    {
        protected override InputAction? checkAction => InputAction.Slash;
        protected override bool setDir => true;
        protected override UnitState? newState => UnitState.Poke;

        protected override void ProcessImpl()
        {
            StopUnit();
        }
    }

    public class Combo : BaseUnitStateInputLogic
    {
        static readonly List<UnitState> newStates = new List<UnitState> { UnitState.Combo1, UnitState.Combo2, UnitState.Combo3 };
        public int N;

        protected override InputAction? checkAction => InputAction.Slash;
        protected override UnitState? newState => newStates[N-1];

        protected override bool CheckCondition()
            => CheckUnitHitDetected();
    }

    public class HitBack : BaseUnitStateInputLogic
    {
        protected override InputAction? checkAction => InputAction.Slash;
        protected override bool setDir => true;
        protected override UnitState? newState => UnitState.HitBack;
        protected override bool CheckCondition()
            => CheckInputDirecton(xNotZero: true, yNotZero: false)
            && CheckInputToCurrentDirecton(xEqual: false);

        protected override void ProcessImpl()
        {
            StopUnit();
        }
    }
}