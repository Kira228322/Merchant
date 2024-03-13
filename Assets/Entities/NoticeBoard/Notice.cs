using UnityEngine;
using UnityEngine.UI;

public abstract class Notice : MonoBehaviour
{
    //ќбъ€влени€ должны иметь кнопку, чтобы их сорвать, место дл€ текста, возможно действие при срыве объ€влени€.
    //¬озможно, сделать абстрактный класс и иметь EventNotice, QuestNotice etc.
    [HideInInspector] public string NoticeName;
    [HideInInspector] public string NoticeDescription;
    protected Noticeboard Noticeboard; //Ќужно иметь ссылку на доску объ€влений
    protected int SpawnPointIndex; //Ќужно знать, какое по счЄту это объ€вление на доске
                                   //дл€ того, чтобы их правильно убирать
    public Button DisplayButton;
    public abstract void Initialize(Noticeboard noticeboard, string name, string text, int number);
    public abstract void OnNoticeTake();

}
