using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cursor = UnityEngine.Cursor;



/// <summary>
/// 현재 동작, 기본 동작, 잠긴 동작, 오버라이딩 동작, Input등
/// 땅에 서있는지, BaseBehaviour를 상속받은 동작들을 업데이트 시켜준다.
/// </summary>
public class PlayerController : MonoBehaviour
{
    public int currentBehaviourCode; // 현재 동작 해시코드
    private int defaultBehaviourCode; // 기본 동작 해시코드
    private int LockedBehaviourCode; // 잠긴 동작 해시코드

    // -- Behaviour
    private List<BaseBehaviour> behaviours = new List<BaseBehaviour>(); // 동작들
    private List<BaseBehaviour> overrideBehaviours = new List<BaseBehaviour>(); // 우선시 되는 동작
    
    
    // 캐싱.
    public float turnSmoothing = 1.0f; // 카메라를 향하도록 움직일때 회전속도
    private bool changedFOV; // 달리기 동작이 카메라 시야각이 변경되었을때 저장됐는지.
    public float runFOV = 80.0f; // 달리기 시야각.
    private Vector3 lastDirection; // 마지막 향했던 방향
    private bool isRun; // 현재 달리는중인가?
    
    public ThirdPersonCamera GetCameraScript => playerComponents.thirdPersonCameraScript;
    
    public int GetDefaultBehaviour { get => defaultBehaviourCode; }
    
    

    // -- Components
    public PlayerComponents playerComponents;
    
    public Rigidbody GetRigidbody => playerComponents.playerRigidbody;
    public Animator GetAnimator => playerComponents.playerAnimator;
    
    
    
    // -- Value
    private PlayerValues playerValues;
    
    // InputSystem
    public PlayerInput PlayerInputSystem { get; private set; }
    public float GetHorizontal => PlayerInputSystem.horizontal;
    public float GetVertical => PlayerInputSystem.vertical;
    
    // -- State
    private bool initalized = false;

    // -- Health
    private PlayerHealth playerHealth;
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Initialize(StageManager owner)
    {
        playerComponents = UtilComponent<PlayerComponents>.GetComponent(transform);
        PlayerInputSystem = UtilComponent<PlayerInput>.GetComponent(transform);
        playerValues = UtilComponent<PlayerValues>.GetComponent(transform);
        playerHealth = UtilComponent<PlayerHealth>.GetComponent(transform);
        playerComponents.Initialize(owner);
        playerValues.Initialize();
        
        initalized = true;
    }
    
    private void Update()
    {
        if (initalized is false)
        {
            return;
        }
        
        OnInputUpdate();
        OnMovementUpdate();
        OnBehaviourUpdate();
    }

    private void OnInputUpdate()
    {
        PlayerInputSystem.OnInputUpdate();
    }
    
    private void OnMovementUpdate()
    {
        if (PlayerInputSystem.run)
        {
            playerComponents.thirdPersonCameraScript.SetFOV(runFOV);
        }
        else
        {
            playerComponents.thirdPersonCameraScript.ResetFOV();
        }

        playerComponents.playerAnimator.SetFloat(playerValues.horizontalParamHash, PlayerInputSystem.horizontal, 0.1f, Time.deltaTime);
        playerComponents.playerAnimator.SetFloat(playerValues.verticalParamHash, PlayerInputSystem.vertical, 0.1f, Time.deltaTime);
        playerComponents.playerAnimator.SetBool(playerValues.groundedParamHash, IsGrounded());
    }

    private void OnBehaviourUpdate()
    {
        foreach (var behaviour in playerComponents.behaviours)
        {
            behaviour.BehaviourUpdate();
        }
    }



    #region Move State
    
    public bool IsMoving()
    {
        return Mathf.Abs(PlayerInputSystem.horizontal) > Mathf.Epsilon || Mathf.Abs(PlayerInputSystem.vertical) > Mathf.Epsilon;
    }

    public bool IsHorizontalMoving()
    {
        return Mathf.Abs(PlayerInputSystem.horizontal) > Mathf.Epsilon;
    }
    
    private bool CanRun()
    {
        return behaviours.All(behaviour => behaviour.AllowRun) && overrideBehaviours.All(overrideBehaviour => overrideBehaviour.AllowRun);
    }
    
    public bool IsRun()
    {
        return isRun && IsMoving() && CanRun();
    }
    
    private bool IsGrounded()
    {
        var ray = new Ray(playerComponents.playerTransform.position + Vector3.up * 2 * playerValues.colliderExtents.x, Vector3.down);
        return Physics.SphereCast(ray, playerValues.colliderExtents.x, playerValues.colliderExtents.x + 0.2f);
    }
    
    #endregion
    #region Move Set

    /// <summary>
    /// [재헌] RigidBody를 사용할 때, 캐릭터가 틀어지는걸 방지하기위해
    /// y값을 0 으로 Repositioning해주는 함수.
    /// </summary>
    public void Repositioning()
    {
        if (lastDirection == Vector3.zero)
        {
            return;
        }
        
        lastDirection.y = 0.0f;
            
        var targetRotation = Quaternion.LookRotation(lastDirection);
        var newRotation = Quaternion.Slerp(playerComponents.playerRigidbody.rotation, targetRotation
            ,turnSmoothing);
            
        playerComponents.playerRigidbody.MoveRotation(newRotation);
    }

    #endregion
    
    
    private void FixedUpdate()
    {
        if (initalized is false)
        {
            return;
        }
        
        bool isAnyBehaviourActive = false;

        if (LockedBehaviourCode > 0 || overrideBehaviours.Count == 0)
        {
            foreach (BaseBehaviour behaviour in behaviours)
            {
                if (currentBehaviourCode == behaviour.GetBehaviourCode)
                {
                    isAnyBehaviourActive = true;
                    behaviour.BehaviourFixedUpdate();
                }
            }
        }
        else
        {
            foreach (BaseBehaviour behaviour in overrideBehaviours)
            {
                behaviour.BehaviourFixedUpdate();
            }
        }

        if (!isAnyBehaviourActive && overrideBehaviours.Count == 0)
        {
            playerComponents.playerRigidbody.useGravity = true;
            Repositioning();
        }
    }

    private void LateUpdate()
    {
        if (initalized is false)
        {
            return;
        }
        
        if (LockedBehaviourCode > 0 || overrideBehaviours.Count == 0)
        {
            foreach (BaseBehaviour behaviour in behaviours)
            {
                if (behaviour.isActiveAndEnabled && currentBehaviourCode == behaviour.GetBehaviourCode)
                {
                    behaviour.BehaviourLateUpdate();
                }
            }
        }
        else
        {
            foreach (BaseBehaviour behaviour in overrideBehaviours)
            {
                behaviour.BehaviourLateUpdate();
            }
        }
    }

    public Vector3 GetLastDirection()
    {
        return lastDirection;
    }

    public void SetLastDirection(Vector3 direction)
    {
        lastDirection = direction;
    }

    public void SubScribeBehaviour(BaseBehaviour behaviour)
    {
        behaviours.Add(behaviour);
    }

    public void RegisterDefaultBehaviour(int behaviourCode)
    {
        defaultBehaviourCode = behaviourCode;
        currentBehaviourCode = behaviourCode;
    }

    public void RegisterBehaviour(int behaviourCode)
    {
        if (currentBehaviourCode == defaultBehaviourCode)
        {
            currentBehaviourCode = behaviourCode;
        }
    }

    public void UnregisterBehaviour(int behaviourCode)
    {
        if (currentBehaviourCode == behaviourCode)
        {
            currentBehaviourCode = defaultBehaviourCode;
        }
    }

    public bool RegisterOverrideBehaviour(BaseBehaviour behaviour)
    {
        if (!overrideBehaviours.Contains(behaviour))
        {
            if (overrideBehaviours.Count == 0)
            {
                foreach (BaseBehaviour behaviour1 in behaviours)
                {
                    if (behaviour1.isActiveAndEnabled && currentBehaviourCode == behaviour1.GetBehaviourCode)
                    {
                        behaviour1.OnOverride();
                        break;
                    }
                }
            }
            overrideBehaviours.Add(behaviour);
            return true;
        }
        return false;
    }

    public bool UnregisterOverrideBehaviour(BaseBehaviour behaviour)
    {
        if (overrideBehaviours.Contains(behaviour))
        {
            overrideBehaviours.Remove(behaviour);
            return true;
        }

        return false;
    }

    public bool isOverriding(BaseBehaviour behaviour = null)
    {
        if (behaviour == null)
        {
            return overrideBehaviours.Count > 0;
        }

        return overrideBehaviours.Contains(behaviour);
    }

    public bool IsCurrentBehaviour(int behaviourCode)
    {
        return this.currentBehaviourCode == behaviourCode;
    }

    public bool GetLockStatus(int behaviourCode = 0)
    {
        return LockedBehaviourCode != 0 && LockedBehaviourCode != behaviourCode;
    }

    public void LockBehaviour(int behaviourCode)
    {
        if (LockedBehaviourCode == 0)
        {
            LockedBehaviourCode = behaviourCode;
        }
    }

    public void UnLockBehaviour(int behaviourCode)
    {
        if (LockedBehaviourCode == behaviourCode)
        {
            LockedBehaviourCode = 0;
        }
    }
}


/// <summary>
/// Behaviour의 기반 클래스
/// </summary>
public abstract class BaseBehaviour : MonoBehaviour
{
    protected PlayerController PlayerController;
    protected int behaviourCode;
    protected bool canRun;
    
    public virtual void Initialize()
    {
        PlayerController = GetComponent<PlayerController>();
        canRun = true;
        
        // 동작 타입을 해시코드로 가짐, 구별용으로 사용.
        behaviourCode = this.GetType().GetHashCode(); 
    }

    // 필요한 기능 override해서 사용
    public virtual void BehaviourUpdate()
    {
        
    }
    
    public virtual void BehaviourFixedUpdate()
    {
    
    }
    
    public virtual void BehaviourLateUpdate()
    {
    
    }
    
    public int GetBehaviourCode
    {
        get => behaviourCode;
    }

    public bool AllowRun
    {
        get => canRun;
    }


    public virtual void OnOverride()
    {
    
    }
}
