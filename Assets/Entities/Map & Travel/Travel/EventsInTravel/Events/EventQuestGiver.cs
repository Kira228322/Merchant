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

        if (questGiver == null) //���� � ������� ������� ��� ���������� �����������, �� ������ �� ������ �������
        {
            List<Region> otherRegions = FindObjectOfType<RegionHandler>().Regions.Where(region => region != MapManager.CurrentRegion).ToList();
            questGiver = otherRegions[Random.Range(0, otherRegions.Count)].GetRandomFreeQuestGiver();
        }
        if (questGiver != null)
        {
            _generatedQuest = questGiver.GiveRandomQuest();
            _eventWindow.ChangeDescription("��� ������������� ���������� � �������: \"� ���� ������� ���. ���� ������� ���-��� ��������, ����� � ����.\"" +
                $"\n�� ������� ��� ������, � ������� �������� ���������: <i>{_generatedQuest.description}</i>" +
                "\n�� ������ ������� �� ��� �������?");
            ButtonsLabel.Add("����� �������");
            ButtonsLabel.Add("����������");
        }
        else
        {
            _eventWindow.ChangeDescription("��� ������������� ����������. �� �������: \"� ���� ������� ���. ���� ������� ���-��� ��������, ����� � ����..." +
                "\n� ���, ������, � ������. ��� ������� ��� ���������. �������� � ������ ���!\"");
            ButtonsLabel.Add("����� ������");
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
                    _eventWindow.ChangeDescription("�� ����� �������. �� ������� ����� ��� � ���� ��������.");
                }
                else
                    _eventWindow.ChangeDescription("�� ������� ������, �� ������������.");
                break;
            case 1:
                _eventWindow.ChangeDescription("�� ������� ������. ���������� ������ ��� �����: \"�� � �����! ����� ��� ������� ������ ���-�� ������!\"");
                break;
        }
    }
}
