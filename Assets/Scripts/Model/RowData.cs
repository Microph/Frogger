using System.Collections.Generic;

public class RowData
{
    public RowMovingDirection RowMovingDirection;
    public RowMovingSpeed RowMovingSpeed;
    public List<MovableEntityData> MovableEntityDataList;

    public RowData(RowDataConfig rowDataConfig)
    {
        RowMovingDirection = rowDataConfig.RowMovingDirection;
        RowMovingSpeed = rowDataConfig.RowMovingSpeed;
        MovableEntityDataList = new List<MovableEntityData>();
        //foreach(float initialXPosition in rowDataConfig.InitialObstacleXPositions)
        //{
            //TODO
        //}
    }
}

public enum RowMovingDirection
{
    Right,
    Left
}

public enum RowMovingSpeed
{
    Slow = 1,
    Medium = 2,
    Fast = 3
}
