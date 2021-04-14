using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Unit
{
    public abstract class UnitStateChangeModel
    {
        private static readonly UnitStateInputLogic defaultInputLogic = new UnitStateInputLogic_Idle();
        public abstract UnitState State { get; }

        protected virtual List<UnitStateInputLogic> unitStateInputLogic { get; }
        protected virtual InputAction ActionToLockBuffer { get; }

        public bool ProcessInput(Unit unit, InputAction action, Vector2Int dir, bool force)
        {
            if (unitStateInputLogic == null)
                return false;

            foreach (var strategy in unitStateInputLogic)
            {
                var processed = strategy.ProcessInput(unit, action, dir, force);
                if (processed) {
                    return true;
                }
            }

            return false;
        }

        public static bool ProcessDefault(Unit unit, InputAction action, Vector2Int dir, bool force)
        {
            return defaultInputLogic.ProcessInput(unit, action, dir, force);
        }

        public InputAction GetActionToLockBuffer() => ActionToLockBuffer;
    }
}