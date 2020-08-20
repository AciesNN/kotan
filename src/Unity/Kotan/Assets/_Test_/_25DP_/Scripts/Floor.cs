using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
  public float h;
  public new Collider2D collider;

  private void Awake()
  {
    collider = GetComponent<Collider2D>();
  }
}
