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
            }.ForEach(item => items[item.State] = item);
        }

        public UnitStateChangeModel GetModel(UnitState unitState)
            => items[unitState];
    }
}