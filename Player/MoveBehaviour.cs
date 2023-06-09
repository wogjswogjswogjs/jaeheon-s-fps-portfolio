using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 이동과 점프 동작을 담당하는 컴포넌트
/// 충돌처리에 대한 기능도 포함
/// 기본 동작으로써 작동
/// </summary>
[RequireComponent(typeof(PlayerController))] 
public class MoveBehaviour : BaseBehaviour
{
    #region AnimatorParamHash
    
    private int groundedParamHash;
    private int speedParamHash;
    
    #endregion

    #region Component

    private CapsuleCollider capsuleCollider;
    private Transform playerTransform;
    
    #endregion

    #region Values

    public float walkSpeed = 0.15f;
    public float runSpeed = 1.0f;
    public float sprintSpeed = 1.3f;
    public float speedDampTime = 0.1f;
    public float runForce = 5.0f;
    
    private float speed;
    private Vector3 cameraForward = Vector3.zero;
    private Vector3 cameraRight = Vector3.zero;
    private Vector3 targetDirection = Vector3.zero;
    
    #endregion
    
    public override void Initialize()
    {
        base.Initialize();
        
        playerTransform = transform;
        capsuleCollider = GetComponent<CapsuleCollider>();

        speedParamHash = Animator.StringToHash(AnimatorKey.Speed);
        
        PlayerController.SubScribeBehaviour(this);
        PlayerController.RegisterDefaultBehaviour(this.behaviourCode);
    }
    
    private Vector3 Rotating(float horizontal, float vertical)
    {
        targetDirection = cameraForward * vertical + cameraRight * horizontal;

        if (PlayerController.IsMoving() && targetDirection != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(targetDirection);
            
            Quaternion rot = Quaternion.Slerp(PlayerController.GetRigidbody.rotation,
                targetRot, PlayerController.turnSmoothing);
            PlayerController.GetRigidbody.MoveRotation(rot);
            PlayerController.SetLastDirection(targetDirection);
        }

        if (!(Mathf.Abs(horizontal) > 0.9f || Mathf.Abs(vertical) > 0.9f))
        {
            PlayerController.Repositioning();
        }

        return targetDirection;
    }

    private void Moving(float horizontal, float vertical)
    {
        /*if (behaviourController.IsGrounded())
        {
            behaviourController.GetRigidbody.useGravity = true;
        }*/
        Rotating(horizontal, vertical);

        if (PlayerController.IsRun())
        {
            speed = Vector3.ClampMagnitude(new Vector3(horizontal, 0, vertical)
                , 1.0f).magnitude * sprintSpeed;
        }
        else
        {
            speed = Vector3.ClampMagnitude(new Vector3(horizontal, 0, vertical)
                , 1.0f).magnitude * runSpeed;
        }
        targetDirection = cameraForward * vertical  + cameraRight * horizontal ;
        
        PlayerController.GetRigidbody.MovePosition(
            PlayerController.GetRigidbody.position + targetDirection * speed * runForce * Time.deltaTime);
        
        PlayerController.GetAnimator.SetFloat(speedParamHash, speed, speedDampTime, Time.deltaTime);
    }

    public override void BehaviourFixedUpdate()
    {
        cameraForward = PlayerController.playerComponents.thirdPersonCameraScript.transform.TransformDirection(Vector3.forward);
        cameraRight = PlayerController.playerComponents.thirdPersonCameraScript.transform.TransformDirection(Vector3.right);
        cameraForward.y = 0;
        cameraForward = cameraForward.normalized;
        Moving(PlayerController.PlayerInputSystem.horizontal, PlayerController.PlayerInputSystem.vertical);
    }
 

}
