using System;

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
