﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInputUtil;

public class FrogGameObject : MonoBehaviour
{
    public FrogData FrogData; //Assigned by GameState
    public Transform SpriteTransform;
    public Animator FrogAnimator;

    private Transform _frogTransform;

    private void Awake()
    {
        _frogTransform = GetComponent<Transform>();
    }

    public void FrameUpdate(PlayerFrogAction inputFrogAction, GameStateSnapshot lastFrameSnapshot, float dt, GameConfig gameConfig)
    {
        FrogData.UpdateFrogData(inputFrogAction, lastFrameSnapshot, dt, gameConfig);
        _frogTransform.position = FrogData.CurrentPosition;
        FrogAnimator.SetBool("Jumping", FrogData.State == FrogState.Jumping);
        if (FrogData.State == FrogState.Jumping)
        {
            SpriteTransform.eulerAngles = GetSpriteRotationValue(FrogData.FacingDirection);
            FrogAnimator.SetBool("Jumping", true);
        }
        else if(FrogData.State == FrogState.Die)
        {
            SpriteTransform.eulerAngles = GetSpriteRotationValue(FacingDirection.Up);
            if (FrogData.IsDrown())
            {
                FrogAnimator.SetBool("DrownDie", true);
            }
            else
            {
                FrogAnimator.SetBool("CollisionDie", true);
            }
        }
        else
        {
            FrogAnimator.SetBool("CollisionDie", false);
            FrogAnimator.SetBool("DrownDie", false);
        }
    }

    private Vector3 GetSpriteRotationValue(FacingDirection facingDirection)
    {
        switch (facingDirection)
        {
            case FacingDirection.Up: return new Vector3(0, 0, 0);
            case FacingDirection.Down: return new Vector3(0, 0, 180);
            case FacingDirection.Left: return new Vector3(0, 0, 90);
            case FacingDirection.Right: return new Vector3(0, 0, -90);
            default: return new Vector3(0, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collide with tag: " + other.tag);
        FrogData.UpdateFrogDataTriggerEnter2D(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("stay with tag: " + other.tag);
        FrogData.UpdateFrogDataTriggerOnStay2D(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("exit tag: " + other.tag);
        FrogData.UpdateFrogDataTriggerOnExit2D(other);
    }
}