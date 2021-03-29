using UI;

namespace Unit
{
    public abstract class UnitStateActionLogic
    {
        public UnitStateChangeArg Do(Unit unit, InputAction action)
        {
            return CheckChangeCondition(unit, action) ? GetChangeArg(unit, action) : null;
        }

        protected abstract bool CheckChangeCondition(Unit unit, InputAction action);
        protected abstract UnitStateChangeArg GetChangeArg(Unit unit, InputAction action);
    }

    public class UnitStateActionLogic_Poke : UnitStateActionLogic
    {
        protected override bool CheckChangeCondition(Unit unit, InputAction action)
            => action == InputAction.Slash && !unit.HitDetected;

        protected override UnitStateChangeArg GetChangeArg(Unit unit, InputAction action)
            => new UnitStateChangeArg()
            {
                State = UnitState.Poke,
                ReplayAnimation = true,
            };
    }

    public class UnitStateActionLogic_Combo1 : UnitStateActionLogic
    {
        protected override bool CheckChangeCondition(Unit unit, InputAction action)
            => action == InputAction.Slash && !unit.HitDetected;

        protected override UnitStateChangeArg GetChangeArg(Unit unit, InputAction action)
            => new UnitStateChangeArg()
            {
                State = UnitState.Combo1,
            };
    }
}