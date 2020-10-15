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

        private static RaycastHit2D[] castResult = new RaycastHit2D[ 64 ];
        private static Collider2D[] overlapResult = new Collider2D[ 64 ];

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

        struct ProcessResult
        {
            public Vector3 velocity;
            public Vector3 position;
            public Vector2 fraction; //hor [x, z], vert [y]
            public bool isGrounded;
        }

        struct ProcessInfo
        {
            public string name;

            public Vector3 velocity;
            public Vector3 position;
            public bool isGrounded;

            public BoxCollider2D collider;
            public float deltaTime;
            public bool useGravity;
        }

        private static void ProcessKinematicObject(Kinematic25D ko, float deltaTime)
        {
            if (ko._debug)
            {
                var _name = ko.gameObject.name;
                //debug
            }

            ProcessResult OUT = ProcessKinematicObjectImpl(ko, deltaTime);
            if (!Mathf.Approximately(OUT.fraction.x, OUT.fraction.y))
            {
                var secondPhaseDeltaTime = deltaTime * (1 - Mathf.Min(OUT.fraction.x, OUT.fraction.y));
                OUT = ProcessKinematicObjectImpl(ko, secondPhaseDeltaTime);
            }
        }

        private static ProcessResult ProcessKinematicObjectImpl(Kinematic25D ko, float deltaTime)
        {
            ProcessInfo IN = CreateProcessKinematicObjectObjectInfo(ko, deltaTime);
            ProcessKinematicObject(IN, out ProcessResult OUT);
            LoadProcessResultToObject(ko, OUT);
            return OUT;
        }

        private static void LoadProcessResultToObject(Kinematic25D ko, ProcessResult OUT)
        {
            ko.Velocity = OUT.velocity;
            ko.Position = OUT.position;
            ko.IsGrounded = OUT.isGrounded;
            //fraction ?
        }

        private static ProcessInfo CreateProcessKinematicObjectObjectInfo(Kinematic25D ko, float deltaTime)
        {
            return new ProcessInfo()
            {
                name = ko.name,//debug

                velocity = ko.Velocity,
                position = ko.Position,
                isGrounded = ko.IsGrounded,

                deltaTime = deltaTime,
                collider = ko.Collider2D != null && ko.Collider2D.enabled ? ko.Collider2D : null,
                useGravity = ko.UseGravity,
            };
        }

        private static void ProcessKinematicObject(ProcessInfo IN, out ProcessResult OUT) //result: fraction
        {
            OUT = new ProcessResult();

            OUT.fraction = CalculateFraction(ref IN);
            ChangeVelocityAndPosition(ref IN, ref OUT);
            CheckIsGrounded(ref IN, ref OUT);
            CheckGravity(ref IN, ref OUT);
        }

        private static Vector2 CalculateFraction(ref ProcessInfo IN)
        {
            float obstacleFraction = 
                IN.collider != null 
                && (!Mathf.Approximately(IN.velocity.x, 0) || !Mathf.Approximately(IN.velocity.z, 0))
                ? CheckCollideObstacle(ref IN)
                : 1;

            float floorFraction =
                IN.collider != null
                && IN.useGravity
                && !IN.isGrounded
                && IN.velocity.y < 0
                ? CheckCollideFloor(ref IN)
                : 1;

            return new Vector3(obstacleFraction, floorFraction);
        }

        private static void CheckGravity(ref ProcessInfo IN, ref ProcessResult OUT)
        {
            if (!OUT.isGrounded && IN.useGravity)
            {
                OUT.velocity += Vector3.down * Gravity * IN.deltaTime;
            }

            if (OUT.isGrounded)
            {
                OUT.velocity.y = 0;
            }
        }

        private static void CheckIsGrounded(ref ProcessInfo IN, ref ProcessResult OUT)
        {
            if (IN.collider != null)
            {
                OUT.isGrounded = CheckStillGrounded(IN.collider, IN.position);
            }
            else
            {
                OUT.isGrounded = false;
            }
        }

        private static void ChangeVelocityAndPosition(ref ProcessInfo IN, ref ProcessResult OUT)
        {
            var fraction = Mathf.Min(OUT.fraction.x, OUT.fraction.y);

            OUT.position = IN.position + IN.velocity.To25D() * fraction * IN.deltaTime;

            if (OUT.fraction.x.LessThenOne())
            {
                OUT.velocity.x = 0;
                OUT.velocity.z = 0;
            }

            if (OUT.fraction.y.LessThenOne())
            {
                OUT.velocity.y = 0;
            }
        }

        private static bool CheckStillGrounded(BoxCollider2D collider, Vector3 position)
        {
            var resCount = collider.OverlapCollider(floorFilter, overlapResult);
            for (int i = 0; i < resCount; i++)
            {
                var hitInfo = overlapResult[i];
                float altitude = GetGeometryObjectAltitude(hitInfo.gameObject);
                if (Mathf.Approximately(position.Get25Altitude(), altitude))
                {
                    return true;
                }
            }

            return false;
        }

        private static float CheckCollideFloor(ref ProcessInfo IN)
        {
            Vector3 velocity25D = IN.velocity.To25D();
            Vector3 nextPosition = IN.position + velocity25D * IN.deltaTime;

            var resCount = IN.collider.Cast(velocity25D, floorFilter, castResult, ((Vector2)velocity25D).magnitude * IN.deltaTime, true);
            var candidatesIndexes = new int[resCount];
            var candidatesCount = 0;

            for (int i = 0; i < resCount; i++)
            {
                var hitInfo = castResult[i];
                float floorAltitude = GetGeometryObjectAltitude(hitInfo.collider.gameObject);
                if (floorAltitude <= IN.position.Get25Altitude() && floorAltitude >= nextPosition.Get25Altitude())
                {
                    candidatesIndexes[candidatesCount] = i;
                    candidatesCount++;
                }
            }

            for (int i = 0; i < candidatesCount; i++)
            {
                var hitInfo = castResult[candidatesIndexes[i]];
                float altitude = GetGeometryObjectAltitude(hitInfo.collider.gameObject);
                float altitudeFraction = Mathf.InverseLerp(IN.position.Get25Altitude(), nextPosition.Get25Altitude(), altitude);
                Vector3 fractionPosition = Vector3.Lerp(IN.position, nextPosition, altitudeFraction);
                Vector3 move = fractionPosition - IN.position;

                var overlapped = Physics2D.OverlapAreaNonAlloc((Vector2)fractionPosition + IN.collider.offset, IN.collider.bounds.size, overlapResult, floorFilter.useLayerMask ? floorFilter.layerMask.value : Physics2D.DefaultRaycastLayers);
                for (int j = 0; j < overlapped; j++)
                {
                    if (overlapResult[j] == hitInfo.collider)
                    {
                        return hitInfo.fraction;
                    }
                }
            }

            return 1;
        }

        private static float CheckCollideObstacle(ref ProcessInfo IN)
        {
            var fraction = 1.0f;
            Vector3 horizontalVelocity = new Vector3(IN.velocity.x, 0, IN.velocity.z);

            var resCount = IN.collider.Cast(horizontalVelocity, obstacleFilter, castResult, horizontalVelocity.magnitude * IN.deltaTime, true);

            for (int i = 0; i < resCount; i++)
            {
                var hitInfo = castResult[i];
                var obstacle = hitInfo.collider.gameObject.GetComponent<Obstacle25D>();//?
                if (!obstacle) 
                {
                    continue;
                }

                float obstacleAltitude = GetGeometryObjectAltitude(hitInfo.collider.gameObject);

                if (obstacleAltitude <= IN.position.Get25Altitude() && 
                    (obstacleAltitude + obstacle.Height >= IN.position.Get25Altitude() || obstacle.InfiniteHeight))
                {
                    var obstacleFraction= CheckCollideObstacle(ref IN, hitInfo.collider);
                    fraction = Mathf.Min(fraction, obstacleFraction);
                }
            }

            return fraction;
        }

        private static float CheckCollideObstacle(ref ProcessInfo IN, Collider2D obstacle)
        {
            Vector3 horizontalVelocity = new Vector3(IN.velocity.x, 0, IN.velocity.z);

            float obstacleAltitude = GetGeometryObjectAltitude(obstacle.gameObject);
            float altitude = IN.position.Get25Altitude();
            float altitudeDelta = altitude - obstacleAltitude;

            RaycastHit2D[] castResultTemp = new RaycastHit2D[64];

            var resCount = Physics2D.BoxCast(IN.collider.bounds.center - Vector3.down * altitudeDelta, IN.collider.size, 0, horizontalVelocity, obstacleFilter, castResultTemp, horizontalVelocity.magnitude * IN.deltaTime);
            for (int i = 0; i < resCount; i++)
            {
                var hitInfo = castResult[i];
                if (hitInfo.collider == obstacle)
                {
                    return hitInfo.fraction;
                }
            }
            return 1;
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
            var IN = CreateProcessKinematicObjectObjectInfo(ko, 666);
            if (!CheckCollideFloor(ref IN).LessThenOne())
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

        public static bool LessThenOne(this float val)
        {
            return val < 1 && !Mathf.Approximately(1, val);
        }
    }
}