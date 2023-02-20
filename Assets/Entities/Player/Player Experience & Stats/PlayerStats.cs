using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    private int _totalDiplomacy;
    private int _totalToughness;
    private int _totalLuck;
    
    private int _baseDiplomacy;
    private int _baseToughness;
    private int _baseLuck;
    
    public int AddititionalDiplomacy;
    public int AddititionalToughness;
    public int AddititionalLuck;
    
    public int TotalDiplomacy => _totalDiplomacy; //������ �� ���� � ���������� �����������
    public int TotalToughness => _totalToughness; //������ �� �������� �������� ��� � ���
    public int TotalLuck => _totalLuck; // ������ �� ������� ������������ ������������� � ���������� �������

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
        Debug.Log(_totalLuck);
    }
    
    public void IncreaseDiplomacy()
    {
        _baseDiplomacy++;
    }
    public void IncreaseToughness()
    {
        _baseToughness++;
    }
    public void IncreaseLuck()
    {
        _baseLuck++;
    }

    public float GetCoefForNegativeEvent()
    {
        return (float)(1 - 0.06f * _totalLuck / (0.06 * _totalLuck + 1.5f));
    }

    public float GetCoefForPositiveEvent()
    {
        return (float)(1 + 0.06f * _totalLuck / (0.06 * _totalLuck + 1.5f));
    }
    
}
