using UnityEngine;

public class EventSunflower : EventInTravel
{
    public override void SetButtons()
    {
        ButtonsLabel.Add("Собрать подсолнухи");
        ButtonsLabel.Add("Ехать дальше");
        SetInfoButton("");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                if (InventoryController.Instance.TryCreateAndInsertItem(ItemDatabase.GetItem("Подсолнух"), 2, 0))
                {
                    int skippedMinutes = Random.Range(5, 16);
                    _eventWindow.ChangeDescription($"Вы собрали пару подсолнухов и потратили на это 1 час и {skippedMinutes} минут.");
                    GameTime.TimeSkip(0, 1, skippedMinutes);
                }
                else
                {
                    _eventWindow.ChangeDescription("У вас не было места в инвентаре, и вы поехали дальше.");
                }

                break;
            case 1:
                _eventWindow.ChangeDescription("Вы решили не задерживаться в своем пути.");
                break;
        }

    }
}
