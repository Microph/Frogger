using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    public GameConfig GameConfig;
    public UIManager UIManager;

    public GameState GameState;
    public GameStateSnapshot LastTickSnapshot;
    public GameStateSnapshot CurrentTickSnapshot;

    private PlayerInput _playerInput;
    private GameConfig _gameConfig;
    
    private void Awake()
    {
        _gameConfig = GameConfig;
        _instance = this;
        _playerInput = new PlayerInput();
        GameState.Initialize(_gameConfig);
        LastTickSnapshot = GameState.GetSnapshot();
        CurrentTickSnapshot = LastTickSnapshot;
    }

    private void OnEnable()
    {
        if (_playerInput != null)
        {
            _playerInput.Enable();
        }
    }

    private void OnDisable()
    {
        if (_playerInput != null)
        {
            _playerInput.Disable();
        }
    }

    void Update()
    {
        //Main game loop
        float dt = Time.deltaTime;
        GameStateSnapshot nextSnapShot = GameState.UpdateToNextTick(LastTickSnapshot, dt, _gameConfig, _playerInput.PlayerFrog);
        LastTickSnapshot = CurrentTickSnapshot;
        CurrentTickSnapshot = nextSnapShot;
        UIManager.UpdateUI(LastTickSnapshot);
    }
}
