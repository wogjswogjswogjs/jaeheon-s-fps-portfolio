using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/QuestDataBase")]
public class QuestDataBase : ScriptableObject
{
    public QuestData[] questObjects;
}
