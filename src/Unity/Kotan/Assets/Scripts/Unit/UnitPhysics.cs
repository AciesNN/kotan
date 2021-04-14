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
        private float jumpChangeSpeed = 2;
        private float jumpIdleSpeed = 0.5f;

        private bool isJumping;
        private bool isFalling;
        private bool isJumpingFallingFromIdle;

        public Vector2Int startJumpDir { get; protected set; }
        
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

        public void Move(Vector2Int dir)
        {
            if (isJumping || isFalling) {
                var newSpeed = RB.velocity.x + Mathf.Sign(dir.x) * (isJumpingFallingFromIdle ? jumpIdleSpeed : jumpChangeSpeed);
                SetMove(dir, newSpeed);
            } else {
                var newSpeed = GetSpeedForState(unit.State);
                SetMove(dir, newSpeed);
            }
        }

        public void StopMove()
        {
            SetMove(Vector2Int.zero, 0);
        }

        public void Jump(Vector2Int dir)
        {
            startJumpDir = dir;
            StartJump(startJumpDir, jumpSpeed);
        }
        #endregion

        #region Impl
        private float GetSpeedForState(UnitState state)
        {
            return speeds.ContainsKey(state) ? speeds[state] : 0;
        }

        private void SetMove(Vector2Int dir, float speed)
        {
            RB.velocity = new Vector2(dir.x, dir.y) * speed;
        }

        private void StartJump(Vector2Int dir, float speed)
        {
            isJumping = true;
            isJumpingFallingFromIdle = Mathf.Approximately(RB.velocity.x, 0);
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
            isFalling = false;

            _stopFalling();

            unit.AnimationEvent("AnimationComplete");
        }

        private void ProcessJump()
        {
            if (isJumping) {
                _processJump();
            } else if (isFalling) {
                _processFalling();
            } else {
                _process();
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
        
        private void _process()
        {
            _jumpRB.transform.localPosition = Vector3.zero;
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