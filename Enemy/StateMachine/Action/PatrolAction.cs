using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Actions/Patrol")]
public class PatrolAction : Action
{
    private Vector3 lastDirection;
    public override void UpdateAction(StateController controller)
    {
        Patrol(controller);
    }

    /// <summary>
    /// Start가 필요하면 override받아서 사용하면된다
    /// </summary>
    public override void OnEnabledAction(StateController controller)
    {
        controller.focusSight = false;
        controller.enemyNav.speed = controller.baseStats.patrolSpeed;
        controller.patroltimer = 0.0f;
        controller.enemyNav.stoppingDistance = 0.5f;
    }

    private void Patrol(StateController controller)
    {
        if (controller.patrolWaypoints.Count == 0)
        {
            return;
        }
        
        // 현재 멈춘 상태라면?
        if (controller.enemyNav.remainingDistance <= controller.enemyNav.stoppingDistance)
        {
            controller.patroltimer += Time.deltaTime;
            if (controller.patroltimer >= controller.baseStats.patrolWaitTime + 1)
            {
                controller.patroltimer = 0;
                controller.wayPointIndex = (controller.wayPointIndex + 1) % controller.patrolWaypoints.Count;
            }
        }
        controller.needToMovePosition = controller.patrolWaypoints[controller.wayPointIndex].position;
        controller.enemyNav.destination = controller.needToMovePosition;
    }
}
