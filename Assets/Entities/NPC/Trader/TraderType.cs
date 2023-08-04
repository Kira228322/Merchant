using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "newTraderType", menuName = "Trader/TraderType")]
public class TraderType : ScriptableObject
{
    [HideInInspector] public List<TraderGoodType> TraderGoodTypes = new ();
    [System.Serializable]
    public class TraderGoodType
    {
        public Item.ItemType ItemType;
        public float Coefficient;
        public int CountToBuy;

        public TraderGoodType(TraderGoodType original)
        {
            ItemType = original.ItemType;
            Coefficient = original.Coefficient;
            CountToBuy = original.CountToBuy;
        }
        public TraderGoodType()
        {

        }
    }
}
