using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig
{
    public float FROG_JUMP_DISTANCE = 1;
    public float FROG_JUMP_TIME = 0.15f;
    public float SAME_MOVE_PENALTY_TIME = 0.15f;
    public float MOVE_COOLDOWN = 0.05f;
    public float TURTLE_START_OFFSET_MIN = 3f, TURTLE_START_OFFSET_MAX = 12f;
    public float TURTLE_REPEAT_INTERVAL_MIN = 1f, TURTLE_REPEAT_INTERVAL_MAX = 6f;

    public RowDataConfig[] RowDataConfigs;
    //MOCK
    public GameConfig()
    {
        RowDataConfigs = new RowDataConfig[11];
        RowDataConfigs[0] = new RowDataConfig(ObstacleType.CarRed);
        RowDataConfigs[1] = new RowDataConfig(ObstacleType.None);
        RowDataConfigs[2] = new RowDataConfig(ObstacleType.None);
        RowDataConfigs[3] = new RowDataConfig(ObstacleType.CarRed);
        RowDataConfigs[4] = new RowDataConfig(ObstacleType.None);
        RowDataConfigs[5] = new RowDataConfig(ObstacleType.None); //should be empty
        RowDataConfigs[6] = new RowDataConfig(ObstacleType.Turtle);
        RowDataConfigs[7] = new RowDataConfig(ObstacleType.Turtle);
        RowDataConfigs[8] = new RowDataConfig(ObstacleType.Turtle);
        RowDataConfigs[9] = new RowDataConfig(ObstacleType.Turtle);
        RowDataConfigs[10] = new RowDataConfig(ObstacleType.Turtle);

        //RowDataConfigs = new RowDataConfig[1];
        //RowDataConfigs[0] = new RowDataConfig();
    }
}

public class RowDataConfig
{
    public ObstacleType ObstacleType; //will be list for random
    public float[] InitialObstacleXPositions;
    public RowMovingDirection RowMovingDirection;
    public float RowMovingUnitPerSec;
    public int MinGap, MaxGap;

    public RowDataConfig(ObstacleType obstacleType) //mock
    {
        ObstacleType = obstacleType;
        InitialObstacleXPositions = new float[3];
        //InitialObstacleXPositions[0] = 0.5f;
        //InitialObstacleXPositions[1] = 1.5f;
        //InitialObstacleXPositions[2] = 3.5f;
        RowMovingDirection = RowMovingDirection.Right;
        RowMovingUnitPerSec = 2;
        MinGap = 2;
        MaxGap = 4;
    }

}