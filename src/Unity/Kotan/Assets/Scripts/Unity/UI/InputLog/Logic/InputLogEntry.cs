using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class InputLogEntry : IInputLogEntry
    {
        public const int PressFramesLimit = 30;

        public event Action OnChanged;

        public bool IsPressed => isPressed;
        private bool isPressed;

        public Vector2Int Dir => dir;
        private readonly Vector2Int dir;

        public List<InputLogAction> States => states; //TODO array?
        private readonly List<InputLogAction> states;

        public InputLogEntry(Vector2Int dir, bool isPressed, InputLogAction[] states = null)
        {
            this.dir = dir;
            this.isPressed = isPressed;
            this.states = states.Where(e => e != InputLogAction.None).OrderBy(e => e).ToList();
        }

        public void SetPressed()
        {
            if (!isPressed)
            {
                isPressed = true;
                OnChanged?.Invoke();
            }
        }
    }
}