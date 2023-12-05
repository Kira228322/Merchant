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
    [SerializeField] private AudioSource _wagonSound;
    private EventInTravel _nextEvent;
    private Transform _mainCanvas;
    [SerializeField] private Transform _cameraTransform;
    private int _delayToNextEvent;

    private bool _banditEvent;
    private Transform _previousPlayerParent;

    public float RoadBadnessMultiplier = 1;
    private void Start()
    {
        _mainCanvas = CanvasWarningGenerator.Instance.gameObject.transform;
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

    public void ChangeTravelTime(int addTime)
    {
        _timeCounter.ChangeDuraion(addTime);
        RollNextEvent();
    }

    public void BreakingItemAfterJourney()
    {
        OnTravelSceneExit();
        
        List<InventoryItem> unverifiedItems = new();
        
        foreach (var item in Player.Instance.Inventory.ItemList)
            if (!item.ItemData.IsQuestItem)
                unverifiedItems.Add(item);

        List<InventoryItem> deletedItems = new();
        
        float Roadbadness = (100 - MapManager.CurrentRoad.Quality * RoadBadnessMultiplier)
                            / Player.Instance.WagonStats.QualityModifier;
        // формула веро€тности сломать предмет хрупкостью 100%
        
        
        while (unverifiedItems.Count > 0)
        {
            InventoryItem randomItem = unverifiedItems[Random.Range(0, unverifiedItems.Count)];
            
            for (int i = 0; i < randomItem.CurrentItemsInAStack; i++)
            {
                if (EventFire(Roadbadness * randomItem.ItemData.Fragility / 100f))
                {
                    Roadbadness *= 0.98f;
                    deletedItems.Add(Instantiate(randomItem));
                    Player.Instance.Inventory.BaseItemGrid.RemoveItemsFromAStack(randomItem, 1);
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
        _wagonSound.Stop();
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
        _wagonSound.Play();
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
        RoadBadnessMultiplier = 1;
        
        _previousPlayerParent = Player.Instance.transform.parent;
        Player.Instance.transform.parent = _cameraTransform;
        Player.Instance.transform.position = Vector3.zero;

        if (MapManager.Advertisement == null)
            MapManager.Advertisement = true;
        else if (MapManager.Advertisement == false)
            MapManager.Advertisement = null;
        
        if (EventFire(MapManager.CurrentRoad.Danger * MapManager.CurrentRoad.DangerMultiplier))
            _banditEvent = true;
        else _banditEvent = false;
        MapManager.CurrentRoad.SetNormalDangerMultiplier();
        
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

        if (_delayToNextEvent >= _timeCounter.Duration - 1)
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

    
    public static bool EventFire(float probability, bool positiveEvent = true, PlayerStats.PlayerStat playerStat = null)
    {
        if (playerStat != null)
        {

            if (positiveEvent)
            {
                if (Random.Range(0, 101) <= 100 - (100 - probability) / playerStat.GetCoefForPositiveEvent())
                    return true;
            }
            else
            {
                if (Random.Range(0, 101) <= probability * playerStat.GetCoefForNegativeEvent())
                    return true;
            }
        }
        else if (Random.Range(0, 101) <= probability)
            return true;

        return false;
    }
    public static float GetProbability(float probability, PlayerStats.PlayerStat playerStat, bool positiveEvent = true )
    {
        float result;
        
        if (positiveEvent)
        {
            result = (float)Math.Round(100 - (100 - probability) / playerStat.GetCoefForPositiveEvent(), 1);
            if (result < 0)
                return 0;
            return result;
        }

        result = (float)Math.Round(probability * playerStat.GetCoefForNegativeEvent(), 1);
        if (result < 0)
            return 0;
        
        return result;
    }
    
}
