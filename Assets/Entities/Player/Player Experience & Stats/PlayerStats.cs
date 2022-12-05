using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    private int _diplomacy;
    private int _toughness;
    private int _luck;
    public int Diplomacy => _diplomacy; //¬ли€ет на цены и успешность переговоров
    public int Toughness => _toughness; //¬ли€ет на скорость убывани€ сна
    public int Luck => _luck; // вли€ет на частоту происшестви€ благопри€тных и негативных событий

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

    public float GetCoefForNegativeEvent()
    {
        return (float)(1 - 0.06f * _luck / (0.06 * _luck + 1.5f));
    }

    public float GetCoefForPositiveEvent()
    {
        return (float)(1 + 0.06f * _luck / (0.06 * _luck + 1.5f));
    }
    
}
