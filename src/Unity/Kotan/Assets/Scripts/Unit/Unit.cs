using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    public class Unit : MonoBehaviour, IUnit
    {
        [SerializeField] Animator animator;
        [SerializeField] UnitSettings settings;

        Rigidbody rb;

        #region MonoBehaviour
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        #endregion

        #region IUnit
        public void Move(Vector2Int dir, UnitMoveType type)
        {
            var speed = type == UnitMoveType.Walk ? settings.HorizontalSpeed : settings.HorizontalRunSpeed;
            rb.velocity = new Vector3(speed * dir.x, speed * dir.y);
            animator.SetBool("Walk", type == UnitMoveType.Walk);
            animator.SetBool("Run", type == UnitMoveType.Run);
            if (dir.x != 0 && dir.x * animator.transform.localScale.x < 0)
            {
                animator.transform.localScale = new Vector3(dir.x, 1, 1);
            }
        }

        public void Stop()
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            rb.velocity = Vector2.zero;
        }
        #endregion
    }
}