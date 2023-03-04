using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
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

    private void OnSceneChange(bool isTravellingScene)
    {
        HidePlayer(isTravellingScene);
        SetSpawnPosition();
        
        if (isTravellingScene)
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
        Vector3 point = FindObjectOfType<SpawPoint>().gameObject.transform.position;
        transform.position = new Vector3(point.x, point.y, point.z); 
    }
    private void OnEnable()
    {
        GameTime.MinuteChanged += OnMinuteChanged;
        _transiter.SceneChanged += OnSceneChange;
    }
    private void OnDisable() 
    { 
        GameTime.MinuteChanged -= OnMinuteChanged;
        _transiter.SceneChanged -= OnSceneChange;
    }
    private void OnMinuteChanged()
    {
        Needs.UpdateNeeds();
    }

    public void AddExperience(int amount)
    {
        Experience.AddExperience(amount);
    }

    public void SaveData(SaveData saveData)
    {
        saveData.Player = Instance;
    }

    public void LoadData(Player player)
    {
        Needs = player.Needs;
        
        Money = player.Money;

        
    }
}
