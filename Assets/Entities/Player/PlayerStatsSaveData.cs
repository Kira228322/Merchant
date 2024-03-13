using System;

[Serializable]
public class PlayerStatsSaveData
{
    public int BaseDiplomacy;
    public int BaseToughness;
    public int BaseLuck;
    public int BaseCrafting;
    public PlayerStatsSaveData(int diplomacy, int toughness, int luck, int crafting)
    {
        BaseDiplomacy = diplomacy;
        BaseToughness = toughness;
        BaseLuck = luck;
        BaseCrafting = crafting;
    }

}