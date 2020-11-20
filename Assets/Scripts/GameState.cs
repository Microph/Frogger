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
    public FrogManager FrogManager;
    public ObstacleManager ObstacleManager;
    public FinishSpotGameObject[] FinishSpots;

    private PlayerFrogAction _inputFrogAction = PlayerFrogAction.None;
    
    public void Initialize(GameConfig gameConfig)
    {
        FrogManager.Initialize(new FrogData(new MovableEntityData(new Vector2(0.5f, -7.5f), FacingDirection.Up)));
        ObstacleManager.Initialize(gameConfig);
    }

    public GameStateSnapshot GetSnapshot()
    {
        GameStateSnapshot snapshot = new GameStateSnapshot();
        snapshot.InputFrogAction = _inputFrogAction;
        return snapshot;
    }

    public GameStateSnapshot UpdateToNextTick(GameStateSnapshot lastTickGameStateSnapshot, float dt, GameConfig gameConfig, PlayerInput.PlayerFrogActions currentTickPlayerFrogAction)
    {
        //MoveFrog toward player direction with speed specified in gameConfig and move pixel by ratio according to dt
        Vector2 inputVector2 = currentTickPlayerFrogAction.Move.ReadValue<Vector2>();
        //Debug.Log($"inputVector2: {inputVector2}");
        _inputFrogAction = PlayerInputUtil.ConvertVector2ToPlayerFrogActionEnum(inputVector2);
        FrogManager.TickUpdate(_inputFrogAction, lastTickGameStateSnapshot, dt, gameConfig);
        ObstacleManager.TickUpdate(dt, gameConfig);
        //_gameState.UpdateGameStatus(dt, _gameConfig);
        return GetSnapshot();
    }
}
