using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newTraderData", menuName = "NPCs/TraderData")]
public class NPCTraderData : NPCData
{
    public List<TraderType> TraderTypes;
    public int RestockCycle;
    public int LastRestock;
    public List<NPCTrader.TraderGood> Goods;
    

}
