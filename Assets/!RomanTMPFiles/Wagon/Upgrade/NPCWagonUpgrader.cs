using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWagonUpgrader : NPC
{
    [SerializeField] private List<Wheel> _wheelProgression;
    [SerializeField] private List<Body> _bodyProgression;
    [SerializeField] private List<Suspension> _suspensionProgression;

    private PlayerWagonStats _wagonStats;

    private void Start()
    {
        _wagonStats = Player.Singleton.WagonStats;
    }

    public override void Interact()
    {
        //Тачку на прокачку
        //Открыть меню с текущими статами каждой запчасти и статами следующей запчасти, с кнопкой купить.
        base.Interact();
        //По поводу интеракта сделаю позже, сейчас тесты:
        Upgrade(_wagonStats.Body);
        Upgrade(_wagonStats.Suspension);
        Upgrade(_wagonStats.Wheel);

    }

    public void Upgrade(WagonPart wagonPart)
    {
        
        switch (wagonPart)
        {
            case Wheel:
                Debug.Log("До прокачки колесо: " + $"{_wagonStats.Wheel.QualityModifier}");
                for (int i = 0; i < _wheelProgression.Count - 1; i++)
                {
                    if (wagonPart.Level == _wheelProgression[i].Level)
                    {
                        wagonPart.Replace(_wheelProgression[i + 1]);
                    }
                }
                break;

            case Body:
                Debug.Log("До прокачки тело: " + $"{_wagonStats.Body.InventoryRows}");
                for (int i = 0; i < _bodyProgression.Count - 1; i++)
                {
                    if (wagonPart.Level == _bodyProgression[i].Level)
                    {
                        wagonPart.Replace(_bodyProgression[i + 1]);
                    }
                }
                break;

            case Suspension:
                Debug.Log("До прокачки суспензия: " + $"{_wagonStats.Suspension.MaxWeight}");
                for (int i = 0; i < _suspensionProgression.Count - 1; i++)
                {
                    if (wagonPart.Level == _suspensionProgression[i].Level)
                    {
                        wagonPart.Replace(_suspensionProgression[i + 1]);
                    }
                }
                break;
        }
        _wagonStats.RecalculateValues();
        Debug.Log("После прокачки: " + $"{_wagonStats.Wheel.QualityModifier} {_wagonStats.Body.InventoryRows} {_wagonStats.Suspension.MaxWeight}");
    }


}
