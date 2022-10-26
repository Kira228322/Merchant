using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    // ���� � ������� Item'a ��� ������� ����, �� ������� ��� ����� ������ � ��������
    // ������ �������� ����� ����� ���� ���� �� ���� �����. ��� � ���� � ����� ���� ���
    
    public List<Item> Goods;
    // public List<int> NewPrice;
    // public List<int> CountOfGood;
    private int[] _newPrice;
    private int[] _countOfGood;

    private void Start()
    {
        for (int i = 0; i < Goods.Count; i++)
        {
            Goods[i].Price = _newPrice[i];
        }
    }




    [CustomEditor(typeof(Merchant))]
    public class MerchantEditor : Editor // �����-�� �� ���� ������ ����� 
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            
            Merchant merchant = (Merchant)target;

            merchant._newPrice = new int[merchant.Goods.Count];
            merchant._countOfGood = new int[merchant.Goods.Count];
            for (int i = 0; i < merchant.Goods.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(merchant.Goods[i].Name, GUILayout.MaxWidth(110));
                
                EditorGUILayout.Space(3, true);
                EditorGUILayout.LabelField("Price", GUILayout.MaxWidth(40));
                merchant._newPrice[i] = EditorGUILayout.IntField(merchant._newPrice[i]);
                
                EditorGUILayout.Space(3, true);
                EditorGUILayout.LabelField("Count", GUILayout.MaxWidth(40));
                merchant._countOfGood[i] = EditorGUILayout.IntField(merchant._countOfGood[i]);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
