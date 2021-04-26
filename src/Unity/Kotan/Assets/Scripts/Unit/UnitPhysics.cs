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
        private float jumpXSpeed = 2;
        private float jumpYSpeed = 1;
        private float jumpXForceSpeed = 4;

        private bool isJumping;
        private bool isFalling;

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

        public void Move(Vector2Int dir, bool _ /*force*/)
        {
            //ignore "force" for now, get speed from state
            if (isJumping || isFalling) {
            } else {
                var newSpeed = GetMoveSpeedForState(unit.State, dir);
                SetMove(dir, newSpeed);
            }
        }

        public void StopMove()
        {
            SetMove(Vector2Int.zero, 0);
        }

        public void Jump(Vector2Int dir, bool force)
        {
            //ignore "force" for now, get speed from state
            startJumpDir = dir;
            var moveSpeed = GetJumpMoveSpeed(dir, force);
            StartJump(dir, moveSpeed, jumpSpeed);
        }
        #endregion

        #region Impl
        private Vector2 GetJumpMoveSpeed(Vector2Int dir, bool force)
        {
            var xSpeed = 0f;
            if (dir.x != 0) {
                xSpeed = force ? jumpXForceSpeed : jumpXSpeed;
            }

            var ySpeed = 0f;
            if (dir.y != 0) {
                ySpeed = force ? 0 : jumpYSpeed;
            }

            return new Vector2(xSpeed, ySpeed);
        }

        private float GetMoveSpeedForState(UnitState state, Vector2Int dir)
        {
            return speeds.ContainsKey(state) ? speeds[state] : 0;
        }

        private void SetMove(Vector2Int dir, float speed)
        {
            RB.velocity = new Vector2(dir.x, dir.y) * speed;
        }

        private void StartJump(Vector2Int dir, Vector2 moveSpeed, float jumpSpeed)
        {
            isJumping = true;
            _startJump(dir, moveSpeed, jumpSpeed);
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
        private void _startJump(Vector2Int dir, Vector2 moveSpeed, float jumpSpeed)
        {
            RB.velocity = new Vector2(dir.x * Mathf.Abs(moveSpeed.x), dir.y * Mathf.Abs(moveSpeed.y));

            _jumpRB.velocity = new Vector2(0, 1) * jumpSpeed;
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