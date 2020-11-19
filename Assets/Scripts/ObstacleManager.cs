using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    None,
    Log,
    Turtle,
    CarPink,
    Truck,
    CarRed,
    CarYellow
}

public class ObstacleManager : MonoBehaviour
{
    public List<RowData> RowDatas;

    public void Initialize(GameConfig gameConfig)
    { 
        RowDatas = new List<RowData>();
        foreach (RowDataConfig rowDataConfig in gameConfig.RowDataConfigs)
        {
            RowDatas.Add(new RowData(rowDataConfig));
        }
    }
    
    public void FrameUpdate(float dt, GameConfig _gameConfig)
    {
        for (int i = 0; i < RowDatas.Count; i++)
        {
            if(RowDatas[i] == null)
            {
                continue;
            }

            RowData rowData = RowDatas[i];
            if (rowData.RowMovingDirection == RowMovingDirection.Left)
            {
                //move obstacle that fully left view back to pool 
                //no need ^ for now cuz it will eventually be back when obj pool call

                //ex. left -> check if right most (last obstacle in list) have gap to the right that is at least minGap in _gameConfig -> spawn new obstacle from pool
                float lastObstacleX = GetLastObstacleTailPosX(rowData);
                bool gapBetweenLastObstacleAndBorderIsEnoughToSpawn = Mathf.Abs(7.5f - lastObstacleX) >= _gameConfig.RowDataConfigs[i].MinGap; //7.5 is right border
                if (rowData.MovableEntityDataList.Count == 0 || gapBetweenLastObstacleAndBorderIsEnoughToSpawn) 
                {
                    if (_gameConfig.RowDataConfigs[i].ObstacleType != ObstacleType.None)
                    {
                        float newObstacleSpawnPosX = lastObstacleX + UnityEngine.Random.Range(_gameConfig.RowDataConfigs[i].MinGap, _gameConfig.RowDataConfigs[i].MaxGap);
                        Vector2 spawnPos = new Vector2(newObstacleSpawnPosX, i - 6.5f);
                        GameObject a = ObjectPooler.Instance.SpawnFromPool(_gameConfig.RowDataConfigs[i].ObstacleType.ToString(), spawnPos);
                        ObstacleGameObject obstacleGameObject = a.GetComponent<ObstacleGameObject>();
                        obstacleGameObject.RowIndex = i;
                        obstacleGameObject.MovableEntityData = new MovableEntityData();
                        obstacleGameObject.MovableEntityData.CurrentPosition = spawnPos;
                        //SpriteRenderer = ?
                        obstacleGameObject.MovableEntityData.FacingDirection = FacingDirection.Left;
                        obstacleGameObject.MovableEntityData.Width = 3;
                        rowData.MovableEntityDataList.Add(obstacleGameObject.MovableEntityData);
                    }
                }

                foreach (MovableEntityData obstacleEntityData in rowData.MovableEntityDataList)
                {
                    //currentX - dt * MoveSpeed (MoveSpeed is X block per sec)
                    obstacleEntityData.CurrentPosition -= new Vector2(dt * _gameConfig.RowDataConfigs[i].RowMovingUnitPerSec, 0);
                }
            }
            else
            {
                //move obstacle that fully left view back to pool 
                //no need ^ for now cuz it will eventually be back when obj pool call

                //ex. right -> check if left most (last obstacle in list) have gap to the left that is at least minGap in _gameConfig -> spawn new obstacle from pool
                float lastObstacleX = GetLastObstacleTailPosX(rowData);
                bool gapBetweenLastObstacleAndBorderIsEnoughToSpawn = Mathf.Abs(-7.5f - lastObstacleX) >= _gameConfig.RowDataConfigs[i].MinGap; //-7.5 is left border
                if (rowData.MovableEntityDataList.Count == 0 || gapBetweenLastObstacleAndBorderIsEnoughToSpawn) 
                {
                    if (_gameConfig.RowDataConfigs[i].ObstacleType != ObstacleType.None)
                    {
                        float newObstacleSpawnPosX = lastObstacleX - UnityEngine.Random.Range(_gameConfig.RowDataConfigs[i].MinGap, _gameConfig.RowDataConfigs[i].MaxGap);
                        Vector2 spawnPos = new Vector2(newObstacleSpawnPosX, i - 6.5f);
                        GameObject a = ObjectPooler.Instance.SpawnFromPool(_gameConfig.RowDataConfigs[i].ObstacleType.ToString(), spawnPos);
                        ObstacleGameObject obstacleGameObject = a.GetComponent<ObstacleGameObject>();
                        obstacleGameObject.RowIndex = i;
                        obstacleGameObject.MovableEntityData = new MovableEntityData();
                        obstacleGameObject.MovableEntityData.CurrentPosition = spawnPos;
                        //SpriteRenderer = ?
                        obstacleGameObject.MovableEntityData.FacingDirection = FacingDirection.Right;
                        obstacleGameObject.MovableEntityData.Width = 3;
                        rowData.MovableEntityDataList.Add(obstacleGameObject.MovableEntityData);
                    }
                }

                foreach (MovableEntityData obstacleEntityData in rowData.MovableEntityDataList)
                {
                    //currentX - dt * MoveSpeed (MoveSpeed is X block per sec)
                    obstacleEntityData.CurrentPosition += new Vector2(dt * _gameConfig.RowDataConfigs[i].RowMovingUnitPerSec, 0);
                }
            }
        }
    }

    private float GetLastObstacleTailPosX(RowData rowData)
    {
        if(rowData.MovableEntityDataList.Count == 0)
        {
            return rowData.RowMovingDirection == RowMovingDirection.Left ? -7.5f : 7.5f;
        }

        MovableEntityData lastObstacle = rowData.MovableEntityDataList[rowData.MovableEntityDataList.Count - 1];
        float x = rowData.RowMovingDirection == RowMovingDirection.Left ? lastObstacle.CurrentPosition.x + lastObstacle.Width : lastObstacle.CurrentPosition.x - lastObstacle.Width;
        return Mathf.Clamp(x, -7.5f, 7.5f);
    }
}
