using System;
using UI;
using UnityEngine;

namespace Unit
{
    public class UnitDirectionInputController : MonoBehaviour
    {
        [SerializeField] BufferedInputController bufferedInputController;
        private BufferedDirectonInput bufferedDirectonInput;
        
        [SerializeField] protected Unit unit;

        protected UnitChangeStateLogicFactory stateLogicFactory;

        private UnitStateChangeModel CurrentStateModel => stateLogicFactory.GetModel(unit.State);

        #region MonoBehaviour
        private void Awake()
        {
            bufferedDirectonInput = new BufferedDirectonInput(bufferedInputController);
            bufferedDirectonInput.OnSetDir += BufferedDirectonInput_SetDir;
            bufferedDirectonInput.OnAction += BufferedDirectonInput_OnAction;

            stateLogicFactory = new UnitChangeStateLogicFactory(unit, bufferedDirectonInput); //FIXME: DI

            unit.OnAnimationComplete += Unit_OnAnimationComplete;
            unit.OnStateChanged += Unit_OnStateChanged;
        }

        private void OnDestroy()
        {
            bufferedDirectonInput.OnSetDir -= BufferedDirectonInput_SetDir; 
            bufferedDirectonInput.OnAction -= BufferedDirectonInput_OnAction;

            unit.OnAnimationComplete -= Unit_OnAnimationComplete;
            unit.OnStateChanged -= Unit_OnStateChanged;
        }
        #endregion

        #region Impl
        private void Unit_OnStateChanged()
        {
            bufferedDirectonInput.RefreshActionBuffer();
        }

        private void BufferedDirectonInput_SetDir(Vector2Int dir, bool forceMove)
        {
            UpdateUnitStateFromInput();
        }

        private void BufferedDirectonInput_OnAction(InputAction action)
        {
            var updated = UpdateUnitStateFromInput();
            if (!updated) {
                if (action != InputAction.None && action == CurrentStateModel?.GetActionToLockBuffer()) {
                    bufferedDirectonInput.LockBufferedAction(action);
                }
            }
        }

        private void Unit_OnAnimationComplete()
        {
            var updated = UpdateUnitStateFromInput();
            if (!updated) {
                UpdateUnitStateDefault();
            }
        }

        private bool UpdateUnitStateFromInput()
        {
            return CurrentStateModel?.ProcessInput() ?? false;
        }

        private bool UpdateUnitStateDefault()
        {
            return stateLogicFactory.GetModel(UnitState.Idle).ProcessInput();
        }
        #endregion
    }
}