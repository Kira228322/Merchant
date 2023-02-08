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
        Upgrade(_wagonStats.Wheel);
        Upgrade(_wagonStats.Body);
        Upgrade(_wagonStats.Suspension);

    }

    public void Upgrade(WagonPart wagonPart)
    {
        Debug.Log("start:" + Player.Singleton.WagonStats.Wheel.Level);
        switch (wagonPart)
        {
            case Wheel:
                for (int i = 0; i < _wheelProgression.Count - 1; i++)
                {
                    if (wagonPart.Level == _wheelProgression[i].Level)
                    {
                        wagonPart = _wheelProgression[i];
                        //wagonPart.Replace(_wheelProgression[i + 1]);
                        Debug.Log(Player.Singleton.WagonStats.Wheel.Level);
                        Debug.Log(Player.Singleton.WagonStats.Wheel.QualityModifier);
                    }
                }
                break;

            case Body:
                for (int i = 0; i < _bodyProgression.Count - 1; i++)
                {
                    if (wagonPart.Level == _bodyProgression[i].Level)
                    {
                        wagonPart.Replace(_bodyProgression[i + 1]);
                    }
                }
                break;

            case Suspension:
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
    }


}
