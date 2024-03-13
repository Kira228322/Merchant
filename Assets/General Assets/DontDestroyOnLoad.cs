using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    // Накидывать этот скрипт на объекты, которые не удалять при загрузке
    // Канвас и различные менеджеры, может что-то еще. Я думаю лучше отдельный скрипт для этого пусть будет. Так нагляднее
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
