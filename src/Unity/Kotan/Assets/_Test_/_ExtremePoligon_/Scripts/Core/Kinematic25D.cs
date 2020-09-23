using UnityEngine;

namespace P25D
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Kinematic25D : MonoBehaviour25D
    {
        [SerializeField] public bool _debug;

        public bool IsGrounded { get; set; }

        public Vector3 Velocity { get; set; }
        public Collider2D Collider2D { get; set; }

        [SerializeField] private bool useGravity = true;
        public bool UseGravity { get => useGravity; set { useGravity = value; } }

        protected override void Awake()
        {
            base.Awake();
            Collider2D = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            Physics25D.RegisterKinematicObject(this);
        }
    }
}