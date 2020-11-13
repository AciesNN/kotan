using P25D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput3D : MonoBehaviour
{
    public Kinematic ko;

    void Update()
    {
        if (ko)
        {
            ko.Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
            if (Input.GetButtonDown("Jump"))
            {
                ko.Jump();
            }
        }
    }
}
