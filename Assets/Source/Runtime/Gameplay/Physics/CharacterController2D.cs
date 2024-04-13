using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coco
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterController2D : MonoBehaviour
    {
        [SerializeField] private float _minMoveDistance = 0.001f;
        [SerializeField] private float _skinWidth = 0.05f;
        [SerializeField][Range(0.1f, 90.0f)] private float _slopeAngleLimit = 45.0f;
        [SerializeField][Range(0.1f, 45.0f)] private float _ceilingAngleLimit = 5.0f;

        public Vector2 Velocity = Vector2.zero;

        // Physics Objects
        private Rigidbody2D _rigidbody2D = null;
        private ContactFilter2D _contactFilter;
        private RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];

        // Collisiong State
        private bool _isGrounded = false;
        private bool _isAgainstWall = false;
        private bool _isAgainstCeiling = false;
        private Vector2 _groundNormal = Vector2.zero;
        private Vector2 _groundTangent = Vector2.zero;
        private Vector2 _wallNormal = Vector2.zero;
        private Vector2 _wallUpward = Vector2.zero;
        private float _minimumSlopeY = 0.0f;
        private float _minimumCeilingY = 0.0f;

        public bool IsGrounded => _isGrounded;
        public bool IsAgainstWall => _isAgainstWall;
        public bool IsAgainstCeiling => _isAgainstCeiling;
        public Vector2 GroundNormal => _groundNormal;
        public Vector2 GroundTangent => _groundTangent;
        public Vector2 WallNormal => _wallNormal;
        public Vector2 WallUpward => _wallUpward;

        private int kOneWayPlatformLayer = -1;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _minimumSlopeY = Mathf.Cos(Mathf.Deg2Rad * _slopeAngleLimit);
            _minimumCeilingY = -Mathf.Cos(Mathf.Deg2Rad * _ceilingAngleLimit);

            _contactFilter.useTriggers = false;
            kOneWayPlatformLayer = LayerMask.NameToLayer("OneWayPlatforms");
        }

        public void Move(float deltaTime)
        {
            _contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));

            Vector2 horizontalDirection = _isGrounded ? _groundTangent : Vector2.right;
            Vector2 verticalDirection = _isAgainstWall ? _wallUpward : Vector2.up;

            Vector2 horizontalMovement = (Velocity.x * horizontalDirection) * deltaTime;
            Vector2 verticalMovement = (Velocity.y * verticalDirection) * deltaTime;

            float distance = (horizontalMovement + verticalMovement).magnitude;
            if (distance < _minMoveDistance) return;

            _isGrounded = false;
            _isAgainstWall = false;
            _isAgainstCeiling = false;

            InternalMove(ref horizontalMovement);
            InternalMove(ref verticalMovement);

            _rigidbody2D.velocity = Velocity;
        }

        private void InternalMove(ref Vector2 movement)
        {
            float distance = movement.magnitude;
            int count = _rigidbody2D.Cast(movement, _contactFilter, _hitBuffer, distance + _skinWidth);

            for (int i = 0; i < count; i++)
            {
                Vector2 hitNormal = _hitBuffer[i].normal;
                float hitDistance = _hitBuffer[i].distance - _skinWidth;

                if (hitNormal.y > _minimumSlopeY)
                {
                    _isGrounded = true;
                    _groundNormal = hitNormal;
                    _groundTangent = new Vector2(hitNormal.y, -hitNormal.x);
                }
                else if (hitNormal.y >= -0.001f)
                {
                    // OneWayPlatforms are not walls.
                    if (_hitBuffer[i].collider.gameObject.layer != kOneWayPlatformLayer)
                    {
                        _isAgainstWall = true;
                        _wallNormal = hitNormal;
                        _wallUpward = new Vector2(hitNormal.y, -hitNormal.x) * -Mathf.Sign(hitNormal.x);

                    }
                } 
                else if (hitNormal.y < _minimumCeilingY)
                {
                    // OneWayPlatforms are not ceiling.
                    if (_hitBuffer[i].collider.gameObject.layer != kOneWayPlatformLayer)
                    {
                        _isAgainstCeiling = true;
                    }
                }

                //float projection = Vector2.Dot(Velocity, hitNormal);
                //if (projection < 0)
                //{
                //    Velocity -= Vector2.Dot(Velocity, hitNormal) * hitNormal;
                //}

                distance = Mathf.Min(hitDistance, distance);
            }
        }
    }

}