using UnityEngine;

namespace Unit
{
    public class UnitAnimator : MonoBehaviour
    {
        [SerializeField] Animator animator;

        public void SetState(UnitState state, object[] stateArgs)
        {
            animator.SetTrigger(state.ToString());

            switch (state) {
                case UnitState.Walk:
                case UnitState.Run:
                    SetDir((Vector2Int)stateArgs[0]);
                    break;
                default:
                    SetDir(Vector2Int.right);
                    break;
            }
        }

        private void SetDir(Vector2Int dir)
        {
            animator.transform.localScale = new Vector3(Mathf.Sign(dir.x) , 1, 1);
        }
    }
}