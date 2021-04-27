using System.Collections.Generic;
using UI;
using Unit.UnitStateInputLogic;

namespace Unit
{
    public abstract class UnitStateChangeModel
    {
        public abstract UnitState State { get; }

        protected virtual List<BaseUnitStateInputLogic> unitStateInputLogic { get; }

        #region DI: FIXME
        protected BufferedStatedInput input;
        protected Unit unit;
        public void Init(Unit unit, BufferedStatedInput input)
        {
            this.unit = unit;
            this.input = input;
        }
        #endregion

        public virtual bool ProcessInput()
        {
            if (unitStateInputLogic == null)
                return false;

            foreach (var strategy in unitStateInputLogic)
            {
                strategy.SetCurrentData(unit, input); //TODO move to init

                var processed = strategy.ProcessInput();
                if (processed) {
                    return true;
                }
            }

            return false;
        }
    }
}