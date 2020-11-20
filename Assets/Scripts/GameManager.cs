using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    
    public GameState GameState;
    public GameStateSnapshot LastTickSnapshot;
    public GameStateSnapshot CurrentTickSnapshot;

    private PlayerInput _playerInput;
    private GameConfig _gameConfig;
    
    private void Awake()
    {
        _instance = this;
        _playerInput = new PlayerInput();
        _gameConfig = new GameConfig();
        GameState.Initialize(_gameConfig);
        LastTickSnapshot = GameState.GetSnapshot();
        CurrentTickSnapshot = LastTickSnapshot;

#if UNITY_IOS || UNITY_ANDROID
        //mobileInputs.SetActive(true);
        Screen.orientation = ScreenOrientation.Portrait;
#endif

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

    void Start()
    {
        //SetupNewGame
    }

    private void SetupNewGame()
    {
        //initize stuffs
    }

    void FixedUpdate()
    {
        //main game loop
        float dt = Time.fixedDeltaTime;
        GameStateSnapshot nextSnapShot = GameState.UpdateToNextTick(LastTickSnapshot, dt, _gameConfig, _playerInput.PlayerFrog);
        LastTickSnapshot = CurrentTickSnapshot;
        CurrentTickSnapshot = nextSnapShot;
    }
}
