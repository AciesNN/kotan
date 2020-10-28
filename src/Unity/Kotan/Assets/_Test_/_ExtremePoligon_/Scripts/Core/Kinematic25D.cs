using System;
using UnityEngine;

namespace P25D
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Kinematic25D : MonoBehaviour25D
    {
        [SerializeField] public bool _debug;

        public bool IsGrounded { get; set; }

        public Vector3 Velocity { get; set; }
        public BoxCollider2D Collider2D { get; set; }

        [SerializeField] private bool useGravity = true;
        public bool UseGravity { get => useGravity; set { useGravity = value; } }

        protected override void Awake()
        {
            base.Awake();
            Collider2D = GetComponent<BoxCollider2D>();
        }

        private void OnEnable()
        {
            Physics25D.RegisterKinematicObject(this);
        }

        public void Move(Vector2 dir)
        {
            if (IsGrounded)
            {
                Velocity = new Vector3(dir.x, 0, dir.y);
            }
        }

        public void Jump()
        {
            if (IsGrounded)
            {
                Velocity += Vector3.up;
            }
        }
    }
}