using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

public class TravelEventHandler : MonoBehaviour
{
    // —крипт, который будет отвечать за генерацию событий в поездке
    [SerializeField] private TravelTimeCounter _timeCounter;
    [SerializeField] private EventWindow _eventWindow;
    [SerializeField] private Mover _mover;
    [SerializeField] private RandomBGGenerator _generator;

    [SerializeField] private List<EventInTravel> _eventsInTravels = new ();
    [SerializeField] private EventInTravel _eventInTravelBandits;
    [SerializeField] private EventInTravel _eventAdvertisement;
    [SerializeField] private BreakingWindow _breakingWindowPrefab;
    [SerializeField] private Animator _wagonAnimator;
    [SerializeField] private Animator _donkeyAnimator;
    private EventInTravel _nextEvent;
    private Transform _mainCanvas;
    [SerializeField] private Transform _cameraTransform;
    private int _delayToNextEvent;

    private bool _banditEvent;
    private Transform _previousPlayerParent;

    public enum EventMultiplierType {Luck, Diplomacy, Null}

    private void Start()
    {
        _mainCanvas = FindObjectOfType<CanvasWarningGenerator>().gameObject.transform;
    }

    

    public void StartEventIfTimerExpired() 
    {
        _delayToNextEvent--;
        if (_delayToNextEvent == 0)
        {
            EventStart(_nextEvent);
            RollNextEvent();
        }
    }

    public void BreakingItemAfterJourney()
    {
        OnTravelSceneExit();
        
        List<InventoryItem> unverifiedItems = new();
        
        foreach (var item in Player.Instance.Inventory.ItemList)
            unverifiedItems.Add(item);

        List<InventoryItem> deletedItems = new();
        
        float Roadbadness = (100 - MapManager.CurrentRoad.Quality) / Player.Instance.WagonStats.QualityModifier;
        // формула веро€тности сломать предмет хрупкостью 100%
        
        
        while (unverifiedItems.Count > 0)
        {
            InventoryItem randomItem = unverifiedItems[Random.Range(0, unverifiedItems.Count)];
            
            for (int i = 0; i < randomItem.CurrentItemsInAStack; i++)
            {
                if (EventFire(Roadbadness * randomItem.ItemData.Fragility / 100))
                {
                    Roadbadness *= 0.9f - (Player.Instance.WagonStats.QualityModifier - 1) * 0.1f;
                    deletedItems.Add(Instantiate(randomItem));
                    Player.Instance.Inventory.ItemGrid.RemoveItemsFromAStack(randomItem, 1);
                    i--;
                }
            }
            
            unverifiedItems.Remove(randomItem); 
        }
        if (deletedItems.Count > 0)
        {
            GameObject breakingWindow = Instantiate(_breakingWindowPrefab.gameObject, _mainCanvas);
            breakingWindow.GetComponent<BreakingWindow>().Init(deletedItems);
            breakingWindow.transform.position = new Vector3(Screen.width / 2, Screen.height / 2);
        }
        else
        {
            GameTime.SetTimeScale(1);
            MapManager.TransitionToVillageScene();
            FindObjectOfType<TravelTimeCounter>().gameObject.SetActive(false);
        }
    }
    
    private void EventStart(EventInTravel eventInTravel)
    {
        FreezeTravelScene();
        _eventWindow.gameObject.SetActive(true);
        _eventWindow.Init(eventInTravel);
    }

    public void FreezeTravelScene()
    {
        _generator.enabled = false;
        foreach (var cloud in _generator.CloudsOnScene)
        {
            if (cloud != null)
                cloud.enabled = false;
        }
        _mover.enabled = false;
        _wagonAnimator.SetTrigger("Stop/Unstop");
        _donkeyAnimator.SetTrigger("Stop/Unstop");
        GameTime.SetTimeScale(0);
    }

    public void UnFreezeTravelScene()
    {
        _generator.enabled = true;
        foreach (var cloud in _generator.CloudsOnScene)
        {
            if (cloud != null)
                cloud.enabled = true;
        }
        _mover.enabled = true;
        _wagonAnimator.SetTrigger("Stop/Unstop");
        _donkeyAnimator.SetTrigger("Stop/Unstop");
        GameTime.SetTimeScale(GameTime.TimeScaleInTravel);
    }

    public void EventEnd()
    {
        UnFreezeTravelScene();
        StartCoroutine(_eventWindow.EventEnd());
    }

    public void OnTravelSceneEnter()
    {
        _previousPlayerParent = Player.Instance.transform.parent;
        Player.Instance.transform.parent = _cameraTransform;
        Player.Instance.transform.position = Vector3.zero;

        if (MapManager.Advertisement == null)
            MapManager.Advertisement = true;
        else if (MapManager.Advertisement == false)
            MapManager.Advertisement = null;
        
        if (EventFire(MapManager.CurrentRoad.Danger, false, EventMultiplierType.Luck))
            _banditEvent = true;
        else _banditEvent = false;
        
        RollNextEvent();
    }

    public void OnTravelSceneExit()
    {
        Player.Instance.transform.parent = _previousPlayerParent;
    }

    private void RollNextEvent()
    {
        _delayToNextEvent = 2; // событи€ максимум каждые 2 часа или реже  
        while (_delayToNextEvent < _timeCounter.Duration-1) // «а 2 часа до конца поездки ивента быть не может
        {
            if (_banditEvent) // ролим событие разбiйники, если оно должно случитьс€
                if (Random.Range(0, _timeCounter.Duration-2 - _delayToNextEvent) == 0)
                {
                    _banditEvent = false;
                    _nextEvent = _eventInTravelBandits;
                    break;
                }

            if (Random.Range(0, Convert.ToInt32(Math.Floor(
                    28/(Math.Pow(_delayToNextEvent, 0.2f) + Math.Pow(_delayToNextEvent, 0.8f)))) -2) == 0)
            {
                _nextEvent = ChooseEvent();
                break;
            }
            
            _delayToNextEvent++;
        }

        if (_delayToNextEvent == _timeCounter.Duration - 1)
            _delayToNextEvent += 10; // событие не случитс€ 
    }

    private EventInTravel ChooseEvent()
    {
        if (MapManager.Advertisement == true)
        {
            MapManager.Advertisement = false;
            return _eventAdvertisement;
        }

        List<int> index = new();
        int max = 0;
        List<int> randomWeights = new();

        for (int i = 0; i < _eventsInTravels.Count; i++)
            randomWeights.Add(Random.Range(0, _eventsInTravels[i].Weight + 1));

        for (int i = 0; i < _eventsInTravels.Count; i++)
            if (max < randomWeights[i])
                max = randomWeights[i];

        for (int i = 0; i < _eventsInTravels.Count; i++)
            if (max == randomWeights[i])
                index.Add(i);

        return _eventsInTravels[index[Random.Range(0, index.Count)]];
    }

    
    public static bool EventFire(float probability, bool positiveEvent = true, EventMultiplierType luckMultiplier = EventMultiplierType.Luck)
    {
        if (probability < 1)
            probability *= 100; // ≈сли веро€тность по ошибке была написана не в %, а в дол€х
        switch (luckMultiplier)
        {
            case EventMultiplierType.Luck:
                if (positiveEvent)
                {
                    if (Random.Range(0, 101) <= 100 - (100 - probability) / Player.Instance.Statistics.GetCoefForPositiveEvent())
                        return true;
                }
                else
                {
                    if (Random.Range(0, 101) <= probability * Player.Instance.Statistics.GetCoefForNegativeEvent())
                        return true;
                }
                break;
            case EventMultiplierType.Diplomacy:
                if (positiveEvent)
                {
                    if (Random.Range(0, 101) <= 100 - (100 - probability) / Player.Instance.Statistics.GetCoefForDiplomacyPositiveEvent())
                        return true;
                }
                else
                {
                    if (Random.Range(0, 101) <= probability * Player.Instance.Statistics.GetCoefForDiplomacyNegativeEvent())
                        return true;
                }
                break;
            case EventMultiplierType.Null:
                if (Random.Range(0, 101) <= probability)
                    return true;
                break;
        }
        
        return false;
    }
}
