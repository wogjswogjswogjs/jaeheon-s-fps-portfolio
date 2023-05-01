using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Transitions/Look")]
public class LookTransition : Transition
{

    public override bool CheckTransition(StateController controller)
    {
        if (CheckTargetInRadius(controller, controller.baseStats.lookRadius))
        {
            Vector3 dirToTarget = controller.aimTarget.transform.position - controller.transform.position;
            bool inFOVTarget = Vector3.Angle(controller.transform.forward, dirToTarget) <
                               controller.baseStats.lookRadius;
            if (inFOVTarget)
            {
                controller.targetInSight = true;
                return true;
            }
        }

        return false;
    }

    public override void OnEnabledTransition(StateController controller)
    {
        
    }

    
}