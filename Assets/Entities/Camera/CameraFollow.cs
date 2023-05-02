using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        Vector3 targetPosition = target.position;
        targetPosition.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneChange;
    }

    private void OnSceneChange(Scene arg0, LoadSceneMode arg1)
    {
        // Такая котовасия нужна, потому что в момент SceneManager.sceneLoaded игрок еще не изменил свою позицию
        // на SpawnPoint поэтому камера летит из позиции, в которой она была на прошлой сцене в позицию спавн поинта
        // Нужно пропустить кадр, чтобы игрок оказался в SpawnPoint после чего переместить туда же камеру!!
        StartCoroutine(SkipFrame());
    }

    private IEnumerator SkipFrame()
    {
        yield return new WaitForFixedUpdate();
        transform.position = target.position;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneChange;
    }
}
