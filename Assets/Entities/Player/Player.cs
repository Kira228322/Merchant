using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static Player Singleton;

    private PlayerMover _playerMover;
    private PlayersInventory _inventory;
    private int _money;
    
    
    public PlayerMover PlayerMover => _playerMover;
    public PlayersInventory Inventory => _inventory;

    public PlayerExperience Experience = new();
    public PlayerStats Statistics = new();
    public PlayerNeeds Needs = new();
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

        if (Singleton == null) 
        {
            Singleton = this; 
        }
        else
        {
            Singleton = this; //�������� ������� �� ������ ����������, ��� �� ��� ������� �������������� ������ ���������.
            PlayerSingletonChanged?.Invoke(); //��� �������� ����� ������� 
        }

        _inventory = FindObjectOfType<PlayersInventory>(true);
        _playerMover = GetComponent<PlayerMover>();
        if (_playerMover == null) Debug.LogError("�� ������� GetComponent<PlayerMover>, �� �� ������� ��� ��� �������� (�� �� ���� ����� ����� �� ������ ���������). ��������, ����� ���-�� ��������� � ����.");
        
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
        saveData.Player = Singleton;
    }

    public void LoadData(Player player)
    {
        Needs = player.Needs;
        
        Money = player.Money;
    }
}
