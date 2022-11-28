using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    public int Diplomacy { get; private set; } //Влияет на цены и успешность переговоров
    public int Toughness { get; private set; } //Влияет на скорость убывания сна
    public int Luck { get; private set; } //???
    public PlayerStats()
    {
        Diplomacy = 0;
        Toughness = 0;
        Luck = 0;
    }

    public void IncreaseDiplomacy()
    {
        Diplomacy++;
    }
    public void IncreaseToughness()
    {
        Toughness++;
    }
    public void IncreaseLuck()
    {
        Luck++;
    }
    
}
