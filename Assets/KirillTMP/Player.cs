using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private SlidersController _hungerScale;
    [SerializeField] private SlidersController _sleepScale;
    
    public int MaxHunger; // ћаксимальное значение голода и сна можно будет подн€ть отдав скилл поинт в стат
    public int MaxSleep; // "—тойкость" или типо того.
    public int CurrentHunger;
    public int CurrentSleep; // пока хз как быть с доступом этих полей. ѕусть пока будут public

    public void EatFood(int foodValue)
    {
        CurrentHunger += foodValue;
        if (CurrentHunger > MaxHunger)
            CurrentHunger = MaxHunger;
        _hungerScale.SetValue(CurrentHunger, MaxHunger);
    }

    public void Sleep()
    {
        // TODO думаю сделать так, чтобы было можно было выбирать врем€ сна и за каждый час сна восстанавливать
        // 1/8 от MaxSleep (то есть максимум можно поспать 8 часов, больше нет смысла) 
    }
    
}
