using UnityEngine;

namespace UI
{
    public struct InputState
    {
        public Vector2Int dir;
        public bool force;
        public InputAction action;

        public override string ToString()
        {
            var forceStr = "";
            var nonForceStr = "(!)";
            return $"Input [{action}] {dir} {(force ? forceStr: nonForceStr)}";
        }
    }
}