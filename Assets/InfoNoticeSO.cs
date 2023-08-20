using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "newInfoNoticeSO", menuName = "InfoNoticeSO")]
public class InfoNoticeSO : ScriptableObject
{
    [Serializable]
    public class NoticeInformation
    {
        public string Name;
        public string Description;
    }

    [SerializeField] private List<NoticeInformation> _noticeInfos = new();
    public List<Noticeboard.CompactedInfoNotice> GetRandomNoticeInfos(int number)
    {
        if (number > _noticeInfos.Count)
        {
            Debug.LogError("Слишком мало объявлений в данном SO. Нужно написать больше");
            return null;
        }
        List<Noticeboard.CompactedInfoNotice> result = new();
        List<NoticeInformation> shuffledNoticeInfos = new(_noticeInfos);
        shuffledNoticeInfos.Shuffle();
        for (int i = 0; i < number; i++)
        {
            result.Add(new(shuffledNoticeInfos[i].Name, shuffledNoticeInfos[i].Description));
        }
        return result;
    }
}
