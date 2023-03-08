using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWagonUpgrader : NPC
{
    [SerializeField] private List<Wheel> _wheelProgression;             
    [SerializeField] private List<Body> _bodyProgression;               
    [SerializeField] private List<Suspension> _suspensionProgression;   

    private PlayerWagonStats _wagonStats;

    protected void Start()
    {
        _wagonStats = Player.Instance.WagonStats;
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
