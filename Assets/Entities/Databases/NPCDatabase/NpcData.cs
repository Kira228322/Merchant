using UnityEngine;

[CreateAssetMenu(fileName = "newNPC", menuName = "NPCs/NPC")]
public class NpcData : ScriptableObject, IResetOnExitPlaymode, ISaveable<NpcSaveData>
{
    public string Name;
    public int StartWalkingTime;
    public int FinishWalkingTime;

    public int GameStartMoney;
    public int CurrentMoney;

    public int ID;


    public TextAsset InkJSON;

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
