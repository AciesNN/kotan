using UnityEngine;

namespace Unit
{
    public class UnitAnimator : MonoBehaviour
    {
        [SerializeField] Animator animator;

        public bool IsAnimationComplete { get; internal set; }

        public void SetState(UnitState newState)
        {
            SetAnim(newState.ToString());
        }

        private void SetAnim(string animName)
        {
            IsAnimationComplete = false;
            animator.SetTrigger(animName);
        }

        public void SetDirection(Vector2Int dir)
        {
            if (dir.x != 0) {
                animator.transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
            }
        }

        public void AnimationComplete()
        {
            IsAnimationComplete = true;
        }
    }
}