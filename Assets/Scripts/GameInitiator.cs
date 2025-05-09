using System.Collections;
using UnityEngine;
using ImprovedTimers;

public class GameInitiator : MonoBehaviour
{
    [SerializeField] private Camera _mainCameraPrefab;
    [SerializeField] private Light _lightPrefab;
    [SerializeField] private CountdownTimerTest _timerTestPrefab;

    private Camera _mainCamera;
    private Light _light;
    private CountdownTimerTest _timerTest;


    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        _mainCamera = Instantiate(_mainCameraPrefab);
        _light = Instantiate(_lightPrefab);
        _timerTest = Instantiate(_timerTestPrefab);
    }
}