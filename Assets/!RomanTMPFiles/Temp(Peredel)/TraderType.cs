using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roman
{
    [CreateAssetMenu(fileName = "newRomanTraderType", menuName = "Roman/TraderType")]
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
}
