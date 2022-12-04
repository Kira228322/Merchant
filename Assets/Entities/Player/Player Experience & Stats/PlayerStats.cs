using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    private int _diplomacy;
    private int _toughness;
    private int _luck;
    public int Diplomacy => _diplomacy; //������ �� ���� � ���������� �����������
    public int Toughness => _toughness; //������ �� �������� �������� ���
    public int Luck => _luck; //???

    public void IncreaseDiplomacy()
    {
        _diplomacy++;
    }
    public void IncreaseToughness()
    {
        _toughness++;
    }
    public void IncreaseLuck()
    {
        _luck++;
    }
    
}
