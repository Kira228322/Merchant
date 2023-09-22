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

    // private float _qualityModifier; // Перемещено в PlayerWagonStats.QualityModifier

    private void Start()
    {
        _wagonStats = Player.Instance.WagonStats;
        
        //^ из-за порядка выполнения скриптов мы не можем помещать подписку на ивент в OnEnable (см. пост #5 https://forum.unity.com/threads/onenable-before-awake.361429/)
        //Поэтому подписываемся в старте, после того как Player и его подсосы выполнят Awake.
        //Единственная проблема может быть тем, что Start срабатывает только один раз при спавне этого скрипта.
        //Нужно будет проверять, правильно ли подписывается, если будем включать/выключать Вагон

        //Загрузка статоу из Player.WagonStats

        Wheel = _wagonStats.Wheel;
        Body = _wagonStats.Body;
        Suspension = _wagonStats.Suspension;

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
