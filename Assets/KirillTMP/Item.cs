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
    public int Price; // ��� ������ ����, � ������� ����� ����� ������� ���� �� ������� ���� ����� ��������� ��� ������        
                      // � �������� �� ������� �� �� ����� �������
                      // ������� ������ ����� � �������� �� 5, ��� ������� ����� �� 4
                      // � ����� ������ �������� �� �������, �� �������������� � � �
                      
    public float Weight;
    public int MaxCountInSlot; // ������������ ����������� ������� �������, ������� ���������� � 1 ������
                               // (�������� �� 9 ����� � ���� ������)
                               
    public int CellSize; // ��� ������ ��������� ����, ��� �� � ��� �� ��������, ��� �� ���� ��� � ������ (1�1����, 2�2, 1�2..)

    [HideInInspector]public bool Perishable; // ���������� �� �������? // �������, ��� perish - ���������, � perishable - ���������������
    [HideInInspector] public float _daysToHalfSpoil; // ����� ������� ��� ��� �� ������� �������� � 2 ����� 
    [HideInInspector] public float _daysToSpoil;                       // ����� �������� daysToHalfSpoil - ������� ���������� ���� �� �������� 
                                                      // � ��� �� ������� �� �� �� ���������, ��� � �����
                                                      // ����� ������� �� 50-70%. � ����� daysToSpoil - ������� ������ 

    [CustomEditor(typeof(Item))] 
    public class ItemEditor : Editor // �����-�� �� ���� ������ ����� 
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
