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

    /// <summary>
    /// QuestUIManager에서 QuestManager의 uiUpdateEvent를 구독한다
    /// QuestUIManager의 UpdateQuestUI함수를 통해서 이 함수가 호출된다.
    /// </summary>
    /// <returns></returns>
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
