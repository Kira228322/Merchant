using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLinePanel : MonoBehaviour
{
    public string QuestLineName;
    public string QuestLineDescription;
    public int DayStartedOn;
    public int HourStartedOn;

    public int DayFinishedOn;
    public int HourFinishedOn;

    public void Initialize(QuestLine questLine)
    {
        QuestLineName = questLine.QuestLineName;
        QuestLineDescription = questLine.QuestLineDescription;

        //TODO: (Day/Hour)(Started/Finished)On. Ќадо ли вообще? я думаю использовать дл€ сортировки
    }
}
