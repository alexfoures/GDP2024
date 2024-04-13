using UnityEngine;

namespace Coco
{
    public class BasicMovementEffector : MovementEffector
    {
        public float MovementSpeed = 8.0f;
        public float HorizontalAcceleration = 20.0f;
        public bool CanMove = true;

        public override void Apply(in PlayerMovementContext context, ref Vector2 velocity)
        {
            var inputMovement = context.playerInputState.HorizontalMovement;
            bool isGrounded = context.collisionState.IsGrounded.Value;

            if (CanMove)
            {
                // To make the gameplay more responsive, when the player change direction, we reset its horizontal velocity to 0.
                bool playerChangedDirection = Mathf.Sign(inputMovement) != Mathf.Sign(velocity.x);
                if (isGrounded && playerChangedDirection)
                {
                    velocity.x = Mathf.MoveTowards(0.0f, inputMovement * MovementSpeed, HorizontalAcceleration * Time.fixedDeltaTime);
                }
                else
                {
                    velocity.x = Mathf.MoveTowards(velocity.x, inputMovement * MovementSpeed, HorizontalAcceleration * Time.fixedDeltaTime);
                }
            }
            else
            {
                velocity.x = Mathf.MoveTowards(velocity.x, 0.0f, HorizontalAcceleration * Time.fixedDeltaTime);
            }
        }
    }
}
