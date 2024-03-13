using System;

[Serializable]
public abstract class GlobalEvent_Base
{
    public abstract string GlobalEventName { get; } //��������� � ����� ����������. null, ���� ����� �� ����� ���������
    public abstract string Description { get; }    //�������� � ����� ����������. null, ���� ����� �� ����� ����������
    public int DurationHours { get; set; }         //������� ����� ������ �����
    public abstract void Execute();
    public abstract void Terminate();
}
