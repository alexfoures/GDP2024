using UnityEngine;

namespace Coco
{
    public class GravityEffector : MovementEffector
    {
        public float Gravity = 20.0f;

        public override void Apply(in PlayerMovementContext context, ref Vector2 velocity)
        {
            velocity.y -= Gravity * Time.fixedDeltaTime;
        }
    }
}
