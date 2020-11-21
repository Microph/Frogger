using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    None,
    Log3,
    Log5,
    Log7,
    RandomLog,
    Turtle2,
    Turtle3,
    CarPink,
    CarRed,
    CarYellow,
    Tractor,
    Truck
}

public class ObstacleManager : MonoBehaviour
{
    public List<RowData> RowDatas;

    public void Initialize(GameConfig gameConfig, int currentRound)
    {
        RowDatas = new List<RowData>();
        foreach (RowDataConfig rowDataConfig in gameConfig.RowDataConfigs)
        {
            RowDatas.Add(new RowData(rowDataConfig, gameConfig, currentRound));
        }
    }

    public void ResetToNewLevel(GameConfig gameConfig, int currentRound)
    {
        foreach(RowData rowData in RowDatas)
        {
            foreach(Obstacle obstacle in rowData.ObstacleGameObjectList)
            {
                obstacle.gameObject.SetActive(false);
            }
        }

        Initialize(gameConfig, currentRound);
    }

    public void TickUpdate(float dt, GameConfig _gameConfig, ObjectPooler objectPooler)
    {
        for (int i = 0; i < RowDatas.Count; i++)
        {
            if (RowDatas[i] == null)
            {
                continue;
            }

            RowData rowData = RowDatas[i];
            float lastObstacleX = GetLastObstacleTailPosX(rowData);
            bool isGapBetweenLastObstacleAndBorderEnoughToSpawn = Mathf.Abs((int)rowData.RowMovingDirection * 7.5f - lastObstacleX) >= _gameConfig.RowDataConfigs[i].MinGap; // 7.5/-7.5 is border
            float newObstacleSpawnPosX = lastObstacleX + ((int)rowData.RowMovingDirection * UnityEngine.Random.Range(_gameConfig.RowDataConfigs[i].MinGap, _gameConfig.RowDataConfigs[i].MaxGap));
            if (rowData.ObstacleGameObjectList.Count == 0 || isGapBetweenLastObstacleAndBorderEnoughToSpawn)
            {
                if(_gameConfig.RowDataConfigs[i].ObstacleType == ObstacleType.RandomLog)
                {
                    float sumLogWholeChance = _gameConfig.LOG_3_PERCENT_CHANCE + _gameConfig.LOG_5_PERCENT_CHANCE + _gameConfig.LOG_7_PERCENT_CHANCE;
                    float randomOutcome = UnityEngine.Random.Range(0, sumLogWholeChance);
                    if(randomOutcome < _gameConfig.LOG_3_PERCENT_CHANCE)
                    {
                        SpawnObstacle(_gameConfig, ObstacleType.Log3, i, rowData, newObstacleSpawnPosX, objectPooler);
                    }
                    else if(randomOutcome < _gameConfig.LOG_5_PERCENT_CHANCE)
                    {
                        SpawnObstacle(_gameConfig, ObstacleType.Log5, i, rowData, newObstacleSpawnPosX, objectPooler);
                    }
                    else
                    {
                        SpawnObstacle(_gameConfig, ObstacleType.Log7, i, rowData, newObstacleSpawnPosX, objectPooler);
                    }
                }
                else if (_gameConfig.RowDataConfigs[i].ObstacleType != ObstacleType.None)
                {
                    SpawnObstacle(_gameConfig, _gameConfig.RowDataConfigs[i].ObstacleType, i, rowData, newObstacleSpawnPosX, objectPooler);
                }
            }

            for (int j = rowData.ObstacleGameObjectList.Count - 1; j >= 0; j--)
            {
                Obstacle obs = rowData.ObstacleGameObjectList[j];
                obs.UpdateTick(dt, _gameConfig, rowData);
                if(ObstacleLeftScreen(rowData, obs))
                {
                    obs.OnLeftScreen();
                    rowData.ObstacleGameObjectList.RemoveAt(j);
                }
            }

        }
    }

    private bool ObstacleLeftScreen(RowData rowData, Obstacle obs)
    {
        float tailX = GetTailPosX(rowData, obs);
        if(rowData.RowMovingDirection == RowMovingDirection.Left)
        {
            return tailX <= -7.5f;
        }
        else
        {
            return tailX >= 7.5f;
        }
    }

    private static void SpawnObstacle(GameConfig _gameConfig, ObstacleType obstacleType, int i, RowData rowData, float newObstacleSpawnPosX, ObjectPooler objPooler)
    {
        Vector2 spawnPosVector = new Vector2(newObstacleSpawnPosX, i - 6.5f);
        GameObject pooledObj = objPooler.SpawnFromPool(obstacleType.ToString(), spawnPosVector);
        Obstacle obstacleGameObject = pooledObj.GetComponent<Obstacle>();
        obstacleGameObject.Initialize(i, spawnPosVector, rowData.RowMovingDirection, _gameConfig);
        rowData.ObstacleGameObjectList.Add(obstacleGameObject);
    }

    private float GetLastObstacleTailPosX(RowData rowData)
    {
        if (rowData.ObstacleGameObjectList.Count == 0)
        {
            return -(int)rowData.RowMovingDirection * 7.5f;
        }

        Obstacle lastObstacle = rowData.ObstacleGameObjectList[rowData.ObstacleGameObjectList.Count - 1];
        return Mathf.Clamp(GetTailPosX(rowData, lastObstacle), -7.5f, 7.5f);
    }

    private float GetTailPosX(RowData rowData, Obstacle obstacle)
    {
        MovableEntityData entityData = obstacle.MovableEntityData;
        float x = entityData.CurrentPosition.x + ((int)rowData.RowMovingDirection * obstacle.ObstacleWidth);
        return x;
    }
}
