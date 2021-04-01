using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    public class UnitPhysics : MonoBehaviour
    {
        Unit unit;

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
            {UnitState.Run, 5},
            {UnitState.Dash, 2},
        };

        private float jumpSpeed = 4;

        private bool isJumping;
        private bool isFalling;

        #region MonoBehaviour
        private void Update()
        {
            ProcessJump();
        }
        #endregion

        #region Intefrace
        public void Init(Unit unit)
        {
            this.unit = unit;
        }

        public void SetState(UnitStateChangeArg newState)
        {
            if (newState.ProcessJump) {
                StartJump(jumpSpeed);
            } else {
                var newSpeed = GetSpeed(newState.State);
                SetMove(newState.Dir, newSpeed);
            }
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

        private void StartJump(float speed)
        {
            isJumping = true;
            _startJump(speed);
        }

        private void StartFall()
        {
            isJumping = false;
            isFalling = true;

            unit.SetState(newState: UnitState.Fall);
        }

        private void StopFalling()
        {
            _stopFalling();

            unit.SetState(newState: UnitState.Idle);
        }

        private void ProcessJump()
        {
            if (isJumping) {
                _processJump();
            } else if (isFalling) {
                _processFalling();
            }
        }
        #endregion

        #region _test
        [SerializeField] private Rigidbody _jumpRB;
        private void _startJump(float speed)
        {
            _jumpRB.velocity = new Vector2(0, 1) * speed;
            _jumpRB.useGravity = true;
        }

        private void _processJump()
        {
            if (_jumpRB.velocity.y < 0) {
                StartFall();
            }
        }
        private void _processFalling()
        {
            if (_jumpRB.transform.localPosition.y < 0) {
                StopFalling();
            }
        }
        private void _stopFalling()
        {
            _jumpRB.velocity = Vector3.zero;
            _jumpRB.transform.localPosition = Vector3.zero;
            _jumpRB.useGravity = false;
        }
        #endregion
    }
}