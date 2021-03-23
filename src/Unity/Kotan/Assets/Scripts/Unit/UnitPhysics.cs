using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    public class UnitPhysics : MonoBehaviour
    {
        Rigidbody rb;
        Rigidbody RB {
            get {
                if (null == rb)
                    rb = GetComponent<Rigidbody>();
                return rb;
            }
        }

        Dictionary<UnitState, float> speeds = new Dictionary<UnitState, float>() {
            {UnitState.Idle, 0},
            {UnitState.Walk, 2},
            {UnitState.Run, 3},
            {UnitState.Dash, 2},
        };

        #region MonoBehaviour
        #endregion

        #region Intefrace
        public void SetState(UnitStateChangeArg newState)
        {
            var newSpeed = GetSpeed(newState.NewState);
            SetMove(newState.Dir, newSpeed);
        }
        #endregion

        #region Impl
        private float GetSpeed(UnitState state)
        {
            return speeds.ContainsKey(state) ? speeds[state] : 0;
        }

        private void SetMove(Vector2Int dir, float speed)
        {
            RB.velocity = new Vector2(dir.x, dir.y) * speed;
        }
        #endregion
    }
}