using UnityEngine;

namespace Unit
{
    public class UnitHPBody : MonoBehaviour
    {
        Unit unit;

        public void Init(Unit unit)
        {
            this.unit = unit;
        }

        private void OnTriggerEnter(Collider collision)
        {
            HitDetection();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            HitDetection();
        }

        private void HitDetection()
        {
            unit.Dmg();
        }
    }
}