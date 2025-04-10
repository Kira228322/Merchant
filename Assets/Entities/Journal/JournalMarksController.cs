using UnityEngine;

public class JournalMarksController : MonoBehaviour
{
    private static JournalMarksController Instance;
    [SerializeField] private GameObject _mainRedPoint;
    [SerializeField] private GameObject _questsRedPoint;
    [SerializeField] private GameObject _complitedQuestRedPoint;
    [SerializeField] private GameObject _statsRedPoint;

    private void Start()
    {
        QuestHandler.QuestChangedState += CheckQuests;
        Player.Instance.Experience.SkillPointsChanged += CheckUnspentSkillPoints;
    }
    private void OnDisable()
    {
        QuestHandler.QuestChangedState -= CheckQuests;
        Player.Instance.Experience.SkillPointsChanged -= CheckUnspentSkillPoints;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public static void CheckQuests(Quest quest)
    {
        if (QuestHandler.AnyUncollectedRewards())
        {
            Instance._mainRedPoint.SetActive(true);
            Instance._questsRedPoint.SetActive(true);
            Instance._complitedQuestRedPoint.SetActive(true);
        }
        else
        {
            Instance._mainRedPoint.SetActive(false);
            Instance._questsRedPoint.SetActive(false);
            Instance._complitedQuestRedPoint.SetActive(false);
        }
    }

    public static void CheckUnspentSkillPoints()
    {
        if (Player.Instance.Experience.AnyUnspentSkillPoints())
        {
            Instance._mainRedPoint.SetActive(true);
            Instance._statsRedPoint.SetActive(true);
        }
        else
        {
            Instance._mainRedPoint.SetActive(false);
            Instance._statsRedPoint.SetActive(false);
        }
    }
}
