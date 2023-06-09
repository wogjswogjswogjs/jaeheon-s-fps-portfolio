using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 조건을 체크하는 클래스. 상태를 변경하기 위한 조건을 체크하는 함수
/// 조건 체크를 위해 특정 위치로 부터 원하는 검색 반경에 있는 충돌체를 찾아서 그 안에 타겟이 있는지 확인
/// </summary>
public abstract class Transition : ScriptableObject
{
    public State trueState;

    /// <summary>
    /// 전이를 위한 조건을 판단하는 함수
    /// </summary>
    public abstract bool CheckTransition(EnemyController controller);

    public virtual void OnEnabledTransition(EnemyController controller)
    {
        
    }
    
    public static bool CheckTargetInRadius(EnemyController controller, float radius)
    {
        if (controller.aimTarget.GetComponent<HealthBase>().IsDead)
        {
            return false;
        }
        
        Collider[] targetInRadius = Physics.OverlapSphere(controller.transform.position,
            radius, controller.baseStats.targetMask);

        return targetInRadius.Length > 0;
    }
    
}
