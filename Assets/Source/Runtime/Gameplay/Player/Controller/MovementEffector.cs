using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Coco
{
    public abstract class MovementEffector : MonoBehaviour
    {
        public new bool enabled = true;
        public abstract void Apply(in PlayerMovementContext context, ref Vector2 velocity);
    }
}
