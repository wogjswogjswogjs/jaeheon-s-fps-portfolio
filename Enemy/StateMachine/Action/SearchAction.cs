using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 타겟이 있다면 타겟까지 이동하지만, 타겟을 잃었다면 가만히 서있다.
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObject/Actions/Search")]
public class SearchAction : Action
{
    public override void ExecuteAction(EnemyController controller)
    {
        // 움직여야하는 타겟이 있으면 chase속도로 따라가고, 아니면 가만히 있어라.
        if (controller.needToMovePosition == Vector3.positiveInfinity)
        {
            controller.enemyNav.destination = controller.transform.position;
        }
        else
        {
            controller.enemyNav.speed = controller.baseStats.chaseSpeed;
            controller.enemyNav.destination = controller.needToMovePosition;
        }
    }
    
    public override void OnEnabledAction(EnemyController controller)
    {
        controller.focusSight = false;
        controller.isAiming = false;
        controller.enemyAnimation.enemyAnimator.SetBool(AnimatorKey.Crouch, false);
    }
    public override void ExitAction(EnemyController controller)
    {
        
    }
    
}
