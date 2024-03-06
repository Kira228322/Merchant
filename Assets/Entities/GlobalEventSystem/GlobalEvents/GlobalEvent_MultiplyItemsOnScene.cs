using System;

[Serializable]
public class GlobalEvent_MultiplyItemsOnScene : GlobalEvent_Base
{
    public override string GlobalEventName => IsPositive ? ($"������� ������� ��������� � {LocationVillageName}!")
                                                         : ($"������ �������� � {LocationVillageName}");

    public override string Description => IsPositive ? ($"��������� ��������� �������, � {LocationVillageName} ��������� ����������� ������� ������� �������� {ItemToMultiplyName}.")
                                                     : ($"��-�� ����������� ������� ����� �� ������� {LocationVillageName}, � ��� ��������� ������ �������� {ItemToMultiplyName}");

    public bool IsPositive;
    public string LocationSceneName;
    public string LocationVillageName;
    public float MultiplyCoefficient;
    public string ItemToMultiplyName;

    public override void Execute()
    {
        Location location = MapManager.GetLocationBySceneName(LocationSceneName);
        location.MultiplyItemsInTraders(ItemToMultiplyName, MultiplyCoefficient);
    }

    public override void Terminate()
    {
        //����������� �����.
        //����� ��������/������� �������� ��� ��� �������� ������ ������������ ���� ��-�� ������� ���������.
        //������� ������ �� ���������� ����� ����� �������������.
    }
}
