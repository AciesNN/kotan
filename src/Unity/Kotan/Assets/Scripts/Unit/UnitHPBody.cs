using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    public class UnitHPBody : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            HitDetection();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            HitDetection();
        }

        private void HitDetection()
        {
        }
    }
}