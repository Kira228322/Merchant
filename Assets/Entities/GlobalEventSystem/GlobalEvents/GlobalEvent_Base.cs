using System;

[Serializable]
public abstract class GlobalEvent_Base
{
    public abstract string GlobalEventName { get; } //«аголовок в доске объ€влений. null, если ивент не имеет заголовка
    public abstract string Description { get; }    //ќписание в доске объ€влений. null, если ивент не имеет объ€влени€
    public int DurationHours { get; set; }         //—колько часов длитс€ ивент
    public abstract void Execute();
    public abstract void Terminate();
}
