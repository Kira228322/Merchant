using UnityEditor;
using UnityEngine;

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
            _status.Effects.Add(new Status.Effect());
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
                effect.stat = (Status.Effect.Stat)EditorGUILayout.EnumPopup(effect.stat);
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