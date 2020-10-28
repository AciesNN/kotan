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
                if ( ko== null || !ko.enabled || !ko.gameObject.activeInHierarchy)
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
            public Vector2 maxMovement; //hor [x, z], vert [y]
            public bool isGrounded;

            public override string ToString() => JsonUtility.ToJson(this);
        }

        struct ProcessInfo
        {
            public bool _debug;
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
                if (ko.gameObject.transform.localPosition.x > -3.0f)
                {
                    int i = 0;
                }
            }

            ProcessResult OUT = ProcessKinematicObjectPhase(ko, deltaTime);
            if (!Mathf.Approximately(OUT.maxMovement.x, OUT.maxMovement.y)
                && OUT.velocity.sqrMagnitude > 0)
            {
                var firstPhaseDeltaTime = Mathf.Min(OUT.maxMovement.x, OUT.maxMovement.y);
                var secondPhaseDeltaTime = deltaTime * (1 - firstPhaseDeltaTime);
                OUT = ProcessKinematicObjectPhase(ko, secondPhaseDeltaTime);
            }
        }

        private static ProcessResult ProcessKinematicObjectPhase(Kinematic25D ko, float deltaTime)
        {
            ProcessInfo IN = CreateProcessInfo(ko, deltaTime);
            ProcessResult OUT = ProcessKinematicObject(IN);
            LoadResultToObject(ko, ref OUT);
            return OUT;
        }

        private static void LoadResultToObject(Kinematic25D ko, ref ProcessResult OUT)
        {
            ko.Velocity = OUT.velocity; 
            ko.Position = OUT.position;
            ko.IsGrounded = OUT.isGrounded;
        }

        private static ProcessInfo CreateProcessInfo(Kinematic25D ko, float deltaTime)
        {
            return new ProcessInfo()
            {
                name = ko.name,//debug
                _debug = ko._debug,

                velocity = ko.Velocity,
                position = ko.Position,
                isGrounded = ko.IsGrounded,

                deltaTime = deltaTime,
                collider = ko.Collider2D != null && ko.Collider2D.enabled ? ko.Collider2D : null,
                useGravity = ko.UseGravity,
            };
        }

        private static ProcessResult ProcessKinematicObject(ProcessInfo IN)
        {
            var OUT = new ProcessResult();

            OUT.maxMovement = GetMaxMovement(ref IN);
            SetVelocityAndPosition(ref IN, ref OUT);
            CheckIsGrounded(ref IN, ref OUT);
            CheckGravity(ref IN, ref OUT);

            return OUT;
        }

        private static Vector2 GetMaxMovement(ref ProcessInfo IN)
        {
            var checkCollide = IN.collider != null;

            var checkHorizontal = checkCollide 
                && (!Mathf.Approximately(IN.velocity.x, 0) || !Mathf.Approximately(IN.velocity.z, 0));
            float maxHorizontalMovement = checkHorizontal ? GetMaxHorizontalMovement( ref IN) : 1;

            var _ = Vector3.zero;
            var checkVertical = checkCollide && IN.useGravity && !IN.isGrounded && IN.velocity.y < 0;
            float maxVerticalMovement = checkVertical ? GetMaxVerticalMovement( ref IN, ref _) : 1;

            return new Vector3( maxHorizontalMovement, maxVerticalMovement );
        }

        private static void CheckGravity(ref ProcessInfo IN, ref ProcessResult OUT)
        {
            if (!OUT.isGrounded && IN.useGravity)
            {
                OUT.velocity = IN.velocity + Vector3.down * Gravity * IN.deltaTime;
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
                OUT.isGrounded = CheckStillGrounded(ref IN, ref OUT);
            }
            else
            {
                OUT.isGrounded = false;
            }
        }

        private static void SetVelocityAndPosition(ref ProcessInfo IN, ref ProcessResult OUT)
        {
            var maxMovement = Mathf.Min(OUT.maxMovement.x, OUT.maxMovement.y);

            OUT.velocity = IN.velocity;
            OUT.position = IN.position + OUT.velocity.To25D() * maxMovement * IN.deltaTime;

            if (OUT.maxMovement.x.LessThenOne())
            {
                OUT.velocity.x = 0;
                OUT.velocity.z = 0;
            }

            if (OUT.maxMovement.y.LessThenOne())
            {
                OUT.velocity.y = 0;
            }
        }

        private static bool CheckStillGrounded(ref ProcessInfo IN, ref ProcessResult OUT)
        {
            var position = OUT.position - IN.position + IN.collider.bounds.center;
            var resCount = Physics2D.OverlapBox(position, IN.collider.bounds.size, 0, floorFilter, overlapResult);
            for (int i = 0; i < resCount; i++)
            {
                var hitInfo = overlapResult[i];
                if (hitInfo.gameObject == IN.collider.gameObject)
                {
                    continue;
                }
                float altitude = GetGeometryObjectAltitude(hitInfo.gameObject);
                if (Mathf.Approximately(OUT.position.Get25Altitude(), altitude))
                {
                    return true;
                }
            }

            return false;
        }

        //return fracton
        private static float GetMaxVerticalMovement( ref ProcessInfo IN, ref Vector3 floorPosition)
        {
            Vector3 velocity25D = IN.velocity.To25D();
            Vector3 nextPosition = IN.position + velocity25D * IN.deltaTime;
            float colliderSizeDelta = IN.collider.bounds.size.y / 2;

            var resCount = Physics2D.BoxCast/*NonAlloc*/(IN.collider.bounds.center, IN.collider.bounds.size, 0, velocity25D, floorFilter, castResult, ((Vector2)velocity25D).magnitude * IN.deltaTime);
            var candidatesIndexes = new int[resCount];
            var candidatesCount = 0;

            for (int i = 0; i < resCount; i++)
            {
                var hitInfo = castResult[i];
                if (hitInfo.collider == IN.collider)
                {
                    continue;
                }
                float floorAltitude = GetGeometryObjectAltitude(hitInfo.collider.gameObject);
                if (floorAltitude.LessOrApproximatelyEqualThan(IN.position.Get25Altitude())
                    && floorAltitude.GreaterOrApproximatelyEqualThan(nextPosition.Get25Altitude()))
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
                        floorPosition = fractionPosition;
                        return altitudeFraction;
                    }
                }
            }

            floorPosition = nextPosition;
            return 1;
        }

        private static float GetMaxHorizontalMovement( ref ProcessInfo IN)
        {
            var fraction = 1.0f;
            Vector3 horizontalVelocity = new Vector3(IN.velocity.x, 0, IN.velocity.z);

            var resCount = Physics2D.BoxCast/*NonAlloc*/(IN.collider.bounds.center, IN.collider.size, 0, horizontalVelocity, obstacleFilter, castResult, horizontalVelocity.magnitude * IN.deltaTime);

            for (int i = 0; i < resCount; i++)
            {
                var hitInfo = castResult[i];
                if (hitInfo.collider == IN.collider)
                {
                    continue;
                }
                var obstacle = hitInfo.collider.gameObject.GetComponent<Obstacle25D>();//?
                if (!obstacle) 
                {
                    continue;
                }

                float obstacleAltitude = GetGeometryObjectAltitude(hitInfo.collider.gameObject);

                if (obstacleAltitude <= IN.position.Get25Altitude() && 
                    (obstacleAltitude + obstacle.Height >= IN.position.Get25Altitude() || obstacle.InfiniteHeight))
                {
                    var obstacleFraction= GetMaxHorizontalMovement( ref IN, hitInfo.collider);
                    fraction = Mathf.Min(fraction, obstacleFraction);
                }
            }

            return fraction;
        }

        private static float GetMaxHorizontalMovement( ref ProcessInfo IN, Collider2D obstacle)
        {
            Vector3 horizontalVelocity = new Vector3(IN.velocity.x, 0, IN.velocity.z);

            float obstacleAltitude = GetGeometryObjectAltitude(obstacle.gameObject);
            float altitude = IN.position.Get25Altitude();
            float altitudeDelta = altitude - obstacleAltitude;

            RaycastHit2D[] castResultTemp = new RaycastHit2D[64];

            var resCount = Physics2D.BoxCast/*NonAlloc*/(IN.collider.bounds.center - Vector3.down * altitudeDelta, IN.collider.size, 0, horizontalVelocity, obstacleFilter, castResultTemp, horizontalVelocity.magnitude * IN.deltaTime);
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

            var IN = CreateProcessInfo(ko, 666);
            IN.velocity = Vector3.down;
            var fraction = GetMaxVerticalMovement( ref IN, ref floorPosition);
            if (fraction.LessThenOne())
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

        public static bool GreaterOrApproximatelyEqualThan(this float val, float compare)
        {
            return val > compare || Mathf.Approximately(val, compare);
        }

        public static bool LessOrApproximatelyEqualThan(this float val, float compare)
        {
            return val < compare || Mathf.Approximately(val, compare);
        }
    }
}