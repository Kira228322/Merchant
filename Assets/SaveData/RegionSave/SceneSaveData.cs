using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SceneSaveData
    //На какой сцене игрок находился в момент сохранения
{
    public string sceneName;

    public SceneSaveData(string sceneName)
    {
        this.sceneName = sceneName;

    }
}
