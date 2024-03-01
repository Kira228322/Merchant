using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUIObject : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _shell;
    [SerializeField] private GameObject _infoWindow;

    private ActiveStatus _activeStatus;

    public void Init(ActiveStatus activeStatus)
    {
        _activeStatus = activeStatus;

        _icon.sprite = activeStatus.StatusData.Icon;
        if (activeStatus.StatusData.Type == Status.StatusType.Buff)
            _shell.color = new Color(39f / 255, 255f / 255, 0);
        else
            _shell.color = new Color(255f / 255, 24f / 255, 0);

        activeStatus.StatusUpdated += Refresh;
    }
    private void Refresh()
    {
        if (!_activeStatus.IsActive)
            Destroy(gameObject);
        
        _shell.fillAmount = _activeStatus.CurrentDurationHours / _activeStatus.StatusData.HourDuration;
    }
    public void OnClick()
    {
        if (StatusManager.Instance.CurrentStatusInfoWindow != null)
            Destroy(StatusManager.Instance.CurrentStatusInfoWindow);
        GameObject window = Instantiate(_infoWindow, MapManager.Canvas.transform);
        window.GetComponent<StatusInfoWindow>().Init
            (_activeStatus.StatusData.StatusName,
            _activeStatus.StatusData.StatusDescription,
            _activeStatus.CurrentDurationHours,
            _activeStatus.StatusData.HourDuration);

        Vector3 position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        position += new Vector3(0, _icon.rectTransform.rect.height + window.GetComponent<Image>().rectTransform.rect.height / 2, 0);

        window.transform.position = position;
    }

}