using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "newItem", menuName = "Item")]
public class Item : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icone; 
    public int Price; // вот насчет уены, в будущем думаю стоит сделать цену по которой этот товар продается для игрока        
                      // и отдельно по которой он ее может продать
                      // условно яблоко купил у торговца за 5, сам продать может за 4
                      // а потом всякие накрутки за локацию, за обстоятельства и т д
                      
    public float Weight;
    public int MaxCountInSlot; // максимальное колличество данного ресурса, которое помещается в 1 ячейку
                               // (например до 9 яблок в одну ячейку)
                               
    public int CellSize; // Это дальше продумать надо, это то о чем мы говорили, что бы было как в диабло (1х1слот, 2х2, 1х2..)

    [HideInInspector]public bool Perishable; // портящийся ли продукт? // забавно, что perish - погибнуть, а perishable - скоропортящийся
    [HideInInspector] public float _daysToHalfSpoil; // думал сделать так что бы продукт протился в 2 этапа 
    [HideInInspector] public float _daysToSpoil;                       // когда проходит daysToHalfSpoil - продукт становится хуже по качеству 
                                                      // и его не продать за ту же стоимость, что и новый
                                                      // можно продать за 50-70%. А после daysToSpoil - продать нельзя 

    [CustomEditor(typeof(Item))] 
    public class ItemEditor : Editor // когда-то ты меня научил этому 
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Item item = (Item)target;

            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Perishable", GUILayout.MaxWidth(80));
            item.Perishable = EditorGUILayout.Toggle(item.Perishable);
            EditorGUILayout.EndHorizontal();
            
            if (item.Perishable)
            {
                EditorGUILayout.Space(-4);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(20, true);
                EditorGUILayout.LabelField("Days to half spoil", GUILayout.MaxWidth(110));
                item._daysToHalfSpoil = EditorGUILayout.FloatField(item._daysToHalfSpoil);
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(20, true);
                EditorGUILayout.LabelField("Days to spoil", GUILayout.MaxWidth(110));
                item._daysToSpoil = EditorGUILayout.FloatField(item._daysToSpoil);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
