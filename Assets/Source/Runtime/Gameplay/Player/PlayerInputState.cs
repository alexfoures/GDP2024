using UnityEngine;
using UnityEngine.InputSystem;

namespace Coco
{
    public class PlayerInputState : MonoBehaviour
    {
        public float HorizontalMovement = 0.0f;
        public float VerticalInput = 0.0f;
        public BoolTracker WasJumpPressedThisFrame = BoolTracker.Create();
        public BoolTracker Jump = BoolTracker.Create();
        public BoolTracker WasAttackPressedThisFrame = BoolTracker.Create();
        public BoolTracker WasDashPressedThisFrame = BoolTracker.Create();

        public void OnMove(InputAction.CallbackContext ctx)
        {
            var input = ctx.ReadValue<Vector2>();
            HorizontalMovement = input.x;
            VerticalInput = input.y;
        }

        public void OnJump(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                WasJumpPressedThisFrame.Value = true;
            }

            Jump.Value = ctx.ReadValueAsButton();
        }

        public void OnAttack(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                WasAttackPressedThisFrame.Value = true;
            }
        }

        public void OnDash(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                WasDashPressedThisFrame.Value = true;
            }
        }

        public void Consume()
        {
            WasJumpPressedThisFrame.Value = false;
            WasAttackPressedThisFrame.Value = false;
            WasDashPressedThisFrame.Value = false;
        }
    }
}