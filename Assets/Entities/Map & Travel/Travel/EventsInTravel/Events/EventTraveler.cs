using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTraveler : EventInTravel
{
    // ѕутник может либо отблагодарить игрока, за то, что тот ему помог, либо оказатьс€ вором
    [SerializeField] private int _chanceTravelerIsThief; // ¬еро€тность, что вор (веро€тность, что не вор q = 1 - p)
    [SerializeField] private int _money;
    [SerializeField] private int _experience;
    private bool _isThief;
    
    // TODO так подумал может лучше сделать чтобы путник не просил нас подвести его,а просил может быть еды покушать.
    // “ак как если делать логику с вором, то какое то дополнительное окно нужно делать в конце поездки, что в падлу.
    // ≈сли давать ему еду, то он будет давать экспу, ну или можно не давать, тогда ничего не будет.
    
    public override void SetButtons()
    {
        ButtonsLabel.Add("ѕодвезти путника");
        ButtonsLabel.Add("≈хать дальше");
        SetInfoButton("");
    }

    protected override void Start()
    {
        base.Start();
        _isThief = TravelEventHandler.EventFire(_chanceTravelerIsThief, false, Player.Instance.Statistics.Luck);
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                break;
            case 1:
                break;
        }
        
    }
}
