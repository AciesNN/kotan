using System;
using UnityEngine;

namespace UI
{
    public class BufferedDirectonInput : IDisposable
    {
        public event Action<Vector2Int, bool> SetDir;

        private IBufferedInputController controller;

        private Vector2Int lastPreForceDir;
        private Vector2Int curDir;
        private bool currentForce;

        public Vector2Int CurrentDir => curDir;
        public bool CurrentForce => currentForce;

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
        #endregion

        #region Implementation
        private void Subscribe()
        {
            controller.OnJoystickSetPosition += Controller_OnJoystickSetPosition;
            controller.OnJoystickNeitralPosition += Controller_OnJoystickNeitralPosition;
            controller.OnJoystickPressPosition += Controller_OnJoystickPressPosition;
        }

        private void Unsubscribe()
        {
            controller.OnJoystickSetPosition -= Controller_OnJoystickSetPosition;
            controller.OnJoystickNeitralPosition -= Controller_OnJoystickNeitralPosition; 
            controller.OnJoystickPressPosition -= Controller_OnJoystickPressPosition;
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

            FireSetDirEvent();
        }

        private void FireSetDirEvent()
        {
            SetDir?.Invoke(curDir, currentForce);
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
        #endregion
    }
}