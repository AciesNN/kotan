using UnityEngine;

namespace Unit
{
    public class UnitStateChangeArg
    {
        public UnitState State;
        public bool ChangeDir = true;
        public Vector2Int Dir;
        public bool ReplayAnimation;
        public bool ProcessJump;
    }
}
