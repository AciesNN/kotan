using UnityEngine;
using UnityEditor;

namespace P25D
{
    [CustomEditor(typeof(MonoBehaviour25D), true)]
    [CanEditMultipleObjects]
    public class MonoBehaviour25DEditor : Editor
    {
        const float dz = 3;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var t = target as MonoBehaviour25D;
            if (!t)
            {
                return;
            }

            var depth = t.transform.position.z;
            var newDepth = EditorGUILayout.Slider("Depth", depth, -dz, dz);
            var dDepth = newDepth - depth;
            if (!Mathf.Approximately(dDepth, 0))
            {
                t.transform.position = new Vector3(t.transform.position.x, t.transform.position.y + dDepth, newDepth);
            }
        }
    }
}