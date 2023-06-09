using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// 에임 동작을 담당하는 컴포넌트
/// 마우스 오른쪽 버튼을 눌렀을 때, overrideBehaviour에 Register
/// 마우스 오른쪽 버튼을 떼었을 때, overrideBehaviour에 UnRegister 
/// </summary>
public class AimBehaviour : BaseBehaviour
{
   #region AnimatorParamHash

   private int speedParamHash;
   private int cornerParamHash;
   private int aimParamHash;

   #endregion

   #region AnimatorAvatar

   private Vector3 avatarRootRotation;
   private Vector3 avatarHipRotation;
   private Vector3 avatarSpineRotation;
   private Vector3 avatarLeftShoulderRotation;
   private Vector3 avatarRightShoulderRotation;

   #endregion

   #region Component

   private Transform playerTransform;

   #endregion

   #region CamOffset

   public Vector3 aimCamPivotOffset; // Aim중일 때, Camera에 적용할 PivotOffset값
   public Vector3 aimCamPosOffset; // Aim중일 때, Camera에 적용할 PositionOffset값

   #endregion
   
   #region Values
   
   public bool isAim;
   public float aimTurnSmoothing = 0.15f; // 카메라를 향하도록 조준할 때 회전속도.
   
   private Vector3 cameraForward = Vector3.zero;
   
   public Texture2D crossHair;
   
   #endregion

   private void Awake()
   {
      playerTransform = transform;
   }

   public override void Initialize()
   {
      base.Initialize();
      AnimationIK_Initialize();
   }

   private void AnimationIK_Initialize()
   {
      aimParamHash = Animator.StringToHash(AnimatorKey.Aim);
      speedParamHash = Animator.StringToHash(AnimatorKey.Speed);
      cornerParamHash = Animator.StringToHash(AnimatorKey.Corner);

      Transform hip = PlayerController.GetAnimator.GetBoneTransform(HumanBodyBones.Hips);
      Transform spine = PlayerController.GetAnimator.GetBoneTransform(HumanBodyBones.Spine);
      Transform leftShoulder = PlayerController.GetAnimator.GetBoneTransform(HumanBodyBones.LeftShoulder);
      Transform rightShoulder = PlayerController.GetAnimator.GetBoneTransform(HumanBodyBones.RightShoulder);
      
      avatarHipRotation = hip.localEulerAngles;
      avatarRootRotation = hip.parent.localEulerAngles;
      avatarSpineRotation = spine.localEulerAngles;
      avatarLeftShoulderRotation = leftShoulder.localEulerAngles;
      avatarRightShoulderRotation = rightShoulder.localEulerAngles;
   }
   
   /// <summary>
   /// 카메라에 따라 플레이어를 올바른 방향으로 회전.
   ///
   /// </summary>
   void Rotating()
   {
      cameraForward = PlayerController.playerComponents.thirdPersonCameraScript.transform.TransformDirection(Vector3.forward);
      cameraForward.y = 0.0f;
      cameraForward = cameraForward.normalized;
      
      // 보간용 Quaternion
      Quaternion targetRotation = Quaternion.Euler(0.0f, PlayerController.GetCameraScript.GetAngleH,0.0f);
      float rotateSpeed = Quaternion.Angle(playerTransform.rotation, targetRotation) * aimTurnSmoothing;
      
      PlayerController.SetLastDirection(cameraForward);
      playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation,
         rotateSpeed * Time.deltaTime);
      
   }
   
   private IEnumerator AimOn()
   { 
      // 조준이 불가능한 상태일때에 대한 예외처리
      if (PlayerController.GetLockStatus(this.behaviourCode)||
          PlayerController.isOverriding(this))
      {
         yield return false;
      }
      else
      {
         isAim = true;
         
         PlayerController.GetAnimator.SetBool(aimParamHash, true);
         
         yield return new WaitForSeconds(0.1f);
         
         PlayerController.GetAnimator.SetFloat(speedParamHash, 0.0f);
         PlayerController.GetCameraScript.SetTargetOffset(aimCamPivotOffset, aimCamPosOffset);
         
         PlayerController.RegisterOverrideBehaviour(this); // 이 Behaviour를 OverrideBehaviour로 등록
      }
   }
   
   private IEnumerator AimOff()
   {
      isAim = false;
      PlayerController.GetCameraScript.ResetTargetOffsets(); // Camera의 Offset값을 초기화
      PlayerController.GetCameraScript.ResetMaxVerticalAngle(); // 조준 시,위아래 MaxAngle을 설정
      PlayerController.GetAnimator.SetBool(aimParamHash, false);
      yield return new WaitForSeconds(0.1f);
      PlayerController.UnregisterOverrideBehaviour(this); // 이 Behaviour를 OverrideBehaviour에서 해제
   }
   
   
   public override void BehaviourUpdate()
   {
      if(Input.GetMouseButtonDown(1) && !isAim)
      {
         StartCoroutine(AimOn());
      }
      else if (isAim && Input.GetMouseButtonUp(1))
      {
         StartCoroutine(AimOff());
      }
      canRun = !isAim;
      PlayerController.GetAnimator.SetBool(aimParamHash, isAim);
   }
   
   public override void BehaviourFixedUpdate()
   {
      Rotating();
   }

   public override void BehaviourLateUpdate()
   {
      if (isAim)
      {
         //PlayerController.GetCameraScript.SetTargetOffset(aimCamPivotOffset, aimCamPosOffset);
      }
   }
   
  

   private void OnGUI()
   {
      if (crossHair != null)
      {
         float length = PlayerController.GetCameraScript.GetCurrentPivotMagnitude(aimCamPivotOffset);
         if (length < 0.05f)
         {
            GUI.DrawTexture(new Rect(Screen.width * 0.5f - (crossHair.width * 0.5f),
               Screen.height * 0.5f - (crossHair.height * 0.5f),
               crossHair.width, crossHair.height), crossHair);
         }

      }
   }
}
