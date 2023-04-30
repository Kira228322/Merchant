using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGlobalEvent
{
    static string Description { get; }      //ќписание в доске объ€влений. null, если ивент не имеет объ€влени€
    int DurationHours { get; set; }         //—колько часов длитс€ ивент
    void Execute();
    void Terminate();
}

public interface IRandomGlobalEvent : IGlobalEvent
{
    static float BaseChance { get; }        //Ѕазовый шанс выпадени€ этого ивента. ”величиваетс€, если ивент долго не выпадал
    static int CooldownDays { get; }        //»вент не может выпасть чаще чем раз в CooldownDays дней
    static bool IsConcurrent { get; }       //ћожет ли одновременно быть два и более ивента такого типа?
}