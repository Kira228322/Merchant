using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventQuestGiver : EventInTravel
{
    private QuestParams _generatedQuest;
    public override void SetButtons()
    {
        NpcQuestGiverData questGiver = MapManager.CurrentRegion.GetRandomFreeQuestGiver();

        if (questGiver == null) //Если в текущем регионе нет свободного квестгивера, он берётся из любого региона
        {
            List<Region> otherRegions = FindObjectOfType<RegionHandler>().Regions.Where(region => region != MapManager.CurrentRegion).ToList();
            questGiver = otherRegions[Random.Range(0, otherRegions.Count)].GetRandomFreeQuestGiver();
        }
        if (questGiver != null)
        {
            _generatedQuest = questGiver.GiveRandomQuest();
            _eventWindow.ChangeDescription("Вас останавливает незнакомец и говорит: \"Я тебя повсюду ищу. Тебе просили кое-что передать, лично в руки.\"" +
                $"\nОн вручает вам письмо, в котором написано следующее: <i>{_generatedQuest.description}</i>" +
                "\nВы готовы взяться за это задание?");
            ButtonsLabel.Add("Взять задание");
            ButtonsLabel.Add("Отказаться");
        }
        else
        {
            _eventWindow.ChangeDescription("Вас останавливает незнакомец. Он говорит: \"Я тебя повсюду ищу. Тебе просили кое-что передать, лично в руки..." +
                "\nА нет, извини, я ошибся. Все задания уже разобрали. Увидимся в другой раз!\"");
            ButtonsLabel.Add("Ехать дальше");
        }


        SetInfoButton("");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                if (_generatedQuest != null)
                {
                    QuestHandler.AddQuest(_generatedQuest);
                    _eventWindow.ChangeDescription("Вы взяли задание. Вы сможете найти его в своём дневнике.");
                }
                else
                    _eventWindow.ChangeDescription("Вы поехали дальше, не задерживаясь.");
                break;
            case 1:
                _eventWindow.ChangeDescription("Вы поехали дальше. Незнакомец кричит вам вслед: \"Ну и езжай! Тогда это задание возьмёт кто-то другой!\"");
                break;
        }
    }
}
