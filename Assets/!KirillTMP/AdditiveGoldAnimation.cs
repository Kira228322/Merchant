using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class AdditiveGoldAnimation : MonoBehaviour
{
    private TMP_Text _playerGold;
    [SerializeField] private TMP_Text _AdditiveGold;
    private Coroutine _currentAnimation;

    private Vector2 _startPos;

    private void Start()
    {
        _playerGold = gameObject.GetComponent<TMP_Text>();
        _startPos = _AdditiveGold.rectTransform.anchoredPosition;
    }

    public void PlayGoldIncrease(int amount)
    {
        if (_currentAnimation != null)
            StopCoroutine(_currentAnimation);
        _currentAnimation = StartCoroutine(GoldIncrease(amount));
    }

    public void PlayGoldDecrease(int amount)
    {
        if (_currentAnimation != null)
            StopCoroutine(_currentAnimation);
        _currentAnimation = StartCoroutine(GoldDecrease(amount));
    }

    private IEnumerator GoldIncrease(int gold)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);

        int startGold = Player.Instance.Money;
        int finishGold = startGold + gold;

        _AdditiveGold.gameObject.SetActive(true);
        _AdditiveGold.rectTransform.anchoredPosition = _startPos;
        _AdditiveGold.color = new Color(233f / 255, 177f / 255, 0);
        Color fadedColor = _AdditiveGold.color;
        _AdditiveGold.text = $"<alpha=#00>{_playerGold.text}<alpha=#FF> + {gold}";
        
        for (int i = 1; i <= 20; i++)
        {
            _AdditiveGold.rectTransform.anchoredPosition += new Vector2(-0.35f - 0.03f * i,0);
            fadedColor.a -= 0.02f;
            _AdditiveGold.color = fadedColor;
            yield return waitForSeconds;
        }
        
        for (int i = 1; i <= 30; i++)
        {
            _AdditiveGold.rectTransform.anchoredPosition += new Vector2(-0.95f - 0.03f * i,0);
            fadedColor.a -= 0.02f;
            _AdditiveGold.color = fadedColor;
            _playerGold.text = Math.Ceiling(math.lerp(startGold, finishGold, i*i/900f)).ToString();
            yield return waitForSeconds;
        }

        _playerGold.text = finishGold.ToString();
        _AdditiveGold.gameObject.SetActive(false);
    }

    private IEnumerator GoldDecrease(int gold)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);

        int startGold = Player.Instance.Money;
        int finishGold = startGold - gold;

        _AdditiveGold.gameObject.SetActive(true);
        _AdditiveGold.rectTransform.anchoredPosition = _startPos;
        _AdditiveGold.color = new Color(0.849f, 0.144f, 0);
        Color fadedColor = _AdditiveGold.color;
        _AdditiveGold.text = $"<alpha=#00>{_playerGold.text}<alpha=#FF> - {gold}";
        
        for (int i = 1; i <= 20; i++)
        {
            _AdditiveGold.rectTransform.anchoredPosition += new Vector2(0,0.1f + 0.07f * i);
            fadedColor.a -= 0.02f;
            _AdditiveGold.color = fadedColor;
            yield return waitForSeconds;
        }
        
        for (int i = 1; i <= 30; i++)
        {
            _AdditiveGold.rectTransform.anchoredPosition += new Vector2(0,1.5f + 0.07f * i);
            fadedColor.a -= 0.02f;
            _AdditiveGold.color = fadedColor;
            _playerGold.text = Math.Ceiling(math.lerp(startGold, finishGold, i*i/900f)).ToString();
            yield return waitForSeconds;
        }

        _playerGold.text = finishGold.ToString();
        _AdditiveGold.gameObject.SetActive(false);
    }
    
    
}
