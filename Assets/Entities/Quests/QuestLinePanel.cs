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

        if (questPanels.Length == 0)
        {
            _questLineNameText.color = _activeColor;
            return;
        }

        bool allCompleted = questPanels.All(questPanel => questPanel.Quest.CurrentState is Quest.State.Completed or Quest.State.Failed);
        bool anyRewardUncollected = questPanels.Any(questPanel => questPanel.Quest.CurrentState == Quest.State.RewardUncollected);

        _questLineNameText.color = allCompleted ? _completedColor : _activeColor;

        if (!allCompleted && anyRewardUncollected)
            _redPoint.SetActive(true);
        else
            _redPoint.SetActive(false);
    }

}
