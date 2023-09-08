using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAdvertisement : EventInTravel
{
    public override void SetButtons()
    {
        ButtonsLabel.Add("���������� �������������");
        ButtonsLabel.Add("�������� ����");
    }

    public override void OnButtonClick(int n)
    {
        // TODO
        switch (n)
        {
            case 0:
                RewardedAds.Instance.ShowAd();
                break;
            case 1:
                break;
        }
    }
}
