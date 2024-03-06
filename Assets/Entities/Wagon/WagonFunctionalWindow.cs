using UnityEngine;

public class WagonFunctionalWindow : MonoBehaviour
{
    [SerializeField] private GameObject _sleepPanelPrefab;

    private void OnEnable()
    {
        Player.Instance.PlayerMover.DisableMove();
        GameManager.Instance.CurrentFunctionalWindow = gameObject;
    }

    public void OnSleepButtonClick()
    {
        Instantiate(_sleepPanelPrefab, MapManager.Canvas.transform);
        Destroy(gameObject);
    }

    public void OnSaveButtonClick()
    {
        GameManager.Instance.SaveGame();
        Player.Instance.PlayerMover.EnableMove();
        Destroy(gameObject);
    }

    public void OnCloseButtonClick()
    {
        Player.Instance.PlayerMover.EnableMove();
        GameManager.Instance.CurrentFunctionalWindow = null;
        Destroy(gameObject);
    }
}
