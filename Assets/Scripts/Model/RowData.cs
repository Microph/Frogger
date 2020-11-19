using System.Collections.Generic;

public class RowData
{
    public RowMovingDirection RowMovingDirection;
    public float RowMovingUnitPerSec;
    public List<MovableEntityData> MovableEntityDataList;

    public RowData(RowDataConfig rowDataConfig)
    {
        RowMovingDirection = rowDataConfig.RowMovingDirection;
        RowMovingUnitPerSec = rowDataConfig.RowMovingUnitPerSec;
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
