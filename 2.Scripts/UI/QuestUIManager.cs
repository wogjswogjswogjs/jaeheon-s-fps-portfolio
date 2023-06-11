using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
public class QuestUIManager : MonoBehaviour
{
    private Text questText;
    private Text questLabel;
    private void Awake()
    {
        if (questText == null)
        {
            questText = transform.Find("QuestHUD/QuestText").GetComponent<Text>();
        }

        if (questLabel == null)
        {
            questLabel = transform.Find("QuestHUD/QuestLabel").GetComponent<Text>();
        }

        FindObjectOfType<QuestManager>().uiUpdateEvent += UpdateQuestHUD;
    }

    public void UpdateQuestHUD(QuestData questDate)
    {
        this.questLabel.text = questDate.questName;
        this.questText.text = questDate.ChangeUIText();
    }
}
