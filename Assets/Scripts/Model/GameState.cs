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
    //Frog stuff
    public FrogGameObject FrogGameObject;

    //Other
    public List<RowData> RowDatas;
    public FinishSpotData[] FinishSpotDatas;

    private PlayerFrogAction _inputFrogAction = PlayerFrogAction.None;
    
    //for quick debug
    public void Initialize(GameConfig gameConfig)
    {
        FrogGameObject.FrogData = new FrogData(new MovableEntityData(new Vector2(0.5f, -7.5f), FacingDirection.Up));
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
        snapshot.InputFrogAction = _inputFrogAction;
        return snapshot;
    }

    public GameStateSnapshot UpdateToNextFrame(GameStateSnapshot lastFrameGameStateSnapshot, float dt, GameConfig gameConfig, PlayerInput.PlayerFrogActions currentFramePlayerFrogAction)
    {
        //MoveFrog toward player direction with speed specified in gameConfig and move pixel by ratio according to dt
        Vector2 inputVector2 = currentFramePlayerFrogAction.Move.ReadValue<Vector2>();
        //Debug.Log($"inputVector2: {inputVector2}");
        _inputFrogAction = PlayerInputUtil.ConvertVector2ToPlayerFrogActionEnum(inputVector2);
        FrogGameObject.ApplyActionToFrog(_inputFrogAction, lastFrameGameStateSnapshot, dt, gameConfig);
        UpdateObstacle(dt, gameConfig);
        //_gameState.UpdateFrogStatus(dt, _gameConfig);
        //_gameState.UpdateGameStatus(dt, _gameConfig);
        return GetSnapshot();
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
