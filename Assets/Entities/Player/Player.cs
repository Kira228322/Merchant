using System.Collections.Generic;
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
        Money = 9999; // TODO  тест 
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
            GlobalEventHandler.Instance.AddGlobalEvent<Test_GlobalEvent>(12);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuestHandler.AddQuest(PregenQuestDatabase.GetQuestParams("collect3apples_wait3hours"));
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Current Location: " + MapManager.CurrentLocation.SceneName);
            /*RegionHandler regionHandler = FindObjectOfType<RegionHandler>(true);
            foreach(Region region in regionHandler.Regions)
            {
                foreach (Location location in region.Locations)
                {
                    Debug.Log(location.VillageName +": ");
                    foreach (var item in location.CountOfEachItem)
                    {
                        Debug.Log(item.Key + " " + item.Value);
                    }
                }
            }*/
        }

    }

}
