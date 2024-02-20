using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventQuestGiver : EventInTravel
{
    public override void SetButtons()
    {
        ButtonsLabel.Add("Взять задание");
        ButtonsLabel.Add("Отказаться");
        SetInfoButton("");
    }

    public override void OnButtonClick(int n)
    {
        // TODO надо выдавать квест на сбор и доставку предметов какому-то случайному нпс со случайной локации.
        switch (n)
        {
            case 0:
                NpcQuestGiverData questGiver = MapManager.CurrentRegion.GetRandomFreeQuestGiver();

                if (questGiver == null) //Если в текущем регионе нет свободного квестгивера, он берётся из любого региона
                {
                    List<Region> otherRegions = FindObjectOfType<RegionHandler>().Regions.Where(region => region != MapManager.CurrentRegion).ToList();
                    questGiver = otherRegions[Random.Range(0, otherRegions.Count)].GetRandomFreeQuestGiver();
                }
                if (questGiver != null)
                {
                    questGiver.GiveRandomQuest();
                }
                break;
            case 1:
                break;
        }
    }
}
