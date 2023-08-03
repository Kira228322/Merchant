using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : ISaveable<PlayerStatsSaveData>
{
    public class PlayerStat
    {
        public int Base;
        public int Additional;
        public int Total => Base + Additional;
    }

    public PlayerStat Diplomacy = new(); //������ �� ���� � ���������� �����������
    public PlayerStat Toughness = new(); //������ �� �������� �������� ��� � ���
    public PlayerStat Luck = new(); //������ �� ������� ������������ ������������� � ���������� �������
    public PlayerStat Crafting = new(); //������ �� ����������� ��������� �������� ������

    public void OnToughnessChanged()
    {
        // TODO ������, ����� ���������� ���������

        Player.Instance.Needs.HungerDecayRate = 12 + Toughness.Total;
        Player.Instance.Needs.SleepDecayRate = 16 + Toughness.Total;
        Player.Instance.Needs.MaxHunger = 90 + Toughness.Total * 3;
    }

    public void IncreaseStat(PlayerStat playerStat)
    {
        playerStat.Base++;
    }

    public float GetCoefForNegativeEvent(PlayerStat playerStat)
    {
        return (float)(1 - 0.07f * playerStat.Total / (0.07 * playerStat.Total + 1));
    }

    public float GetCoefForPositiveEvent(PlayerStat playerStat)
    {
        return (float)(1 + 0.07f * playerStat.Total / (0.07 * playerStat.Total + 1));
    }

    //����������� ������ ������� �����, ������ ��� ���������� ����� ����� ����������� ����� �������
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