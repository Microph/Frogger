﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class GameManager : MonoBehaviour
{
    public GameConfig GameConfig;
    public UIManager UIManager;
    public ObjectPooler ObjectPooler;

    public GameState GameState;
    public GameStateSnapshot LastTickSnapshot;

    private PlayerInput _playerInput;
    private GameConfig _gameConfig;
    
    private void Awake()
    {
        _gameConfig = GameConfig;
        _playerInput = new PlayerInput();
        GameState.Initialize(_gameConfig);
        LastTickSnapshot = GameState.GetSnapshot();
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
        UIManager.UpdateUI(LastTickSnapshot);
        float dt = Time.deltaTime;
        GameStateSnapshot nextSnapShot = GameState.UpdateToNextTick(LastTickSnapshot, dt, _gameConfig, ObjectPooler, UIManager, _playerInput.PlayerFrog);
        LastTickSnapshot = nextSnapShot;
    }
}
