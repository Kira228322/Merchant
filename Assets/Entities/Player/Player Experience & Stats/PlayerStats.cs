using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : ISaveable<PlayerStatsSaveData>
{
    public int AdditionalDiplomacy;
    public int AdditionalToughness;
    public int AdditionalLuck;
    public int AdditionalCrafting;
    public int BaseDiplomacy { get; private set; }
    public int BaseToughness { get; private set; }
    public int BaseLuck { get; private set; }
    public int BaseCrafting { get; private set; }

    public int TotalDiplomacy => BaseDiplomacy + AdditionalDiplomacy; //Влияет на цены и успешность переговоров
    public int TotalToughness => BaseToughness + AdditionalToughness; //Влияет на скорость убывания сна и еды
    public int TotalLuck => BaseLuck + AdditionalLuck; //Влияет на частоту происшествия благоприятных и негативных событий
    public int TotalCrafting => BaseCrafting + AdditionalCrafting; //Влияет на доступность некоторых рецептов крафта

    public void IncreaseDiplomacy()
    {
        BaseDiplomacy++;
    }
    public void IncreaseToughness()
    {
        BaseToughness++;
    }
    public void IncreaseLuck()
    {
        BaseLuck++;
    }
    public void IncreaseCrafting()
    {
        BaseCrafting++;
    }

    public float GetCoefForNegativeEvent()
    {
        return (float)(1 - 0.07f * TotalLuck / (0.07 * TotalLuck + 1));
    }

    public float GetCoefForPositiveEvent()
    {
        return (float)(1 + 0.07f * TotalLuck / (0.07 * TotalLuck + 1));
    }

    public float GetCoefForDiplomacyPositiveEvent()
    {
        return (float)(1 + 0.07f * TotalDiplomacy / (0.07 * TotalDiplomacy + 1));
    }

    public float GetCoefForDiplomacyNegativeEvent()
    {
        return (float)(1 - 0.07f * TotalDiplomacy / (0.07 * TotalDiplomacy + 1));
    }

    //Сохраняются только базовые статы, потому что аддитивные статы будут добавляться через эффекты
    public PlayerStatsSaveData SaveData()
    {
        PlayerStatsSaveData saveData = new(BaseDiplomacy, BaseToughness, BaseLuck, BaseCrafting);
        return saveData;
    }

    public void LoadData(PlayerStatsSaveData data)
    {
        BaseDiplomacy = data.BaseDiplomacy;
        BaseToughness = data.BaseToughness;
        BaseLuck = data.BaseLuck;
        BaseCrafting = data.BaseCrafting;
    }
}