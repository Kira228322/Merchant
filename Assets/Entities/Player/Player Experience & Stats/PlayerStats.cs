using UnityEngine.Events;

public class PlayerStats : ISaveable<PlayerStatsSaveData>
{
    public class PlayerStat
    {
        private int _base;
        private int _additional;
        public int Base
        {
            get => _base;
            set
            {
                _base = value;
                StatChanged?.Invoke();
            }
        }
        public int Additional
        {
            get => _additional;
            set
            {
                _additional = value;
                StatChanged?.Invoke();
            }
        }
        public int Total => Base + Additional;
        public event UnityAction StatChanged;
        public float GetCoefForPositiveEvent()
        {
            return (float)(1 + 0.08f * Total / (0.08 * Total + 1));
        }
        public float GetCoefForNegativeEvent()
        {
            return (float)(1 - 0.08f * Total / (0.08 * Total + 1));
        }
        public void IncreaseStat()
        {
            Base++;
        }
    }

    public PlayerStat Diplomacy = new(); //Влияет на цены и успешность переговоров
    public PlayerStat Toughness = new(); //Влияет на скорость убывания сна и еды
    public PlayerStat Luck = new(); //Влияет на частоту происшествия благоприятных и негативных событий
    public PlayerStat Crafting = new(); //Влияет на доступность некоторых рецептов крафта
    public float StatusDurationModifier = 1;
    public void OnToughnessChanged()
    {
        Player.Instance.Needs.HungerDecayRate = 19 + 2 * Toughness.Total;
        Player.Instance.Needs.SleepDecayRate = 21 + 2 * Toughness.Total;
        Player.Instance.Needs.MaxHunger = 90 + Toughness.Total * 3;
    }
    //Сохраняются только базовые статы, потому что аддитивные статы будут добавляться через эффекты
    public PlayerStatsSaveData SaveData()
    {
        PlayerStatsSaveData saveData = new(Diplomacy.Base, Toughness.Base, Luck.Base, Crafting.Base);
        return saveData;
    }

    public void LoadData(PlayerStatsSaveData data)
    {
        Diplomacy.Base = data.BaseDiplomacy;
        Toughness.Base = data.BaseToughness;
        Luck.Base = data.BaseLuck;
        Crafting.Base = data.BaseCrafting;
    }
}