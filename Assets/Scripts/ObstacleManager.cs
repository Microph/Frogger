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
                if (_gameConfig.RowDataConfigs[i].ObstacleType != ObstacleType.None)
                {
                    Vector2 spawnPosVector = new Vector2(newObstacleSpawnPosX, i - 6.5f);
                    GameObject pooledObj = ObjectPooler.Instance.SpawnFromPool(_gameConfig.RowDataConfigs[i].ObstacleType.ToString(), spawnPosVector);
                    ObstacleGameObject obstacleGameObject = pooledObj.GetComponent<ObstacleGameObject>();
                    obstacleGameObject.Initialize(i, spawnPosVector, rowData.RowMovingDirection);
                    rowData.ObstacleGameObjectList.Add(obstacleGameObject);
                }
            }

            foreach (ObstacleGameObject obstacleGameObject in rowData.ObstacleGameObjectList)
            {
                obstacleGameObject.UpdateFrame(dt, _gameConfig);
            }
        }
    }

    private float GetLastObstacleTailPosX(RowData rowData)
    {
        if (rowData.ObstacleGameObjectList.Count == 0)
        {
            return -(int)rowData.RowMovingDirection * 7.5f;
        }

        ObstacleGameObject lastObstacle = rowData.ObstacleGameObjectList[rowData.ObstacleGameObjectList.Count - 1];
        MovableEntityData entityData = lastObstacle.MovableEntityData;
        float x = entityData.CurrentPosition.x + ((int)rowData.RowMovingDirection * lastObstacle.ObstacleWidth);
        return Mathf.Clamp(x, -7.5f, 7.5f);
    }
}
