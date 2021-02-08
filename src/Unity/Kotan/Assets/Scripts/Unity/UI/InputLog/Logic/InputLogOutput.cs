using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public interface IPlayerInputController
    {
        event Action<Vector2Int> OnJoystickSetPosition;
        event Action<Vector2Int> OnJoystickPressPosition;
        event Action OnJoystickNeitralPosition;

        event Action OnJoystickPressAction;

        Vector2Int GetJoystickPositionInt();
        bool GetJoystickDirIsPressed { get; }

        InputLogAction[] GetJoystickActions();
    }

    public interface IInputLogEntry
    {
        Vector2Int Dir { get; }
        bool IsPressed { get; }
        List<InputLogAction> States { get; }

        event Action OnChanged;

        void SetPressed();
    }

    public class InputLogOutput : IInputLogOutput
    {
        public event Action OnEntryAdded;

        public IInputLogEntry LastEntry => lastEntry;
        private IInputLogEntry lastEntry;

        private IPlayerInputController controller;

        #region Public
        public InputLogOutput(IPlayerInputController controller)
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

        private IInputLogEntry CreateEntry()
        {
            var dir = controller.GetJoystickPositionInt();
            var isPressed = controller.GetJoystickDirIsPressed;
            var actions = controller.GetJoystickActions();
            lastEntry = new InputLogEntry(dir, isPressed, actions); //TODO new!?
            OnEntryAdded?.Invoke();

            return lastEntry;
        }
        #endregion
    }
}