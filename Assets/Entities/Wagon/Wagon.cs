    using System;
    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    using UnityEngine.EventSystems;

    public class Wagon : MonoBehaviour
{
    
    
    [SerializeField] private GameObject _wheelGameObject;
    [SerializeField] private GameObject _bodyGameObject;
    [SerializeField] private GameObject _suspensionGameObject;
    [SerializeField] private List<GameObject> _mediumWeightObjects = new ();
    [SerializeField] private List<GameObject> _highWeightObjects = new ();

    public Wheel Wheel;
    public Body Body;
    public Suspension Suspension;

    private PlayerWagonStats _wagonStats;

    private void Start()
    {
        _wagonStats = Player.Instance.WagonStats;

        //Загрузка статоу из Player.WagonStats

        Wheel = _wagonStats.Wheel;
        Body = _wagonStats.Body;
        Suspension = _wagonStats.Suspension;
        OnWeightChange(Player.Instance.Inventory.CurrentTotalWeight, Player.Instance.Inventory.MaxTotalWeight);
        //_qualityModifier = Wheel.QualityModifier;
    }

    private void OnEnable()
    {
        Player.Instance.WagonStats.WagonStatsRefreshed += OnWagonStatsRefreshed;
        Player.Instance.Inventory.WeightChanged += OnWeightChange;
    }

    private void OnDisable()
    {
        Player.Instance.WagonStats.WagonStatsRefreshed -= OnWagonStatsRefreshed;
        Player.Instance.Inventory.WeightChanged -= OnWeightChange;
    }

    private void OnWeightChange(float currentWeight, float maxWeight)
    {
        if (currentWeight >= 0.9f * maxWeight)
        {
            foreach (var gameObject in _mediumWeightObjects)
                gameObject.SetActive(true);
            
            foreach (var gameObject in _highWeightObjects)
                gameObject.SetActive(true);
        }
        else if (currentWeight >= 0.5f * maxWeight)
        {
            foreach (var gameObject in _mediumWeightObjects)
                gameObject.SetActive(true);
            
            foreach (var gameObject in _highWeightObjects)
                gameObject.SetActive(false);
        }
        else
        {
            foreach (var gameObject in _mediumWeightObjects)
                gameObject.SetActive(false);
            
            foreach (var gameObject in _highWeightObjects)
                gameObject.SetActive(false);
        }
    }

    private void OnWagonStatsRefreshed()
    {
        Wheel = _wagonStats.Wheel;
        Body = _wagonStats.Body;
        Suspension = _wagonStats.Suspension;

        _wheelGameObject.GetComponent<SpriteRenderer>().sprite = Wheel.Sprite;
        _bodyGameObject.GetComponent<SpriteRenderer>().sprite = Body.Sprite;
        _suspensionGameObject.GetComponent<SpriteRenderer>().sprite = Suspension.Sprite;
    }

    
}
