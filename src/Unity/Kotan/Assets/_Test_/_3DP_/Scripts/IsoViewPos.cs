using System;
using UnityEngine;

public class IsoViewPos : MonoBehaviour
{
    [SerializeField] private Transform view;

    private Vector3 startPos;
    private float depthCoef = 1.4f;
    private float heightCoef = 1.4f;

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
            startPos.y + GetDepthTransform() + GetHeightTransform(),
            startPos.z );
    }

    private float GetDepthTransform() => GetDepth() * depthCoef;

    private float GetDepth() => transform.position.z;

    private float GetHeightTransform() => GetHeight() * heightCoef;

    private float GetHeight() => transform.position.y;
}
