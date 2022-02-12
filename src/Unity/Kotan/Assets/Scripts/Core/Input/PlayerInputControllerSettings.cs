using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Input
{
    [CreateAssetMenu]
    public class PlayerInputControllerSettings : ScriptableObject
    {
        public float DirectionTimeout => directionTimeout;
        [SerializeField] float directionTimeout = 0.06f;
        public float DirectionPressTimeout => directionPressTimeout;
        [SerializeField] float directionPressTimeout = 1.0f;
        public float DirectionNeitralTimeout => directionNeitralTimeout;
        [SerializeField] float directionNeitralTimeout = 0.5f;
        public float ActionTimeout => actionTimeout;
        [SerializeField] float actionTimeout = 0.06f;
    }
}