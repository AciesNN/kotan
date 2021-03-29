using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    [SerializeField] UnityEvent<string> OnEvent;

    void FireEvent(string eventName)
    {
        OnEvent?.Invoke(eventName);
    }
}
