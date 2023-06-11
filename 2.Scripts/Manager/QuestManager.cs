using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// 현재 씬의 QuestDataBase를 링크 걸어놓아야함.
/// 퀘스트로인해 변경되어야할 ui들은 uiUpdateEvent delegate에 구독해놓아야함.
/// Update에서 지속적으로 퀘스트의 진행도를 체크
/// </summary>
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

