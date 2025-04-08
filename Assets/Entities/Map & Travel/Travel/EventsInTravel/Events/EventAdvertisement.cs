public class EventAdvertisement : EventInTravel
{
    public override void SetButtons()
    {
        ButtonsLabel.Add("���������� �������������");
        ButtonsLabel.Add("�������� ����");
        SetInfoButton("");
    }

    public override void OnButtonClick(int n)
    {

        switch (n)
        {
            case 0:
                if (RewardedAds.Instance.IsAdLoaded)
                {
                    RewardedAds.Instance.ShowAd();
                    _eventWindow.ChangeDescription("�� ����������� ������������� � �������� �������! ��������� ���!");
                }
                else
                {
                    _eventWindow.ChangeDescription("������, � ��� ��� ����������� � ��������� ��� ��� ��� ��� ���������� �������. ����������, ��������� ����������� � ����, ����� �������� ������� ��� ��������� �������.");
                }
                break;
            case 1:
                _eventWindow.ChangeDescription("��� ���������� �������� � ������ �����, �� �� ����������...");
                break;
        }
    }
}
