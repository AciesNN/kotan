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
                Stop();
            }
            else
            {
                if (lastPreForceDir.Equals(dir))
                {
                    lastPreForceDir = dir;
                    Move(dir, forceMove: true);
                }
                else
                {
                    lastPreForceDir = dir;
                    Move(dir);
                }
            }

            curDir = dir;
        }

        private void Move(Vector2Int dir, bool forceMove = false)
        {
            currentForce = forceMove;
            SetDir?.Invoke(dir, currentForce);
        }

        private void Stop()
        {
            currentForce = false;
            SetDir?.Invoke(Vector2Int.zero, currentForce);
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