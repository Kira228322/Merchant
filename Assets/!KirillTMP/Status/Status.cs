using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewStatus", menuName = "Status or Item/Status")]
public class Status : ScriptableObject
{
    [Header("Status description")]
    [SerializeField] private string _statusName;
    [SerializeField] [TextArea(2,5)] private string _statusDescription;
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
        public enum Stat {Luck, ExpGain}
        public Stat stat;
        public int value;
    }

    [CustomEditor(typeof(Status))]
    public class StatusEditor : Editor
    {
        private Status _status;

        private void OnEnable()
        {
            _status = (Status)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            GUILayout.Space(20);
            if (GUILayout.Button("Добавить эффект статуса"))
            {
                _status.Effects.Add(new Effect());
            }

            if (_status.Effects.Count > 0)
            {
                foreach (var effect in _status.Effects)
                {
                    EditorGUILayout.BeginVertical("box");

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("X", GUILayout.Height(17), GUILayout.Width(18)))
                    {
                        _status.Effects.Remove(effect);
                        break;
                    }
                    EditorGUILayout.Space(5);
                    GUILayout.Label("Player Stat", GUILayout.MaxWidth(72));
                    effect.stat = (Effect.Stat)EditorGUILayout.EnumPopup(effect.stat);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Value", GUILayout.MaxWidth(38));
                    effect.value = (int)EditorGUILayout.Slider(effect.value, -100, 100);
                    EditorGUILayout.EndHorizontal();
                    
                    EditorGUILayout.EndVertical();
                }
            }
            if (GUI.changed)
            {
                EditorUtility.SetDirty(_status);
            }
        }
    }
}
