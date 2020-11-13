using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Kinematic : MonoBehaviour
{
  private Rigidbody rb;

  private void Start()
  {
    rb = GetComponent<Rigidbody>();
  }

  public void Move( Vector2 dir )
  {
    rb.velocity = new Vector3(dir.x, 0, dir.y);
  }

  internal void Jump()
  {
    throw new NotImplementedException();
  }
}
