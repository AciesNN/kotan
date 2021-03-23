using UnityEngine;

namespace Unit
{
    public class UnitAnimator : MonoBehaviour
    {
        [SerializeField] Animator animator;

        private Vector2Int curDir;

        public void SetState(UnitStateChangeArg newState, bool changeAnim)
        {
            if (changeAnim) {
                SetAnim(newState.NewState.ToString());
            }

            if (newState.ChangeDir) {
                SetDir(newState.Dir);
            }
        }

        private void SetAnim(string animName)
        {
            animator.SetTrigger(animName);
        }

        private void SetDir(Vector2Int dir)
        {
            curDir = dir;
            if (dir.x != 0) {
                animator.transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
            }
        }
    }
}