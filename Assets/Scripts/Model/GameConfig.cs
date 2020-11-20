using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public Vector2 FROG_START_POINT = new Vector2(0.5f, -7.5f);
    public float FROG_JUMP_DISTANCE = 1;
    public float FROG_JUMP_TIME = 0.15f;
    public float PER_ROUND_SPEED = 0.15f;
    public float SAME_MOVE_PENALTY_TIME = 0.15f;
    public float MOVE_COOLDOWN = 0.05f;
    public float TURTLE_START_OFFSET_MIN = 3f, TURTLE_START_OFFSET_MAX = 12f;
    public float TURTLE_REPEAT_INTERVAL_MIN = 1f, TURTLE_REPEAT_INTERVAL_MAX = 6f;
    public float SPEED_INCREASE_PERCENT = 10f;
    public float LOG_3_PERCENT_CHANCE = 33f;
    public float LOG_5_PERCENT_CHANCE = 33f;
    public float LOG_7_PERCENT_CHANCE = 33f;
    public RowDataConfig[] RowDataConfigs;
}

[CreateAssetMenu(fileName = "RowDataConfig", menuName = "ScriptableObjects/RowDataConfig", order = 2)]
public class RowDataConfig : ScriptableObject
{
    [SerializeField]
    public ObstacleType ObstacleType = ObstacleType.None; //will be list for random
    //public float[] InitialObstacleXPositions;
    [SerializeField]
    public RowMovingDirection RowMovingDirection = RowMovingDirection.Left;
    [SerializeField]
    public int MinGap = 2 , MaxGap = 4;

    [SerializeField]
    private RowMovingSpeed _rowMovingSpeed;
    
    public int GetRowMovingUnitPerSec()
    {
        return (int)_rowMovingSpeed;
    }

    public RowDataConfig(ObstacleType obstacleType, RowMovingDirection rowMovingDirection, RowMovingSpeed rowMovingSpeed, int minGap, int maxGap) //mock
    {
        ObstacleType = obstacleType;
        RowMovingDirection = rowMovingDirection;
        _rowMovingSpeed = rowMovingSpeed;
        MinGap = minGap;
        MaxGap = maxGap;
        //InitialObstacleXPositions = new float[3];
        //InitialObstacleXPositions[0] = 0.5f;
        //InitialObstacleXPositions[1] = 1.5f;
        //InitialObstacleXPositions[2] = 3.5f;
    }

}