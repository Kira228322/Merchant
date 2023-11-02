using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class GiveItemsGoal : Goal
{
    // �� ����, ���������� CollectItemsGoal � TalkToNpcGoal.
    // � ���� ������� ���� ��������� �� ������� ��� ����� - � ����� ������� ����� ������� �������,
    // ���� ������ ����� ��� �� "������� ��������",
    // ������ ����� ��� �� "���������� � �����" � ���� ������ ��� ��� ��������.
    // �� ���� �������� ����� Goal, �� ������� ����� ����������.
    // ����� � ������� ��������� RequiredLine, �� ����������� �������� �������� � Goal ����� ��������.
    // �����, ������, ����������� ������, ����� �������� ������������� ���� � ��������� � ������ ����������.
    // ������ ������ ����� ���� � �������, ��� ����� ������� � Ink ������� �������� ���������.

    [NonSerialized] public Item RequiredItem;
    [NonSerialized] public NpcData RequiredNPC;
    public string RequiredItemName;
    public int RequiredIDOfNPC;
    public string RequiredLine; //��, � ��� ���� ����������. � Ink ���������� ��� invoke(RequiredLine) ������ � Ink: invoke(talked_about_trading)

    public GiveItemsGoal(State currentState, string description, int currentAmount, int requiredAmount, string requiredItemName, int requiredIDofNPC, string requiredLine) : base(currentState, description, currentAmount, requiredAmount)
    {
        RequiredItemName = requiredItemName;
        RequiredIDOfNPC = requiredIDofNPC;
        RequiredLine = requiredLine;
    }

    public override void Initialize()
    {
        RequiredNPC = NpcDatabase.GetNPCData(RequiredIDOfNPC);
        RequiredItem = ItemDatabase.GetItem(RequiredItemName);

        DialogueManager.Instance.TalkedToNPCAboutSomething += OnTalkWithNPC;

        Evaluate();
    }

    public override void Deinitialize()
    {
        DialogueManager.Instance.TalkedToNPCAboutSomething -= OnTalkWithNPC;
    }

    private void OnTalkWithNPC(NpcData npcData, string line)
    {
        if (npcData == RequiredNPC)
        {
            if (line == RequiredLine)
            {
                //������� ����������� �������� � ������
                //���������������, ��� � ������� ��� ������� ���������� ������ ���� ����������� �������� ����!

                //TODO ����� ��������, ����� ������ ���� ������ �� ���������
                if (!Player.Instance.Inventory.HasEnoughItemsOfThisItemData(RequiredItem, RequiredAmount))
                {
                    Debug.LogWarning("�� ���� ���������� ���������, ����� �� �������!");
                    return;
                }

                Player.Instance.Inventory.RemoveItemsOfThisItemData(RequiredItem, RequiredAmount);
                CurrentAmount += RequiredAmount;
                Evaluate();
            }
        }
    }
}
