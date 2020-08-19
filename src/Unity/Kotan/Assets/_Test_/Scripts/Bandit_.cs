﻿using UnityEngine;
using System.Collections;

public class Bandit_ : MonoBehaviour
{

  [SerializeField] Vector2 m_speed = new Vector2(4.0f, 2.0f);
  [SerializeField] float m_jumpForce = 7.5f;

  private Animator m_animator;
  private Rigidbody2D m_body2d;
  [SerializeField] private Rigidbody2D m_anim2d;
  private bool m_grounded = false;
  private bool m_combatIdle = false;
  private bool m_isDead = false;

  // Use this for initialization
  void Start()
  {
    m_animator = GetComponentInChildren<Animator>();
    m_body2d = GetComponent<Rigidbody2D>();
  }

  // Update is called once per frame
  void Update()
  {
    //Check if character just landed on the ground
    var checkGrounded = CheckGrounded();
    if ( !m_grounded && checkGrounded )
    {
      m_grounded = true;
      m_animator.SetBool( "Grounded", m_grounded );
    }

    //Check if character just started falling
    if ( m_grounded && !checkGrounded )
    {
      m_grounded = false;
      m_animator.SetBool( "Grounded", m_grounded );
    }

    // -- Handle input and movement --
    float inputX = Input.GetAxis( "Horizontal" );

    // Swap direction of sprite depending on walk direction
    if ( inputX > 0 )
      transform.localScale = new Vector3( -1.0f, 1.0f, 1.0f );
    else if ( inputX < 0 )
      transform.localScale = new Vector3( 1.0f, 1.0f, 1.0f );

    float inputY = Input.GetAxis( "Vertical" );

    // Move
    Run( inputX, inputY );

    //Set AirSpeed in animator
    float verticalSpeed = GetVerticalSpeed();
    m_animator.SetFloat( "AirSpeed", verticalSpeed );

    // -- Handle Animations --
    //Death
    if ( Input.GetKeyDown( "e" ) )
    {
      if ( !m_isDead )
        m_animator.SetTrigger( "Death" );
      else
        m_animator.SetTrigger( "Recover" );

      m_isDead = !m_isDead;
    }

    //Hurt
    else if ( Input.GetKeyDown( "q" ) )
      m_animator.SetTrigger( "Hurt" );

    //Attack
    else if ( Input.GetMouseButtonDown( 0 ) )
    {
      m_animator.SetTrigger( "Attack" );
    }

    //Change between idle and combat idle
    else if ( Input.GetKeyDown( "f" ) )
      m_combatIdle = !m_combatIdle;

    //Jump
    else if ( Input.GetKeyDown( "space" ) && m_grounded )
    {
      m_animator.SetTrigger( "Jump" );
      m_grounded = false;
      m_animator.SetBool( "Grounded", m_grounded );
      Jump();
    }

    //Run
    else if ( Mathf.Abs( inputX ) > Mathf.Epsilon || Mathf.Abs( inputY ) > Mathf.Epsilon )
      m_animator.SetInteger( "AnimState", 2 );

    //Combat Idle
    else if ( m_combatIdle )
      m_animator.SetInteger( "AnimState", 1 );

    //Idle
    else
      m_animator.SetInteger( "AnimState", 0 );
  }

  private void Jump()
  {
    m_anim2d.velocity = new Vector2( 0, m_jumpForce );
    //m_groundSensor.Disable( 0.2f );
  }

  private float GetVerticalSpeed()
  {
    return m_anim2d.velocity.y;
  }

  private void Run( float inputX, float inputY )
  {
    m_body2d.velocity = new Vector2( inputX, inputY ) * m_speed; //m_body2d.velocity.y
  }

  private static bool CheckGrounded()
  {
    return true;
  }
}
