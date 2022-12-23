using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    //���� ������ ����� ��������� ������. ���� ����� ������� ��� ��������. ��������, �� ���� �� ������� ����� ������������� ��� ������� ������ � ���� ����������� 
    //������ ��������� ����� ���� �� ������� ������ � GameManager, ��� ����� ������ QuestManager

    private static QuestHandler Singleton;

    [SerializeField] private GameObject _questsGameObject; //������, �� ������� ����� ��� ������. ��������, ���� ������.
    [SerializeField] private QuestLog _questLog; //UI-��������

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else if (Singleton != this)
        {
            Destroy(gameObject);
        }
    }

    public static void AddQuest(string questName)
    {
        Quest quest = (Quest)Singleton._questsGameObject.AddComponent(System.Type.GetType(questName));
        Singleton.StartCoroutine(AddQuestCoroutine(quest));
    }
    public static IEnumerator AddQuestCoroutine(Quest quest)
        //� ���������, �������� �������� �������.
        //������� � ���, ��� ���� ������ ���������� �����, �� �� �������� ������� ���� Awake � ��� �� �����
        //� ������ �� ������� ���������� ���������� � ��.
        //����� ��������� ���������� �����, ���� ��������� Awake, ����� �� ��. � ����� �����, ��� �� �� �������� ������� ������
                                                                                            //� ��� �� �����, ��� �������� �����


        //UPD 15 ����� ������: �����, � ������-�� ��������. ���� � ������ ��� ���� �� ��� ���� ��� ���������� ������,
        //�� ������ ������ ����������� �����, ����� �� ������� � Exception

    {
        yield return new WaitForFixedUpdate();
        Singleton._questLog.AddToActiveQuests(quest);
    }
    public static void RemoveQuest(System.Type questType)
    {
        Quest quest = (Quest)Singleton._questsGameObject.GetComponent(questType);
        Singleton._questLog.RemoveFromActiveQuests(quest);
        Destroy(Singleton._questsGameObject.GetComponent(questType));
    }
    
}
