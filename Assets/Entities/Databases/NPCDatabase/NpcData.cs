using UnityEngine;

[CreateAssetMenu(fileName = "newNPC", menuName = "NPCs/NPC")]
public class NpcData : ScriptableObject, IResetOnExitPlaymode, ISaveable<NpcSaveData>
{
    public string Name;
    public int StartWalkingTime;
    public int FinishWalkingTime;
    public int Money;

    public int ID;


    public TextAsset InkJSON;

    public NpcSaveData SaveData()
    {
        NpcSaveData saveData = new(ID, Money);
        return saveData;
    }

    public void LoadData(NpcSaveData data)
    {
        Money = data.Money;
    }

    public void ResetOnExitPlaymode()
    {

    }
}
