using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Coco
{
    public class JumpEffector : MovementEffector
    {
        public float JumpHeight = 5.0f;
        public float JumpDuration = 0.7f;
        public float WallHorizontalForce = 10.0f;
        public float JumpEarlyTolerance = 0.2f;
        public float JumpLateTolerance = 0.2f;

        public bool CanWallJump = true;

        [NonSerialized]
        public BoolTracker isJumping = BoolTracker.Create();

        private static float JumpCurve(float time, float curveParameter = 1.5f)
        {
            return curveParameter * Mathf.Pow(1.0f - time, curveParameter - 1.0f);
        }

        public override void Apply(in PlayerMovementContext context, ref Vector2 velocity)
        {
            var collisionState = context.collisionState;
            var inputState = context.playerInputState;

            bool isGroundedStrict = collisionState.IsGrounded.Value;
            bool isGroundedTolerant = collisionState.IsGrounded.GetValueWithTolerance(JumpLateTolerance);
            bool jumpInputStrict = inputState.WasJumpPressedThisFrame.Value;
            bool jumpInputTolerant = inputState.WasJumpPressedThisFrame.GetValueWithTolerance(JumpEarlyTolerance);

            bool isAgainstWall = collisionState.IsAgainstWall.Value;

            // Regular Jump
            if (!isJumping.Value && (isGroundedStrict && jumpInputTolerant) || (isGroundedTolerant && jumpInputStrict))
            {
                isJumping.Value = true;
            }

            // Wall Jump
            if (CanWallJump)
            {
                if (isAgainstWall && jumpInputStrict && !isGroundedStrict)
                {
                    var direction = Mathf.Sign(context.collisionState.WallNormal.x);
                    velocity.x = direction * WallHorizontalForce;
                    isJumping.Value = true;
                }
            }

            // Jump Update
            if (isJumping.Value)
            {
                double jumpDuration = Time.timeAsDouble - isJumping.LastTimeChanged;
                if (jumpDuration < JumpDuration && inputState.Jump.Value)
                {
                    velocity.y = JumpHeight / JumpDuration * JumpCurve((float)jumpDuration / JumpDuration);
                }
                else
                {
                    isJumping.Value = false;
                }
            }
        }
    }
}
