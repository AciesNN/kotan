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

        private InputAction currentAction;

        private float lastActionChange;
        public InputAction CurrentAction => GetCurrentAction();

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
            currentAction = InputAction.None;
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

        Stack<InputState> bufferOfActons = new Stack<InputState>();
        public void AddBuffer()
        {
            var currentAction = new InputState() {
                dir = CurrentDir,
                force = CurrentForce,
                action = CurrentAction,
            };
            bufferOfActons.Push(currentAction);
        }

        private InputState? currentBufferedState;
        public void PopBuffer() {
            if (bufferOfActons.Count > 0) {
                currentBufferedState = bufferOfActons.Pop();
            } else {
                currentBufferedState = null;
            }
        }

        public InputState GetInputState()
        {
            if (currentBufferedState.HasValue) {
                return currentBufferedState.Value;
            }
            return new InputState() {
                dir = CurrentDir,
                force = CurrentForce,
                action = CurrentAction,
            };
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
            var actions = controller.GetJoystickActions();
            currentAction = actions[0]; //FIXME: really?
            lastActionChange = Time.time;
            FireOnActionEvent();
        }

        private void FireOnActionEvent()
        {
            OnAction?.Invoke(currentAction);
        }

        private InputAction GetCurrentAction()
        {
            if (Time.time - lastActionChange > actionThreshold) {
                return InputAction.None;
            }

            return currentAction;
        }
        #endregion
    }
}