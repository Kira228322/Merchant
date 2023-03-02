using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class TestClickMoveToAnotherScene : MonoBehaviour, IPointerClickHandler
{
    public string DestinationSceneName;
    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene(DestinationSceneName);
    }
}
