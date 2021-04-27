using System;
using UI;
using UnityEngine;

namespace Unit
{
    public class UnitDirectionInputController : MonoBehaviour
    {
        [SerializeField] BufferedInputController bufferedInputController;
        private BufferedStatedInput bufferedStatedInput;
        
        [SerializeField] protected Unit unit;

        protected UnitChangeStateLogicFactory stateLogicFactory;

        #region MonoBehaviour
        private void Awake()
        {
            bufferedStatedInput = new BufferedStatedInput(bufferedInputController);
            bufferedStatedInput.OnSetDir += BufferedDirectonInput_SetDir;
            bufferedStatedInput.OnAction += BufferedDirectonInput_OnAction;

            stateLogicFactory = new UnitChangeStateLogicFactory(unit, bufferedStatedInput); //FIXME: DI

            unit.OnAnimationComplete += Unit_OnAnimationComplete;
            unit.OnStateChanged += Unit_OnStateChanged;
        }

        private void OnDestroy()
        {
            bufferedStatedInput.OnSetDir -= BufferedDirectonInput_SetDir; 
            bufferedStatedInput.OnAction -= BufferedDirectonInput_OnAction;

            unit.OnAnimationComplete -= Unit_OnAnimationComplete;
            unit.OnStateChanged -= Unit_OnStateChanged;
        }
        #endregion

        #region Impl
        private void Unit_OnStateChanged()
        {
            bufferedStatedInput.RefreshActionBuffer();
        }

        private void BufferedDirectonInput_SetDir(Vector2Int dir, bool forceMove)
        {
            UpdateUnitStateFromInput(unit.State);
        }

        private void BufferedDirectonInput_OnAction(InputAction action)
        {
            UpdateUnitStateFromInput(unit.State);
        }

        private void Unit_OnAnimationComplete()
        {
            var updated = UpdateUnitStateFromInput(unit.State);
            if (!updated) {
                UpdateUnitStateFromInput(UnitState.Idle);
            }
        }

        private bool UpdateUnitStateFromInput(UnitState unitState)
        {
            return stateLogicFactory?.GetModel(unitState)?.ProcessInput() ?? false;
        }
        #endregion
    }
}