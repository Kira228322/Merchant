using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewStatus", menuName = "Status/Status")]
public class Status : ScriptableObject
{
    [Header("Status description")]
    [SerializeField] private string _statusName;

    public string StatusName => _statusName;
    [SerializeField] [TextArea(2,5)] private string _statusDescription;
    public string StatusDescription => _statusDescription;
    [SerializeField] private Sprite _icon;
    public Sprite Icon => _icon;
    [SerializeField] private StatusType _type;
    public StatusType Type => _type;
    public enum StatusType {Buff, Debuff}
    
    [Header("Status stats")]
    [SerializeField] private int _hourDuration;

    public int HourDuration => _hourDuration;
    [HideInInspector] public List<Effect> Effects;
    
    [Serializable]
    public class Effect
    {
        public enum Stat {Luck, ExpGain, Diplomacy, Toughness, MoveSpeed}
        public Stat stat;
        public int value;
    }

}
