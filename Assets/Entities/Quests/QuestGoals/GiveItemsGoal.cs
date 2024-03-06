using System;

[Serializable]
public class GiveItemsGoal : Goal
{
    // По сути, комбинация CollectItemsGoal и TalkToNpcGoal.
    // С моей стороны было упущением не сделать это сразу - я хотел сделать через цепочку квестов,
    // типа первый квест был бы "собрать предметы",
    // второй квест был бы "поговорить с челом" и типа отдать ему эти предметы.
    // Но если добавить такой Goal, то намного проще получается.
    // Когда в диалоге сработает RequiredLine, то необходимые предметы спишутся и Goal будет засчитан.
    // Нужно, однако, внимательно чекать, чтобы предметы действительно были в инвентаре в нужном количестве.
    // Чекать скорее всего буду в диалоге, для этого добавлю в Ink функцию подсчета предметов.

    [NonSerialized] public Item RequiredItem;
    [NonSerialized] public NpcData RequiredNPC;
    public string RequiredItemName;
    public int RequiredIDOfNPC;
    public string RequiredLine; //То, о чём надо поговорить. В Ink вызывается как invoke(RequiredLine) пример в Ink: invoke(talked_about_trading)

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
        if (npcData == RequiredNPC || npcData == null)
        {
            if (line == RequiredLine)
            {
                //Списать необходимые предметы у игрока
                //Подразумевается, что в диалоге эта реплика появляется только если необходимые предметы есть!



                Player.Instance.Inventory.RemoveItemsOfThisItemData(RequiredItem, RequiredAmount);
                CurrentAmount += RequiredAmount;
                Evaluate();
            }
        }
    }
}
