using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyAnimation : MonoBehaviour
{
    [HideInInspector] public Animator enemyAnimator;
    private StateController controller;
    private NavMeshAgent enemyNav;
    
    private Transform hips, spine, rightHand;// born transform
    private Vector3 initialRootRotation;
    private Vector3 initialHipsRotation;
    private Vector3 initialSpineRotation;
    
    private Quaternion lastRotation;
    private float timeCountAim;
    private float timeCountGuard;
    private const float turnSpeed = 25.0f;

    private int speedParamHash;
    private int angleParamHash;
    private int horizontalParamHash;
    private int verticalParamHash;

    private void Awake()
    {
        controller = GetComponent<StateController>();
        enemyNav = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        enemyNav.updateRotation = false; // 회전은 직접 할것이므로 Nav의 회전은 막는다.

        rightHand = enemyAnimator.GetBoneTransform(HumanBodyBones.RightHand);
        hips = enemyAnimator.GetBoneTransform(HumanBodyBones.Hips);
        spine = enemyAnimator.GetBoneTransform(HumanBodyBones.Spine);
        initialRootRotation = hips.parent.localEulerAngles;
        initialHipsRotation = hips.localEulerAngles;
        initialSpineRotation = spine.localEulerAngles;

        speedParamHash = Animator.StringToHash(AnimatorKey.Speed);
        horizontalParamHash = Animator.StringToHash(AnimatorKey.Horizontal);
        verticalParamHash = Animator.StringToHash(AnimatorKey.Vertical);
        angleParamHash = Animator.StringToHash(AnimatorKey.Angle);
        
    }

    public void Update()
    {
        NavAnimParamSetup();
    }
    
    /// <summary>
    /// navmesh가 update되는 동안 호출 할 예정
    /// </summary>
    

    private void NavAnimParamSetup()
    {
        // speedParam navmesh의 움직임에따라 speedParam 자동 세팅
        float speed = enemyNav.desiredVelocity.magnitude;

        // 타겟을 포커스한다면. 타겟을 바라보아라

        /*if (controller.needToMoveTarget != Vector3.positiveInfinity)
        {
            Vector3 dest = controller.needToMoveTarget - transform.position;
            dest.y = 0.0f;

            dest = dest.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(dest);
            transform.rotation = Quaternion.Lerp(transform.rotation,
                targetRotation, turnSpeed * Time.deltaTime);
        }
        else
        {*/
        if (enemyNav.desiredVelocity.magnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(enemyNav.desiredVelocity);
            Vector3 dest = controller.needToMovePosition - transform.position;
            dest.y = 0.0f;

            dest = dest.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(dest);
            transform.rotation = Quaternion.Lerp(transform.rotation,
                targetRotation, turnSpeed * Time.deltaTime);
        }
        
        if (controller.focusSight == true)
        {
            Vector3 dest = controller.aimTarget.transform.position - transform.position;
            dest.y = 0.0f;

            dest = dest.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(dest);
            transform.rotation = Quaternion.Lerp(transform.rotation,
                targetRotation, turnSpeed * Time.deltaTime);
        }

        // 최종
        enemyAnimator.SetFloat(speedParamHash, speed, controller.baseStats.speedDampTime, Time.deltaTime);
    }
    
    private void LateUpdate()
    {
        
    }
    
}
