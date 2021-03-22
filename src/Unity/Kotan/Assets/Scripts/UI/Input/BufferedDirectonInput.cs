using System;
using UnityEngine;

namespace UI
{
    public class BufferedDirectonInput : IDisposable
    {
        private IBufferedInputController controller;

        private Vector2Int lastPreRunDir;
        private Vector2Int curDir;

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
                if (lastPreRunDir.Equals(dir))
                {
                    lastPreRunDir = dir;
                    Run(dir);
                }
                else
                {
                    lastPreRunDir = dir;
                    Move(dir);
                }
            }

            curDir = dir;
        }

        private void Run(Vector2Int dir)
        {
        }

        private void Move(Vector2Int dir)
        {
        }

        private void Stop()
        {
        }

        private void Controller_OnJoystickPressPosition(Vector2Int dir)
        {
            lastPreRunDir = Vector2Int.zero;
            //should already move
        }

        private void Controller_OnJoystickNeitralPosition()
        {
            lastPreRunDir = Vector2Int.zero;
            //should be already stopped
        }
        #endregion
    }
}