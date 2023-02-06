using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static Player Singleton;

    private PlayerMover _playerMover;
    private PlayersInventory _inventory;
    private PlayerWagonStats _wagonStats;
    private int _money;

    public PlayerMover PlayerMover => _playerMover;
    public PlayersInventory Inventory => _inventory;
    public PlayerWagonStats WagonStats => _wagonStats;

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

/* Надо тестировать переход между сценами. Прямое обращение к синглтону будет работать в любом случае (e.g. Player.Singleton.AddExperience()),
   однако ивенты ловятся неправильно. Если какой-нибудь наш DontDestroyOnLoad подписывается на ивент экземпляра Player.Singleton,
   то при изменении этого PlayerSingleton он не обновит подписку и продолжит получать ивент от уже исчезнувшего экземпляра.
   Поэтому наверное стоит добавить *статический* ивент PlayerSingletonChanged и для объектов, которые подписываются на ивенты игрока,
   сделать также подписку на этот ивент, при срабатывании этого ивента обновлять ссылку на текущий синглтон.
*/

        if (Singleton == null) 
        {
            Singleton = this; 
        }
        else
        {
            Singleton = this; //Возлагаю надежды на гарбаж коллектора, что он сам удаляет неиспользуемый старый экземпляр.
            PlayerSingletonChanged?.Invoke(); //При переходе между сценами 
        }


        _playerMover = GetComponent<PlayerMover>(); //Висит на Player не всегда, есть сцены без движения, поэтому нехорошо
        _wagonStats = GetComponent<PlayerWagonStats>(); //Хранит только данные, висит всегда
        _inventory = FindObjectOfType<PlayersInventory>(true); //Допустимо, ведь инвентарь в целом всегда доступен 
        
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
