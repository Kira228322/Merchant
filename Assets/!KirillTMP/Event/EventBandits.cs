using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBandits : EventInTravel
{
    [SerializeField] private float _minWeight;
    [SerializeField] private float _maxWeight;
    [SerializeField] private int _money;

    public override void SetButtons()
    {
        ButtonsLabel.Add("Разбойники разграбят повозку");
        ButtonsLabel.Add("Отдать разбойникам золото");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                // TODO забираем предметы на Rand(min,max) веса у игрока 
                break;
            case 1:
                // TODO забираем деньги у игрока
                break;
        }
        _eventHandler.EventEnd();
    }
}
