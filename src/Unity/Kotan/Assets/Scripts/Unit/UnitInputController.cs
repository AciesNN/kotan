using UI;
using UnityEngine;

namespace Unit
{
    public class UnitInputController : MonoBehaviour
    {
        [SerializeField] Unit unit;

        [SerializeField] BufferedInputController bufferedInputController;
        private BufferedDirectonInput bufferedDirectonInput;
        private UnitChangeStateLogicFactory stateLogicFactory = new UnitChangeStateLogicFactory();

        private UnitStateChangeModel CurrentStateModel => stateLogicFactory.GetModel(unit.UnitState);

        #region MonoBehaviour
        private void Awake()
        {
            bufferedDirectonInput = new BufferedDirectonInput(bufferedInputController);

            bufferedInputController.OnJoystickSetPosition += BufferedInputController_OnJoystickSetPosition;
            bufferedInputController.OnJoystickNeitralPosition += BufferedInputController_OnJoystickNeitralPosition;
        }

        private void OnDestroy()
        {
            bufferedInputController.OnJoystickSetPosition -= BufferedInputController_OnJoystickSetPosition;
            bufferedInputController.OnJoystickNeitralPosition -= BufferedInputController_OnJoystickNeitralPosition;
        }
        #endregion

        #region Impl
        private void BufferedInputController_OnJoystickSetPosition(Vector2Int dir)
            => SetUnitDirection(dir);

        private void BufferedInputController_OnJoystickNeitralPosition()
            => SetUnitDirection(Vector2Int.zero);

        private void SetUnitDirection(Vector2Int dir, bool run = false)
        {
            var newState = CurrentStateModel.SetDirection(dir, run);
            unit.SetState(newState);
        }
        #endregion
    }
}