using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  [SerializeField]
  float speed;

  [SerializeField]
  GameObject hit;

  [SerializeField]
  GameObject runGO;

  SpriteRenderer sprite;

  void Awake()
  {
    sprite = GetComponent<SpriteRenderer>();
    if ( runGO ) 
      runGO.SetActive( false );
    if ( hit )
      hit.SetActive( false );
  }

  public void Hit()
  {
    if (hit) hit.SetActive(true);
  }

  Vector2 lastRunDir;
  bool runState;
  public void Move( Vector2 dir, bool run )
  {
    UpdateRunState( dir, run );
    SetRunState( runState );

    gameObject.transform.Translate( dir * Time.deltaTime * (runState ? 2 : 1) );
  }

  void UpdateRunState( Vector2 dir, bool run )
  {
    if ( run )
    {
      lastRunDir = dir;
      runState = true;
    }
    else
    {
      if ( runState )
      {
        runState = lastRunDir.Equals( dir ) && !dir.Equals( Vector2.zero );
      }

      if ( !runState )
      {
        lastRunDir = Vector2.zero;
      }
    }
  }

  void SetRunState( bool run )
  {
    if ( runGO ) runGO.SetActive( run );
  }

  public void SetPressState( bool pressed )
  {
    sprite.color = pressed ? Color.blue : Color.white;
  }
}
