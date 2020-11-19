using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInputUtil;

public struct GameStateSnapshot
{
    public PlayerFrogAction InputFrogAction;
    //Can add more data here if needed
}

public class GameState : MonoBehaviour
{
    public FrogGameObject FrogGameObject;
    public ObstacleManager ObstacleManager;
    public FinishSpotData[] FinishSpotDatas;

    private PlayerFrogAction _inputFrogAction = PlayerFrogAction.None;
    
    //for quick debug
    public void Initialize(GameConfig gameConfig)
    {
        FrogGameObject.FrogData = new FrogData(new MovableEntityData(new Vector2(0.5f, -7.5f), FacingDirection.Up));
        ObstacleManager.Initialize(gameConfig);
    }

    public GameStateSnapshot GetSnapshot()
    {
        GameStateSnapshot snapshot = new GameStateSnapshot();
        snapshot.InputFrogAction = _inputFrogAction;
        return snapshot;
    }

    public GameStateSnapshot UpdateToNextFrame(GameStateSnapshot lastFrameGameStateSnapshot, float dt, GameConfig gameConfig, PlayerInput.PlayerFrogActions currentFramePlayerFrogAction)
    {
        //MoveFrog toward player direction with speed specified in gameConfig and move pixel by ratio according to dt
        Vector2 inputVector2 = currentFramePlayerFrogAction.Move.ReadValue<Vector2>();
        //Debug.Log($"inputVector2: {inputVector2}");
        _inputFrogAction = PlayerInputUtil.ConvertVector2ToPlayerFrogActionEnum(inputVector2);
        FrogGameObject.FrameUpdate(_inputFrogAction, lastFrameGameStateSnapshot, dt, gameConfig);
        ObstacleManager.FrameUpdate(dt, gameConfig);
        //_gameState.UpdateFrogStatus(dt, _gameConfig);
        //_gameState.UpdateGameStatus(dt, _gameConfig);
        return GetSnapshot();
    }
}
