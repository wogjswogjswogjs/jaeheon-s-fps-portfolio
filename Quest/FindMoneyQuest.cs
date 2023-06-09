using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Quest/FindMoneyQuest")]
public class FindMoneyQuest : QuestData
{
    public int moneyTotal;
    
    public override void Initialize()
    {
        moneyTotal = 300000;
        questCode = this.GetType().GetHashCode();
        QUESTSTATUS = QUESTSTATUS.ACTIVE;
    }

    public override string ChangeUIText()
    {
        return StageManager.Instance.playerMoney + "/" + moneyTotal;
    }

    public override bool CheckSuccessQuest()
    {
        if (StageManager.Instance.playerMoney > 300000)
        {
            return true;
        }

        return false;
    }
    
}
