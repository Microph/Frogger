using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            RowData rowData = RowDatas[i];
            if (rowData.RowMovingDirection == RowMovingDirection.Left)
            {
                //move obstacle that fully left view back to pool 
                //no need ^ for now cuz it will eventually be back when obj pool call

                //ex. left -> check if right most (last obsbtacle in list) have gap to the right that is at least minGap in _gameConfig -> spawn new obstacle from pool
                float lastObstacleX = rowData.MovableEntityDataList.Count == 0 ? 7.5f : rowData.MovableEntityDataList[rowData.MovableEntityDataList.Count - 1].CurrentPosition.x * rowData.MovableEntityDataList[rowData.MovableEntityDataList.Count - 1].Width;
                if (rowData.MovableEntityDataList.Count == 0 || 7.5f - lastObstacleX >= _gameConfig.RowDataConfigs[i].MinGap)
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
                foreach (MovableEntityData obstacleEntityData in rowData.MovableEntityDataList)
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
