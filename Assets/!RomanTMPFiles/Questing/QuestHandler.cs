using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    //���� ������ ����� ��������� ������. ���� ����� ������� ��� ��������. ��������, �� ���� �� ������� ����� ������������� ��� ������� ������ � ���� ����������� 
    //������ ��������� ����� ���� �� ������� ������ � GameManager, ��� ����� ������ QuestManager

    public static QuestHandler Singleton;

    [SerializeField] private GameObject _questsGameObject; //������, �� ������� ����� ��� ������. ��������, ���� ������.

    private void Awake()
    {
        Singleton = this;
    }

    public void AddQuest(string questName)
    {
        _questsGameObject.AddComponent(System.Type.GetType(questName));
    }
    public void RemoveQuest(string questName)
    {
        Destroy(_questsGameObject.GetComponent(questName));
    }
    //public void GiveReward(Quest quest) { } //�������� ��� ���� ��� �������� � Quest.cs? �����
}
