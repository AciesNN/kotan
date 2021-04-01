using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyAttack : MonoBehaviour
{
    [SerializeField] Animator animator;
    void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            animator.SetTrigger("Attack");
    }
}
