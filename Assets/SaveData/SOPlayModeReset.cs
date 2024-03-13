using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// 
/// ���� ������ �������� ������ ���� � ������������, � ����� �� ��������.
/// �� ���� ��� ScriptableObject ��� �������� ���������� � �������� ��, ������� ����� ���� ���������
/// (��� ����� ���. ��������� IResetOnExitPlaymode)
/// 
/// </summary>

static class SOPlayModeReset
{
    [InitializeOnLoadMethod]
    static void RegisterResets()
    {
        EditorApplication.playModeStateChanged += ResetScriptableObjects;
    }

    static void ResetScriptableObjects(PlayModeStateChange change)
    {
        if (change == PlayModeStateChange.ExitingPlayMode)
        {
            var assets = FindAssets<ScriptableObject>();
            foreach (ScriptableObject asset in assets)
            {
                if (asset is IResetOnExitPlaymode)
                {
                    (asset as IResetOnExitPlaymode).ResetOnExitPlaymode();
                }
            }
        }
    }

    static T[] FindAssets<T>() where T : Object
    {
        var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
        var assets = new T[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            var path = AssetDatabase.GUIDToAssetPath(guids[i]);
            assets[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }
        return assets;

    }
}
#endif
public interface IResetOnExitPlaymode
{
    public void ResetOnExitPlaymode();
}