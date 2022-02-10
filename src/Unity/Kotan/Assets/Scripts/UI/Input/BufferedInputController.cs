using Core.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class BufferedInputController : MonoBehaviour, IBufferedInputController
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

        [SerializeField] PlayerInputControllerSettings settings;
        [SerializeField] string AxisPostfix;
        
        float lastDirChange;
        bool neitral = true;
        bool dirChanged = false;
        Vector2Int curDir;
        bool dirPressed;

        float lastActionChange;
        bool clearStates;
        InputAction[] actions;
        InputAction[] curActions = new InputAction[MaxStates]; //TODO all logic now is about 2 max actions (PlayerInputController.CheckActions())

        RawInput rawInput;

        #region MonoBehaviour
        private void Awake()
        {
            rawInput = new RawInput(AxisPostfix);
            actions = Enum.GetValues(typeof(InputAction)).Cast<InputAction>().Where(e => e != InputAction.None).ToArray();
        }

        void Update()
        {
            CheckDirection();
            CheckActions();
        }
        #endregion

        #region IPlayerInputController interface
        public InputAction[] GetJoystickActions() => curActions;

        public Vector2Int GetJoystickPositionInt() => rawInput.GetJoystickPositionInt();

        public bool GetJoystickDirIsPressed => dirPressed && !neitral;
        #endregion

        #region Implementaton
        private void CheckActions()
        {
            if (clearStates)
            {
                curActions[0] = InputAction.None;
                curActions[1] = InputAction.None;
                clearStates = false;
            }

            foreach (var action in actions)
            {
                if (rawInput.GetButtonDown(action.ToString()))
                {
                    if (curActions[0] != InputAction.None)
                    {
                        curActions[1] = action;
                        OnJoystickPressAction?.Invoke();
                        clearStates = true;
                        break;
                    }
                    else
                    {
                        curActions[0] = action;
                        lastActionChange = Time.time;
                    }
                }
            }

            if (curActions[0] != InputAction.None && curActions[1] == InputAction.None)
            {
                var timeSinceLastChanged = Time.time - lastActionChange;
                if (timeSinceLastChanged > settings.ActionTimeout)
                {
                    OnJoystickPressAction?.Invoke();
                    clearStates = true;
                }
            }
        }

        private void CheckDirection()
        {
            var dir = GetJoystickPositionInt();

            if (!dir.Equals(curDir))
            {
                curDir = dir;
                neitral = curDir.Equals(Vector2Int.zero);
                if (!dirChanged)
                {
                    lastDirChange = Time.time;
                }
                dirChanged = true;
                dirPressed = false;
            }


            var timeSinceLastChanged = Time.time - lastDirChange;
            if (dirChanged && timeSinceLastChanged > settings.DirectionTimeout)
            {
                OnJoystickSetPosition?.Invoke(curDir);
                dirChanged = false;
            }

            if (!dirPressed)
            {
                if (neitral)
                {
                    if (timeSinceLastChanged > settings.DirectionNeitralTimeout)
                    {
                        OnJoystickNeitralPosition?.Invoke();
                        dirPressed = true;
                    }
                }
                else
                {
                    if (timeSinceLastChanged > settings.DirectionPressTimeout)
                    {
                        OnJoystickPressPosition?.Invoke(curDir);
                        dirPressed = true;
                    }
                }
            }
        }
        #endregion
    }
}