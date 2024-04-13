using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Coco
{
    public class GroundStickingEffector : MovementEffector
    {
        public float GroundStickingVelocity = 1.0f;

        public override void Apply(in PlayerMovementContext context, ref Vector2 velocity)
        {
            // Ceiling.
            if (context.collisionState.IsAgainstCeiling.Value)
            {
                if (context.TryGetMovementEffector<JumpEffector>(out var jumpEffector))
                {
                    if (jumpEffector.isJumping.Value)
                    {
                        jumpEffector.isJumping.Value = false;
                        velocity.y = 0.0f;
                    }
                }
            }

            if (velocity.y < 0.0f && context.collisionState.IsGrounded.Value)
            {
                velocity.y = -GroundStickingVelocity;
            }
        }
    }
}
