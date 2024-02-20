using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventQuestGiver : EventInTravel
{
    public override void SetButtons()
    {
        ButtonsLabel.Add("����� �������");
        ButtonsLabel.Add("����������");
        SetInfoButton("");
    }

    public override void OnButtonClick(int n)
    {
        // TODO ���� �������� ����� �� ���� � �������� ��������� ������-�� ���������� ��� �� ��������� �������.
        switch (n)
        {
            case 0:
                NpcQuestGiverData questGiver = MapManager.CurrentRegion.GetRandomFreeQuestGiver();

                if (questGiver == null) //���� � ������� ������� ��� ���������� �����������, �� ������ �� ������ �������
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
