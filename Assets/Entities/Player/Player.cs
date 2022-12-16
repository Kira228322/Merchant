using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static Player Singleton;

    [SerializeField] private SlidersController _hungerScale;
    [SerializeField] private SlidersController _sleepScale;

    [Tooltip("—колько нужно минут, чтобы голод уменьшилс€ на 1")] [SerializeField] int _hungerDecayRate;
    [Tooltip("¬о сколько раз замедл€етс€ падение голода во врем€ сна")] [SerializeField] int _hungerDivisorWhenSleeping;
    [Tooltip("—колько нужно минут, чтобы сон уменьшилс€ на 1")] [SerializeField] int _sleepDecayRate;
    [Tooltip("—колько нужно минут во врем€ сна, чтобы сон восстановилс€ на 1")] [SerializeField] int _sleepRestorationRate;  
   
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
        Singleton = this;
    }

    private void Start()
    {
        _inventory = FindObjectOfType<PlayersInventory>(true);

        _playerMover = GetComponent<PlayerMover>();

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
        _hungerDecayRate *= _hungerDivisorWhenSleeping; //голод замедл€етс€ в _hungerMultiplierWhenSleeping раз
        _hoursLeftToSleep = hours;
        IsSleeping = true;
    }
    public void StopSleeping()
    {
        _hungerDecayRate /= _hungerDivisorWhenSleeping; //голод возвращаетс€ в предыдущую скорость падени€
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
        Money = player.Money;
        MaxSleep = player.MaxSleep;
    }
}
