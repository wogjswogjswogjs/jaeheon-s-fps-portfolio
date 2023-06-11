using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObject/BaseStats")]
public class BaseStats : ScriptableObject
{
    [Header("Speed")]
    [Tooltip("npc 정찰 속도 normal state")]
    public float patrolSpeed = 1.0f;
    [Tooltip("npc 따라오는 속도 warning state")]
    public float chaseSpeed = 4.0f;
    [Tooltip("npc 숨는 속도 battle state")]
    public float hideSpeed = 5.0f;
    
    [Header("Time")]
    [Tooltip("패트롤 웨이트 포인트에서 대기하는 시간")]
    public float patrolWaitTime = 2.0f;
    [Tooltip("타겟이 사라진 후, 찾는시간")]
    public float disappearTargetTime = 10.0f;
    [Tooltip("총을 쏘는 딜레이")] 
    public float shotDelay;

    [Header("Sense")] // 감각
    [Range(0,50)][Tooltip("볼수있는 시야거리")]
    public float lookRadius; 
    [Range(0,360)][Tooltip("볼수있는 시야 각도")]
    public float lookAngle;
    [Range(0,25)][Tooltip("근처 느낄 수 있는 거리")]
    public float feelRadius; // 무조건 공격
    [Range(0, 100)] [Tooltip("근처 들을 수 있는 거리")]
    public float hearRadius;
    [Range(0, 50)] [Tooltip("경고를 들을 수 있는 거리")]
    public float alertRadius;
    
    [Header("Cover")]
    [Tooltip("장애물레이어 마스크")]
    public LayerMask coverMask;
    [Tooltip("타겟 레이어 마스크")]
    public LayerMask targetMask;

    [Header("Animation")] 
    [Tooltip("Speed파라미터의 DampTime")]
    public float speedDampTime = 0.4f;

    [Header("Shot")] 
    [Tooltip("총쏠때의 오발률")] 
    public float shotErrorRate = 5.0f;
    [Tooltip("총의 사정거리")]
    public float shotDistance = 100.0f;
    [Tooltip("처음 총을 쏘기 시작할 시간")] 
    public float startShootDelay = 2.0f;
}
