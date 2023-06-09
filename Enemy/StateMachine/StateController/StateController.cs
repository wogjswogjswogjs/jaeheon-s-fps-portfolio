using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;


/// <summary>
/// state -> actions update -> transition(decision)
/// state에 필요한 기능들, 애니메이션 콜백들
/// 시야 체크, 찾아놓은 엄폐물 장소중 가장 가까운 위치를 찾는 기능.
/// </summary>
public class StateController : MonoBehaviour
{
    public BaseStats baseStats;
    public GunData gunData;
    public State currentState;

    // 캐싱을위한 변수들
    [HideInInspector]public NavMeshAgent enemyNav;
    [HideInInspector]public EnemyAnimation enemyAnimation;

    public Transform gunMuzzle;
    // 패트롤 waypoint
    public List<Transform> patrolWaypoints; // 맵에 생성된 빈 게임 오브젝트의 Transform을 가진 List
    [HideInInspector]public int wayPointIndex; // 현재 waypoint index
    
    // 움직이거나, 에임해야하는 타겟
    [HideInInspector]public Vector3 needToMovePosition = Vector3.positiveInfinity; // player일수도 있고,cover일수도 있고, patrol일수도 있고. 움직여야하는 position
    public GameObject aimTarget = null;
    
   
    
    private bool isAiActive; // ai가 Active되어있나
    private bool aiming;
    public bool isAiming
    {
        get => aiming;
        set
        {
            enemyAnimation.enemyAnimator.SetBool("Aim",value);
            aiming = value;
        }
    }
    
    // 이 변수값을 조정하면서 controller에서 state를 변경해준다
    public bool isFeelAlert; // 위협을 느꼈나?
    public bool isHearAlert; // 뭔가 들었나?
    public float waitInCoverTime; // 얼만큼 엄폐를 끼고 있을것인지?
    public bool coverTime; // 
    public bool targetInSight; // 타겟이 내 시야 안에 있냐
    public bool focusSight; // 그 타겟을 포커스 할것인가
    public bool blockedSight; // 타겟을 보는데 막혀있나?
   
    
    public float patroltimer;
    public int currentShots;

    /// <summary>
    /// 상태 변경
    /// </summary>

    private void Awake()
    {
        enemyNav = GetComponent<NavMeshAgent>();
        enemyAnimation = gameObject.AddComponent<EnemyAnimation>();
        
        aimTarget = GameObject.FindWithTag("Player");
        needToMovePosition = Vector3.positiveInfinity;
        
        focusSight = false;
        isAiActive = true;
        Debug.Log("엥");
    }

    public void Start()
    {
        currentState.OnEnbledState(this);
    }

    public void Update()
    {
        if (isAiActive == false) return;
        
        currentState.UpdateState(this);
        currentState.CheckTransitions(this);
    }

    public void TransitionToState(State nextState)
    {
        if(nextState != currentState)
        {
            enemyNav.SetDestination(transform.position);
            currentState = nextState;
        }
    }
    /// <summary>
    /// 뭔가 들었을 때
    /// </summary>
    public void AlertCallback(Vector3 target)
    {
        if (!aimTarget.transform.root.GetComponent<HealthBase>().IsDead)
        {
            isHearAlert = true;
            needToMovePosition = target;
        }
    }

    /// <summary>
    ///  타겟을 보는데 막혀있는지 아닌지 판단하는 함수
    /// </summary>
    public bool BlockedSight()
    {
        Vector3 target = aimTarget.transform.position;
        Vector3 eyePosition = transform.position + Vector3.up * 1.8f; // 1.8은 눈의 높이를 맞추기 위함.
        Vector3 dirToTarget = target - eyePosition;

        blockedSight = Physics.Raycast(eyePosition, dirToTarget, out RaycastHit hit, dirToTarget.magnitude,
            baseStats.coverMask);

        return blockedSight;
    }

    public void SearchPatrolPoint(int index)
    {
        Transform enemyPatrhoPoints = GameObject.Find("EnemyPatrolPoints").transform.GetChild(index);
        for (int i = 0; i < 3; i++)
        {
            patrolWaypoints[i] = (enemyPatrhoPoints.GetChild(i).transform);   
        }
    }

    private void OnDrawGizmos()
    {
        if (currentState != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * 2.5f, 0.2f);
        }
    }
}
