using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Coco
{
    [RequireComponent(typeof(Animator))]
    public class PlayerVisual : MonoBehaviour
    {
        public PlayerController Controller;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnAttack()
        {
            _animator.SetTrigger(PlayerAnimatorProperties.Attack);
        }

        private void OnEnable()
        {
            Controller.OnAttack += OnAttack;
        }

        private void OnDisable()
        {
            Controller.OnAttack -= OnAttack;
        }

        private void FixedUpdate()
        {
            UpdateAnimator();
            SetFacing(Controller.facing);
        }

        public void SetFacing(Facing facing)
        {
            _spriteRenderer.flipX = facing == Facing.Left;
        }

        private void UpdateAnimator()
        {
            var velocity = Controller.CharacterController.Velocity;
            var isGrounded = Controller.collisionState.IsGrounded.Value;
            var isAgainstWall = Controller.collisionState.IsAgainstWall.Value;

            _animator.SetFloat(PlayerAnimatorProperties.HorizontalVelocity, Mathf.Abs(velocity.x));
            _animator.SetFloat(PlayerAnimatorProperties.VerticalVelocity, velocity.y);
            _animator.SetBool(PlayerAnimatorProperties.IsGrounded, isGrounded);
            _animator.SetBool(PlayerAnimatorProperties.IsAgainstWall, isAgainstWall);

            if (Controller.TryGetMovementEffector<JumpEffector>(out var jumpEffector))
            {
                var isJumping = jumpEffector.enabled && jumpEffector.isJumping.Value;
                _animator.SetBool(PlayerAnimatorProperties.Jump, isJumping);
            }

            if (Controller.TryGetMovementEffector<DashEffector>(out var dashEffector))
            {
                var isDashing = dashEffector.enabled && dashEffector.isDashing.Value;
                _animator.SetBool(PlayerAnimatorProperties.Dash, isDashing);
            }
        }
    }

}
