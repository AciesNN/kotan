using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _3d
{
    public class Kinematic : MonoBehaviour
    {
        private Rigidbody _rb;
        private Rigidbody rb { get { if (_rb == null) { _rb = GetComponent<Rigidbody>(); } return _rb; } }

        public float Depth => _transform.position.z;
        public bool IsFlying { get; private set; }

        private Transform _transform;

        public bool IsMoveHorizontaly { get; private set; }

        private void Awake()
        {
            IsMoveHorizontaly = true;
            _transform = transform;
        }

        public void Move(Vector3 dir, Vector2 speed)
        {
            if (!IsMoveHorizontaly)
            {
                return;
            }

            rb.velocity = new Vector3(
                dir.x * speed.x,
                0,
                dir.y * speed.y);
            IsFlying = false;
        }

        public void Fly(Vector3 dir, float speed)
        {
            if (!IsMoveHorizontaly)
            {
                return;
            }

            if (dir.sqrMagnitude < float.Epsilon)
            {
                return;
            }

            if (speed < float.Epsilon)
            {
                return;
            }

            rb.velocity = dir * speed;
            IsFlying = true;
        }

        public void Jump(float force)
        {
            if (!IsMoveHorizontaly)
            {
                return;
            }

            IsFlying = false;
            rb.useGravity = true;
            IsMoveHorizontaly = false;
            TurnOffVerticalVelocity();
            AddForce(Vector2.up * force);
        }

        public void Strike(Vector2 dir, float force)
        {
            if (IsMoveHorizontaly)
            {
                return;
            }

            AddForce(dir * force);
        }

        private void AddForce(Vector2 force)
        {
            rb.AddForce(force, ForceMode.Impulse);
        }

        private void TurnOffVerticalVelocity()
        {
            rb.velocity *= Vector2.right;
        }
    }
}