using System;
using UnityEngine;

namespace UI
{
    public class BufferedDirectonInput : IDisposable
    {
        private const float actionThreshold = 0.3f; //TODO - in settings

        public event Action<Vector2Int, bool> OnSetDir;
        public event Action<InputAction> OnAction;

        private IBufferedInputController controller;

        private Vector2Int lastPreForceDir;
        private Vector2Int curDir;
        private bool currentForce;

        public Vector2Int CurrentDir => curDir;
        public bool CurrentForce => currentForce;

        private InputAction currentAction;
        private InputAction lockedBufferedAction;

        private float lastActionChange;
        public InputAction CurrentAction => GetCurrentAction();

        #region Life circle
        public BufferedDirectonInput(IBufferedInputController controller)
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
            lockedBufferedAction = InputAction.None;
            currentAction = InputAction.None;
        }

        public void LockBufferedAction(InputAction action)
        {
            lockedBufferedAction = action;
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
            if (lockedBufferedAction != InputAction.None) {
                return lockedBufferedAction;
            }

            if (Time.time - lastActionChange > actionThreshold) {
                return InputAction.None;
            }

            return currentAction;
        }
        #endregion
    }
}