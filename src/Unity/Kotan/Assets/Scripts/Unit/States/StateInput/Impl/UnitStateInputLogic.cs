using UI;
using UnityEngine;

namespace Unit
{
    public class UnitStateInputLogic_Idle : UnitStateInputLogic
    {
        protected override bool CheckCondition()
            => CheckInputDirecton(
                xNotZero: false,
                yNotZero: false);

        protected override void ProcessImpl()
        {
            SetUnitState(UnitState.Idle);
            SetUnitDirection();
            StopUnit();
        }
    }

    public class UnitStateInputLogic_Run : UnitStateInputLogic
    {
        protected override bool CheckCondition()
            => CheckInputDirecton(
                xNotZero: true,
                yNotZero: false)
            && CheckInputForce(true);

        protected override void ProcessImpl()
        {
            SetUnitState(UnitState.Run);
            UnitMove();
        }
    }

    public class UnitStateInputLogic_ContinueRun : UnitStateInputLogic
    {
        protected override bool CheckCondition()
            => dir.x != 0 
            && dir.x == unit.CurDir.x;

        protected override void ProcessImpl()
        {
            SetUnitState(UnitState.Run);
            UnitMove();
        }
    }

    public class UnitStateInputLogic_Dash : UnitStateInputLogic
    {
        protected override bool CheckCondition()
            => CheckInputDirecton(
                xNotZero: false,
                yNotZero: true)
            && CheckInputForce(true);

        protected override void ProcessImpl()
        {
            SetUnitState(UnitState.Dash);
            UnitMove();
            ResetForce();
        }
    }

    public class UnitStateInputLogic_Walk : UnitStateInputLogic
    {
        protected override bool CheckCondition()
            => CheckInputDirecton(
                xNotZero: true,
                yNotZero: true,
                andCheck: false /*or*/);

        protected override void ProcessImpl()
        {
            SetUnitState(UnitState.Walk);
            UnitMove();
        }
    }

    public class UnitStateInputLogic_JumpMove : UnitStateInputLogic
    {
        protected override bool CheckCondition()
            => CheckInputDirecton(
                xNotZero: true,
                yNotZero: false,
                andCheck: false /*or*/)
            && !unit.IsAnimationComplete;

        protected override void ProcessImpl()
        {
            UnitMove();
        }
    }

    public class UnitStateInputLogic_Jump : UnitStateInputLogic
    {
        protected override bool CheckCondition()
            => CheckInputAction(InputAction.Jump)
            && CheckInputDirecton(
                yNotZero: false);
        
        protected override void ProcessImpl()
        {
            SetUnitState(UnitState.Jump);
            UnitJump();
        }
    }

    public class UnitStateInputLogic_Poke : UnitStateInputLogic
    {
        protected override bool CheckCondition()
            => CheckInputAction( InputAction.Slash);

        protected override void ProcessImpl()
        {
            SetUnitState(UnitState.Poke);
            SetUnitDirection();
            StopUnit();
        }
    }

    public class UnitStateInputLogic_Combo1 : UnitStateInputLogic
    {
        protected override bool CheckCondition()
            => CheckInputAction( InputAction.Slash)
            && unit.HitDetected;

        protected override void ProcessImpl()
        {
            SetUnitState(UnitState.Combo1);
            SetUnitDirection();
        }
    }    
    
    public class UnitStateInputLogic_Combo2 : UnitStateInputLogic
    {
        protected override bool CheckCondition()
            => CheckInputAction(InputAction.Slash)
            && unit.HitDetected;

        protected override void ProcessImpl()
        {
            SetUnitState(UnitState.Combo2);
            SetUnitDirection();
        }
    }    
    
    public class UnitStateInputLogic_Combo3 : UnitStateInputLogic
    {
        protected override bool CheckCondition()
            => CheckInputAction( InputAction.Slash)
            && unit.HitDetected;

        protected override void ProcessImpl()
        {
            SetUnitState(UnitState.Combo3);
            SetUnitDirection();
        }
    }
}