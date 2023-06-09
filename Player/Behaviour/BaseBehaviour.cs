using UnityEngine;

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