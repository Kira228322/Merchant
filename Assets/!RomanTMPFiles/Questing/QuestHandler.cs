using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    //Этот типчик будет раздавать квесты. Пока думаю сделать ему синглтон. Вероятно, на этом же объекте будут отслеживаться все текущие квесты в виде компонентов 
    //Вместо синглтона можно было бы сделать ссылку в GameManager, или какой нибудь QuestManager

    private static QuestHandler Singleton;

    [SerializeField] private GameObject _questsGameObject; //Объект, на котором висят все квесты. Возможно, этот объект.

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
    //public void GiveReward(Quest quest) { } //Добавить это сюда или оставить в Quest.cs? Думаю
}
