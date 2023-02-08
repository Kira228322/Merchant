using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    public Wheel Wheel;
    public Body Body;
    public Suspension Suspension;

    private PlayerWagonStats _wagonStats;

    // private float _qualityModifier; // Перемещено в PlayerWagonStats.QualityModifier

    private void Start()
    {
        _wagonStats = Player.Singleton.WagonStats;
        Player.Singleton.WagonStats.WagonStatsRefreshed += OnWagonStatsRefreshed;
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

    private void OnDisable()
    {
        Player.Singleton.WagonStats.WagonStatsRefreshed -= OnWagonStatsRefreshed;
    }

    private void OnWagonStatsRefreshed()
    {
        Wheel = _wagonStats.Wheel;
        Body = _wagonStats.Body;
        Suspension = _wagonStats.Suspension;

        Wheel.gameObject.GetComponent<SpriteRenderer>().sprite = Wheel.Sprite;
        Body.gameObject.GetComponent<SpriteRenderer>().sprite = Body.Sprite;
        Suspension.gameObject.GetComponent<SpriteRenderer>().sprite = Suspension.Sprite;


    }
}
