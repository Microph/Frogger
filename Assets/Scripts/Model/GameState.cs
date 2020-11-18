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
    public PlayerFrogAction InputFrogAction;
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
    public List<RowData> RowDatas;
    public FinishSpotData[] FinishSpotDatas;
    public PlayerFrogAction InputFrogAction = PlayerFrogAction.None;

    private float _currentSameMoveDelay = 1f;
    private float _elapsedJumpingTime = 0f;
    private Vector2 _startPosition;
    private Vector2 _targetPosition;

    //for quick debug
    public GameState(GameConfig gameConfig)
    {
        FrogData = new FrogData(new MovableEntityData(new Vector2(0.5f, -7.5f), null, FacingDirection.Up), FrogState.Idle);
        RowDatas = GetRowDatasFromConfig(gameConfig.RowDataConfigs);
    }

    private List<RowData> GetRowDatasFromConfig(RowDataConfig[] rowDataConfigs)
    {
        List<RowData> rowDataList = new List<RowData>();
        foreach(RowDataConfig rowDataConfig in rowDataConfigs)
        {
            rowDataList.Add(new RowData(rowDataConfig));
        }

        return rowDataList;
    }

    public GameStateSnapshot GetSnapshot()
    {
        GameStateSnapshot snapshot = new GameStateSnapshot();
        snapshot.FrogPosition = FrogData.CurrentPosition;
        snapshot.FrogState = FrogData.State;
        snapshot.FrogFacingDirection = FrogData.FacingDirection;
        snapshot.InputFrogAction = InputFrogAction;
        snapshot.CurrentSameMoveDelay = _currentSameMoveDelay;
        //snapshot.Obstacles;
        //snapshot.FinishSpotDatas;

        return snapshot;
    }

    public GameStateSnapshot UpdateToNextFrame(GameStateSnapshot lastFrameGameStateSnapshot, float dt, GameConfig gameConfig, PlayerInput.PlayerFrogActions currentFramePlayerFrogAction)
    {
        ApplyActionToFrog(lastFrameGameStateSnapshot, dt, gameConfig, currentFramePlayerFrogAction);
        UpdateObstacle(dt, gameConfig);
        //_gameState.UpdateFrogStatus(dt, _gameConfig);
        //_gameState.UpdateGameStatus(dt, _gameConfig);
        return GetSnapshot();
    }

    private void ApplyActionToFrog(GameStateSnapshot lastFrameSnapshot, float dt, GameConfig gameConfig, PlayerInput.PlayerFrogActions playerFrogAction)
    {
        //MoveFrog toward player direction with speed specified in gameConfig and move pixel by ratio according to dt
        Vector2 inputVector2 = playerFrogAction.Move.ReadValue<Vector2>();
        //Debug.Log($"inputVector2: {inputVector2}");
        InputFrogAction = PlayerInputUtil.ConvertVector2ToPlayerFrogActionEnum(inputVector2);
        //Debug.Log($"CurrentPlayerFrogActionEnum: {InputFrogAction}");
        FacingDirection InputFrogActionDirection = PlayerInputUtil.ActionEnumToFacingDirection(InputFrogAction);

        switch (FrogData.State)
        {
            case FrogState.Idle:
                if (InputFrogAction != PlayerFrogAction.None)
                {
                    if(InputFrogAction == lastFrameSnapshot.InputFrogAction && FrogData.FacingDirection == InputFrogActionDirection)
                    {
                        if(_currentSameMoveDelay > 0)
                        {
                            //Debug.Log("SAME MOVE DELAY");
                            _currentSameMoveDelay -= dt;
                        }
                        else
                        {
                            FrogData.FacingDirection = InputFrogActionDirection;
                            _startPosition = FrogData.CurrentPosition;
                            _targetPosition = _startPosition + (PlayerInputUtil.GetNormalizedVector2FromFacingDirection(FrogData.FacingDirection) * gameConfig.FROG_JUMP_DISTANCE);
                            FrogData.State = FrogState.Jumping;
                        }
                    }
                    else
                    {
                        _currentSameMoveDelay = gameConfig.SAME_MOVE_DELAY;
                        FrogData.FacingDirection = InputFrogActionDirection;
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
                    //TODO: Check hitting corner first
                    FrogData.CurrentPosition = Vector2.Lerp(_startPosition, _targetPosition, _elapsedJumpingTime / gameConfig.FROG_JUMP_TIME);
                    //TODO: check falling into water / hit obstacle
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

    private void UpdateObstacle(float dt, GameConfig _gameConfig)
    {
       for(int i = 0; i < RowDatas.Count; i++)
        {
            RowData rowData = RowDatas[i];
            if (rowData.RowMovingDirection == RowMovingDirection.Left)
            {
                //move obstacle that fully left view back to pool 
                //no need ^ for now cuz it will eventually be back when obj pool call

                //ex. left -> check if right most (last obsbtacle in list) have gap to the right that is at least minGap in _gameConfig -> spawn new obstacle from pool
                float lastObstacleX = rowData.MovableEntityDataList.Count == 0 ? 7.5f : rowData.MovableEntityDataList[rowData.MovableEntityDataList.Count - 1].CurrentPosition.x * rowData.MovableEntityDataList[rowData.MovableEntityDataList.Count - 1].Width;
                if(rowData.MovableEntityDataList.Count == 0 || 7.5f - lastObstacleX >= _gameConfig.RowDataConfigs[i].MinGap)
                {
                    Vector2 spawnPos = new Vector2(7.5f + UnityEngine.Random.Range(_gameConfig.RowDataConfigs[i].MinGap, _gameConfig.RowDataConfigs[i].MaxGap), i - 6.5f);
                    GameObject a = ObjectPooler.Instance.SpawnFromPool(_gameConfig.RowDataConfigs[i].ObstacleType.ToString(), spawnPos);
                    ObstacleGameObject obstacleGameObject = a.GetComponent<ObstacleGameObject>();
                    obstacleGameObject.MovableEntityData = new MovableEntityData();
                    obstacleGameObject.MovableEntityData.CurrentPosition = spawnPos;
                    //SpriteRenderer = ?
                    obstacleGameObject.MovableEntityData.FacingDirection = FacingDirection.Left;
                    obstacleGameObject.MovableEntityData.Width = 1;
                    rowData.MovableEntityDataList.Add(obstacleGameObject.MovableEntityData);
                }
            }
            else
            {
                //TODO
            }
        }
        //move everything
        for (int i = 0; i < RowDatas.Count; i++)
        {
            RowData rowData = RowDatas[i];
            if (rowData.RowMovingDirection == RowMovingDirection.Left)
            {
                foreach(MovableEntityData obstacleEntityData in rowData.MovableEntityDataList)
                {
                    //currentX - dt * MoveSpeed (MoveSpeed is X block per sec)
                    obstacleEntityData.CurrentPosition -= new Vector2(dt * _gameConfig.RowDataConfigs[i].RowMovingSpeed.GetHashCode(), 0);
                }
            }
            else
            {
                foreach (MovableEntityData obstacleEntityData in rowData.MovableEntityDataList)
                {
                    //currentX - dt * MoveSpeed (MoveSpeed is X block per sec)
                    obstacleEntityData.CurrentPosition += new Vector2(dt * _gameConfig.RowDataConfigs[i].RowMovingSpeed.GetHashCode(), 0);
                }
            }

        }
    }
}
