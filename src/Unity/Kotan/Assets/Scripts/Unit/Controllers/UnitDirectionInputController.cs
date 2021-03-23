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
        }

        private void OnDestroy()
        {
            bufferedDirectonInput.SetDir -= BufferedDirectonInput_SetDir;
        }
        #endregion

        #region Impl
        private void BufferedDirectonInput_SetDir(Vector2Int dir, bool forceMove)
            => SetUnitDirection(dir, run: forceMove);

        private void SetUnitDirection(Vector2Int dir, bool run)
        {
            var newState = CurrentStateModel?.ChangeDirection(dir, run);
            if (newState != null) {
                unit.SetState(newState);
            }
        }
        #endregion
    }
}