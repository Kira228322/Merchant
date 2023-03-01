using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [Header("Wagon stats")]
    //��� ������ �� �������� ������� ��� ��������� ����, �� ����� ����� �� ����������.
    //� ������� ������� PlayerWagonStats [Serializable], �� ��� �������� ��� ��� ����� ������������ � null.
    //��� ��������, � ��� ����� �������� ����� ���������. ��������, � ���-�� �� ��� �� �������.
    [SerializeField] private Wheel _startingWheel;
    [SerializeField] private Body _startingBody;
    [SerializeField] private Suspension _startingSuspension;

    private PlayerMover _playerMover;
    private PlayersInventory _inventory;
    private int _money;

    public PlayerMover PlayerMover => _playerMover;
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

    public static event UnityAction PlayerSingletonChanged;

    public event UnityAction<int> MoneyChanged;


    private void Awake()
    {

/* ���� ����������� ������� ����� �������. ������ ��������� � ��������� ����� �������� � ����� ������ (e.g. Player.Singleton.AddExperience()),
   ������ ������ ������� �����������. ���� �����-������ ��� DontDestroyOnLoad ������������� �� ����� ���������� Player.Singleton,
   �� ��� ��������� ����� PlayerSingleton �� �� ������� �������� � ��������� �������� ����� �� ��� ������������ ����������.
   ������� �������� ����� �������� *�����������* ����� PlayerSingletonChanged � ��� ��������, ������� ������������� �� ������ ������,
   ������� ����� �������� �� ���� �����, ��� ������������ ����� ������ ��������� ������ �� ������� ��������.
*/

        if (Instance == null) 
        {
            Instance = this; 
        }
        else
        {
            Instance = this; //�������� ������� �� ������ ����������, ��� �� ��� ������� �������������� ������ ���������.
            PlayerSingletonChanged?.Invoke(); //��� �������� ����� ������� 
        }


        _playerMover = GetComponent<PlayerMover>();
        _inventory = FindObjectOfType<PlayersInventory>(true);

        WagonStats = new(_startingWheel, _startingBody, _startingSuspension);
    }

    private void Start()
    {
        Needs.Initialize();
    }

    private void OnEnable()
    {
        GameTime.MinuteChanged += OnMinuteChanged;
    }
    private void OnDisable() 
    { 
        GameTime.MinuteChanged -= OnMinuteChanged;
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
