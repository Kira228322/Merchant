using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private Color _activeColor;
    [SerializeField] private Color _completedColor;

    [SerializeField] private GameObject _redPoint;

    public void Initialize(QuestLine questLine)
    {
        if (questLine == null)
            QuestLineName = "Разное";
        else
        {
            QuestLineName = questLine.QuestLineName;
        }
        _questLineNameText.text = QuestLineName;
    }

    public void Refresh()
    {
        QuestPanel[] questPanels = ItemContentTransform.GetComponentsInChildren<QuestPanel>();
        if (questPanels.Length != 0 && questPanels.All(questPanel => questPanel.Quest.CurrentState == Quest.State.Completed))
        {
            _questLineNameText.color = _completedColor;
        }
        else
        {
            _questLineNameText.color = _activeColor;
            if (questPanels.Any(questPanel => questPanel.Quest.CurrentState == Quest.State.RewardUncollected))
            {
                _redPoint.gameObject.SetActive(true);
            }
            else
            {
                _redPoint.gameObject.SetActive(false);
            }
        }
    }

}
