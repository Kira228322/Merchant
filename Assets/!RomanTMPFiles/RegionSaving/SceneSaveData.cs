using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SceneSaveData
    //�� ����� ����� ����� ��������� � ������ ����������
{
    public string sceneName;

    public SceneSaveData(string sceneName)
    {
        this.sceneName = sceneName;

    }
}
