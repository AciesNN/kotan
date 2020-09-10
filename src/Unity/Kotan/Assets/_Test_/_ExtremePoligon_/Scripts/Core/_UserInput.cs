using P25D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _UserInput : MonoBehaviour
{
    public Kinematic25D fly1;
    public Kinematic25D fly2;
    public Kinematic25D fly3;

    public Kinematic25D ko;


    void Update()
    {
        if (fly1 && Input.GetKeyDown(KeyCode.Alpha1) && !fly1.UseGravity)
        {
            fly1.UseGravity = true;
        }

        if (fly2 && Input.GetKeyDown(KeyCode.Alpha2) && fly2.UseGravity)
        {
            fly2.Velocity += Vector3.right * 3;
        }

        if (fly3 && Input.GetKeyDown(KeyCode.Alpha3) && !fly3.UseGravity)
        {
            fly3.UseGravity = true;
            fly3.Velocity = Vector3.up * 4;
        }
    }
}
