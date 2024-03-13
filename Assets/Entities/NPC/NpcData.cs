using UnityEngine;

[CreateAssetMenu(fileName = "newNPC", menuName = "NPCs/NPC")]
public class NpcData : ScriptableObject, IResetOnExitPlaymode, ISaveable<NpcSaveData>
{
    public string Name;
    public int StartWalkingTime;
    public int FinishWalkingTime;

    public int GameStartMoney;
    public int CurrentMoney;

    //29.06.23: «ачем всЄ-таки было нужно сохран€ть айди: дл€ того чтобы датабаза
    //правильно назначала сейвдату при загрузке, основыва€сь на этом айди
    //¬ теории оно должно было бы работать и так, ведь там идет проход по NpcList,
    //но дл€ уверенности сделали тогда так
    //(см. комментарий в NpcDatabase.LoadData())
    public int ID;

    public NpcSaveData SaveData()
    {
        NpcSaveData saveData = new(ID, CurrentMoney);
        return saveData;
    }

    public void LoadData(NpcSaveData data)
    {
        CurrentMoney = data.Money;
    }

    public void ResetOnExitPlaymode()
    {
        CurrentMoney = GameStartMoney;
    }
}
