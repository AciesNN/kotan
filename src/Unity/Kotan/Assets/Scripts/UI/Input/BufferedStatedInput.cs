using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class BufferedStatedInput: IDisposable
    {
        private const float actionThreshold = 0.3f; //TODO - in settings

        public event Action<Vector2Int, bool> OnSetDir;
        public event Action<InputAction> OnAction;

        private IBufferedInputController controller;

        private Vector2Int lastPreForceDir;

        private Vector2Int curDir;
        public Vector2Int CurrentDir => curDir;

        private bool currentForce;
        public bool CurrentForce => currentForce;
        public InputAction CurrentAction => controller.GetJoystickActions()[0]; //FIXME: really;

        private InputState? lastInputState;

        private float lastActionChange;

        Stack<InputState> bufferOfActons = new Stack<InputState>();

        private InputState? currentBufferedState;

        #region Life circle
        public BufferedStatedInput(IBufferedInputController controller)
        {
            this.controller = controller;

            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        public void RefreshActionBuffer()
        {
            lastInputState = null;
            Debug.Log($"buff1: {lastInputState}");
            currentBufferedState = null;
        }

        public void ResetForce()
        {
            currentForce = false;
        }

        public void ClearBuffer()
        {
            bufferOfActons.Clear();
            currentBufferedState = null;
        }

        public void AddBuffer()
        {
            InputState currentAction = GetCurrentInputState();
            Debug.Log($"buff2: {currentAction}");
            bufferOfActons.Push(currentAction);
        }

        public void PopBuffer() {
            if (bufferOfActons.Count > 0) {
                currentBufferedState = bufferOfActons.Pop();
            } else {
                currentBufferedState = null;
            }
            Debug.Log($"buff2 pop: {currentBufferedState}");
        }

        public InputState GetEffectiveInputState()
        {
            if (currentBufferedState.HasValue) {
                return currentBufferedState.Value;
            }

            if (Time.time - lastActionChange < actionThreshold && lastInputState.HasValue) {
                return lastInputState.Value;
            }

            return GetCurrentInputState();
        }
        #endregion

        #region Implementation
        private void Subscribe()
        {
            controller.OnJoystickSetPosition += Controller_OnJoystickSetPosition;
            controller.OnJoystickNeitralPosition += Controller_OnJoystickNeitralPosition;
            controller.OnJoystickPressPosition += Controller_OnJoystickPressPosition;

            controller.OnJoystickPressAction += BufferedInputController_OnJoystickPressAction;
        }

        private void Unsubscribe()
        {
            controller.OnJoystickSetPosition -= Controller_OnJoystickSetPosition;
            controller.OnJoystickNeitralPosition -= Controller_OnJoystickNeitralPosition; 
            controller.OnJoystickPressPosition -= Controller_OnJoystickPressPosition;

            controller.OnJoystickPressAction -= BufferedInputController_OnJoystickPressAction;
        }

        private void Controller_OnJoystickSetPosition(Vector2Int dir)
        {
            if (dir.Equals(Vector2Int.zero))
            {
                curDir = Vector2Int.zero;
                currentForce = false;
            }
            else
            {
                currentForce = lastPreForceDir.Equals(dir);
                curDir = lastPreForceDir = dir;
            }

            FireOnSetDirEvent();
        }

        private void FireOnSetDirEvent()
        {
            OnSetDir?.Invoke(curDir, currentForce);
        }

        private void Controller_OnJoystickPressPosition(Vector2Int dir)
        {
            lastPreForceDir = Vector2Int.zero;
            //should already move
        }

        private void Controller_OnJoystickNeitralPosition()
        {
            lastPreForceDir = Vector2Int.zero;
            //should be already stopped
        }

        private void BufferedInputController_OnJoystickPressAction()
        {
            lastInputState = GetCurrentInputState();
            Debug.Log($"buff1: {lastInputState}");
            lastActionChange = Time.time;
            OnAction?.Invoke(CurrentAction);
        }

        private InputState GetCurrentInputState()
        {
            return new InputState()
            {
                dir = CurrentDir,
                force = CurrentForce,
                action = CurrentAction,
            };
        }
        #endregion
    }
}