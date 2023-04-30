using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notice : MonoBehaviour
{
    //ќбъ€влени€ должны иметь кнопку, чтобы их сорвать, место дл€ текста, возможно действие при срыве объ€влени€.
    //¬озможно, сделать абстрактный класс и иметь EventNotice, QuestNotice etc.
    [SerializeField] private TMPro.TMP_Text _noticeText;
}
