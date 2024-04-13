using UnityEngine;

namespace Coco
{
    public class CollisionState
    {
        public BoolTracker IsGrounded;
        public BoolTracker IsAgainstWall;
        public BoolTracker IsAgainstCeiling;

        public Vector2 GroundNormal;
        public Vector2 GroundTangent;
        public Vector2 WallNormal;
        public Vector2 WallUpward;

        public void Sync(CharacterController2D controller)
        {
            IsGrounded.Value = controller.IsGrounded;
            IsAgainstWall.Value = controller.IsAgainstWall;
            IsAgainstCeiling.Value = controller.IsAgainstCeiling;

            GroundNormal = controller.GroundNormal;
            GroundTangent = controller.GroundTangent;
            WallNormal = controller.WallNormal;
            WallUpward = controller.WallUpward;
        }

        public static CollisionState Create() => new CollisionState()
        {
            IsGrounded = BoolTracker.Create(),
            IsAgainstWall = BoolTracker.Create(),
            IsAgainstCeiling = BoolTracker.Create()
        };
    }

}