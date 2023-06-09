using UnityEngine;


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

    public virtual bool CheckSuccessQuest()
    {
        return false;
    }
}



