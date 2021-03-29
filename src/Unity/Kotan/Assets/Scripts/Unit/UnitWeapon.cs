using System;
using System.Collections;
using UnityEngine;

namespace Unit
{
    public class UnitWeapon : MonoBehaviour
    {
        public event Action OnHitDetected;

        [SerializeField] GameObject weaponCollider;
        [SerializeField] GameObject _testHitEffect;

        public bool HitDetected { get; private set; }

        private void HitDetection()
        {
            HitDetected = true;
            OnHitDetected?.Invoke();

            HitEffect();
        }

        private void HitEffect()
        {
            _testHitEffect.SetActive(true);
        }
        


        private void OnTriggerEnter(Collider other)
        {
            HitDetection();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            HitDetection();
        }

        public void Reset()
        {
            HitDetected = false;
            SetActive(false);
        }

        public void SetActive(bool active)
            => weaponCollider.SetActive(active);
    }
}