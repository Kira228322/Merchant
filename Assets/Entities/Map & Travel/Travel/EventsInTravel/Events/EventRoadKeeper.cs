using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRoadKeeper : EventInTravel
{
    private int money;
    private float _reduceQuality = 0.08f;
    public override void SetButtons()
    {
        money = Random.Range(28, 38) - Player.Instance.Statistics.Diplomacy.Total - Player.Instance.Statistics.Diplomacy.Total/2;
        ButtonsLabel.Add("«аплатить");
        ButtonsLabel.Add("»гнорировать");
        SetInfoButton("¬аша дипломати€ уменьшает стоимость платы.");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                if (Player.Instance.Money < money)
                {
                    FindObjectOfType<TravelEventHandler>().RoadBadnessMultiplier += _reduceQuality;
                    _eventWindow.ChangeDescription("” вас не было достаточно золота. " +
                                                   "’ранитель разозлилс€ и ударил по вашему колесу молотом. " +
                                                   "Ёто может привести к увеличенному количеству сломанных предметов после поездки");
                }
                else
                {
                    Player.Instance.Money -= money;
                    _eventWindow.ChangeDescription("’ранитель дороги был рад прин€ть налог за проезд. ќн ухмыльнулс€," +
                                                   " хрюкнул и позволил вам проехать");
                }
                break;
            case 1:
                FindObjectOfType<TravelEventHandler>().RoadBadnessMultiplier += _reduceQuality;
                _eventWindow.ChangeDescription("’ранитель разозлилс€ и ударил по вашему колесу молотом. " +
                                               "Ёто может привести к увеличенному количеству сломанных предметов после поездки");
                break;
        }
    }
}
