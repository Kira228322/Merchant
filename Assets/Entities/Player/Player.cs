using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, ISaveable<PlayerData>
{
    public static Player Instance;

    [SerializeField] private SceneTransiter _transiter;

    [HideInInspector] public PlayerMover PlayerMover;
    private PlayersInventory _inventory;
    private int _money;

    public PlayersInventory Inventory => _inventory;

    public ItemGrid BaseItemGrid => Inventory.BaseItemGrid;
    public ItemGrid QuestItemGrid => Inventory.QuestItemGrid;

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
        }
    }

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

        Needs.HungerDecayRate = Int32.MaxValue;
        Needs.SleepDecayRate = Int32.MaxValue;

        Statistics.Toughness.StatChanged += Statistics.OnToughnessChanged;
    }

    private void OnDestroy()
    {
        Statistics.Toughness.StatChanged -= Statistics.OnToughnessChanged;
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
        GameTime.TimeSkipped += OnTimeSkipped;
        SceneManager.sceneLoaded += OnSceneChange;
    }
    private void OnDisable()
    {
        GameTime.MinuteChanged -= OnMinuteChanged;
        GameTime.TimeSkipped -= OnTimeSkipped;
        SceneManager.sceneLoaded -= OnSceneChange;
    }
    private void OnMinuteChanged()
    {
        Needs.UpdateNeeds();
    }
    private void OnTimeSkipped(int daysSkipped, int hoursSkipped, int minutesSkipped)
    {
        int totalMinutesSkipped = (daysSkipped * 24 + hoursSkipped) * 60 + minutesSkipped;
        Needs.SkipNeeds(totalMinutesSkipped);
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
}