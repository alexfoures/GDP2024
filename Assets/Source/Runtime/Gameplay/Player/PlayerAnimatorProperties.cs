using UnityEngine;

namespace Coco
{
    public static class PlayerAnimatorProperties
    {
        public static int HorizontalVelocity = Animator.StringToHash("HorizontalVelocity");
        public static int VerticalVelocity = Animator.StringToHash("VerticalVelocity");
        public static int IsGrounded = Animator.StringToHash("IsGrounded");
        public static int IsAgainstWall = Animator.StringToHash("IsAgainstWall");
        public static int Attack = Animator.StringToHash("Attack");
        public static int Jump = Animator.StringToHash("Jump");
        public static int Dash = Animator.StringToHash("Dash");
    }
}