using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    public class UnitPhysics : MonoBehaviour
    {
        Rigidbody rb;

        #region MonoBehaviour
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        #endregion

        #region Intefrace
        public void SetState(UnitState state, object[] stateArgs)
        {
            switch (state) {
                case UnitState.Idle:
                    rb.velocity = Vector3.zero;
                    break;
                case UnitState.Walk:
                    Move((Vector2Int)stateArgs[0], 2);
                    break;
            }
        }
        #endregion

        #region Impl
        private void Move(Vector2Int dir, float speed)
        {
            rb.AddForce(new Vector2(dir.x, dir.y) * speed, ForceMode.VelocityChange);
        }
        #endregion
    }
}