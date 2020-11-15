using System.Collections.Generic;

public class RowData
{
    public RowMovingDirection RowMovingDirection;
    public float RowMovingSpeed;
    public List<MovableEntityData> MovableEntityDataList;
}

public enum RowMovingDirection
{
    Right,
    Left
}