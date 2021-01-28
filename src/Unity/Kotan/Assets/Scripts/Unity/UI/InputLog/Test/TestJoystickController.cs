using System;
using UnityEngine;

namespace UI.Test
{
    public class TestJoystickController : MonoBehaviour, IPlayerControllerEvents
    {
        public event Action<Vector2> OnJoystickSetPosition;
        public event Action<InputLogAction> OnJoystickPressAction;

        public bool GetJoystickActionState(InputLogAction action)
        {
            switch (action)
            {
                case InputLogAction.Action:
                    return Input.GetButton("Fire1");
            }

            return false;
        }

        public Vector2 GetJoystickPosition()
        {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        bool dirSet = false;
        void Update()
        {
            if (Input.GetKeyDown("up")
                || Input.GetKeyDown("down")
                || Input.GetKeyDown("left")
                || Input.GetKeyDown("right"))
            {
                var dir = GetJoystickPosition();
                OnJoystickSetPosition?.Invoke(dir);
                dirSet = true;
            } else if (dirSet
                && !Input.GetKey("up")
                && !Input.GetKey("down")
                && !Input.GetKey("left")
                && !Input.GetKey("right"))
            {
                OnJoystickSetPosition?.Invoke(Vector2.zero);
                dirSet = false;
            }

            if (Input.GetButtonDown("Fire1"))
                OnJoystickPressAction?.Invoke(InputLogAction.Action);
        }
    }
}