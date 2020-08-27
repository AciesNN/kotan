using UnityEngine;

public class IsoViewPos : MonoBehaviour
{
    [SerializeField] private Transform view;

    private Vector3 startPos;

    void Start()
    {
        startPos = view.localPosition;
    }

    void Update()
    {
        UpdateViewPos();
    }

    private void UpdateViewPos()
    {
        view.transform.localPosition = new Vector3(
            startPos.x,
            startPos.y,
            startPos.z);
    }
}
