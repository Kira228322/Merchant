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
        if (QuestHandler.IsQuestActiveForThisNPC(NpcData.ID))
        {
            BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
            ExclamationMark = Instantiate(_exclamationMarkPrefab, gameObject.transform);
            ExclamationMark.transform.position = collider.bounds.center + new Vector3(0,collider.bounds.size.y);
        }
        else if (ExclamationMark != null)
        {
            Destroy(ExclamationMark);
        }
    }
}
