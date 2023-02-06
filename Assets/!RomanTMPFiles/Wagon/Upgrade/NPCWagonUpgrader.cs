using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWagonUpgrader : NPC
{
    [SerializeField] private List<Wheel> _wheelProgression;
    [SerializeField] private List<Body> _bodyProgression;
    [SerializeField] private List<Suspension> _suspensionProgression;

    private PlayerWagonStats _wagonStats;

    private void Start()
    {
        _wagonStats = Player.Singleton.WagonStats;
    }

    public override void Interact()
    {
        //����� �� ��������
        //������� ���� � �������� ������� ������ �������� � ������� ��������� ��������, � ������� ������.
        base.Interact();
        //�� ������ ��������� ������ �����, ������ �����:
        Upgrade(_wagonStats.Body);
        Upgrade(_wagonStats.Suspension);
        Upgrade(_wagonStats.Wheel);

    }

    public void Upgrade(WagonPart wagonPart)
    {
        
        switch (wagonPart)
        {
            case Wheel:
                Debug.Log("�� �������� ������: " + $"{_wagonStats.Wheel.QualityModifier}");
                for (int i = 0; i < _wheelProgression.Count - 1; i++)
                {
                    if (wagonPart.Level == _wheelProgression[i].Level)
                    {
                        wagonPart.Replace(_wheelProgression[i + 1]);
                    }
                }
                break;

            case Body:
                Debug.Log("�� �������� ����: " + $"{_wagonStats.Body.InventoryRows}");
                for (int i = 0; i < _bodyProgression.Count - 1; i++)
                {
                    if (wagonPart.Level == _bodyProgression[i].Level)
                    {
                        wagonPart.Replace(_bodyProgression[i + 1]);
                    }
                }
                break;

            case Suspension:
                Debug.Log("�� �������� ���������: " + $"{_wagonStats.Suspension.MaxWeight}");
                for (int i = 0; i < _suspensionProgression.Count - 1; i++)
                {
                    if (wagonPart.Level == _suspensionProgression[i].Level)
                    {
                        wagonPart.Replace(_suspensionProgression[i + 1]);
                    }
                }
                break;
        }
        _wagonStats.RecalculateValues();
        Debug.Log("����� ��������: " + $"{_wagonStats.Wheel.QualityModifier} {_wagonStats.Body.InventoryRows} {_wagonStats.Suspension.MaxWeight}");
    }


}
