using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurtleState
{
    Swimming,
    StartDiving,
    Diving
}

public class TurtleGameObject : ObstacleGameObject
{
    public float StartOffsetTime = 1f;
    public float RepeatIntervalTime = 1f;
    public Animator TurtleAnimator;

    private float _currentOffsetCounter;
    private float _currentIntervalCounter;

    private void OnEnable()
    {
        _currentOffsetCounter = StartOffsetTime;
        _currentIntervalCounter = RepeatIntervalTime;
    }

    private void FixedUpdate()
    {
        _currentOffsetCounter -= Time.fixedDeltaTime;
        if(_currentOffsetCounter > 0 )
        {
            return;
        }

        _currentIntervalCounter -= Time.fixedDeltaTime;
        if(_currentIntervalCounter <= 0)
        {
            _currentIntervalCounter = RepeatIntervalTime;

        }
    }
}
