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
            {UnitState.Jump, 1},
        };

        private float jumpUpSpeed = 4;
        private float jumpUpForcedSpeed = 5;
        private float jumpSpeed = 2;
        private float jumpForcedSpeed = 3;

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

        public void Move(Vector2Int dir, bool force)
        {
            if (isJumping || isFalling) {
                if (isJumpingFallingFromIdle) {
                    var moveSpeed = GetMoveSpeedForState(unit.State, dir, force);
                    SetMove(dir, moveSpeed);
                }
            } else {
                var moveSpeed = GetMoveSpeedForState(unit.State, dir, force);
                SetMove(dir, moveSpeed);
            }
        }

        public void StopMove()
        {
            SetMove(Vector2Int.zero, Vector2.zero);
        }

        public void Jump(Vector2Int dir, bool force)
        {
            //ignore "force" for now, get speed from state
            startJumpDir = dir;
            var moveSpeed = GetJumpMoveSpeed(unit.State, dir, force);
            var upSpeed = force ? jumpUpForcedSpeed : jumpUpSpeed;
            StartJump(dir, moveSpeed, upSpeed);
        }
        #endregion

        #region Impl
        private Vector2 GetJumpMoveSpeed(UnitState state, Vector2Int dir, bool force)
        {
            var xSpeed = 0f;
            if (dir.x != 0) {
                xSpeed = force ? jumpForcedSpeed : jumpSpeed;
            }

            var ySpeed = 0f;
            if (dir.y != 0) {
                ySpeed = force ? 0 : jumpSpeed;
            }

            return new Vector2(xSpeed, ySpeed);
        }

        private Vector2 GetMoveSpeedForState(UnitState state, Vector2Int dir, bool force)
        {
            var speed = speeds.ContainsKey(state) ? speeds[state] : 0;
            return Vector2.one * speed;
        }

        private void SetMove(Vector2Int dir, Vector2 moveSpeed)
        {
            RB.velocity = new Vector2(dir.x * Mathf.Abs(moveSpeed.x), dir.y * Mathf.Abs(moveSpeed.y));
        }

        private void StartJump(Vector2Int dir, Vector2 moveSpeed, float jumpUpSpeed)
        {
            isJumping = true;
            isJumpingFallingFromIdle = Mathf.Approximately(RB.velocity.x, 0);

            SetMove(dir, moveSpeed);
            _startJump(dir, moveSpeed, jumpUpSpeed);
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

            unit.AnimationEvent("AnimationComplete");//TODO ???
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
        private void _startJump(Vector2Int dir, Vector2 moveSpeed, float jumpUpSpeed)
        {
            _jumpRB.velocity = Vector2.up * jumpUpSpeed;
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