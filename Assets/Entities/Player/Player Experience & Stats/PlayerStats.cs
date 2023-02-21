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
    
    public int TotalDiplomacy => _totalDiplomacy; //Влияет на цены и успешность переговоров
    public int TotalToughness => _totalToughness; //Влияет на скорость убывания сна и еды
    public int TotalLuck => _totalLuck; // влияет на частоту происшествия благоприятных и негативных событий

    public void RefreshStats()
    {
        // Как оно работает:
        // бафы и дебафы меняют добавочные значения статов. 
        // Если это баф, то в Activate он прибавляет добавочный стат
        // В Deactivate он отнимает такое же количество статов.
        // Для дебафов аналогично.
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
