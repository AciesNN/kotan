using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class InputLogEntry : IInputLogEntry
    {
        public const int MaxStates = 3;

        public event Action OnChanged;

        public int Frame => frame;
        private readonly int frame;

        public Vector2Int Dir => dir;
        private Vector2Int dir;

        public List<InputLogAction> States => states;
        private List<InputLogAction> states = new List<InputLogAction>(MaxStates);

        public InputLogEntry(int frame)
        {
            this.frame = frame;
        }

        public void SetDir(Vector2Int dir)
        {
            this.dir = dir;
            OnChanged?.Invoke();
        }

        public void AddState(InputLogAction state)
        {
            if (states.Count >= MaxStates)
            {
                return;
            }

            states.Add(state);
            OnChanged?.Invoke();
        }

        public override bool Equals(object obj)
        {
            var val = obj as InputLogEntry;
            if (val == null)
                return false;

            return Frame.Equals(val.Frame);
        }

        public override int GetHashCode()
        {
            return Frame.GetHashCode();
        }
    }
}