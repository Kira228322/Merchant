using System;

[Serializable]
public class GlobalEvent_Weather : GlobalEvent_Base
{
    public override string GlobalEventName => null;
    public override string Description => null;
    public int StrengthOfWeather { get; set; }
    public override void Execute()
    {
        //�� �� ������, ����� ���������� ��� � ��� � WeatherController.
        //������ ������ �� �����, ����� ��-���� ������� �������.
        UnityEngine.Object.FindObjectOfType<WeatherController>().StartWeather();
    }

    public override void Terminate()
    {
        UnityEngine.Object.FindObjectOfType<WeatherController>().RemoveEvent();
    }
}
