using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGlobalEvent
{
    static string Description { get; }      //�������� � ����� ����������. null, ���� ����� �� ����� ����������
    int DurationHours { get; set; }         //������� ����� ������ �����
    void Execute();
    void Terminate();
}

public interface IRandomGlobalEvent : IGlobalEvent
{
    static float BaseChance { get; }        //������� ���� ��������� ����� ������. �������������, ���� ����� ����� �� �������
    static int CooldownDays { get; }        //����� �� ����� ������� ���� ��� ��� � CooldownDays ����
    static bool IsConcurrent { get; }       //����� �� ������������ ���� ��� � ����� ������ ������ ����?
}