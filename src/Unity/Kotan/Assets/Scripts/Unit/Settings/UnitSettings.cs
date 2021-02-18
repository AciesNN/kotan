using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    [CreateAssetMenu]
    public class UnitSettings : ScriptableObject
    {
        public float HorizontalSpeed => horizontalSpeed;
        [SerializeField] float horizontalSpeed;
        public float HorizontalRunSpeed => horizontalRunSpeed;
        [SerializeField] float horizontalRunSpeed;
    }
}