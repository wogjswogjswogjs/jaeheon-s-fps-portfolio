using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObject/Quest/EscapeQuest")]
public class EscapeQuest : QuestData
{
    public override void Initialize()
    {
        questCode = this.GetType().GetHashCode();
        QUESTSTATUS = QUESTSTATUS.ACTIVE;
    }
    
    public override bool CheckSuccessQuest()
    {

        return false;
    }
}
