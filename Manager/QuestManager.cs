using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;



public class QuestManager : MonoBehaviour
{
    public delegate void UIUpdateHandle(QuestData questData);
    public event UIUpdateHandle uiUpdateEvent;
    
    private int currentQuestIndex = 0;

    public QuestDataBase questDataBase;
    public void Initialize()
    {
        questDataBase.questObjects[currentQuestIndex].Initialize();
    }

    public void Update()
    {
        uiUpdateEvent(questDataBase.questObjects[currentQuestIndex]);
       
        if (questDataBase.questObjects[currentQuestIndex].QUESTSTATUS == QUESTSTATUS.ACTIVE)
        {
            if (questDataBase.questObjects[currentQuestIndex].CheckSuccessQuest())
            {
                questDataBase.questObjects[currentQuestIndex].CompleteQuest();
                currentQuestIndex++;
                questDataBase.questObjects[currentQuestIndex].Initialize();
            }
        }
    }
    
    public bool IsActiveQuest(QuestData questData)
    {
        
        return true;
    }
}

