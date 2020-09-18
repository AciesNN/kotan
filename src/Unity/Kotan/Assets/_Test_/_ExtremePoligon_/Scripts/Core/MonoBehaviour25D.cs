using UnityEngine;

namespace P25D
{
    public class MonoBehaviour25D : MonoBehaviour
    {
        public Vector3 Position
        {
            get => new Vector3(Transform.position.x, Transform.position.y, Depth);
            set
            {
                Transform.position = value;
            }
        }

        public float Depth => Transform.position.z;

        private Transform _transform;
        public Transform Transform
        {
            get
            {
                if (!_transform) _transform = transform;
                return _transform;
            }
        }

        protected virtual void Awake()
        {
        }

        #region Gizmo
        private void OnDrawGizmosSelected()
        {
            var dz = transform.position.z;
            var dz0 = Mathf.Approximately(dz, 0);
            var lastLinePos = transform.position;
            if (!dz0)
            {
                lastLinePos = DrawNextGizmo(new Vector3(transform.position.x - dz, transform.position.y - dz, 0), lastLinePos);
            }

            lastLinePos = DrawNextGizmo(new Vector3(transform.position.x - dz, 0, 0), lastLinePos);
        }

        private static Vector3 DrawNextGizmo(Vector3 gPos, Vector3 lastLinePos)
        {
            Gizmos.DrawSphere(gPos, 0.05f);
            Gizmos.DrawLine(lastLinePos, gPos);
            return gPos;
        }
        #endregion
    }
}