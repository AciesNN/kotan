using Core.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class BufferedInputController : BufferedInput
    {
        #region Events
        public event Action<Vector2Int> OnJoystickSetPosition;
        public event Action<Vector2Int> OnJoystickPressPosition;
        public event Action OnJoystickNeitralPosition;

        public event Action OnJoystickPressAction;
        #endregion

        InputAction[] actions;
        InputAction[] curActions = new InputAction[MaxStates];

        #region MonoBehaviour
        protected override void Awake()
        {
            base.Awake();

            actions = Enum.GetValues(typeof(InputAction)).Cast<InputAction>().Where(e => e != InputAction.None).ToArray();
            SetButtons(actions.Select(e => e.ToString()).ToList());
        }

        protected override void Update()
        {
            base.Update();

            CheckEvents();
        }
        #endregion

        public InputAction[] GetJoystickActions() => curActions;

        public Vector2Int GetJoystickPositionInt() => RawInputState.PositionInt;

        public bool JoystickDirIsPressed => CurrentInputState.PositionPressed;

        #region Implementaton
        private void CheckEvents()
        {
            if (CurrentInputState.SetPositionChanged)
            {
                OnJoystickSetPosition?.Invoke(CurrentInputState.PositionInt);
            }

            if (CurrentInputState.SetPositionNeitral)
            {
                OnJoystickNeitralPosition?.Invoke();
            }

            if (CurrentInputState.SetPositionPressed)
            {
                OnJoystickPressPosition?.Invoke(CurrentInputState.PositionInt);
            }

            for (int i = 0; i < CurrentInputState.ButtonsState.Count(); i++)
            {
                var buttonState = CurrentInputState.ButtonsState[i];
                curActions[i] = buttonState < 0 || buttonState >= actions.Length ? InputAction.None : actions[buttonState];
            }

            if (CurrentInputState.SetButtonsState)
            {
                OnJoystickPressAction?.Invoke();
            }
        }
        #endregion
    }
}