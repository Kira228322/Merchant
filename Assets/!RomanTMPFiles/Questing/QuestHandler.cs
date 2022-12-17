using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    //Этот типчик будет раздавать квесты. Пока думаю сделать ему синглтон. Вероятно, на этом же объекте будут отслеживаться все текущие квесты в виде компонентов 
    //Вместо синглтона можно было бы сделать ссылку в GameManager, или какой нибудь QuestManager

    public static QuestHandler Singleton;

    [SerializeField] private GameObject _questsGameObject; //Объект, на котором висят все квесты. Возможно, этот объект.

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
    //public void GiveReward(Quest quest) { } //Добавить это сюда или оставить в Quest.cs? Думаю
}
