using System.Linq;
using System.Collections.Generic;

namespace Unit
{
    public class UnitChangeStateLogicFactory
    {
        private readonly Dictionary<UnitState, UnitStateChangeModel> items = new Dictionary<UnitState, UnitStateChangeModel>();

        public UnitChangeStateLogicFactory()
        {
            new List<UnitStateChangeModel>(){
                new UnitStateIdle(),
                new UnitStateWalk(),
                new UnitStateRun(),
                new UnitStateDash(),
                new UnitStatePoke(),
                new UnitStateCombo1(),
            }.ForEach(item => items[item.State] = item);
        }

        public UnitStateChangeModel GetModel(UnitState unitState)
            => items.ContainsKey(unitState) ? items[unitState] : null;
    }
}