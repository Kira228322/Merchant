using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    public Wheel Wheel;
    public Body Body;
    public Suspension Suspension;

    [SerializeField] private SpriteRenderer _wheelSprite;
    [SerializeField] private SpriteRenderer _bodySprite;
    [SerializeField] private SpriteRenderer _suspensionSprite;
    
    private float _qualityModifier; // при вычислении качества дороги после поездки использовать этот параметр 

    private void Start()
    {
        //«агрузка статоу из Player.WagonStats

        Wheel = Player.Singleton.WagonStats.Wheel;
        Body = Player.Singleton.WagonStats.Body;
        Suspension = Player.Singleton.WagonStats.Suspension;

        _qualityModifier = Wheel.QualityModifier;
    }

    private void OnEnable()
    {
        Player.Singleton.WagonStats.WagonStatsRefreshed += OnWagonStatsRefreshed;
    }
    private void OnDisable()
    {
        Player.Singleton.WagonStats.WagonStatsRefreshed -= OnWagonStatsRefreshed;
    }

    private void OnWagonStatsRefreshed()
    {
        _wheelSprite.sprite = Wheel.Sprite;
        _bodySprite.sprite = Body.Sprite;
        _suspensionSprite.sprite = Suspension.Sprite;

        _qualityModifier = Wheel.QualityModifier;
    }
}
