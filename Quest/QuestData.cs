using UnityEngine;

/// <summary>
/// CheckSuccessQuest를 상속받아서 퀘스트 성공 조건 구현해야함
/// questCode = this.GetType().GetHashCode()를 이용해서 퀘스트 구별용으로 사용
/// 현재 진행 여부를 QUESTSTATUS 변수에 저장. NONE상태가 비활성화 상태 
/// </summary>
public abstract class QuestData : ScriptableObject
{
    public int questCode;
    public string questName;

    public QUESTSTATUS QUESTSTATUS = QUESTSTATUS.NONE;
    
    public void ActivateQuest()
    {
        QUESTSTATUS = QUESTSTATUS.ACTIVE;
    }

    public void CompleteQuest()
    {
        QUESTSTATUS = QUESTSTATUS.COMPLETED;
    }

    public virtual void Initialize()
    {
        
    }

    public virtual string ChangeUIText()
    {
        return null;
    }

    public abstract bool CheckSuccessQuest();
}



