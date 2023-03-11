using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, ISaveable<PlayerData>
{
    public static Player Instance;

    [SerializeField] private SceneTransiter _transiter;
    
    private PlayerMover _playerMover;
    private PlayersInventory _inventory;
    private int _money;

    public PlayersInventory Inventory => _inventory;

    public ItemGrid ItemGrid => Inventory.ItemGrid;

    public PlayerExperience Experience = new();
    public PlayerStats Statistics = new();
    public PlayerNeeds Needs = new();
    public PlayerWagonStats WagonStats = new();
    public int Money 
    { 
        get => _money;
        set
        {
            _money = value;
            if (_money < 0)
            {
                _money = 0;
            }
            MoneyChanged?.Invoke(Money);
        }
    }

    public event UnityAction<int> MoneyChanged;


    private void Awake()
    {
        
        Instance = this;

        _inventory = FindObjectOfType<PlayersInventory>(true);
    }

    private void Start()
    {
        Needs.Initialize();
        WagonStats.RecalculateValues();
    }

    private void OnSceneChange(Scene scene, LoadSceneMode mode)
    {
        HidePlayer(MapManager.IsActiveSceneTravel);
        SetSpawnPosition();

        if (MapManager.IsActiveSceneTravel)
        {
            GetComponent<Rigidbody2D>().simulated = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<PlayerMover>().enabled = false;
        }
        else
        {
            GetComponent<Rigidbody2D>().simulated = true;
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<PlayerMover>().enabled = true;
        }
    }

    private void HidePlayer(bool state)
    {
        Color color = GetComponent<SpriteRenderer>().color;
        color = new Color(color.r, color.g, color.b, state?  0: 1);
        GetComponent<SpriteRenderer>().color = color;
    }

    private void SetSpawnPosition()
    {
        GameObject point = GameObject.FindWithTag("SpawnPoint");
        if (point != null)
        {
            transform.position = new Vector3(point.transform.position.x, point.transform.position.y, 0);
        }
    }
    private void OnEnable()
    {
        GameTime.MinuteChanged += OnMinuteChanged;
        SceneManager.sceneLoaded += OnSceneChange;
    }
    private void OnDisable() 
    { 
        GameTime.MinuteChanged -= OnMinuteChanged;
        SceneManager.sceneLoaded -= OnSceneChange;
    }
    private void OnMinuteChanged()
    {
        Needs.UpdateNeeds();
    }

    public void AddExperience(int amount)
    {
        Experience.AddExperience(amount);
    }

    public PlayerData SaveData()
    {
        PlayerData saveData = new(this);
        return saveData;
    }

    public void LoadData(PlayerData saveData)
    {
        Money = saveData.Money;
        WagonStats.LoadData(saveData.WagonStats);
        Inventory.LoadData(saveData.Inventory);
        Experience = saveData.Experience;
        Needs.LoadData(saveData.Needs);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Experience.AddExperience(100);

            Debug.Log(Experience.CurrentExperience);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveLoadSystem<PlayerData>.SaveAll();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Quest.QuestParams questParams = new();
            questParams.currentState = Quest.State.Active;
            questParams.questName = "TestName";
            questParams.questSummary = "TestQuestSummary";
            questParams.description = "TestDescription";
            questParams.experienceReward = 69;
            questParams.moneyReward = 96;

            List<Quest.ItemReward> itemRewards = new();
            itemRewards.Add(new Quest.ItemReward("Sugus", 2, 0));
            questParams.itemRewards = itemRewards;

            List<Goal> goals = new();


            WaitingGoal goal = new(Goal.State.Active, "Подождать за 60 секунд (3 часа)", 0, 3);

            goals.Add(goal);

            questParams.nextQuestParams = new();

            questParams.nextQuestParams.currentState = Quest.State.Active;
            questParams.nextQuestParams.questName = "TestName2";
            questParams.nextQuestParams.questSummary = "TestQuestSummary2";
            questParams.nextQuestParams.description = "TestDescription2";
            questParams.nextQuestParams.experienceReward = 669;
            questParams.nextQuestParams.moneyReward = 696;

            List<Quest.ItemReward> nextItemRewards = new();
            nextItemRewards.Add(new Quest.ItemReward("Jam", 2, 0));
            questParams.nextQuestParams.itemRewards = nextItemRewards;

            List<Goal> nextGoals = new();


            WaitingGoal nextGoal = new(Goal.State.Active, "Подождать за 60 секунд (3 часа)", 0, 3);

            nextGoals.Add(nextGoal);
            questParams.goals = goals;
            questParams.nextQuestParams.goals = nextGoals;

            QuestHandler.AddQuest(questParams);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveLoadSystem<PlayerData>.LoadAll();
        }
    }

}
