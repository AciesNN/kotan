using System;
using UI;
using UnityEngine;

namespace Unit
{
    public class UnitDirectionInputController : UnitInputController
    {
        [SerializeField] BufferedInputController bufferedInputController;
        private BufferedDirectonInput bufferedDirectonInput;
        
        private UnitStateChangeModel CurrentStateModel => stateLogicFactory.GetModel(unit.UnitState);

        #region MonoBehaviour
        private void Awake()
        {
            bufferedDirectonInput = new BufferedDirectonInput(bufferedInputController);
            bufferedDirectonInput.OnSetDir += BufferedDirectonInput_SetDir;

            bufferedInputController.OnJoystickPressAction += BufferedInputController_OnJoystickPressAction;

            unit.OnAnimationComplete += Unit_OnAnimationComplete;
        }

        private void OnDestroy()
        {
            bufferedDirectonInput.OnSetDir -= BufferedDirectonInput_SetDir; 
            unit.OnAnimationComplete -= Unit_OnAnimationComplete;

            bufferedInputController.OnJoystickPressAction -= BufferedInputController_OnJoystickPressAction;
        }
        #endregion

        #region Impl
        private void BufferedDirectonInput_SetDir(Vector2Int dir, bool forceMove)
        {
            UpdateUnitStateFromInput();
        }
        private void BufferedInputController_OnJoystickPressAction()
        {
            UpdateUnitStateFromInput();
        }

        private void Unit_OnAnimationComplete()
        {
            UpdateUnitStateFromInput(useDefault: true);
        }

        private void UpdateUnitStateFromInput(bool useDefault = false)
        {
            var action = bufferedDirectonInput.CurrentAction;
            var dir = bufferedDirectonInput.CurrentDir;
            var force = bufferedDirectonInput.CurrentForce;

            var newState = CurrentStateModel?.ProcessInput(unit, action, dir, force);
            
            if (newState == null && useDefault) {
                newState = UnitStateChangeModel.ProcessDefault(unit, action, dir, force);
            }

            unit.SetState(newState);
        }
        #endregion
    }
}