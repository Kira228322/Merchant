using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestLinePanel : MonoBehaviour
{
    [HideInInspector] public string QuestLineName;
    [HideInInspector] public int DayStartedOn;
    [HideInInspector] public int HourStartedOn;

    [HideInInspector] public int DayFinishedOn;
    [HideInInspector] public int HourFinishedOn;
    
    public Transform ItemContentTransform;

    [SerializeField] private TMP_Text _questLineNameText;

    public void Initialize(QuestLine questLine)
    {
        if (questLine == null)
            QuestLineName = "Разное";
        else
            QuestLineName = questLine.QuestLineName;

        _questLineNameText.text = QuestLineName;

        //TODO: (Day/Hour)(Started/Finished)On. Надо ли вообще? Я думаю использовать для сортировки
    }
}
