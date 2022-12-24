using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static Player Singleton;

    [SerializeField] private SlidersController _hungerScale;
    [SerializeField] private SlidersController _sleepScale;

    [Tooltip("Сколько нужно минут, чтобы голод уменьшился на 1")] [SerializeField] int _hungerDecayRate;
    [Tooltip("Во сколько раз замедляется падение голода во время сна")] [SerializeField] int _hungerDivisorWhenSleeping;
    [Tooltip("Сколько нужно минут, чтобы сон уменьшился на 1")] [SerializeField] int _sleepDecayRate;
    [Tooltip("Сколько нужно минут во время сна, чтобы сон восстановился на 1")] [SerializeField] int _sleepRestorationRate;  
   
	[SerializeField] private int _currentHunger; 
    [SerializeField] private int _currentSleep;

    private PlayerMover _playerMover;
    private PlayersInventory _inventory;
    private int _money;
    private int _generalTimeCounter = 0;
    private int _timeCounterWhenSleeping = 0;
    private int _hoursLeftToSleep = 0;
    
    public PlayerMover PlayerMover => _playerMover;
    public PlayersInventory Inventory => _inventory;

    public int MaxHunger;
    public int MaxSleep;

    public PlayerExperience Experience = new();
    public PlayerStats Statistics = new();

    public static event UnityAction PlayerSingletonChanged;

    public event UnityAction<int> MoneyChanged;

    public event UnityAction SleptOneHourEvent;
    public event UnityAction FinishedSleeping;

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
    public int CurrentHunger 
    {
        get => _currentHunger;
        set
        {
            _currentHunger = value;
            if (_currentHunger < 0)
            {
                _currentHunger = 0;
            }
            if (_currentHunger > MaxHunger) 
            {
                _currentHunger = MaxHunger;
            }
            _hungerScale.SetValue(CurrentHunger, MaxHunger);
        }
    }
    public int CurrentSleep
    {
        get => _currentSleep;
        set
        {
            _currentSleep = value;
            if (_currentSleep < 0)
            {
                _currentSleep = 0;
            }
            if (_currentSleep > MaxSleep)
            {
                _currentSleep = MaxSleep;
            }
            _sleepScale.SetValue(CurrentSleep, MaxSleep);
        }
    }
    public bool IsSleeping;

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

        _inventory = FindObjectOfType<PlayersInventory>(true);
        _playerMover = GetComponent<PlayerMover>();
        if (_playerMover == null) Debug.LogError("Ты делаешь GetComponent<PlayerMover>, но ты делаешь его без уважения (но на этой сцене игрок не должен двигаться). Наверное, нужно что-то исправить в коде.");
        
    }

    private void Start()
    {
        _sleepScale.SetValue(CurrentSleep, MaxSleep);
        _hungerScale.SetValue(CurrentHunger, MaxHunger);
    }


    private void OnEnable()
    {
        GameTime.MinuteChanged += OnMinuteChanged;
    }
    private void OnDisable() 
    { 
        GameTime.MinuteChanged -= OnMinuteChanged;
    }
    public void StartSleeping(int hours)
    {
        _hungerDecayRate *= _hungerDivisorWhenSleeping; //голод замедляется в _hungerMultiplierWhenSleeping раз
        _hoursLeftToSleep = hours;
        IsSleeping = true;
    }
    public void StopSleeping()
    {
        _hungerDecayRate /= _hungerDivisorWhenSleeping; //голод возвращается в предыдущую скорость падения
        IsSleeping = false;
        FinishedSleeping?.Invoke();
    }

    public void RestoreHunger(int foodValue)
    {
        CurrentHunger += foodValue;
    }

    private void SleptOneHour()
    {
        _hoursLeftToSleep--;

        if (_hoursLeftToSleep <= 0)
        {
            StopSleeping();
        }
        SleptOneHourEvent?.Invoke();
    }
    
    private void OnMinuteChanged()
    {
        _generalTimeCounter++;
        if (_generalTimeCounter % _hungerDecayRate == 0)
        {
            CurrentHunger--;
        }
        if (IsSleeping)
        {
            _timeCounterWhenSleeping++;
            if (_timeCounterWhenSleeping % _sleepRestorationRate == 0)
            {
                CurrentSleep++;
            }
            if (_timeCounterWhenSleeping == 60)
            {
                _timeCounterWhenSleeping = 0;
                SleptOneHour();
            }
        }
        else
        {
            if (_generalTimeCounter % _sleepDecayRate == 0)
            {
                CurrentSleep--;
            }
        }
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
        _hungerScale = player._hungerScale;
        _sleepScale = player._sleepScale;
        
        _hungerDecayRate = player._hungerDecayRate;
        _hungerDivisorWhenSleeping = player._hungerDivisorWhenSleeping;
        _sleepDecayRate = player._sleepDecayRate;
        _sleepRestorationRate = player._sleepRestorationRate;
        
        _currentHunger = player._currentHunger; 
        _currentSleep = player._currentSleep;
        MaxSleep = player.MaxSleep;
        MaxHunger = player.MaxHunger;
        
        Money = player.Money;
    }
}
