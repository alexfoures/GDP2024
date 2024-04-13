using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coco
{
    public class ConsumeJumpTriggerOnEnter : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetTrigger(PlayerAnimatorProperties.Jump);
        }
    }
}