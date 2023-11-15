using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UsableEnvironment : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private MarkerSpawner _markerSpawner;
    [SerializeField] private string _warningLabel;
    [SerializeField] private string _warningMessage;
    [SerializeField] private int _cooldownHours;
    [SerializeField] private Sprite _defaultSprite; //Sprite на который будет носить неюзнутый предмет
    [SerializeField] private Sprite _spriteAfterUse; // Sprite на который будет носить юзнутый предмет
    [SerializeField] private ParticleSystem _particleSystem; // партиклы после юза выключаются 

    private bool _isActive = true;
    private float _distanceToUse = 3.1f;
    private UniqueID _uniqueID;
    private CooldownHandler _cooldownHandler;

    [SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        _cooldownHandler = FindObjectOfType<CooldownHandler>();
        _uniqueID = GetComponent<UniqueID>();
    }
    private void Start()
    {
        var thisObjectInCooldownHandler = _cooldownHandler.ObjectsOnCooldown.FirstOrDefault(item => item.UniqueID == _uniqueID.ID);
        if (thisObjectInCooldownHandler?.HoursLeft > 0)
        {
            CosmeticUse();
        }
        if (thisObjectInCooldownHandler != null && thisObjectInCooldownHandler.HoursLeft <= 0)
        {
            Restore();
            _cooldownHandler.Unregister(_uniqueID.ID);
        }

        _cooldownHandler.ReadyToReset += OnReadyToReset;
    }
    private void OnDisable()
    {
        _cooldownHandler.ReadyToReset -= OnReadyToReset;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isActive) 
            return;
        if ((transform.position - Player.Instance.transform.position).magnitude > _distanceToUse)
            return;
        
        if(IsFunctionalComplete())
        {
            CosmeticUse();
            _cooldownHandler.Register(_uniqueID.ID, _cooldownHours);
            _markerSpawner.DisableMarkerSpawner();
            
            _audioSource.PlayWithRandomPitch();
        }
        else
            CanvasWarningGenerator.Instance.CreateWarning(_warningLabel, _warningMessage);
    }
    protected abstract bool IsFunctionalComplete(); // возвращает true если все прошло как надо

    private void CosmeticUse() //Запретить трогать ещё раз, заменить спрайт и остановить партиклСистему
                               //(Не стоит понимать как функциональное использование)
    {
        GetComponent<SpriteRenderer>().sprite = _spriteAfterUse;
        _particleSystem.Stop();
        _isActive = false;
    }
    private void Restore()
    {
        GetComponent<SpriteRenderer>().sprite = _defaultSprite;
        _particleSystem.Play();
        _isActive = true;
        _markerSpawner.EnableMarkerSpawner();
    }
    private void OnReadyToReset(string uniqueID)
    {
        if (uniqueID == _uniqueID.ID)
        {
            Restore();
            _cooldownHandler.Unregister(_uniqueID.ID);
        }
    }


}
