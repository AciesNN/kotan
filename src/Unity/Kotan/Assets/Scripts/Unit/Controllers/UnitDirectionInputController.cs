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
            bufferedDirectonInput.SetDir += BufferedDirectonInput_SetDir;

            bufferedInputController.OnJoystickPressAction += BufferedInputController_OnJoystickPressAction;

            unit.OnAnimationComplete += Unit_OnAnimationComplete;
        }

        private void OnDestroy()
        {
            bufferedDirectonInput.SetDir -= BufferedDirectonInput_SetDir; 
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
            unit.SetState(UnitState.Idle);
            UpdateUnitStateFromInput();
        }

        private void UpdateUnitStateFromInput()
        {
            var actions = bufferedInputController.GetJoystickActions();
            var action = actions[0]; //FIXME: really?
            var dir = bufferedDirectonInput.CurrentDir;
            var run = bufferedDirectonInput.CurrentForce;
            var newState = CurrentStateModel?.ChangeDirection(unit, action, dir, run);
            if (newState != null) {
                unit.SetState(newState);
            }
        }
        #endregion
    }
}