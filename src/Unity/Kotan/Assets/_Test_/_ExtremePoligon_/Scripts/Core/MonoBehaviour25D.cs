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
            var altitude = transform.position.y - dz;
            if (!dz0)
            {
                lastLinePos = DrawNextGizmo(new Vector3(transform.position.x - dz, altitude, 0), lastLinePos, dz > 0 ? Color.green : Color.yellow);
            }

            lastLinePos = DrawNextGizmo(new Vector3(transform.position.x - dz, 0, 0), lastLinePos, altitude > 0 ? Color.white : Color.red);
        }

        private static Vector3 DrawNextGizmo(Vector3 gPos, Vector3 lastLinePos, Color color)
        {
            var gizmosColor = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawSphere(gPos, 0.05f);
            Gizmos.color = gizmosColor;
            Gizmos.DrawLine(lastLinePos, gPos);
            return gPos;
        }
        #endregion
    }
}