using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Noticeboard : MonoBehaviour
{
    [SerializeField] private List<Transform> _noticeSpawnPoints;
    [SerializeField] private List<GameObject> _noticePrefabs;
    private List<IGlobalEvent> _activeGlobalEvents;
    public List<Notice> Notices = new();
    private void Start() //при заходе на сцену получить список активных ивентов и создать по ним объявления
    {
        _activeGlobalEvents = new(GlobalEventHandler.Instance.ActiveGlobalEvents);
        foreach (IGlobalEvent globalEvent in _activeGlobalEvents)
        {
            if ((string)globalEvent.GetType().GetProperty("Description").GetValue(null) != null) //Если у этого типа написано Description
            {
                //Создать Notice рандомного префаба, добавить ему текст.
                Notice notice = Instantiate(_noticePrefabs[Random.Range(0, _noticePrefabs.Count)], _noticeSpawnPoints[0]).GetComponent<Notice>();
            }
        }
    }

    public void Interact()
    {

    }
}
