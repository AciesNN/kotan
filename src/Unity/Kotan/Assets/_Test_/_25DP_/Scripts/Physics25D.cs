using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//wraping up the physics: collisions/colliders for the 2.5D world
public class Physics25D : MonoBehaviour
{
    //TODO (O) optimize arr usage
    private List<GameObject> objs = new List<GameObject>(255); //TODO GameObject -> interface
    private List<GameObject> obst = new List<GameObject>(255); //TODO GameObject -> interface

    public static Physics25D instance { get; private set; }

    private void Awake()
    {
        instance = this; //TODO (c) check singleton
    }

    public void RegisterKinematicObject(GameObject go)
    {
        objs.Add(go);
    }

    public void DeregisterKinematicObject(GameObject go)
    {
        objs.Remove(go);
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < objs.Count; i++)
        {
            var go = objs[i];
            ProcessObj(go);
        }
    }

    private void ProcessObj(GameObject go)
    {

    }
}
