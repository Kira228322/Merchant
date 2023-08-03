using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, ISaveable<PlayerData>
{
    public static Player Instance;

    [SerializeField] private SceneTransiter _transiter;

    [HideInInspector] public PlayerMover PlayerMover;
    private PlayersInventory _inventory;
    private int _money;

    public PlayersInventory Inventory => _inventory;

    public ItemGrid ItemGrid => Inventory.ItemGrid;

    public PlayerExperience Experience = new();
    public PlayerStats Statistics = new();
    public PlayerNeeds Needs = new();
    public PlayerWagonStats WagonStats = new();
    public List<CraftingRecipe> Recipes = new();
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
        PlayerMover = GetComponent<PlayerMover>();

        _inventory = FindObjectOfType<PlayersInventory>(true);
    }

    private void Start()
    {
        Needs.Initialize();
        WagonStats.RecalculateValues();
        Statistics.OnToughnessChanged();
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

    public void HidePlayer(bool state)
    {
        Color color = GetComponent<SpriteRenderer>().color;
        color = new Color(color.r, color.g, color.b, state ? 0 : 1);
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
        return new PlayerData(this);
    }

    public void LoadData(PlayerData saveData)
    {
        Money = saveData.Money;
        WagonStats.LoadData(saveData.WagonStats);
        Inventory.LoadData(saveData.Inventory);
        Statistics.LoadData(saveData.PlayerStats);
        Experience = saveData.Experience;
        Needs.LoadData(saveData.Needs);
        foreach (string recipeName in saveData.Recipes.recipeNames)
        {
            Recipes.Add(CraftingRecipeDatabase.GetRecipe(recipeName));
        }
    }
    private void Update()
    {
        //TODO Тестовые команды
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Experience.AddExperience(100);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Test Save");
            FindObjectOfType<GameManager>().SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Test Load");
            FindObjectOfType<GameManager>().LoadGame();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            FindObjectOfType<BannedItemEventController>().AddEvent();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuestHandler.AddQuest(PregenQuestDatabase.GetQuestParams("collect3apples_wait3hours"));
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Current Active Global Events:");
            foreach (var globalEvent in GlobalEventHandler.Instance.ActiveGlobalEvents)
            {
                Debug.Log(globalEvent.GlobalEventName + " " + globalEvent.DurationHours);
            }

            Debug.Log("BannedItemController Prediction:");
            {
                BannedItemEventController bannedItemEventController = FindObjectOfType<BannedItemEventController>();
                Debug.Log($"{bannedItemEventController.ItemToBan}, on day {bannedItemEventController.DateOfNextEvent}, hour {bannedItemEventController.HourOfNextEvent}, for {bannedItemEventController.DurationOfEvent} days");
            }

            if (GlobalEventHandler.Instance.IsEventActive<GlobalEvent_BannedItem>())
            {
                GlobalEvent_BannedItem bannedEvent = GlobalEventHandler.Instance.GetActiveEventByType<GlobalEvent_BannedItem>();
                Debug.Log($"Banned Item: {bannedEvent.BannedItemName}, {bannedEvent.DurationHours} часа осталось");
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            QuestHandler.AddQuest(PregenQuestDatabase.GetQuestParams("collect3apples_wait3hours"));
        }






    }

}