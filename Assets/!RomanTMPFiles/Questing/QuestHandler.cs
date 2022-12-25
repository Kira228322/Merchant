using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    //Этот типчик будет раздавать квесты. Пока думаю сделать ему синглтон. Вероятно, на этом же объекте будут отслеживаться все текущие квесты в виде компонентов 
    //Вместо синглтона можно было бы сделать ссылку в GameManager, или какой нибудь QuestManager

    private static QuestHandler Singleton;

    [SerializeField] private GameObject _questsGameObject; //Объект, на котором висят все квесты. Возможно, этот объект.
    [SerializeField] private QuestLog _questLog; //UI-КвестЛог

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
        //К сожалению, пришлось впихнуть костыль.
        //Связано с тем, что если просто передавать квест, он не успевает сделать свой Awake в том же кадре
        //и панель не получит правильной информации о нём.
        //Нужно подождать следующего кадра, пока сработает Awake, тогда всё ок. В целом похуй, нам же не критично создать панель
                                                                                            //в том же кадре, что появился квест


        //UPD 15 минут спустя: Ебать, а вообще-то критично. Если у игрока уже есть всё что надо для выполнения квеста,
        //То панель должна реагировать сразу, иначе всё улетает в Exception

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
