using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRoadKeeper : EventInTravel
{
    private int money;
    private float _reduceQuality = 0.06f;
    public override void SetButtons()
    {
        money = Random.Range(28, 38) - Player.Instance.Statistics.Diplomacy.Total - Player.Instance.Statistics.Diplomacy.Total/2;
        ButtonsLabel.Add("Заплатить");
        ButtonsLabel.Add("Игнорировать");
        SetInfoButton($"Хранитель дороги требует {money} золота.\nВаша дипломатия уменьшает стоимость платы.");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                if (Player.Instance.Money < money)
                {
                    FindObjectOfType<TravelEventHandler>().RoadBadnessMultiplier += _reduceQuality;
                    _eventWindow.ChangeDescription("У вас не было достаточно золота. " +
                                                   "Хранитель разозлился и ударил по вашему колесу молотом. " +
                                                   "Это может привести к увеличенному количеству сломанных предметов после поездки");
                }
                else
                {
                    Player.Instance.Money -= money;
                    _eventWindow.ChangeDescription("Хранитель дороги был рад принять налог за проезд. Он ухмыльнулся," +
                                                   " хрюкнул и позволил вам проехать");
                }
                break;
            case 1:
                FindObjectOfType<TravelEventHandler>().RoadBadnessMultiplier += _reduceQuality;
                _eventWindow.ChangeDescription("Хранитель разозлился и ударил по вашему колесу молотом. " +
                                               "Это может привести к увеличенному количеству сломанных предметов после поездки");
                break;
        }
    }
}
