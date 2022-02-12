using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Input
{
    public struct RawInputState
    {
        public Vector2Int PositionInt;
        public bool[] ButtonStates;

        public RawInputState(int buttonsCount)
        {
            PositionInt = new Vector2Int();
            ButtonStates = new bool[buttonsCount];
        }
    }

    public struct BufferedInputState
    {
        public Vector2Int PositionInt;

        public bool PositionChanged;
        public bool SetPositionChanged; //this frame

        public bool PositionPressed;
        public bool SetPositionPressed; //this frame

        public bool PositionNeitral => PositionInt.Equals(Vector2Int.zero);
        public bool SetPositionNeitral; //this frame

        public int[] ButtonsState;
        public bool SetButtonsState;

        public BufferedInputState(int buttonsCount)
        {
            PositionInt = new Vector2Int();

            PositionChanged = false;
            SetPositionChanged = false;

            PositionPressed = false;
            SetPositionPressed = false;

            SetPositionNeitral = false;

            ButtonsState = new int[buttonsCount];
            SetButtonsState = false;

            ClearButtons();
        }

        public void ClearButtons()
        {
            for (int i = 0; i < ButtonsState.Length; i++)
            {
                ButtonsState[i] = -1;
            }
        }
    }

    [DefaultExecutionOrder(-42)] //before default scripts
    public class BufferedInput: MonoBehaviour
    {
        #region Fields
        public const int MaxStates = 2;

        [SerializeField]
        private List<string> buttons = new List<string>();
        public List<string> Buttons => buttons;

        [SerializeField]
        private string axisName;

        [SerializeField]
        private PlayerInputControllerSettings settings;

        BufferedInputState currentInputState = new BufferedInputState(MaxStates);
        public BufferedInputState CurrentInputState => currentInputState;

        RawInputState rawInputState = new RawInputState();
        public RawInputState RawInputState => rawInputState;

        RawInput rawInput;

        float lastDirChange;

        float lastButtonsChange;
        bool clearButtons;
        #endregion

        #region MonoBehaviour
        protected virtual void Awake()
        {
            rawInput = new RawInput(axisName);
        }

        protected virtual void Update()
        {
            UpdateRawInputState();
            UpdateCurrentDirState();
            UpdateCurrentButtonsState();
        }
        #endregion

        #region Interface
        public void SetButtons(List<string> buttons)
        {
            this.buttons = new List<string>(buttons);
            this.rawInputState = new RawInputState(buttons.Count);
        }
        #endregion

        #region Implementation
        private void UpdateRawInputState()
        {
            rawInputState.PositionInt = rawInput.GetJoystickPositionInt();
            for (int i = 0; i < buttons.Count && i < rawInputState.ButtonStates.Length; i++)
            {
                rawInputState.ButtonStates[i] = rawInput.GetButtonDown(buttons[i]);
            }
        }

        private void UpdateCurrentDirState()
        {
            var dir = rawInputState.PositionInt;

            if (!dir.Equals(currentInputState.PositionInt))
            {
                currentInputState.PositionInt = dir;
                if (!currentInputState.PositionChanged)
                {
                    lastDirChange = Time.time;
                }
                currentInputState.PositionChanged = true;
                currentInputState.PositionPressed = false;
            }

            currentInputState.SetPositionPressed = false;
            currentInputState.SetPositionChanged = false;
            currentInputState.SetPositionNeitral = false;

            var timeSinceLastChanged = Time.time - lastDirChange;

            if (currentInputState.PositionChanged && timeSinceLastChanged > settings.DirectionTimeout)
            {
                currentInputState.SetPositionChanged = true;
                currentInputState.PositionChanged = false;
            }

            if (!currentInputState.PositionPressed)
            {
                if (currentInputState.PositionNeitral)
                {
                    if (timeSinceLastChanged > settings.DirectionNeitralTimeout)
                    {
                        currentInputState.SetPositionNeitral = true;
                    }
                }
                else
                {
                    if (timeSinceLastChanged > settings.DirectionPressTimeout)
                    {
                        currentInputState.SetPositionPressed = true;
                        currentInputState.PositionPressed = true;
                    }
                }
            }
        }

        private void UpdateCurrentButtonsState()
        {
            currentInputState.SetButtonsState = false;

            if (clearButtons)
            {
                currentInputState.ClearButtons();
                clearButtons = false;
            }

            for (int i = 0; i < buttons.Count; i++)
            {
                var button = buttons[i];
                if (rawInput.GetButtonDown(button))
                {
                    if (currentInputState.ButtonsState[0] >= 0)
                    {
                        currentInputState.ButtonsState[1] = i;
                        currentInputState.SetButtonsState = true;
                        clearButtons = true;
                        break;
                    }
                    else
                    {
                        currentInputState.ButtonsState[0] = i;
                        lastButtonsChange = Time.time;
                    }
                }
            }

            if (currentInputState.ButtonsState[0] >= 0 && currentInputState.ButtonsState[1] < 0)
            {
                var timeSinceLastChanged = Time.time - lastButtonsChange;
                if (timeSinceLastChanged > settings.ActionTimeout)
                {
                    currentInputState.SetButtonsState = true;
                    clearButtons = true;
                }
            }
        }
        #endregion
    }
}