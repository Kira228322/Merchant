using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newTraderType", menuName = "Trader/TraderType")]
public class TraderType : ScriptableObject
{
    public List<TraderGoodType> TraderGoodTypes;
    [System.Serializable]
    public class TraderGoodType
    {
        public Item.ItemType ItemType;
        public float Coefficient;
        public int CountToBuy;
    }
}
