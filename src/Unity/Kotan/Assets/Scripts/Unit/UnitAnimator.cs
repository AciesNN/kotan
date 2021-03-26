using UnityEngine;

namespace Unit
{
    public class UnitAnimator : MonoBehaviour
    {
        [SerializeField] Animator animator;

        public void SetState(UnitStateChangeArg newState, bool changeAnim)
        {
            if (changeAnim) {
                SetAnim(newState.State.ToString());
            }

            //TODO???
            /*if (newState.ChangeDir) {
                SetDir(newState.Dir);
            }*/
        }

        private void SetAnim(string animName)
        {
            animator.SetTrigger(animName);
        }

        public void SetDir(Vector2Int dir)
        {
            if (dir.x != 0) {
                animator.transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
            }
        }

        public void AnimationComplete()
        {
        }
    }
}