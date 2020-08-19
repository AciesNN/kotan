using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
  [SerializeField] private new Rigidbody2D rigidbody2D;
  private float minLocalPositionY;

  private
  // Start is called before the first frame update
  void Start()
  {
    rigidbody2D = GetComponent<Rigidbody2D>();
    minLocalPositionY = transform.localPosition.y;
    rigidbody2D.simulated = false;
  }

  // Update is called once per frame
  void Update()
  {
    float y = transform.localPosition.y;

    if ( rigidbody2D.simulated
      && rigidbody2D.velocity.y < 0 
      && IsGrounded() )
    {
      rigidbody2D.simulated = false;
      rigidbody2D.velocity = Vector2.zero;
      y = minLocalPositionY;
    }

    transform.localPosition = new Vector2( 0, y );
  }

  public void Jump( float jumpForce )
  {
    rigidbody2D.simulated = true;
    rigidbody2D.velocity = new Vector2( 0, jumpForce );
  }

  public float GetVerticalSpeed()
  {
    return rigidbody2D.velocity.y;
  }

  public bool IsGrounded()
  {
    return transform.localPosition.y - minLocalPositionY < float.Epsilon; 
  }
}
