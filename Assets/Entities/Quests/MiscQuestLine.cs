using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMiscQuestLine", menuName = "Quest/MiscQuestLine")]
public class MiscQuestLine : QuestLine, IResetOnExitPlaymode
{
    public void ResetOnExitPlaymode()
    {
        QuestsInLine.Clear();
    }

    protected override void OnEnable()
    {
        
    }
}
