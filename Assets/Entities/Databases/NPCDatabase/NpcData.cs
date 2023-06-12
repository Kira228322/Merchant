using UnityEngine;

[CreateAssetMenu(fileName = "newNPC", menuName = "NPCs/NPC")]
public class NpcData : ScriptableObject, IResetOnExitPlaymode, ISaveable<NpcSaveData>
{
    public string Name;
    public int StartWalkingTime;
    public int FinishWalkingTime;
    public int Money;

    public int ID;

    [Range(-100, 100)] [SerializeField] protected int _baseAffinity;

    private int _affinity;

    public int Affinity
    {
        get => _affinity;
        set
        {
            _affinity = value;
            if (_affinity < -100)
            {
                _affinity = -100;
                return;
            }
            if (_affinity > 100)
                _affinity = 100;
        }
    }

    public TextAsset InkJSON;

    private void OnEnable()
    {
        Affinity = _baseAffinity;
    }
    public void ResetOnExitPlaymode()
    {
        Affinity = _baseAffinity;
    }

    public NpcSaveData SaveData()
    {
        NpcSaveData saveData = new(Affinity, ID, Money);
        return saveData;
    }

    public void LoadData(NpcSaveData data)
    {
        Affinity = data.Affinity;
        Money = data.Money;
    }
}
