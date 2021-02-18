using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitMoveType
{
    Walk,
    Run,
    Dash,
}

public interface IUnit
{
    void Move(Vector2Int dir, UnitMoveType type);
    void Stop();
}

namespace UI
{
    public class PlayerInputManager : IDisposable
    {
        private IPlayerInputController controller;
        private IUnit unit;

        private Vector2Int lastPreRunDir;
        private Vector2Int curDir;

        #region Life circle
        public PlayerInputManager(IPlayerInputController controller, IUnit unit)
        {
            this.controller = controller;
            this.unit = unit;

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

            controller.OnJoystickPressAction += Controller_OnJoystickPressAction;
        }

        private void Unsubscribe()
        {
            controller.OnJoystickSetPosition -= Controller_OnJoystickSetPosition;
            controller.OnJoystickNeitralPosition -= Controller_OnJoystickNeitralPosition; 
            controller.OnJoystickPressPosition -= Controller_OnJoystickPressPosition;

            controller.OnJoystickPressAction -= Controller_OnJoystickPressAction;
        }

        private void Controller_OnJoystickSetPosition(Vector2Int dir)
        {
            if (dir.Equals(Vector2Int.zero))
            {
                unit.Stop();
            }
            else
            {
                if (lastPreRunDir.Equals(dir))
                {
                    lastPreRunDir = dir;
                    unit.Move(dir, UnitMoveType.Run);
                }
                else
                {
                    lastPreRunDir = dir;
                    unit.Move(dir, UnitMoveType.Walk);
                }
            }

            curDir = dir;
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

        private void Controller_OnJoystickPressAction()
        {
        }
        #endregion
    }
}