using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace P25D
{
    public class Physics25D : MonoBehaviour
    {
        private static List<Kinematic25D> kinematicObjects = new List<Kinematic25D>(255);

        static float Gravity;

        [SerializeField] float gravity = 1.0f;
        [SerializeField] float minY = -2.0f;

        public static Physics25D instance { get; private set; }

        private void Awake()
        {
            instance = this; //?
            Gravity = gravity;//?
        }

        private void Update()
        {
            DestroyDeadKinematicObjects();
            ProcessKinematicObjects();
        }

        #region Static
        public static void RegisterKinematicObject(Kinematic25D ko)
        {
            kinematicObjects.Add(ko);
        }

        public static void DeregisterKinematicObject(Kinematic25D ko)
        {
            kinematicObjects.Remove(ko);
        }

        private static void ProcessKinematicObjects()
        {
            for (int i = 0; i < kinematicObjects.Count; i++)
            {
                var ko = kinematicObjects[i];
                if (!ko.enabled || !ko.gameObject.activeInHierarchy)
                {
                    continue;
                }
                ProcessKinematicObject(ko, Time.deltaTime);
            }
        }

        private static void DestroyDeadKinematicObjects()
        {
            for (int i = kinematicObjects.Count - 1; i >= 0; i--)
            {
                if (kinematicObjects[i] == null)
                {
                    kinematicObjects.RemoveAt(i);
                }
            }
        }

        private static void ProcessKinematicObject(Kinematic25D ko, float deltaTime)
        {
            var velocity = ko.Velocity;
            var position = ko.Position;
            var isGrounded = ko.IsGrounded;

            bool groundThisUpdate = false;
            if (ko.UseGravity)
            {
                if (!isGrounded && velocity.y < 0)
                {
                    groundThisUpdate = CheckWillGroundThisUpdate(ko, ko.Velocity, deltaTime, ref position);
                    if (groundThisUpdate)
                    {
                        //position = ... (set by ref)
                        velocity = Vector3.zero;
                        isGrounded = true;
                    }
                }
            }

            if (!groundThisUpdate)
            {
                var deltaPos = velocity.To25D();
                position += deltaPos * deltaTime;

                if (ko.UseGravity)
                {
                    if (isGrounded)
                    { 
                        isGrounded = CheckStillGrounded(ko);
                    }

                    if (!isGrounded)
                    {
                        velocity += Vector3.down * Gravity * deltaTime;
                    }
                }
            }

            ko.Velocity = velocity;
            ko.Position = position;
            ko.IsGrounded = isGrounded;
        }

        private static bool CheckStillGrounded(Kinematic25D ko)
        {
            return true;
        }

        private static bool CheckWillGroundThisUpdate(Kinematic25D ko, Vector3 velocity, float deltaTime, ref Vector3 position)
        {
            var collider = ko.Collider2D;
            if (collider == null || !collider.enabled)
            {
                return false;
            }

            var direction = new Vector2(velocity.x, velocity.y);
            var filter = new ContactFilter2D()
            {
                //floor layer filter mask
            };
            RaycastHit2D[] result = new RaycastHit2D[16];
            var resCount = collider.Cast(direction, filter, result, direction.magnitude * deltaTime, true);
            for (int i = 0; i < resCount; i++)
            {
                var hitInfo = result[i];

                float height = GetGeometryObgectHeight(hitInfo.collider.gameObject);
                

                position += velocity.To25D() * deltaTime * hitInfo.fraction;
                return true;
            }
            return false;
        }

        private static float GetGeometryObgectHeight(GameObject go)
        {
            return 0;
        }
        #endregion
    }

    public static class Physics25DExctentions
    {
        public static Vector3 To25D(this Vector3 vector)
        {
            return new Vector3(
                vector.x,
                vector.y + vector.z,
                vector.z);
        }
    }
}