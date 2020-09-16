using P25D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Kinematic25D))]
public class _KinematicHelper : MonoBehaviour
{
    Kinematic25D kinematic25D;
    public SpriteRenderer sprite;

    void Start()
    {
      kinematic25D = GetComponent<Kinematic25D>();
    }

    void Update()
    {
      if (sprite && kinematic25D )
      {
        sprite.color =
          !kinematic25D.UseGravity ? Color.white :
          kinematic25D.IsGrounded ? Color.green : Color.red;
      }
    }
}
