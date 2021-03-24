using UnityEngine;
using UnityEngine.Events;

public class AnimationComplete : MonoBehaviour
{
    [SerializeField] UnityEvent onAnimationComplete;

    void OnAnimationComplete()
    {
        onAnimationComplete?.Invoke();
    }
}
