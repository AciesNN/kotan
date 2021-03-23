using System.Linq;
using System.Collections.Generic;

namespace Unit
{
    public class UnitChangeStateLogicFactory
    {
        private Dictionary<UnitState, UnitStateChangeModel> items = new Dictionary<UnitState, UnitStateChangeModel>();

        public UnitChangeStateLogicFactory()
        {
            new List<UnitStateChangeModel>(){
                new UnitStateIdle(),
                new UnitStateWalk(),
                new UnitStateRun(),
                new UnitStateDash(),
            }.ForEach(item => items[item.State] = item);
            items[UnitState.None] = null;
        }

        public UnitStateChangeModel GetModel(UnitState unitState)
            => items[unitState];
    }
}