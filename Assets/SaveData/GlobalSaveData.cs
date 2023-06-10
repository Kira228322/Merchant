using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GlobalSaveData 
{
    //Сборник всех компонентов, которые нужно сейвить. Чтобы сохранение было единым файлом, а не кучей разных.
    public PlayerData PlayerData;
    public QuestSaveData QuestSaveData;
    public NpcDatabaseSaveData NpcDatabaseSaveData;
    public TimeFlowSaveData TimeFlowSaveData;
    public CooldownHandlerSaveData CooldownHandlerSaveData;
    public GlobalEventHandlerSaveData GlobalEventHandlerSaveData;
    public RegionSaveData RegionSaveData;
    public SceneSaveData SceneSaveData;
}
