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

    private PlayerInput _playerInput;
    private GameConfig _gameConfig;
    
    private void Awake()
    {
        _instance = this;
        _playerInput = new PlayerInput();
        GameState = new GameState();
        _gameConfig = new GameConfig();

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

    void Update()
    {
        //main game loop
        //update GameState with methods
        float dt = Time.deltaTime;
        GameState.MoveFrog(dt, _gameConfig, _playerInput.PlayerFrog);
        //_gameState.UpdateObstacle(dt, _gameConfig);
        /*
        _gameState.CreateObstacle(dt, _gameConfig);
        _gameState.MoveObstacle(dt, _gameConfig);
        _gameState.RemoveObstacle(dt, _gameConfig);
        */
        //_gameState.UpdateFrogStatus(dt, _gameConfig);
        //_gameState.UpdateGameStatus(dt, _gameConfig);
    }
}
