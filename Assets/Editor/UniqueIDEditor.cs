using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UniqueID))]
public class UniqueIDEditor : Editor
{
    private UniqueID uniqueID;
    private void OnEnable()
    {
        uniqueID = (UniqueID)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("������ ���� ������ �� ����� (�� � ���� �������):");
        if (GUILayout.Button("������������� Unique ID"))
        {
            uniqueID.GenerateNewID();
        }

        if (GUI.changed)
            EditorUtility.SetDirty(uniqueID);
    }

}
