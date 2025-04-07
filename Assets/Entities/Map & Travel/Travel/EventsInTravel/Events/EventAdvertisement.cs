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
                    _eventWindow.ChangeDescription("Похоже, у вас нет подключения к интернету или для вас нет подходящей рекламы. Пожалуйста, проверьте подключение к сети, чтобы получить награду или вернитесь позднее.");
                }
                break;
            case 1:
                _eventWindow.ChangeDescription("Вам предлагали радушный и теплый прием, но вы отказались...");
                break;
        }
    }
}
