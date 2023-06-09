using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "ScriptableObject/Actions/FindCover")]
public class FindCoverAction : Action
{
    private List<GameObject> list;
    public override void OnEnabledAction(EnemyController controller)
    {
        controller.focusSight = true;
        controller.enemyAnimation.enemyAnimator.SetBool(AnimatorKey.Crouch, false);
        controller.enemyNav.speed = controller.baseStats.hideSpeed;
        controller.needToMovePosition = GetBestCover(controller, 5.0f);
        controller.enemyNav.destination = controller.needToMovePosition;
        controller.enemyNav.stoppingDistance = 1.0f;
    }

    public override void ExecuteAction(EnemyController controller)
    {
        if (controller.enemyNav.remainingDistance <= 1.0f)
        {
            controller.enemyNav.velocity = Vector3.zero;
            controller.enemyAnimation.enemyAnimator.SetFloat(AnimatorKey.Speed, 0.0f);
            controller.enemyAnimation.enemyAnimator.SetBool(AnimatorKey.Crouch, true);
        }
        
    }

    /// <summary>
    /// 근처의 Bound를 긁어오기
    /// </summary>
    private Vector3 GetBestCover(EnemyController controller, float radius)
    {
        Vector3 retVec;
        Vector3 minDistanceTransform = Vector3.zero;
        float minDistance = radius;
        Collider[] col = Physics.OverlapSphere(controller.transform.position, radius, 1 << LayerMask.NameToLayer("Bound"));
        
        foreach (Collider obj in col)
        {
            Vector3 objectPos = obj.transform.position;
            float dist = (objectPos - controller.transform.position).magnitude;

            if (dist < minDistance)
            {
                minDistance = dist;
                minDistanceTransform = obj.transform.position;
            }
        }
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(minDistanceTransform, out hit, 5.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return Vector3.positiveInfinity;
    }
}
