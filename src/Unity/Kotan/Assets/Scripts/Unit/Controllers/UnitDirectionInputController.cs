using System;
using UI;
using UnityEngine;

namespace Unit
{
    public class UnitDirectionInputController : UnitInputController
    {
        [SerializeField] BufferedInputController bufferedInputController;
        private BufferedDirectonInput bufferedDirectonInput;
        
        private UnitStateChangeModel CurrentStateModel => stateLogicFactory.GetModel(unit.State);

        #region MonoBehaviour
        private void Awake()
        {
            bufferedDirectonInput = new BufferedDirectonInput(bufferedInputController);
            bufferedDirectonInput.OnSetDir += BufferedDirectonInput_SetDir;
            bufferedDirectonInput.OnAction += BufferedDirectonInput_OnAction;

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
            var action = bufferedDirectonInput.CurrentAction;
            var dir = bufferedDirectonInput.CurrentDir;
            var force = bufferedDirectonInput.CurrentForce;

            return CurrentStateModel?.ProcessInput(unit, action, dir, force) ?? false;
        }

        private bool UpdateUnitStateDefault()
        {
            var action = bufferedDirectonInput.CurrentAction;
            var dir = bufferedDirectonInput.CurrentDir;
            var force = bufferedDirectonInput.CurrentForce;

            return UnitStateChangeModel.ProcessDefault(unit, action, dir, force);
        }
        #endregion
    }
}