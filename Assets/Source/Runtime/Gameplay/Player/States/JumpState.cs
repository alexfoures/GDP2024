using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coco
{
    public class JumpState : StateMachineBehaviour
    {
        //public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    var playerMovement = animator.GetComponent<PlayerMovement>();
        //    var isGrounded = playerMovement.CharacterController.IsGrounded;
        //    var isAgainstWall = playerMovement.CharacterController.IsAgainstWall;
        //    var velocity = playerMovement.CharacterController.Velocity;

        //    velocity.y = playerMovement.JumpHeight * JumpCurve(ref stateInfo);

        //    if (!isGrounded && isAgainstWall)
        //    {
        //        var direction = Mathf.Sign(playerMovement.CharacterController.WallNormal.x);
        //        velocity.x = direction * playerMovement.WallJumpForce;
        //    }

        //    playerMovement.CanJump = false;
        //    playerMovement.SetVelocity(velocity);
        //}

        //public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Setup a constant velocity over animation.
        //    var playerMovement = animator.GetComponent<PlayerMovement>();
        //    var velocity = playerMovement.CharacterController.Velocity;
        //    velocity.y = playerMovement.JumpHeight * JumpCurve(ref stateInfo);

        //    playerMovement.SetVelocity(velocity);
        //}

        //public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    var playerMovement = animator.GetComponent<PlayerMovement>();
        //}

        //private static float JumpCurve(ref AnimatorStateInfo stateInfo, float curveParameter = 1.5f)
        //{
        //    float t = stateInfo.normalizedTime % 1.0f;
        //    float value = curveParameter * Mathf.Pow(1.0f - t, curveParameter - 1.0f);
        //    return value * stateInfo.speed;
        //}
    }
}