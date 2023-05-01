using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 실제 상태의 행동을 하게 되는 업데이트 컴포넌트
/// </summary>
public abstract class Action : ScriptableObject
{
    /// <summary>
    /// Update는 무조건적으로 필요하기 때문에 override받아서 사용하면된다.
    /// </summary>
    public abstract void UpdateAction(StateController controller);

    /// <summary>
    /// Start가 필요하면 override받아서 사용하면된다
    /// </summary>
    public virtual void OnEnabledAction(StateController controller)
    {
        // start함수
    }
    public virtual void ExitAction(StateController controller)
    {
        
    }
}