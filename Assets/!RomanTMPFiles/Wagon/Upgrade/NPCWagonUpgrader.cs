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
        Upgrade(_wagonStats.Wheel);
        Upgrade(_wagonStats.Body);
        Upgrade(_wagonStats.Suspension);

    }

    public void Upgrade(WagonPart wagonPart)
    {
        switch (wagonPart)
        {
            case Wheel:
                for (int i = 0; i < _wheelProgression.Count - 1; i++)
                {
                    if (wagonPart.Level == _wheelProgression[i].Level)
                    {
                        Player.Singleton.WagonStats.Wheel = _wheelProgression[i + 1];
                    }
                }
                break;

            case Body:
                for (int i = 0; i < _bodyProgression.Count - 1; i++)
                {
                    if (wagonPart.Level == _bodyProgression[i].Level)
                    {
                        Player.Singleton.WagonStats.Body = _bodyProgression[i + 1];
                    }
                }
                break;

            case Suspension:
                for (int i = 0; i < _suspensionProgression.Count - 1; i++)
                {
                    if (wagonPart.Level == _suspensionProgression[i].Level)
                    {
                        Player.Singleton.WagonStats.Suspension = _suspensionProgression[i + 1];
                    }
                }
                break;
        }
        _wagonStats.RecalculateValues();
    }


}
