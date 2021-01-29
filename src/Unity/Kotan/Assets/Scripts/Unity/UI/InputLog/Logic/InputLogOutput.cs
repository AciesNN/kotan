using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public interface IPlayerControllerEvents
    {
        event Action<Vector2> OnJoystickSetPosition;
        event Action<InputLogAction> OnJoystickPressAction;

        Vector2 GetJoystickPosition();
        bool GetJoystickActionState(InputLogAction action);
    }

    public interface IInputLogEntry
    {
        Vector2Int Dir { get; }
        int Frame { get; }
        bool IsPress { get; }
        List<InputLogAction> States { get; }

        event Action OnChanged;

        void AddState(InputLogAction state);
        bool Equals(object obj);
        int GetHashCode();
        void UpdateDir(int frame, Vector2Int dir);
    }

    public class InputLogOutput : IInputLogOutput
    {
        public event Action OnEntryAdded;

        public IInputLogEntry LastEntry => lastEntry;
        private IInputLogEntry lastEntry;

        private IPlayerControllerEvents controller;

        private Vector2Int curDir;

        #region Public
        public InputLogOutput(IPlayerControllerEvents controller)
        {
            this.controller = controller;
            controller.OnJoystickPressAction += Controller_OnJoystickPressAction;
            controller.OnJoystickSetPosition += Controller_OnJoystickSetPosition;
        }

        public void DeInit()
        {
            controller.OnJoystickPressAction -= Controller_OnJoystickPressAction;
            controller.OnJoystickSetPosition -= Controller_OnJoystickSetPosition;
        }
        #endregion

        #region Private
        private int GetFrame() => Time.frameCount;

        private void Controller_OnJoystickSetPosition(Vector2 dir)
        {
            var newDir = GetCurrentJoystickPos();
            if (newDir != curDir)
            {
                var entry = GetOrCreateEntry();
                curDir = newDir;
            }
        }

        private void Controller_OnJoystickPressAction(InputLogAction action)
        {
            var entry = GetOrCreateEntry();
            entry.AddState(action);
        }

        private IInputLogEntry GetOrCreateEntry()
        {
            var frame = GetFrame();
            if (lastEntry?.Frame == frame)
            {
                return lastEntry;
            }

            var dir = GetCurrentJoystickPos();
            lastEntry?.UpdateDir(frame, dir);
            lastEntry = new InputLogEntry(frame, dir);
            OnEntryAdded?.Invoke();

            return lastEntry;
        }

        private Vector2Int GetCurrentJoystickPos()
        {
            var dir = controller.GetJoystickPosition();
            return new Vector2Int(
                GetAxisValue(dir.x),
                GetAxisValue(dir.y)
            );
        }

        private int GetAxisValue(float val)
        {
            if (Mathf.Approximately(val, 0))
                return 0;
            return val > 0 ? 1 : -1;
        }
        #endregion
    }
}