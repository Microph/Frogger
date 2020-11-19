using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig
{
    public float FROG_JUMP_DISTANCE = 1;
    public float FROG_JUMP_TIME = 0.25f;
    public float SAME_MOVE_PENALTY_TIME = 0.15f;
    public float MOVE_COOLDOWN = 0.05f;
    public RowDataConfig[] RowDataConfigs;

    public GameConfig()
    {
        RowDataConfigs = new RowDataConfig[1];
        RowDataConfigs[0] = new RowDataConfig();
    }
}

public class RowDataConfig
{
    public ObstacleType ObstacleType;
    public float[] InitialObstacleXPositions;
    public RowMovingDirection RowMovingDirection;
    public RowMovingSpeed RowMovingSpeed;
    public int MinGap, MaxGap;

    public RowDataConfig() //mock
    {
        ObstacleType = ObstacleType.CarRed;
        InitialObstacleXPositions = new float[3];
        //InitialObstacleXPositions[0] = 0.5f;
        //InitialObstacleXPositions[1] = 1.5f;
        //InitialObstacleXPositions[2] = 3.5f;
        RowMovingDirection = RowMovingDirection.Left;
        RowMovingSpeed = RowMovingSpeed.Fast;
        MinGap = 2;
        MaxGap = 4;
    }

    
}