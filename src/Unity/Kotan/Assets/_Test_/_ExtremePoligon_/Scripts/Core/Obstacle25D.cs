using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace P25D
{
    public class Obstacle25D : Static25D
    {
        [SerializeField] private bool infiniteHeight;
        public bool InfiniteHeight => infiniteHeight;

        [SerializeField] private float height;
        public float Height => height;
    }
}
