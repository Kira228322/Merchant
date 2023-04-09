using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    private int _totalDiplomacy;
    private int _totalToughness;
    private int _totalLuck;
    private int _totalCrafting;
    
    private int _baseDiplomacy;
    private int _baseToughness;
    private int _baseLuck;
    private int _baseCrafting;
    
    public int AddititionalDiplomacy;
    public int AddititionalToughness;
    public int AddititionalLuck;
    public int AdditionalCrafting;
    
    public int TotalDiplomacy => _totalDiplomacy; //������ �� ���� � ���������� �����������
    public int TotalToughness => _totalToughness; //������ �� �������� �������� ��� � ���
    public int TotalLuck => _totalLuck; //������ �� ������� ������������ ������������� � ���������� �������
    public int TotalCrafting => _totalCrafting; //������ �� ����������� ��������� �������� ������

    public void RefreshStats()
    {
        // ��� ��� ��������:
        // ���� � ������ ������ ���������� �������� ������. 
        // ���� ��� ���, �� � Activate �� ���������� ���������� ����
        // � Deactivate �� �������� ����� �� ���������� ������.
        // ��� ������� ����������.
        _totalDiplomacy = _baseDiplomacy + AddititionalDiplomacy;
        _totalToughness = _baseToughness + AddititionalToughness;
        _totalLuck = _baseLuck + AddititionalLuck;
        _totalCrafting = _baseCrafting + AdditionalCrafting;
    }
    
    public void IncreaseDiplomacy()
    {
        _baseDiplomacy++;
        RefreshStats();
    }
    public void IncreaseToughness()
    {
        _baseToughness++;
        RefreshStats();
    }
    public void IncreaseLuck()
    {
        _baseLuck++;
        RefreshStats();
    }
    public void IncreaseCrafting()
    {
        _baseCrafting++;
        RefreshStats();
    }

    public float GetCoefForNegativeEvent()
    {
        return (float)(1 - 0.07f * _totalLuck / (0.07 * _totalLuck + 1));
    }

    public float GetCoefForPositiveEvent()
    {
        return (float)(1 + 0.07f * _totalLuck / (0.07 * _totalLuck + 1));
    }

    public float GetCoefForDiplomacyPositiveEvent()
    {
        return (float)(1 + 0.07f * _totalDiplomacy / (0.07 * _totalDiplomacy + 1));
    }
    
    public float GetCoefForDiplomacyNegativeEvent()
    {
        return (float)(1 - 0.07f * _totalDiplomacy / (0.07 * _totalDiplomacy + 1));
    }
}
