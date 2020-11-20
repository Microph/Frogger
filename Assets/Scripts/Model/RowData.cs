using System.Collections.Generic;

public enum RowMovingDirection
{
    Left = 1,
    Right = -1
}

public enum RowMovingSpeed
{
    Slow = 1,
    Medium = 2,
    High = 4
}

public class RowData
{
    public RowMovingDirection RowMovingDirection;
    public float RowMovingUnitPerSec;
    public List<ObstacleGameObject> ObstacleGameObjectList;

    public RowData(RowDataConfig rowDataConfig)
    {
        RowMovingDirection = rowDataConfig.RowMovingDirection;
        RowMovingUnitPerSec = rowDataConfig.GetRowMovingUnitPerSec();
        ObstacleGameObjectList = new List<ObstacleGameObject>();
        //foreach(float initialXPosition in rowDataConfig.InitialObstacleXPositions)
        //{
            //TODO
        //}
    }
}