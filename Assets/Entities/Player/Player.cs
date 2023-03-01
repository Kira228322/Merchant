using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [Header("Wagon stats")]
    //Мне совсем не хотелось ставить эти параметры сюда, но иначе никак не получается.
    //Я пытался сделать PlayerWagonStats [Serializable], но все значения так или иначе сбрасываются в null.
    //Все значения, в том числе заданные через инспектор. Возможно, я что-то всё ещё не понимаю.
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

/* Надо тестировать переход между сценами. Прямое обращение к синглтону будет работать в любом случае (e.g. Player.Singleton.AddExperience()),
   однако ивенты ловятся неправильно. Если какой-нибудь наш DontDestroyOnLoad подписывается на ивент экземпляра Player.Singleton,
   то при изменении этого PlayerSingleton он не обновит подписку и продолжит получать ивент от уже исчезнувшего экземпляра.
   Поэтому наверное стоит добавить *статический* ивент PlayerSingletonChanged и для объектов, которые подписываются на ивенты игрока,
   сделать также подписку на этот ивент, при срабатывании этого ивента обновлять ссылку на текущий синглтон.
*/

        if (Instance == null) 
        {
            Instance = this; 
        }
        else
        {
            Instance = this; //Возлагаю надежды на гарбаж коллектора, что он сам удаляет неиспользуемый старый экземпляр.
            PlayerSingletonChanged?.Invoke(); //При переходе между сценами 
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
