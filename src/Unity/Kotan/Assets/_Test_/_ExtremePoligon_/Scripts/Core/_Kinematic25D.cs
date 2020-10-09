using System.Net.Http.Headers;
using UnityEngine;

namespace _25d_
{
    //Wrap up rigidbody2D behavior in 2.5D world
    [RequireComponent(typeof(Rigidbody))]
    public class Kinematic25D : MonoBehaviour
    {
        private Rigidbody _rb;
        private Rigidbody rb { get { if (_rb == null) { _rb = GetComponent<Rigidbody>(); } return _rb; } }

        public float Depth => _transform.position.z;
        public bool IsFlying { get; private set; }
        /*private float _lastTransformY;
        private Transform _lastParent;*/
        private Transform _transform;

        public bool IsMoveHorizontaly { get; private set; }

        private void Awake()
        {
            rb.useGravity = false;
            rb.detectCollisions = false;
            IsMoveHorizontaly = true;
            _transform = transform;
            /*_lastTransformY = _transform.position.y;
            _lastParent = _transform.parent;*/
        }

        private void Start()
        {
            //Physics25D.instance.RegisterKinematicObject(gameObject);
        }

        private void OnDestroy()
        {
            //Physics25D.instance.DeregisterKinematicObject(gameObject);
        }

        /*private void LateUpdate()
        {
            var y = _transform.position.y;
            if (IsMoveHorizontaly 
                && !IsFlying 
                && _lastParent == _transform.parent)
            {
                var delta = y - _lastTransformY;
                if (delta > float.Epsilon || delta < -float.Epsilon)
                {
                    //TODO - floating after fly
                    SetDepth( Depth + delta);
                }
            }
            _lastTransformY = y;
            _lastParent = _transform.parent;
        }*/

        /*private void SetDepth(float depth)
        {
            _transform.transform.position = new Vector3(
                _transform.transform.position.x,
                _transform.transform.position.y,
                depth );
        }*/

        public void Move(Vector3 dir, Vector2 speed)
        {
            if (!IsMoveHorizontaly)
            {
                return;
            }

            rb.velocity = new Vector3(
                dir.x * speed.x,
                dir.y * speed.y,
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