using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeflow : MonoBehaviour
{
    public float TimeScale; // TimeScale ������ ������ �� �������� ������� ����� � �� �������� ����� ���������
    private void Update()
    {
        GameTime.Minutes += Time.deltaTime * TimeScale;
    }
}
