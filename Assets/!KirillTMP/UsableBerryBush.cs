using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableBerryBush : UsableEnvironment
{
    [SerializeField] private Sprite _bushWithoutBerreis;
    protected override void Functional()
    {
        // TODO ��������� ���� �� ����� ��������� � ��������� � �������� ����� �����-������

        GetComponent<SpriteRenderer>().sprite = _bushWithoutBerreis;
    }
}
