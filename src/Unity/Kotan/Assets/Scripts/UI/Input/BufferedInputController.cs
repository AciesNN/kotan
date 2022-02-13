using Core.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class BufferedInputController : MonoBehaviour
    {
        #region Events
        public event Action<Vector2Int> OnJoystickSetPosition;
        public event Action<Vector2Int> OnJoystickPressPosition;
        public event Action OnJoystickNeitralPosition;

        public event Action OnJoystickPressAction;
        #endregion

        #region Consts
        public const int MaxStates = 2;
        #endregion

        [SerializeField] BufferedInput bufferedInput;
        
        InputAction[] actions;
        InputAction[] curActions = new InputAction[MaxStates]; //TODO all logic now is about 2 max actions (PlayerInputController.CheckActions())

        #region MonoBehaviour
        private void Awake()
        {
            actions = Enum.GetValues(typeof(InputAction)).Cast<InputAction>().Where(e => e != InputAction.None).ToArray();
            bufferedInput.SetButtons(actions.Select(e => e.ToString()).ToList());
        }

        void Update()
        {
            CheckEvents();
        }
        #endregion

        public InputAction[] GetJoystickActions() => curActions;

        public Vector2Int GetJoystickPositionInt() => bufferedInput.RawInputState.PositionInt;

        public bool JoystickDirIsPressed => bufferedInput.CurrentInputState.PositionPressed;

        #region Implementaton
        private void CheckEvents()
        {
            var currentState = bufferedInput.CurrentInputState;

            if (currentState.SetPositionChanged)
            {
                OnJoystickSetPosition?.Invoke(currentState.PositionInt);
            }

            if (currentState.SetPositionNeitral)
            {
                OnJoystickNeitralPosition?.Invoke();
            }

            if (currentState.SetPositionPressed)
            {
                OnJoystickPressPosition?.Invoke(currentState.PositionInt);
            }

            for (int i = 0; i < currentState.ButtonsState.Count(); i++)
            {
                var buttonState = currentState.ButtonsState[i];
                curActions[i] = buttonState < 0 || buttonState >= actions.Length ? InputAction.None : actions[buttonState];
            }

            if (currentState.SetButtonsState)
            {
                OnJoystickPressAction?.Invoke();
            }
        }
        #endregion
    }
}