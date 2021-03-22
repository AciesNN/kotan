using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public interface IBufferedInputController
    {
        event Action<Vector2Int> OnJoystickSetPosition;
        event Action<Vector2Int> OnJoystickPressPosition;
        event Action OnJoystickNeitralPosition;

        event Action OnJoystickPressAction;

        Vector2Int GetJoystickPositionInt();
        bool GetJoystickDirIsPressed { get; }

        InputAction[] GetJoystickActions();
    }

    public interface IInputLoggerModelEntry
    {
        Vector2Int Dir { get; }
        bool IsPressed { get; }
        List<InputAction> States { get; }

        event Action OnChanged;

        void SetPressed();
    }

    public class InputLoggerModel : IInputLoggerModel
    {
        public event Action OnEntryAdded;

        public IInputLoggerModelEntry LastEntry => lastEntry;
        private IInputLoggerModelEntry lastEntry;

        private IBufferedInputController controller;

        #region Public
        public InputLoggerModel(IBufferedInputController controller)
        {
            this.controller = controller;

            controller.OnJoystickSetPosition += Controller_OnJoystickSetPosition;
            controller.OnJoystickPressPosition += Controller_OnJoystickPressPosition;
            controller.OnJoystickNeitralPosition += Controller_OnJoystickNeitralPosition;

            controller.OnJoystickPressAction += Controller_OnJoystickPressAction;
        }

        public void DeInit()
        {
            controller.OnJoystickSetPosition -= Controller_OnJoystickSetPosition;
            controller.OnJoystickPressPosition -= Controller_OnJoystickPressPosition; 
            controller.OnJoystickNeitralPosition -= Controller_OnJoystickNeitralPosition;

            controller.OnJoystickPressAction -= Controller_OnJoystickPressAction; 
        }
        #endregion

        #region Private
        private void Controller_OnJoystickNeitralPosition()
        {
            if (lastEntry != null && lastEntry.Dir == Vector2Int.zero)
                return;

            lastEntry = CreateEntry();
        }

        private void Controller_OnJoystickPressPosition(Vector2Int dir)
        {
            lastEntry?.SetPressed();
        }

        private void Controller_OnJoystickSetPosition(Vector2Int dir)
        {
            if (dir == Vector2Int.zero)
                return;

            lastEntry = CreateEntry();
        }

        private void Controller_OnJoystickPressAction()
        {
            lastEntry = CreateEntry();
        }

        private IInputLoggerModelEntry CreateEntry()
        {
            var dir = controller.GetJoystickPositionInt();
            var isPressed = controller.GetJoystickDirIsPressed;
            var actions = controller.GetJoystickActions();
            lastEntry = new InputLoggerModelEntry(dir, isPressed, actions); //TODO new!?
            OnEntryAdded?.Invoke();

            return lastEntry;
        }
        #endregion
    }
}