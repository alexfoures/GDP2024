using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Coco
{
    public class DashEffector : MovementEffector
    {
        public float DashDuration = 0.5f;
        public float DashDistance = 5.0f;
        public float DashCooldown = 1.5f;
        public float DashEarlyTolerance = 0.1f;
        public float GroundStickingVelocity = 1.0f;
        public float MovementRetrievalVelocity = 8.0f;

        [NonSerialized] public BoolTracker isDashing = BoolTracker.Create();
        [NonSerialized] public Vector2 dashDirection = Vector2.zero;

        private static float DashCurve(float time, float curveParameter = 4.0f)
        {
            return curveParameter * Mathf.Pow(1.0f - time, curveParameter - 1.0f);
        }

        public override void Apply(in PlayerMovementContext context, ref Vector2 velocity)
        {
            var collisionState = context.collisionState;
            var inputState = context.playerInputState;

            bool dashInputTolerant = inputState.WasDashPressedThisFrame.GetValueWithTolerance(DashEarlyTolerance);
            bool dashCooldownCompleted = (Time.timeAsDouble - isDashing.LastTimeChanged) >= DashCooldown && !isDashing.Value;

            bool hasMovementInput = !Mathf.Approximately(inputState.HorizontalMovement, 0.0f);

            if (dashCooldownCompleted && dashInputTolerant)
            {
                bool isAgainstWallNotGrounded = collisionState.IsAgainstWall.Value && !collisionState.IsGrounded.Value;
                float direction = (isAgainstWallNotGrounded ? Mathf.Sign(collisionState.WallNormal.x) : (hasMovementInput ? Mathf.Sign(inputState.HorizontalMovement) : context.GetFacingDirection()));
                dashDirection = Vector2.right * direction;
                isDashing.Value = true;
            }

            // Dash Update
            if (isDashing.Value)
            {
                double dashDuration = Time.timeAsDouble - isDashing.LastTimeChanged;
                if (dashDuration < DashDuration)
                {
                    velocity = dashDirection * DashDistance / DashDuration * DashCurve((float)dashDuration / DashDuration, 1.0f);
                }
                else
                {
                    isDashing.Value = false;
                    if (hasMovementInput)
                    {
                        velocity = Vector2.right * Mathf.Sign(inputState.HorizontalMovement) * MovementRetrievalVelocity;
                    }
                    else
                    {
                        velocity = Vector2.zero;
                    }
                }

                if (collisionState.IsGrounded.Value)
                {
                    velocity.y += -GroundStickingVelocity;
                }

                if (context.TryGetMovementEffector<JumpEffector>(out var jumpEffector))
                {
                    jumpEffector.isJumping.Value = false;
                }
            }
        }
    }
}
