public class EventAdvertisement : EventInTravel
{
    public override void SetButtons()
    {
        ButtonsLabel.Add("Посмотреть представление");
        ButtonsLabel.Add("Проехать мимо");
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
                    _eventWindow.ChangeDescription("Вы просмотрели представление и получили награду! Приходите еще!");
                }
                else
                {
                    _eventWindow.ChangeDescription("Похоже, у вас нет подключения к интернету. Пожалуйста, проверьте подключение к сети, чтобы получить награду.");
                }
                break;
            case 1:
                _eventWindow.ChangeDescription("Вам предлагали радушный и теплый прием, но вы отказались...");
                break;
        }
    }
}
