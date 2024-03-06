using System.Collections.Generic;
using UnityEngine;

public class EventForestFire : EventInTravel
{
    [SerializeField] private List<GameObject> _destroyedObjects;
    public override void SetButtons()
    {
        ButtonsLabel.Add("������ � �������");
        ButtonsLabel.Add("�� � ��� ���������");
        SetInfoButton("�� ������� ������ ����� �����-�� ����� � �� ��������� ���� ����.");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                GameTime.TimeSkip(0, 1, Random.Range(10, 30));
                Player.Instance.Needs.CurrentHunger -= 10;
                Player.Instance.Needs.CurrentSleep -= 5;
                int exp = Random.Range(3, 5);
                Player.Instance.Experience.AddExperience(exp);
                _eventWindow.ChangeDescription($"����� ��� ������� �������! �� �������� {exp} �����!");

                foreach (var obj in _destroyedObjects)
                    Destroy(obj);

                break;
            case 1:
                _eventWindow.ChangeDescription("�� ������� ���, ��� ������ �� �������� � ������� ������.");
                break;
        }
    }
}
