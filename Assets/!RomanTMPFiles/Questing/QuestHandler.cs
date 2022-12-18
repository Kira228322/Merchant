using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    //���� ������ ����� ��������� ������. ���� ����� ������� ��� ��������. ��������, �� ���� �� ������� ����� ������������� ��� ������� ������ � ���� ����������� 
    //������ ��������� ����� ���� �� ������� ������ � GameManager, ��� ����� ������ QuestManager

    private static QuestHandler Singleton;

    [SerializeField] private GameObject _questsGameObject; //������, �� ������� ����� ��� ������. ��������, ���� ������.

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else Destroy(gameObject);
    }

    public static void AddQuest(string questName)
    {
        Singleton._questsGameObject.AddComponent(System.Type.GetType(questName));
    }
    public static void RemoveQuest(System.Type quest)
    {
        Destroy(Singleton._questsGameObject.GetComponent(quest));
    }
    //public void GiveReward(Quest quest) { } //�������� ��� ���� ��� �������� � Quest.cs? �����
}
