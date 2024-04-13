using System;
using System.Collections.Generic;
using UnityEngine;

namespace Coco
{
    [RequireComponent(typeof(CharacterController2D), typeof(PlayerInputState))]
    public class PlayerController : MonoBehaviour
    {

        [Header("General Settings")]
        public float AttackCooldown = 0.5f;

        [Header("Movement Effectors")]
        public List<MovementEffector> MovementEffectors = new List<MovementEffector>();

        [Header("Advanced Settings")]
        public float MinimumVerticalVelocity = -20.0f;
        public float MaximumVerticalVelocity = 20.0f;

        public bool IsKinematic = false;
        public Facing facing;

        private CharacterController2D _characterController2D = null;
        private PlayerInputState _input = null;

        private float _attackCooldownTimer = 0.0f;

        public CollisionState collisionState = CollisionState.Create();
        public CharacterController2D CharacterController => _characterController2D;

        public Action OnAttack;
        private BoolTracker _fallThroughPlatforms = BoolTracker.Create();

        private void Start()
        {
            _characterController2D = GetComponent<CharacterController2D>();
            _input = GetComponent<PlayerInputState>();
        }

        public bool TryGetMovementEffector<T>(out T value) where T : MovementEffector
        {
            foreach (var effector in MovementEffectors)
            {
                if (effector.GetType() == typeof(T))
                {
                    value = (T)effector;
                    return true;
                }
            }
            value = null;
            return false;
        }

        private void FixedUpdate()
        {
            if (_input.VerticalInput < -0.5f)
            {
                _fallThroughPlatforms.Value = true;
            }
            else
            {
                _fallThroughPlatforms.Value = false;
            }

            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("OneWayPlatforms"), _fallThroughPlatforms.GetValueWithTolerance(0.1f));

            if (!IsKinematic)
            {
                UpdateMovement();
                UpdateFacing();
            }
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
                if (_attackCooldownTimer >= AttackCooldown && !IsKinematic)
                {
                    OnAttack?.Invoke();
                    _attackCooldownTimer = 0.0f;

                    //// Instant turn when attacking.
                    //if (Mathf.Abs(_inputMovement.x) > 0.2f)
                    //{
                    //    SetFacing(_inputMovement.x < 0.0f ? Facing.Left : Facing.Right);
                    //}
                }
            }
        }

        public float GetFacingDirection()
        {
            return facing == Facing.Right ? 1.0f : -1.0f;
        }

        private void UpdateMovement()
        {
            Vector2 velocity = _characterController2D.Velocity;

            PlayerMovementContext context = new PlayerMovementContext()
            {
                movementEffectors = MovementEffectors,
                facing = facing,
                collisionState = collisionState,
                playerInputState = _input
            };

            foreach (var effector in MovementEffectors)
            {
                if (effector.enabled)
                {
                    effector.Apply(in context, ref velocity);
                }
            }

            velocity.y = Mathf.Clamp(velocity.y, MinimumVerticalVelocity, MaximumVerticalVelocity);

            _characterController2D.Velocity = velocity;
            _characterController2D.Move(Time.fixedDeltaTime);
            collisionState.Sync(_characterController2D);
        }

        private void UpdateFacing()
        {
            bool isAgainstWall = collisionState.IsAgainstWall.Value;
            bool isGrounded = collisionState.IsGrounded.Value;
            Vector2 velocity = _characterController2D.Velocity;

            if (isAgainstWall && !isGrounded)
            {
                facing = _characterController2D.WallNormal.x < 0.0 ? Facing.Left : Facing.Right;
            }
            else if (Mathf.Abs(velocity.x) > 0.2f)
            {
                facing = velocity.x < 0.0f ? Facing.Left : Facing.Right;
            }
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
