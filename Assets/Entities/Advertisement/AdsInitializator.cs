using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializator : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] public string AndroindGameID = "5409405";
    [SerializeField] public string IOSGameID = "5409404";
    [SerializeField] public bool testMode = false;
    private string _gameID;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        _gameID = (Application.platform == RuntimePlatform.IPhonePlayer) ? IOSGameID : AndroindGameID;
        Advertisement.Initialize(_gameID, testMode, this);
    }

    public void OnInitializationComplete()
    {

    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {

    }
}
