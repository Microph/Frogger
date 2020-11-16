using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogData : MovableEntityData
{
    public CurrentStatus CurrentStatus;

    public FrogData(MovableEntityData movableEntityData, CurrentStatus currentStatus) : base(movableEntityData)
    {
        CurrentStatus = currentStatus;
    }
}

public enum CurrentStatus
{
    Idle,
    Jumping,
    Die,
    InFinishLine
}