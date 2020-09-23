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
        private static ContactFilter2D floorFilter = new ContactFilter2D()
        {
          //floor layer filter mask
        };
        private static ContactFilter2D obstacleFilter = new ContactFilter2D()
        {
            //floor layer filter mask
        };

        private static RaycastHit2D[] castResult = new RaycastHit2D[ 16 ];
        private static Collider2D[] overlapResult = new Collider2D[ 16 ];

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
            if (ko._debug)
            {
                var _name = ko.gameObject.name;
                //debug
            }

            var velocity = ko.Velocity;
            var position = ko.Position;
            var isGrounded = ko.IsGrounded;

            bool groundThisUpdate = false;

            float obstacleFraction = 1;
            bool obstacleThisUpdate = CheckWillCollideObstacleThisUpdate(ko, ko.Velocity, deltaTime, ref obstacleFraction);

            if (ko.UseGravity)
            {
                if (!isGrounded && velocity.y < 0)
                {
                    groundThisUpdate = CheckWillGroundThisUpdate(ko, ko.Velocity, deltaTime * obstacleFraction, ref position);
                    if (groundThisUpdate)
                    {
                        //position = ... (set by ref)
                        velocity.y = 0; //velocity = Vector3.zero; //?bounce
                        isGrounded = true;
                    }
                }
            }

            if (!groundThisUpdate)
            {
                var velocity25D = velocity.To25D();
                position += velocity25D * deltaTime * obstacleFraction;

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

            if (obstacleThisUpdate)
            {
                velocity.x = 0;
                velocity.z = 0;
            }
            
            ko.Velocity = velocity;
            ko.Position = position;
            ko.IsGrounded = isGrounded;
        }

        private static bool CheckStillGrounded(Kinematic25D ko)
        {
            var collider = ko.Collider2D;
            if ( collider == null || !collider.enabled )
            {
                return false;
            }

            var resCount = collider.OverlapCollider(floorFilter, overlapResult);
            for (int i = 0; i < resCount; i++)
            {
                var hitInfo = overlapResult[i];
                float altitude = GetGeometryObjectAltitude(hitInfo.gameObject);
                if (Mathf.Approximately(ko.Position.Get25Altitude(), altitude))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool CheckWillGroundThisUpdate(Kinematic25D ko, Vector3 velocity, float deltaTime, ref Vector3 position)
        {
            var collider = ko.Collider2D;
            if (collider == null || !collider.enabled)
            {
                return false;
            }

            Vector3 velocity25D = velocity.To25D();
            Vector3 nextPosition = position + velocity25D * deltaTime;

            var resCount = collider.Cast(velocity25D, floorFilter, castResult, ((Vector2)velocity25D).magnitude * deltaTime, true);
            var candidatesIndexes = new int[resCount];
            var candidatesCount = 0;

            for (int i = 0; i < resCount; i++)
            {
                var hitInfo = castResult[i];
                float floorAltitude = GetGeometryObjectAltitude(hitInfo.collider.gameObject);
                if (floorAltitude <= position.Get25Altitude() && floorAltitude >= nextPosition.Get25Altitude())
                {
                    candidatesIndexes[candidatesCount] = i;
                    candidatesCount++;
                }
            }

            for (int i = 0; i < candidatesCount; i++)
            {
                var hitInfo = castResult[candidatesIndexes[i]];
                float altitude = GetGeometryObjectAltitude(hitInfo.collider.gameObject);
                float altitudeFraction = Mathf.InverseLerp(position.Get25Altitude(), nextPosition.Get25Altitude(), altitude);
                Vector3 fractionPosition = Vector3.Lerp(position, nextPosition, altitudeFraction);
                Vector3 move = fractionPosition - position;

                var overlapped = Physics2D.OverlapAreaNonAlloc((Vector2)fractionPosition + collider.offset, collider.bounds.size, overlapResult, floorFilter.useLayerMask ? floorFilter.layerMask.value : Physics2D.DefaultRaycastLayers);
                for (int j = 0; j < overlapped; j++)
                {
                    if (overlapResult[j] == hitInfo.collider)
                    {
                        position = fractionPosition;
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool CheckWillCollideObstacleThisUpdate(Kinematic25D ko, Vector3 velocity, float deltaTime, ref float fraction)
        {
            var collider = ko.Collider2D;
            if (collider == null || !collider.enabled)
            {
                return false;
            }

            var position = ko.Position;
            Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);
            Vector3 nextPosition = position + horizontalVelocity * deltaTime;

            var resCount = collider.Cast(horizontalVelocity, obstacleFilter, castResult, horizontalVelocity.magnitude * deltaTime, true);

            for (int i = 0; i < resCount; i++)
            {
                var hitInfo = castResult[i];
                var obstacle = hitInfo.collider.gameObject.GetComponent<Obstacle25D>();//?
                if (!obstacle) 
                {
                    continue;
                }

                float obstacleAltitude = GetGeometryObjectAltitude(hitInfo.collider.gameObject);

                if (obstacleAltitude <= position.Get25Altitude() && 
                    (obstacleAltitude + obstacle.Height >= position.Get25Altitude() || obstacle.InfiniteHeight))
                {
                    fraction = hitInfo.fraction;
                    return true;
                }            
            }

            return false;
        }

        private static float GetObstacleHeight(GameObject go)
        {
            var o = go.GetComponent<Obstacle25D>();
            return o == null ? 0 : o.Height;
        }

        private static float GetGeometryObjectAltitude(GameObject go)
        {
            return go.transform.position.y;
        }

        public static bool GetNearestFloorPosition(Kinematic25D ko, out Vector3 floorPosition)
        {
            floorPosition = ko.Position;
            if (ko.IsGrounded)
            {
                return true;
            }
            if (CheckWillGroundThisUpdate(ko, Vector3.down, 666f, ref floorPosition))
            {
                return true;
            }
            return false;
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

        public static float Get25Altitude(this Vector3 vector)
        {
            return vector.y - vector.z;
        }
    }
}