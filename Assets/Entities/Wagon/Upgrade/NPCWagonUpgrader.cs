using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCWagonUpgrader : NPC
{
    [SerializeField] private List<Wheel> _wheelProgression;             
    [SerializeField] private List<Body> _bodyProgression;               
    [SerializeField] private List<Suspension> _suspensionProgression;   

    private PlayerWagonStats _wagonStats;

    public List<WagonPart> WagonUpgrades = new List<WagonPart>();

    protected void Start()
    {
        _wagonStats = Player.Instance.WagonStats;
        
        WagonUpgraderRestock();
    }

    public void Interact() // TODO
    {
        //Тачку на прокачку
        //Открыть меню с текущими статами каждой запчасти и статами следующей запчасти, с кнопкой купить.
        //По поводу интеракта сделаю позже, сейчас тесты:
        Upgrade(_wagonStats.Wheel);
        Upgrade(_wagonStats.Body);
        Upgrade(_wagonStats.Suspension);
    }
    
    

    private void WagonUpgraderRestock()
    {
        WagonUpgrades.Clear();
        
        if (TravelEventHandler.EventFire(80, true, TravelEventHandler.EventMultiplierType.Diplomacy))
            WagonUpgrades.Add(WagonPartDatabase.GetWagonPartByLvl<Body>(Player.Instance.WagonStats.Body.Level + 1));
        
        if (TravelEventHandler.EventFire(80, true, TravelEventHandler.EventMultiplierType.Diplomacy))
            WagonUpgrades.Add(WagonPartDatabase.GetWagonPartByLvl<Suspension>(Player.Instance.WagonStats.Suspension.Level + 1));
        
        if (TravelEventHandler.EventFire(80, true, TravelEventHandler.EventMultiplierType.Diplomacy))
            WagonUpgrades.Add(WagonPartDatabase.GetWagonPartByLvl<Wheel>(Player.Instance.WagonStats.Wheel.Level + 1));
        
        if (TravelEventHandler.EventFire(15, true, TravelEventHandler.EventMultiplierType.Diplomacy))
            WagonUpgrades.Add(WagonPartDatabase.GetWagonPartByLvl<Wheel>(Player.Instance.WagonStats.Wheel.Level + 2));
        
        if (TravelEventHandler.EventFire(15, true, TravelEventHandler.EventMultiplierType.Diplomacy))
            WagonUpgrades.Add(WagonPartDatabase.GetWagonPartByLvl<Wheel>(Player.Instance.WagonStats.Wheel.Level + 2));
        
        if (TravelEventHandler.EventFire(15, true, TravelEventHandler.EventMultiplierType.Diplomacy))
            WagonUpgrades.Add(WagonPartDatabase.GetWagonPartByLvl<Wheel>(Player.Instance.WagonStats.Wheel.Level + 2));
        
    }

    public void Upgrade(WagonPart wagonPart)
    {
        switch (wagonPart)
        {
            case Wheel:
                for (int i = 0; i < _wheelProgression.Count - 1; i++)
                {
                    if (wagonPart.Level == _wheelProgression[i].Level)
                    {
                        Player.Instance.WagonStats.Wheel = _wheelProgression[i + 1];
                    }
                }
                break;

            case Body:
                for (int i = 0; i < _bodyProgression.Count - 1; i++)
                {
                    if (wagonPart.Level == _bodyProgression[i].Level)
                    {
                        Player.Instance.WagonStats.Body = _bodyProgression[i + 1];
                    }
                }
                break;

            case Suspension:
                for (int i = 0; i < _suspensionProgression.Count - 1; i++)
                {
                    if (wagonPart.Level == _suspensionProgression[i].Level)
                    {
                        Player.Instance.WagonStats.Suspension = _suspensionProgression[i + 1];
                    }
                }
                break;
        }
        _wagonStats.RecalculateValues();
    }


}
