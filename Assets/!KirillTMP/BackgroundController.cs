using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    private float _parallaxFactor = 0.01f;
    
    [SerializeField]private RawImage _farBackground;
    [SerializeField]private RawImage _nearBackground;

    private float _leftEdgeX;
    private float _rightEdgeX;
    
    private float _previousPositionX;

    public void Init(float leftEdge, float rightEdge, Sprite nearBG, Sprite farBG)
    {
        _nearBackground.texture = nearBG.texture;
        _farBackground.texture = farBG.texture;
        _previousPositionX = Player.Instance.transform.position.x;
        _leftEdgeX = leftEdge;
        _rightEdgeX = rightEdge;
        UpdateBackground(_previousPositionX);
    }
    
    public void UpdateBackground(float currentPositionX)
    {
        float delta = currentPositionX - _previousPositionX;
        _previousPositionX = currentPositionX;

        _nearBackground.uvRect = new Rect(new Vector2(_nearBackground.uvRect.x + delta *_parallaxFactor, 0), _nearBackground.uvRect.size);
        
        float value = (currentPositionX - _leftEdgeX) / (_rightEdgeX - _leftEdgeX);
        _farBackground.uvRect = new Rect(new Vector2(math.lerp(-0.083f, 0.083f, value), 0), _farBackground.uvRect.size);
    }

    
}
