using System.Linq;
using UnityEngine;

public class EventWell : EventInTravel
{
    [SerializeField] private Status _wellBuff;
    private bool _haveBottle = false;
    private int _probabilityOfWater = 55;
    public override void SetButtons()
    {
        SetInfoButton($"Вероятность того, что в колодце будет вода {TravelEventHandler.GetProbability(_probabilityOfWater, Player.Instance.Statistics.Luck)}%." +
                      $"\nШанс успеха зависит от вашей удачи");
        foreach (var item in Player.Instance.Inventory.BaseItemList)
        {
            if (item.ItemData.Name is "Пустая бутылка" or "Бутылка с водой")
            {
                ButtonsLabel.Add("Наполнить бутыль");
                _haveBottle = true;
                return;
            }
        }
        if (_haveBottle == false)
            ButtonsLabel.Add("Нет бутылки");

    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                if (_haveBottle == false)
                    _eventWindow.ChangeDescription("У вас не было емкости для того чтобы наполнить воду. Вы продолжили свое путешествие.");
                else
                {
                    if (TravelEventHandler.EventFire(_probabilityOfWater, true, Player.Instance.Statistics.Luck))
                    {
                        if (StatusManager.Instance.ActiveStatuses.FirstOrDefault(status =>
                                status.StatusData.StatusName == "Всезнание колодца") != null)
                        {
                            int exp = Random.Range(3, 5);
                            Player.Instance.Experience.AddExperience(exp);
                            CanvasWarningGenerator.Instance.CreateWarning("Мудрость старого колодца", $"Вы получили {exp} опыта");
                        }
                        if (TravelEventHandler.EventFire(20, true, Player.Instance.Statistics.Luck))
                        {
                            StatusManager.Instance.AddStatusForPlayer(_wellBuff);
                        }

                        foreach (var item in Player.Instance.Inventory.BaseItemList)
                        {
                            if (item.ItemData.Name == "Пустая бутылка")
                            {
                                InventoryController.Instance.DestroyItem(Player.Instance.BaseItemGrid, item);
                                InventoryController.Instance.TryCreateAndInsertItem(ItemDatabase.GetItem("Бутылка с водой"), 1, 0);
                                _eventWindow.ChangeDescription("В колодце оказалась вода и вы наполнили бутыль.");
                                return;
                            }
                        }

                        UsableItem bottle = (UsableItem)ItemDatabase.GetItem("Бутылка с водой");
                        Player.Instance.Needs.CurrentHunger += bottle.UsableValue;
                        _eventWindow.ChangeDescription("В колодце оказалась вода, но у вас не оказалось пустой бутылки. " +
                                                       "Вы выпили воду из своей бутылки, а затем наполнили ее.");
                    }
                    else
                        _eventWindow.ChangeDescription("К несчастью, колодец оказался иссохшим.");
                }
                break;
        }
    }
}
