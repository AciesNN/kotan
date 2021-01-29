using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.Test
{
    public class TestJoystickController : MonoBehaviour, IPlayerControllerEvents
    {
        public event Action<Vector2> OnJoystickSetPosition;
        public event Action<InputLogAction> OnJoystickPressAction;

        public bool GetJoystickActionState(InputLogAction action) => Input.GetButtonDown(action.ToString());

        public Vector2 GetJoystickPosition()
        {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        private void Awake()
        {
            actions = Enum.GetValues(typeof(InputLogAction)).Cast<InputLogAction>().ToList();
            actions.Remove(InputLogAction.None);
        }

        bool dirSet = false;
        List<InputLogAction> actions;
        void Update()
        {
            if (Input.GetButtonDown("Horizontal")
                || Input.GetButtonDown("Vertical"))
            {
                var dir = GetJoystickPosition();
                OnJoystickSetPosition?.Invoke(dir);
                dirSet = true;
            }
            else if (dirSet
              && !Input.GetButton("Horizontal")
              && !Input.GetButton("Vertical"))
            {
                OnJoystickSetPosition?.Invoke(Vector2.zero);
                dirSet = false;
            }

            foreach (var action in actions)
            {
                if (Input.GetButtonDown(action.ToString()))
                {
                    OnJoystickPressAction?.Invoke(action);
                }
            }
        }
    }
}