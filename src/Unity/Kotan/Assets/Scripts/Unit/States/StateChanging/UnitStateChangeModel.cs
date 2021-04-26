using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Unit
{
    public abstract class UnitStateChangeModel
    {
        public abstract UnitState State { get; }

        protected virtual List<UnitStateInputLogic> unitStateInputLogic { get; }
        protected virtual InputAction ActionToLockBuffer { get; }

        protected virtual bool resetForce => false;

        #region DI: FIXME
        protected BufferedDirectonInput input;
        protected Unit unit;
        public void Init(Unit unit, BufferedDirectonInput input)
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
                strategy.SetCurrentData(input, unit);
                var processed = strategy.ProcessInput();
                if (processed) {
                    return true;
                }
            }

            return false;
        }

        public InputAction GetActionToLockBuffer() => ActionToLockBuffer;
    }
}