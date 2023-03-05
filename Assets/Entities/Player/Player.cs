using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, ISaveable
{
    public static Player Instance;

    [Header("Wagon stats")]
    //ћне совсем не хотелось ставить эти параметры сюда, но иначе никак не получаетс€.
    //я пыталс€ сделать PlayerWagonStats [Serializable], но все значени€ так или иначе сбрасываютс€ в null.
    //¬се значени€, в том числе заданные через инспектор. ¬озможно, € что-то всЄ ещЄ не понимаю.
    [SerializeField] private Wheel _startingWheel;
    [SerializeField] private Body _startingBody;
    [SerializeField] private Suspension _startingSuspension;

    [SerializeField] private SceneTransiter _transiter;
    
    private PlayerMover _playerMover;
    private PlayersInventory _inventory;
    private int _money;

    public PlayersInventory Inventory => _inventory;

    public ItemGrid ItemGrid => Inventory.ItemGrid;

    public PlayerExperience Experience = new();
    public PlayerStats Statistics = new();
    public PlayerNeeds Needs = new();
    public PlayerWagonStats WagonStats;
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

        WagonStats = new(_startingWheel, _startingBody, _startingSuspension);
    }

    private void Start()
    {
        Needs.Initialize();
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

    public void SaveData()
    {
        SaveLoadSystem<int>.SaveData(Money, "money");
    }

    public void LoadData()
    {
        Money = SaveLoadSystem<int>.LoadData("money");
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Money++;
            Debug.Log(Money);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveData();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadData();
        }
    }
}
