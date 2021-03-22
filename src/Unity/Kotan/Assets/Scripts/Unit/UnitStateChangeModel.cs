using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    public class UnitStateChangeArg
    {
        public readonly UnitState NewState;

        public readonly object[] Args;

        public UnitStateChangeArg(UnitState newState, params object[] args)
        {
            NewState = newState;
            Args = args;
        }
    }

    public abstract class UnitStateChangeModel
    {
        public abstract UnitState State { get; }        

        public abstract UnitStateChangeArg SetDirection(Vector2Int dir, bool run = false);
    }

    public class UnitStateIdle : UnitStateChangeModel
    {
        public override UnitState State => UnitState.Idle;

        public override UnitStateChangeArg SetDirection(Vector2Int dir, bool run = false)
        {
            if (dir.x == 0 && dir.y == 0)
                return new UnitStateChangeArg(UnitState.Idle);

            if (run) {
                if (dir.x != 0) {
                    return new UnitStateChangeArg(UnitState.Run, dir);
                } else {
                    return new UnitStateChangeArg(UnitState.Dash, dir);
                }
            } else {
                return new UnitStateChangeArg(UnitState.Walk, dir);
            }
        }
    }

    public class UnitStateWalk : UnitStateIdle
    {
        public override UnitState State => UnitState.Walk;
    }
}