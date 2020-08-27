using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _3d
{
    public class _TestPlayerInput_ : MonoBehaviour
    {
        [SerializeField] Kinematic kinematic;
        [SerializeField] Vector2 speed;
        [SerializeField] float flySpeed;
        [SerializeField] float jumpForce;
        [SerializeField] float strikeForce;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                kinematic.Jump(jumpForce);
            }
            else
            {
                var moveVector = new Vector2(
                    Input.GetAxis("Horizontal"),
                    Input.GetAxis("Vertical"));
                kinematic.Move(moveVector, speed);

                if (moveVector.sqrMagnitude < float.Epsilon && flySpeed > float.Epsilon)
                {
                    Fly();
                }

                Strike();
            }
        }

        private void Strike()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                kinematic.Strike(Vector2.left, strikeForce);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                kinematic.Strike(Vector2.right, strikeForce);
        }

        private void Fly()
        {
            var flyVector = Vector2.zero;

            if (Input.GetKey(KeyCode.Q))
                flyVector -= Vector2.up;
            else if (Input.GetKey(KeyCode.E))
                flyVector += Vector2.up;

            kinematic.Fly(flyVector, flySpeed);
        }
    }
}