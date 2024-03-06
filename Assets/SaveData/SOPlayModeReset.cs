using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// 
/// Этот скрипт работает только пока в Юнитиэдиторе, в билде не работает.
/// Он ищет все ScriptableObject при закрытии приложения и ресетает те, которые могут быть ресетнуты
/// (Для этого исп. интерфейс IResetOnExitPlaymode)
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