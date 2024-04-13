using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Coco
{
    [RequireComponent(typeof(CharacterController2D), typeof(PlayerInputState))]
    public class PlayerMovement : MonoBehaviour
    {

        [Header("General Movement Settings")]
        public float MovementSpeed = 8.0f;
        public float HorizontalAcceleration = 20.0f;
        public float Gravity = 20.0f;
        public float AttackCooldown = 0.5f;

        [Header("Abilities")]
        public bool CanDash = true;
        public bool CanWallJump = true;

        [Header("Jump")]
        public float JumpHeight = 5.0f;
        public float JumpDuration = 0.7f;
        public float WallHorizontalForce = 10.0f;
        public float JumpEarlyTolerance = 0.2f;
        public float JumpLateTolerance = 0.2f;

        [Header("Dash")]
        public float DashDuration = 0.5f;
        public float DashDistance = 5.0f;
        public float DashCooldown = 1.5f;
        public float DashEarlyTolerance = 0.1f;

        [Header("Advanced Movement Settings")]
        public float MinimumVerticalVelocity = -20.0f;
        public float MaximumVerticalVelocity = 20.0f;
        public float GroundStickingVelocity = 1.0f;

        public bool CanMove = true;
        public bool IsKinematic = false;
        public bool CanJump = false;

        private CharacterController2D _characterController2D = null;
        private PlayerInputState _input = null;
        private Animator _animator = null;
        private SpriteRenderer _spriteRenderer = null;

        private Facing _facing;
        private float _attackCooldownTimer = 0.0f;

        public CollisionState _collisionState = CollisionState.Create();
        private BoolTracker _isJumping = BoolTracker.Create();
        private BoolTracker _isDashing = BoolTracker.Create();
        private Vector2 _dashDirection = Vector2.zero;

        public CharacterController2D CharacterController => _characterController2D;

        private void Start()
        {
            _characterController2D = GetComponent<CharacterController2D>();
            _input = GetComponent<PlayerInputState>();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            if (!IsKinematic)
            {
                UpdateMovement();
                UpdateFacing();
            }
            UpdateAnimator();

            HandleAttack();
            _input.Consume();
        }

        private void HandleAttack()
        {
            if (_attackCooldownTimer < AttackCooldown)
            {
                _attackCooldownTimer += Time.fixedDeltaTime;
            }

            if (_input.WasAttackPressedThisFrame.Value)
            {
                if (_attackCooldownTimer >= AttackCooldown && CanMove)
                {
                    _animator.SetTrigger(PlayerAnimatorProperties.Attack);
                    _attackCooldownTimer = 0.0f;

                    //// Instant turn when attacking.
                    //if (Mathf.Abs(_inputMovement.x) > 0.2f)
                    //{
                    //    SetFacing(_inputMovement.x < 0.0f ? Facing.Left : Facing.Right);
                    //}
                }
            }
        }

        public void SetFacing(Facing facing)
        {
            _facing = facing;
            _spriteRenderer.flipX = facing == Facing.Left;
        }

        public float GetFacingDirection()
        {
            return _facing == Facing.Right ? 1.0f : -1.0f;
        }

        private void UpdateAnimator()
        {
            _animator.SetFloat(PlayerAnimatorProperties.HorizontalVelocity, Mathf.Abs(_characterController2D.Velocity.x));
            _animator.SetFloat(PlayerAnimatorProperties.VerticalVelocity, _characterController2D.Velocity.y);
            _animator.SetBool(PlayerAnimatorProperties.IsGrounded, _characterController2D.IsGrounded);
            _animator.SetBool(PlayerAnimatorProperties.IsAgainstWall, _characterController2D.IsAgainstWall);

            _animator.SetBool(PlayerAnimatorProperties.Jump, _isJumping.Value);
            _animator.SetBool(PlayerAnimatorProperties.Dash, _isDashing.Value);
        }

        private void UpdateMovement()
        {
            Vector2 velocity = _characterController2D.Velocity;

            HandleGravity(ref velocity);
            HandleCollisions(ref velocity);
            HandleBasicMovement(ref velocity);
            HandleWallMovement(ref velocity);
            HandleJump(ref velocity);
            if (CanDash)
            {
                HandleDash(ref velocity);
            }

            velocity.y = Mathf.Clamp(velocity.y, MinimumVerticalVelocity, MaximumVerticalVelocity);

            _characterController2D.Velocity = velocity;
            _characterController2D.Move(Time.fixedDeltaTime);
            _collisionState.Sync(_characterController2D);
        }

        private void UpdateFacing()
        {
            bool isAgainstWall = _collisionState.IsAgainstWall.Value;
            bool isGrounded = _collisionState.IsGrounded.Value;
            Vector2 velocity = _characterController2D.Velocity;

            if (isAgainstWall && !isGrounded)
            {
                SetFacing(_characterController2D.WallNormal.x < 0.0 ? Facing.Left : Facing.Right);
            }
            else if (Mathf.Abs(velocity.x) > 0.2f)
            {
                SetFacing(velocity.x < 0.0f ? Facing.Left : Facing.Right);
            }
        }

        private void HandleGravity(ref Vector2 velocity)
        {
            velocity.y -= Gravity * Time.fixedDeltaTime;
        }

        private void HandleCollisions(ref Vector2 velocity)
        {
            //// Ceiling.
            //if (_collisionState.IsAgainstCeiling.Value)
            //{
            //    _isJumping.Value = false;
            //}

            if (velocity.y < 0.0f && _collisionState.IsGrounded.Value)
            {
                velocity.y = -GroundStickingVelocity;
            }
        }

        private void HandleBasicMovement(ref Vector2 velocity)
        {
            bool isGrounded = _collisionState.IsGrounded.Value;

            if (CanMove)
            {
                // To make the gameplay more responsive, when the player change direction, we reset its horizontal velocity to 0.
                bool playerChangedDirection = Mathf.Sign(_input.HorizontalMovement) != Mathf.Sign(velocity.x);
                if (isGrounded && playerChangedDirection)
                {
                    velocity.x = Mathf.MoveTowards(0.0f, _input.HorizontalMovement * MovementSpeed, HorizontalAcceleration * Time.fixedDeltaTime);
                }
                else
                {
                    velocity.x = Mathf.MoveTowards(velocity.x, _input.HorizontalMovement * MovementSpeed, HorizontalAcceleration * Time.fixedDeltaTime);
                }
            }
            else
            {
                velocity.x = Mathf.MoveTowards(velocity.x, 0.0f, HorizontalAcceleration * Time.fixedDeltaTime);
            }
        }

        private void HandleWallMovement(ref Vector2 velocity)
        {
            bool isAgainstWall = _collisionState.IsAgainstWall.Value;

            // Wall sticking
            if (isAgainstWall)
            {
                float wallDirection = Mathf.Sign(_characterController2D.WallNormal.x);
                float moveDirection = Mathf.Sign(velocity.x);

                // Reset velocity when we try to go through a wall.
                if (moveDirection != wallDirection)
                {
                    velocity.x = 0.0f;
                }

                // Stick to the wall when we don't move.
                if (Mathf.Abs(velocity.x) < 0.01f)
                {
                    velocity.x = -0.01f * wallDirection;
                }
            }
        }

        private void HandleJump(ref Vector2 velocity)
        {
            bool isGroundedStrict = _collisionState.IsGrounded.Value;
            bool isGroundedTolerant = _collisionState.IsGrounded.GetValueWithTolerance(JumpLateTolerance);
            bool jumpInputStrict = _input.WasJumpPressedThisFrame.Value;
            bool jumpInputTolerant = _input.WasJumpPressedThisFrame.GetValueWithTolerance(JumpEarlyTolerance);

            bool isAgainstWall = _collisionState.IsAgainstWall.Value;

            // Regular Jump
            if (!_isJumping.Value && (isGroundedStrict && jumpInputTolerant) || (isGroundedTolerant && jumpInputStrict))
            {
                _isJumping.Value = true;
            }

            // Wall Jump
            if (CanWallJump)
            {
                if (isAgainstWall && jumpInputStrict && !isGroundedStrict)
                {
                    var direction = Mathf.Sign(CharacterController.WallNormal.x);
                    velocity.x = direction * WallHorizontalForce;
                    _isJumping.Value = true;
                }
            }

            // Jump Update
            if (_isJumping.Value)
            {
                double jumpDuration = Time.timeAsDouble - _isJumping.LastTimeChanged;
                if (jumpDuration < JumpDuration && _input.Jump.Value)
                {
                    velocity.y = JumpHeight / JumpDuration * JumpCurve((float)jumpDuration / JumpDuration);
                }
                else
                {
                    _isJumping.Value = false;
                }
            }
        }

        private void HandleDash(ref Vector2 velocity)
        {
            bool dashInputTolerant = _input.WasDashPressedThisFrame.GetValueWithTolerance(DashEarlyTolerance);
            bool dashCooldownCompleted = (Time.timeAsDouble - _isDashing.LastTimeChanged) >= DashCooldown && !_isDashing.Value;

            bool hasMovementInput = !Mathf.Approximately(_input.HorizontalMovement, 0.0f);

            if (dashCooldownCompleted && dashInputTolerant)
            {
                bool isAgainstWallNotGrounded = _collisionState.IsAgainstWall.Value && !_collisionState.IsGrounded.Value;
                float direction = (isAgainstWallNotGrounded ? Mathf.Sign(_collisionState.WallNormal.x) : (hasMovementInput ? Mathf.Sign(_input.HorizontalMovement) : GetFacingDirection())); 
                _dashDirection = Vector2.right * direction;
                _isDashing.Value = true;
            }

            // Dash Update
            if (_isDashing.Value)
            {
                double dashDuration = Time.timeAsDouble - _isDashing.LastTimeChanged;
                if (dashDuration < DashDuration)
                {
                    velocity = _dashDirection * DashDistance / DashDuration * DashCurve((float)dashDuration / DashDuration, 1.0f);
                }
                else
                {
                    _isDashing.Value = false;
                    if (hasMovementInput)
                    {
                        velocity = Vector2.right * Mathf.Sign(_input.HorizontalMovement) * MovementSpeed;
                    }
                    else
                    {
                        velocity = Vector2.zero;
                    }
                }

                if (_collisionState.IsGrounded.Value)
                {
                    velocity.y += -GroundStickingVelocity;
                }

                _isJumping.Value = false;
            }
        }

        private static float JumpCurve(float time, float curveParameter = 1.5f)
        {
            return curveParameter * Mathf.Pow(1.0f - time, curveParameter - 1.0f);
        }

        private static float DashCurve(float time, float curveParameter = 4.0f)
        {
            return curveParameter * Mathf.Pow(1.0f - time, curveParameter - 1.0f);
        }

        private void OnDrawGizmos()
        {
            if (_characterController2D == null) return;

            if (_characterController2D.IsGrounded)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawRay(new Ray(transform.position, _characterController2D.GroundNormal));
            }

            if (_characterController2D.IsAgainstWall)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(new Ray(transform.position, _characterController2D.WallNormal));
            }

            Gizmos.color = Color.red;
            Gizmos.DrawRay(new Ray(transform.position, _characterController2D.Velocity));
        }
    }

}
