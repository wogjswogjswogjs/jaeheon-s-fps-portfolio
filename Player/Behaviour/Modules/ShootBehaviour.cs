using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// 사격 기능 : 사격이 가능한지 여부를 체크하는 기능.
/// 발사 키 입력 받아서 애니메이션 재생, 이펙트 생성, 충돌 체크 기능
/// UI관련 십자선 표시 기능, 발사속도 조정
/// 캐릭터 상체를 IK를 이용해서 조준 시점에 맞춰서 회전
/// 벽이나 충돌체 총알이 피격되었을 경우 피탄 이펙트 생성
/// 재장전 기능까지 포함
/// </summary>
public class ShootBehaviour : BaseBehaviour
{
    #region AnimatorParamHash

    private int weaponTypeParamHash;
    private int changeWeaponTrieerParamHash;
    private int shootingTriggerParamHash;
    private int aimParamHash;
    private int blockedAimParamHash;
    private int reloadParamHash;

    #endregion

    #region AnimatorAvatar

    private Transform hips, spine, chest, rightHand, leftHand, leftArm;
    private Vector3 avatarRootRotation;
    private Vector3 avatarHipsRotation;
    private Vector3 avatarSpineRotation;
    private Vector3 avatarChestRotation;
    public Transform leftHandPivot;
    public float armsRotation = 8.0f;
    
    #endregion

    #region Componenet

    private AimBehaviour aimBehaviour;
    private Transform gunMuzzle;
    
    #endregion

    #region Weapon

    public Dictionary<int, InteractiveWeapon> weapons; // 1 = Pistol, 2 = Riple
    private int activeWeapon = 0;

    #endregion
    
    #region ShotValue

    public float shootErrorRate = 0.01f; // 총 오발률

    #endregion

    #region Values
    
    // 짧은 총, 피스톨 같은 총을 들었을때 조준시 왼팔의 위치 보정값
    public Vector3 leftArmShortAim = new Vector3(-4.0f, 0.0f, 2.0f);
    public Texture2D aimCrossHair, shootCrossHair; // 에임중일때의 크로스헤어와, 총을 발싸할 때의 크로스헤어
    public LayerMask shotMask;
    private bool isAiming, isAimBlocked;
    private float distToHand;
    private float lastFireTime; // 마지막 총을 발사한 지점
    private float shotInterval, originalShotInterval = 0.5f;

    #endregion
    
    public GameObject muzzleFlashEffect,  sparkEffect;

    

    public override void Initialize()
    {
        base.Initialize();
        
        weaponTypeParamHash = Animator.StringToHash(AnimatorKey.Weapon);
        aimParamHash = Animator.StringToHash(AnimatorKey.Aim);
        shootingTriggerParamHash = Animator.StringToHash(AnimatorKey.Shooting);
        reloadParamHash = Animator.StringToHash(AnimatorKey.Reload);
        aimBehaviour = GetComponent<AimBehaviour>();

        weapons = new Dictionary<int, InteractiveWeapon>();

        leftHand = this.PlayerController.GetAnimator.GetBoneTransform(HumanBodyBones.LeftHand);
        rightHand = this.PlayerController.GetAnimator.GetBoneTransform(HumanBodyBones.RightHand);
        hips = this.PlayerController.GetAnimator.GetBoneTransform(HumanBodyBones.Hips);
        spine = this.PlayerController.GetAnimator.GetBoneTransform(HumanBodyBones.Spine);
        chest = this.PlayerController.GetAnimator.GetBoneTransform(HumanBodyBones.Chest);
        leftArm = this.PlayerController.GetAnimator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        
        avatarRootRotation = hips.parent.localEulerAngles;
        avatarHipsRotation = hips.localEulerAngles;
        avatarSpineRotation = spine.localEulerAngles;
        avatarChestRotation = chest.localEulerAngles;
    }
    
    
    public override void BehaviourUpdate()
    {
        if (Input.GetButton(PlayerInput.shootButtonName))
        {
            Fire(activeWeapon);
        }
        else if (Input.GetButtonDown(PlayerInput.reloadButtonName) && activeWeapon > 0)
        {
            weapons[activeWeapon].StartReload();
            PlayerController.GetAnimator.SetBool(reloadParamHash, true);
        }
        else if (Input.GetButtonDown(PlayerInput.dropButtonName) && activeWeapon > 0)
        {
            EndReloadWeapon();
        }
    }
    
    
    
    
    private IEnumerator ShotEffect(int weapon, Vector3 hitPosition)
    {
        weapons[weapon].muzzleFlashEffect.GetComponent<ParticleSystem>().Play();
        aimBehaviour.crossHair = aimCrossHair;
        LineRenderer line = weapons[weapon].GetComponent<LineRenderer>();
        line.SetPosition(0, weapons[weapon].muzzleTransform.position);
        line.SetPosition(1,hitPosition);
        line.enabled = true;
        yield return new WaitForSeconds(0.03f);
        line.enabled = false;
        yield return new WaitForSeconds(0.1f);
        aimBehaviour.crossHair = shootCrossHair;
        
    }
    public void Fire(int weapon)
    {
        if (PlayerController.isOverriding(aimBehaviour) &&
            Time.time >= lastFireTime + weapons[weapon].gunData.timeBetFire &&
            !PlayerController.GetAnimator.GetBool(reloadParamHash))
        {
            lastFireTime = Time.time;
            Shot();
        }
    }
    public void Shot()
    {
        PlayerController.GetCameraScript.BounceVertical(weapons[activeWeapon].gunData.recoilAngle);
        if (weapons[activeWeapon].Shoot())
        {
            RaycastHit hit;
            Vector3 hitPosition = Vector3.zero;
            
            Ray ray = new Ray(PlayerController.playerComponents.thirdPersonCameraScript.transform.position,
                PlayerController.playerComponents.thirdPersonCameraScript.transform.forward + 
                Random.Range(-shootErrorRate, shootErrorRate) * 
                PlayerController.playerComponents.thirdPersonCameraScript.transform.forward);
            
            if (Physics.Raycast(ray, out hit, 500.0f))
            {
                StartCoroutine(ShotEffect(activeWeapon, hit.point));
                
                hit.collider.SendMessageUpwards("HitCallBack",
                    new HealthBase.DamageInfo(hit.point, 
                        ray.direction, weapons[activeWeapon].gunData.damage),
                    SendMessageOptions.DontRequireReceiver);
                
                EffectManager.Instance.EffectOneShot(0, hit.point);
            }
        }
    }
    public void EndReloadWeapon()
    {
        PlayerController.GetAnimator.SetBool(reloadParamHash, false);
        weapons[activeWeapon].EndReload();
    }

    
    public void AddWeapon(InteractiveWeapon weapon)
    {
        PlayerController.GetAnimator.SetTrigger(changeWeaponTrieerParamHash);
        weapon.gameObject.transform.SetParent(rightHand);
        weapon.transform.localPosition = weapon.handPosition;
        weapon.transform.localRotation = Quaternion.Euler(weapon.handRotation);
        
        if (weapon.gunData.WEAPONTYPE == WEAPONTYPE.SHORT)
        {
            if (weapons.ContainsKey(1))
            {
                weapons[1].Drop();
            }
            weapons[1] = weapon;
            activeWeapon = 1;
        }
        else if (weapon.gunData.WEAPONTYPE == WEAPONTYPE.LONG)
        {
            if (weapons.ContainsKey(2))
            {
                weapons[2].Drop();
            }
            weapons[2] = weapon;
            activeWeapon = 2;
        }
    }

    public void OnAnimatorIK(int layerIndex)
    {
        if (PlayerController.isOverriding(aimBehaviour) && activeWeapon > 0)
        {
            Quaternion targetRot = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            
            targetRot *= Quaternion.Euler(avatarRootRotation);
            targetRot *= Quaternion.Euler(avatarHipsRotation);
            targetRot *= Quaternion.Euler(avatarSpineRotation);
            targetRot *= Quaternion.Euler(-30.0f,0.0f,0);
            
            PlayerController.GetAnimator.SetBoneLocalRotation(HumanBodyBones.Spine,
                Quaternion.Inverse(hips.rotation) * targetRot);

            float xCamRot = Quaternion.LookRotation(PlayerController.playerComponents.thirdPersonCameraScript.transform.forward).eulerAngles.x;
            targetRot = Quaternion.AngleAxis(xCamRot + armsRotation, this.transform.right);
            
            Debug.Log(targetRot); 
            
            targetRot *= spine.rotation;
            targetRot *= Quaternion.Euler(avatarChestRotation);
            
            PlayerController.GetAnimator.SetBoneLocalRotation(HumanBodyBones.Chest,
                Quaternion.Inverse(spine.rotation) * targetRot);
        }

        if (!PlayerController.isOverriding(aimBehaviour) && activeWeapon > 0 && 
            !PlayerController.GetAnimator.GetBool(reloadParamHash))
        {
            PlayerController.GetAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
            PlayerController.GetAnimator.SetIKPosition(AvatarIKGoal.LeftHand, weapons[activeWeapon].leftHandPivot.transform.position);
        }
    }

    public override void BehaviourFixedUpdate()
    {
        
    }

    private void LateUpdate()
    {
        if (PlayerController.isOverriding(aimBehaviour) && weapons[activeWeapon] &&
            weapons[activeWeapon].gunData.WEAPONTYPE == WEAPONTYPE.SHORT)
        {
            leftArm.localEulerAngles = leftArm.localEulerAngles + leftArmShortAim;
        }
    }
}
