using UI;
using UnityEngine;

namespace Unit
{
    public abstract class UnitStateActionLogic
    {
        public UnitStateChangeArg Do(InputAction action)
        {
            return CheckChangeCondition(action) ? GetChangeArg(action) : null;
        }

        protected abstract bool CheckChangeCondition(InputAction action);
        protected abstract UnitStateChangeArg GetChangeArg(InputAction action);
    }

    public class UnitStateActionLogic_Poke : UnitStateActionLogic
    {
        protected override bool CheckChangeCondition(InputAction action)
            => action == InputAction.Slash;

        protected override UnitStateChangeArg GetChangeArg(InputAction action)
            => new UnitStateChangeArg()
            {
                State = UnitState.Poke,
                ReplayAnimation = true,
            };
    }
}