using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tst : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    bool f;
    void Update()
    {
        if (Input.GetButton("Horizontal"))
        {
            Debug.Log($"[{Time.frameCount}/{Time.realtimeSinceStartup}] Horizontal axis: {Input.GetAxis("Horizontal")}, raw: {Input.GetAxisRaw("Horizontal")} {Input.GetButtonDown("Horizontal")}");
            f = true;
        } else if (f)
        {
            Debug.Log($"! [{Time.frameCount}/{Time.realtimeSinceStartup}] Horizontal axis: {Input.GetAxis("Horizontal")}, raw: {Input.GetAxisRaw("Horizontal")}");
            if (Input.GetAxis("Horizontal") < float.Epsilon) {
                f = false;
            }
        }
    }
}
