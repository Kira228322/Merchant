using UnityEngine;
using UnityEngine.UI;

public abstract class Notice : MonoBehaviour
{
    //ќбъ€влени€ должны иметь кнопку, чтобы их сорвать, место дл€ текста, возможно действие при срыве объ€влени€.
    //¬озможно, сделать абстрактный класс и иметь EventNotice, QuestNotice etc.
    [HideInInspector] public string NoticeName;
    [HideInInspector] public string NoticeDescription;
    public Button DisplayButton;

    public abstract void Initialize(string name, string text);
    public abstract void OnNoticeTake();

}
