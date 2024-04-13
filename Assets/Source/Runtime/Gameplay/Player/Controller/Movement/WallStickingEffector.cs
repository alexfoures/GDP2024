using UnityEngine;

namespace Coco
{
    public class WallStickingEffector : MovementEffector
    {
        public float WallStickingForce = 5.0f;

        public override void Apply(in PlayerMovementContext context, ref Vector2 velocity)
        {
            bool isAgainstWall = context.collisionState.IsAgainstWall.Value;
            bool isGrounded = context.collisionState.IsGrounded.Value;
            var inputMovement = context.playerInputState.HorizontalMovement;

            // Wall sticking
            if (isAgainstWall)
            {
                float wallDirection = Mathf.Sign(context.collisionState.WallNormal.x);
                float moveDirection = Mathf.Sign(velocity.x);

                bool hasInput = !Mathf.Approximately(0.0f, inputMovement);
                float inputDirection = Mathf.Sign(inputMovement);


                if (hasInput && inputDirection == wallDirection)
                {
                    return;
                }

                // Apply sticky velocity when we try to go through a wall.
                if (moveDirection != wallDirection)
                {
                    if (isGrounded)
                    {
                        velocity.x = -0.01f * wallDirection;
                    }
                    else
                    {
                        velocity.x = -WallStickingForce * wallDirection;
                    }
                }
            }
        }
    }
}
