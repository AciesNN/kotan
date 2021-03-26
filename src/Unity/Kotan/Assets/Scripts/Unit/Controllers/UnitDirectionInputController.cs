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

            unit.OnAnimationComplete += Unit_OnAnimationComplete;
        }

        private void OnDestroy()
        {
            bufferedDirectonInput.SetDir -= BufferedDirectonInput_SetDir; 
            unit.OnAnimationComplete -= Unit_OnAnimationComplete;
        }
        #endregion

        #region Impl
        private void BufferedDirectonInput_SetDir(Vector2Int dir, bool forceMove)
            => SetUnitDirection(dir, run: forceMove); //or SetCurrentUnitDirection() ?

        private void Unit_OnAnimationComplete()
        {
            unit.SetState(UnitState.Idle);
            SetUnitDirection(bufferedDirectonInput.CurrentDir, run: false);//run: false or clear force state?
        }

        /*private void SetCurrentUnitDirection()
            => SetUnitDirection(bufferedDirectonInput.CurrentDir, bufferedDirectonInput.CurrentForce);*/

        private void SetUnitDirection(Vector2Int dir, bool run)
        {
            var newState = CurrentStateModel?.ChangeDirection(unit, dir, run);
            if (newState != null) {
                unit.SetState(newState);
            }
        }
        #endregion
    }
}