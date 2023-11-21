using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GlobalSaveData 
{
    //������� ���� �����������, ������� ����� �������. ����� ���������� ���� ������ ������, � �� ����� ������.
    public PlayerData PlayerData;
    public StatusManagerSaveData StatusManagerSaveData;
    public JournalSaveData JournalSaveData;
    public BannedItemsSaveData BannedItemsSaveData;
    public NpcDatabaseSaveData NpcDatabaseSaveData;
    public TimeFlowSaveData TimeFlowSaveData;
    public CooldownHandlerSaveData CooldownHandlerSaveData;
    public GlobalEventHandlerSaveData GlobalEventHandlerSaveData;
    public RegionSaveData RegionSaveData;
    public SceneSaveData SceneSaveData;
    public TutorialStateTrackerSaveData TutorialTrackerSaveData;
    public MenuSaveData MenuSaveData;
}
