using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWagonUpgrader : NPC
{
    [SerializeField] private List<Wheel> _wheelProgression;
    [SerializeField] private List<Body> _bodyProgression;
    [SerializeField] private List<Suspension> _suspensionProgression;

    private Wagon _wagon;

    private void Start()
    {
        _wagon = Player.Singleton.Wagon;
    }

    public override void Interact()
    {
        //Тачку на прокачку
        //Открыть меню с текущими статами каждой запчасти и статами следующей запчасти, с кнопкой купить.
        base.Interact();
        //По поводу интеракта сделаю позже, сейчас тесты:
        Upgrade(_wagon.Body);
        Upgrade(_wagon.Suspension);
        Upgrade(_wagon.Wheel);

    }

    public void Upgrade(WagonPart wagonPart)
    {
        
        switch (wagonPart)
        {
            case Wheel:
                Debug.Log("До прокачки колесо: " + $"{_wagon.Wheel.QualityModifier}");
                for (int i = 0; i < _wheelProgression.Count - 1; i++)
                {
                    if (wagonPart.Level == _wheelProgression[i].Level)
                    {
                        wagonPart.Replace(_wheelProgression[i + 1]);
                    }
                }
                break;

            case Body:
                Debug.Log("До прокачки тело: " + $"{_wagon.Body.InventoryRows}");
                for (int i = 0; i < _bodyProgression.Count - 1; i++)
                {
                    if (wagonPart.Level == _bodyProgression[i].Level)
                    {
                        wagonPart.Replace(_bodyProgression[i + 1]);
                    }
                }
                break;

            case Suspension:
                Debug.Log("До прокачки суспензия: " + $"{_wagon.Suspension.Weight}");
                for (int i = 0; i < _suspensionProgression.Count - 1; i++)
                {
                    if (wagonPart.Level == _suspensionProgression[i].Level)
                    {
                        wagonPart.Replace(_suspensionProgression[i + 1]);
                    }
                }
                break;
        }

        _wagon.RefreshStats();

        Debug.Log("После прокачки: " + $"{_wagon.Wheel.QualityModifier} {_wagon.Body.InventoryRows} {_wagon.Suspension.Weight}");
    }


}
