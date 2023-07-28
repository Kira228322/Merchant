using System;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public NpcData NpcData;
    [SerializeField] private GameObject _exclamationMarkPrefab;
    public GameObject ExclamationMark;


    private void OnEnable()
    {
        QuestHandler.QuestChangedState += CheckExclamationMark;
    }
    
    private void OnDisable()
    {
        QuestHandler.QuestChangedState -= CheckExclamationMark;
    }

    private void Start()
    {
        CheckExclamationMark();
    }

    private void CheckExclamationMark()
    {
        if (QuestHandler.GetActiveQuestsForThisNPC(NpcData.ID).Count > 0)
        {
            ExclamationMark = Instantiate(_exclamationMarkPrefab, gameObject.transform);
            ExclamationMark.GetComponent<ExclamationMark>().Init(gameObject.GetComponent<BoxCollider2D>());
        }
        else if (ExclamationMark != null)
        {
            Destroy(ExclamationMark);
        }
    }
}
