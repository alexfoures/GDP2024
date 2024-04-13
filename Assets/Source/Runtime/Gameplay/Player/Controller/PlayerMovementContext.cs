using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Coco
{
    public struct PlayerMovementContext
    {
        public List<MovementEffector> movementEffectors;
        public CollisionState collisionState;
        public PlayerInputState playerInputState;
        public Facing facing;

        public bool TryGetMovementEffector<T>(out T value) where T : MovementEffector
        {
            foreach (var effector in movementEffectors)
            {
                if (effector.GetType() == typeof(T))
                {
                    value = (T) effector;
                    return true;
                }
            }
            value = null;
            return false;
        }

        public float GetFacingDirection()
        {
            return facing == Facing.Right ? 1.0f : -1.0f;
        }

    }

}
