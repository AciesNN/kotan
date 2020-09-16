using P25D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Kinematic25D))]
public class Shadow : MonoBehaviour
{
    [SerializeField] private GameObject shadow;
    private Kinematic25D kinematic25D;

    private void Start()
    {
        kinematic25D = GetComponent<Kinematic25D>();
    }

    void Update()
    {
        if (shadow && kinematic25D)
        {
            var isShadow = Physics25D.GetNearestFloorPosition(kinematic25D, out Vector3 shadowPosition);
            if (isShadow)
            {
                shadow.SetActive(true);
                shadow.transform.position = shadowPosition;
            }
            else
            {
                shadow.SetActive(false);
            }
        }
    }
}
