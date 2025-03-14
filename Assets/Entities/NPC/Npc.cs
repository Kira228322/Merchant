using UnityEngine;

public class Npc : MonoBehaviour
{
    public NpcData NpcData;
    public TextAsset InkJSON;
    [SerializeField] private GameObject _exclamationMarkPrefab;
    [HideInInspector] public GameObject ExclamationMark;

    public void StartInteraction()
    {
        Player.Instance.PlayerMover.DisableMove();
        if (gameObject.TryGetComponent(out NPCMovement movement))
            movement.MakeNPCBusy();
    }

    public void StopInteraction()
    {
        Player.Instance.PlayerMover.EnableMove();
        if (gameObject.TryGetComponent(out NPCMovement movement))
            movement.NPCMakeFree();
    }

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
        CheckExclamationMark(null);
    }

    public void CheckExclamationMark(Quest quest)
    {
        if (QuestHandler.IsNpcTargetOfAnyActiveQuest(NpcData.ID))
        {
            if (ExclamationMark == null)
            {
                ExclamationMark = Instantiate(_exclamationMarkPrefab, gameObject.transform);
                ExclamationMark.GetComponent<ExclamationMark>().Init(gameObject.GetComponent<BoxCollider2D>());
            }
        }
        else if (ExclamationMark != null)
        {
            Destroy(ExclamationMark);
            ExclamationMark = null;
        }
    }
}
