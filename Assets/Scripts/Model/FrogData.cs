using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogData : MovableEntityData
{
    public CurrentStatus CurrentStatus;
}

public enum CurrentStatus
{
    Idle,
    Jumping,
    Die,
    InFinishLine
}