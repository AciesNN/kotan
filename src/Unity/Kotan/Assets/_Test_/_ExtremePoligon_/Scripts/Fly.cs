using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
  /*[SerializeField]*/public EdgeCollider2D foot;
  public float d;

  //Vector3 globalPos =>
  //  new Vector3( transform.localPosition.x, transform.localPosition.y, d );

  private void Awake()
  {
    foot = GetComponent<EdgeCollider2D>();
  }

  void Update()
  {
    var f = GetCurrentFoor();
    //Debug.Log( f?.name ?? "null" );
  }

  private Floor GetCurrentFoor()
  {
    var allFloors = FindObjectsOfType<Floor>();
    return allFloors
      .OrderByDescending(f => f.h)
      .FirstOrDefault(f => CheckFloorFootIntersection (f));
  }

  private bool CheckFloorFootIntersection( Floor f )
  {
    return
      CheckFloorPointIntersection(f, foot.bounds.min.x, d )
      || CheckFloorPointIntersection( f, foot.bounds.max.x, d );
  }

  //static
  public static bool CheckFloorPointIntersection(Floor f, float x, float d)
  {
        var relativePoint = new Vector2(x, f.transform.position.y + d);
        return f.collider.OverlapPoint(relativePoint);
  }
}
