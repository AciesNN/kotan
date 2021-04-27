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
            && inputState.dir.x == unit.CurDir.x; //        protected override bool? checkInputForce => true; ???? move logic to inpt buffer? TODO

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
            && inputState.dir.x == unit.CurDir.x; //        protected override bool? checkInputForce => true; ???? move logic to inpt buffer? TODO

        protected override void ProcessImpl()
        {
            UnitJump();
        }
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
}