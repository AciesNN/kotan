using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace P25D
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Kinematic25D : MonoBehaviour
    {
        public bool IsGrounded { get; set; }

        public Vector3 Velocity { get; set; }
        public Collider2D Collider2D { get; set; }

        [SerializeField] private bool useGravity = true;
        public bool UseGravity { get => useGravity; set { useGravity = value; } }

        public Vector3 Position
        {
            get => new Vector3( _transform.position.x, _transform.position.y, Depth );
            set
            {
                _transform.position = value;
            }
        }

        public float Depth => _transform.position.z;
        public float LocalDepth => _transform.localPosition.z;

        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
            Collider2D = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            Physics25D.RegisterKinematicObject(this);
        }
    }
}