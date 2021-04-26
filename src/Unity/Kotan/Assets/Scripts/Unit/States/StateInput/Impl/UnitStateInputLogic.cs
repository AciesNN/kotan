using UI;
using UnityEngine;

namespace Unit
{
    public class UnitStateInputLogic_Idle : UnitStateInputLogic
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

    public class UnitStateInputLogic_Run : UnitStateInputLogic
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

    public class UnitStateInputLogic_ContinueRun : UnitStateInputLogic
    {
        //protected override bool? checkInputForce => true;
        protected override bool setDir => true;
        protected override UnitState? newState => UnitState.Run;

        protected override bool CheckCondition()
            => CheckInputDirecton(xNotZero: true)
            && dir.x == unit.CurDir.x; //        protected override bool? checkInputForce => true; ???? move logic to inpt buffer? TODO

        protected override void ProcessImpl()
        {
            UnitMove();
        }
    }

    public class UnitStateInputLogic_Dash : UnitStateInputLogic
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

    public class UnitStateInputLogic_Walk : UnitStateInputLogic
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

    public class UnitStateInputLogic_JumpMove : UnitStateInputLogic
    {
        protected override bool setDir => true;

        protected override bool CheckCondition()
            => CheckInputDirecton(xNotZero: true);

        protected override void ProcessImpl()
        {
            UnitMove();
        }
    }

    public class UnitStateInputLogic_Jump : UnitStateInputLogic
    {
        protected override InputAction? checkAction => InputAction.Jump;
        protected override bool setDir => true;
        protected override UnitState? newState => UnitState.Jump;

        protected override void ProcessImpl()
        {
            UnitJump();
        }
    }

    public class UnitStateInputLogic_JumpRun : UnitStateInputLogic
    {
        //protected override bool? checkInputForce => true;
        protected override InputAction? checkAction => InputAction.Jump;
        protected override bool setDir => true;
        protected override UnitState? newState => UnitState.Jump;

        protected override bool CheckCondition()
            => CheckInputDirecton(xNotZero: true, yNotZero: false)
            && dir.x == unit.CurDir.x; //        protected override bool? checkInputForce => true; ???? move logic to inpt buffer? TODO

        protected override void ProcessImpl()
        {
            UnitJump();
        }
    }

    public class UnitStateInputLogic_Poke : UnitStateInputLogic
    {
        protected override InputAction? checkAction => InputAction.Slash;
        protected override bool setDir => true;
        protected override UnitState? newState => UnitState.Poke;

        protected override void ProcessImpl()
        {
            StopUnit();
        }
    }

    public class UnitStateInputLogic_Combo1 : UnitStateInputLogic
    {
        protected override InputAction? checkAction => InputAction.Slash;
        protected override UnitState? newState => UnitState.Combo1;

        protected override bool CheckCondition()
            => CheckUnitHitDetected();
    }    
    
    public class UnitStateInputLogic_Combo2 : UnitStateInputLogic
    {
        protected override InputAction? checkAction => InputAction.Slash;
        protected override UnitState? newState => UnitState.Combo2;

        protected override bool CheckCondition()
            => CheckUnitHitDetected();
    }    
    
    public class UnitStateInputLogic_Combo3 : UnitStateInputLogic
    {
        protected override InputAction? checkAction => InputAction.Slash;
        protected override UnitState? newState => UnitState.Combo3;

        protected override bool CheckCondition()
            => CheckUnitHitDetected();
    }
}