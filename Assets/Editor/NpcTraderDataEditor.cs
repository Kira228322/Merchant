using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NpcTraderData))]
class NpcTraderDataEditor : Editor
{
    private NpcTraderData _traderData;
    private void OnEnable()
    {
        _traderData = (NpcTraderData)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(20);
        if (GUILayout.Button("Добавить товар"))
            _traderData.BaseGoods.Add(new NpcTrader.TraderGood());

        if (_traderData.BaseGoods.Count > 0)
        {
            foreach (var good in _traderData.BaseGoods)
            {
                GUILayout.Space(-1);
                EditorGUILayout.BeginVertical("box");


                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("X", GUILayout.Height(17), GUILayout.Width(18)))
                {
                    _traderData.BaseGoods.Remove(good);
                    break;
                }
                EditorGUILayout.Space(5);
                GUILayout.Label("Item", GUILayout.MaxWidth(30));
                good.Good = (Item)EditorGUILayout.ObjectField(good.Good,
                    typeof(Item), false, GUILayout.MaxWidth(156));
                GUILayout.FlexibleSpace();
                if (good.Good != null)
                    EditorGUILayout.LabelField("Average Price - " + good.Good.Price, GUILayout.MaxWidth(130));
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(27);
                GUILayout.Label("Max count", GUILayout.MaxWidth(136));
                good.MaxCount = EditorGUILayout.IntField(good.MaxCount, GUILayout.MaxWidth(50));
                GUILayout.FlexibleSpace();
                good.CurrentCount = good.MaxCount;
                GUILayout.Label("Price", GUILayout.MaxWidth(40));
                good.CurrentPrice = EditorGUILayout.IntField(good.CurrentPrice, GUILayout.MaxWidth(70));
                GUILayout.Space(17);
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(-5);
                EditorGUILayout.EndVertical();
            }
        }

        if (GUI.changed)
            EditorUtility.SetDirty(_traderData);
    }
}
