using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Transitions/Hear")]
public class HearTransition : Transition
{
    public override bool CheckTransition(EnemyController controller)
    {
        if (CheckTargetInRadius(controller, controller.baseStats.hearRadius))
        {
            return true;
        }
        return false;
    }

    public override void OnEnabledTransition(EnemyController controller)
    {
        
    }
}
