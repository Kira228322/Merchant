using System.Collections.Generic;

public class NpcWagonUpgrader : Npc
{
    private NpcWagonUpgraderData _npcWagonUpgraderData;

    private void Awake()
    {
        _npcWagonUpgraderData = (NpcWagonUpgraderData)NpcData;
    }

    public List<WagonPart> CurrentUpgrades => _npcWagonUpgraderData.CurrentWagonUpgrades;


}
