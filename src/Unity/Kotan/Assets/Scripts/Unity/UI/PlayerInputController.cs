using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class PlayerInputController : MonoBehaviour, IPlayerInputController
    {
        #region Events
        public event Action<Vector2Int> OnJoystickSetPosition;
        public event Action<Vector2Int> OnJoystickPressPosition;
        public event Action OnJoystickNeitralPosition;

        public event Action OnJoystickPressAction;
        #endregion

        #region Consts
        public const int MaxStates = 2;

        private const float directionTimeout = 0.06f;
        private const float directionPressTimeout = 1.0f;
        private const float directionNeitralTimeout = 0.5f;
        private const float actionTimeout = 0.06f;
        #endregion

        [SerializeField] string HorizontalAxis;
        [SerializeField] string VerticalAxis;

        float lastDirChange;
        bool neitral = true;
        bool dirChanged = false;
        Vector2Int curDir;
        bool dirPressed;

        float lastActionChange;
        bool clearStates;
        InputLogAction[] actions;
        InputLogAction[] curActions = new InputLogAction[MaxStates]; //TODO all logic now is about 2 max actions (PlayerInputController.CheckActions())

        #region MonoBehaviour
        private void Awake()
        {
            actions = Enum.GetValues(typeof(InputLogAction)).Cast<InputLogAction>().Where(e => e != InputLogAction.None).ToArray();
        }

        void Update()
        {
            CheckDirection();
            CheckActions();
        }
        #endregion

        #region IPlayerInputController interface
        //public bool GetJoystickActionState(InputLogAction action) => Input.GetButtonDown(action.ToString());

        public InputLogAction[] GetJoystickActions() => curActions;

        public Vector2Int GetJoystickPositionInt() => new Vector2Int(
                SignOrZero(Input.GetAxisRaw(HorizontalAxis)),
                SignOrZero(Input.GetAxisRaw(VerticalAxis))
            );
        public bool GetJoystickDirIsPressed => dirPressed && !neitral;
        #endregion

        #region Implementaton
        private void CheckActions()
        {
            if (clearStates)
            {
                curActions[0] = InputLogAction.None;
                curActions[1] = InputLogAction.None;
                clearStates = false;
            }

            foreach (var action in actions)
            {
                if (Input.GetButtonDown(action.ToString()))
                {
                    if (curActions[0] != InputLogAction.None)
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

            if (curActions[0] != InputLogAction.None && curActions[1] == InputLogAction.None)
            {
                var timeSinceLastChanged = Time.time - lastActionChange;
                if (timeSinceLastChanged > actionTimeout)
                {
                    OnJoystickPressAction?.Invoke();
                    clearStates = true;
                }
            }
        }

        private void CheckDirection()
        {
            var isDirButtonDownThisFrame = Input.GetButtonDown(HorizontalAxis) || Input.GetButtonDown(VerticalAxis);
            var isDirButtonUpThisFrame = Input.GetButtonUp(HorizontalAxis) || Input.GetButtonUp(VerticalAxis);

            if (neitral)
            {
                if (isDirButtonDownThisFrame)
                {
                    curDir = GetJoystickPositionInt();
                    neitral = false;
                    if (!dirChanged)
                    {
                        lastDirChange = Time.time;
                    }
                    dirChanged = true;
                    dirPressed = false;
                }
            }
            else
            {
                if (isDirButtonDownThisFrame || isDirButtonUpThisFrame)
                {
                    curDir = GetJoystickPositionInt();
                    neitral = curDir.Equals(Vector2Int.zero);
                    if (!dirChanged)
                    {
                        lastDirChange = Time.time;
                    }
                    dirChanged = true;
                    dirPressed = false;
                }
            }

            var timeSinceLastChanged = Time.time - lastDirChange;
            if (dirChanged && timeSinceLastChanged > directionTimeout)
            {
                OnJoystickSetPosition?.Invoke(curDir);
                dirChanged = false;
            }

            if (!dirPressed)
            {
                if (neitral)
                {
                    if (timeSinceLastChanged > directionNeitralTimeout)
                    {
                        OnJoystickNeitralPosition?.Invoke();
                        dirPressed = true;
                    }
                }
                else
                {
                    if (timeSinceLastChanged > directionPressTimeout)
                    {
                        OnJoystickPressPosition?.Invoke(curDir);
                        dirPressed = true;
                    }
                }
            }
        }

        private int SignOrZero(float val)
        {
            return Mathf.Approximately(val, 0) ?
                0 : val > 0 ? 1 : -1;
        }
        #endregion
    }
}