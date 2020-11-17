using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInputUtil;

public struct GameStateSnapshot
{
    public Vector2 FrogPosition;
    public FrogState FrogState;
    public FacingDirection FrogFacingDirection;
    public ObstacleStruct[][] Obstacles;
    public FinishSpotData[] FinishSpotDatas;
    public PlayerFrogActionEnum PlayerFrogActionEnum;
    public float CurrentSameMoveDelay;
}

public struct ObstacleStruct
{
    public ObstacleType ObstacleType;
    public Vector2 Position;
    public FacingDirection FacingDirection;
    public int Width; //block unit
}

public enum ObstacleType
{
    Log,
    Turtle,
    CarPink,
    Truck,
    CarRed,
    CarYellow
}

public class GameState
{
    public FrogData FrogData;
    public RowData[] GroundRowDatas;
    public RowData[] RiverRowDatas;
    public FinishSpotData[] FinishSpotDatas;
    public PlayerFrogActionEnum PlayerFrogActionEnum = PlayerFrogActionEnum.None;

    private float _currentSameMoveDelay = 1f;
    private float _elapsedJumpingTime = 0f;
    private Vector2 _startPosition;
    private Vector2 _targetPosition;

    //for quick debug
    public GameState()
    {
        FrogData = new FrogData(new MovableEntityData(new Vector2(0.5f, -7.5f), null, FacingDirection.Up), FrogState.Idle);
    }

    public GameStateSnapshot GetSnapshot()
    {
        GameStateSnapshot snapshot = new GameStateSnapshot();
        snapshot.FrogPosition = FrogData.CurrentPosition;
        snapshot.FrogState = FrogData.State;
        snapshot.FrogFacingDirection = FrogData.FacingDirection;
        snapshot.PlayerFrogActionEnum = PlayerFrogActionEnum;
        snapshot.CurrentSameMoveDelay = _currentSameMoveDelay;
        //snapshot.Obstacles;
        //snapshot.FinishSpotDatas;

        return snapshot;
    }

    public GameStateSnapshot UpdateToNextFrame(GameStateSnapshot lastFrameGameStateSnapshot, float dt, GameConfig gameConfig, PlayerInput.PlayerFrogActions currentFramePlayerFrogAction)
    {
        ApplyActionToFrog(lastFrameGameStateSnapshot, dt, gameConfig, currentFramePlayerFrogAction);
        //More
        //GameState.ApplyActionToFrog(dt, _gameConfig, _playerInput.PlayerFrog);
        //_gameState.UpdateObstacle(dt, _gameConfig);
        /*
        _gameState.CreateObstacle(dt, _gameConfig);
        _gameState.MoveObstacle(dt, _gameConfig);
        _gameState.RemoveObstacle(dt, _gameConfig);
        */
        //_gameState.UpdateFrogStatus(dt, _gameConfig);
        //_gameState.UpdateGameStatus(dt, _gameConfig);
        return GetSnapshot();
    }

    private void ApplyActionToFrog(GameStateSnapshot lastFrameSnapshot, float dt, GameConfig gameConfig, PlayerInput.PlayerFrogActions playerFrogAction)
    {
        //MoveFrog toward player direction with speed specified in gameConfig and move pixel by ratio according to dt
        Vector2 inputVector2 = playerFrogAction.Move.ReadValue<Vector2>();
        //Debug.Log($"inputVector2: {inputVector2}");
        PlayerFrogActionEnum = PlayerInputUtil.ConvertVector2ToPlayerFrogActionEnum(inputVector2);
        Debug.Log($"CurrentPlayerFrogActionEnum: {PlayerFrogActionEnum}");
        Debug.Log($"lastFrameSnapshot.PlayerFrogActionEnum: {lastFrameSnapshot.PlayerFrogActionEnum}");
        //Debug.Log($"dt: {dt}");

        switch (FrogData.State)
        {
            case FrogState.Idle:
                if(PlayerFrogActionEnum != PlayerFrogActionEnum.None)
                {
                    if(PlayerFrogActionEnum == lastFrameSnapshot.PlayerFrogActionEnum && FrogData.FacingDirection == PlayerInputUtil.ActionEnumToFacingDirection(PlayerFrogActionEnum))
                    {
                        if(_currentSameMoveDelay > 0)
                        {
                            Debug.Log("SAME MOVE DELAY");
                            _currentSameMoveDelay -= dt;
                        }
                        else
                        {
                            FrogData.FacingDirection = PlayerInputUtil.ActionEnumToFacingDirection(PlayerFrogActionEnum);
                            _startPosition = FrogData.CurrentPosition;
                            _targetPosition = _startPosition + (PlayerInputUtil.GetNormalizedVector2FromFacingDirection(FrogData.FacingDirection) * gameConfig.FROG_JUMP_DISTANCE);
                            FrogData.State = FrogState.Jumping;
                        }
                    }
                    else
                    {
                        _currentSameMoveDelay = gameConfig.SAME_MOVE_DELAY;
                        FrogData.FacingDirection = PlayerInputUtil.ActionEnumToFacingDirection(PlayerFrogActionEnum);
                        _startPosition = FrogData.CurrentPosition;
                        _targetPosition = _startPosition + (PlayerInputUtil.GetNormalizedVector2FromFacingDirection(FrogData.FacingDirection) * gameConfig.FROG_JUMP_DISTANCE);
                        FrogData.State = FrogState.Jumping;
                    }
                }
                else
                {
                    _currentSameMoveDelay = gameConfig.SAME_MOVE_DELAY;
                }
                break;
            case FrogState.Jumping:
                _elapsedJumpingTime += dt;
                float lerpAmount = _elapsedJumpingTime / gameConfig.FROG_JUMP_TIME;
                if (lerpAmount < 1)
                {
                    FrogData.CurrentPosition = Vector2.Lerp(_startPosition, _targetPosition, _elapsedJumpingTime / gameConfig.FROG_JUMP_TIME);
                }
                else
                {
                    FrogData.CurrentPosition = _targetPosition;
                    _elapsedJumpingTime = 0;
                    FrogData.State = FrogState.Idle;
                }
                //Debug.Log($"CurrentPosition: {FrogData.CurrentPosition}");
                break;
            case FrogState.Die:
                break;
            case FrogState.InFinishLine:
                break;
            default: break;
        }
    }
}
