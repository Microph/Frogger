using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInputUtil;

public class FrogGameObject : MonoBehaviour
{
    public FrogData FrogData; //Assigned by GameState
    public SpriteRenderer SpriteRenderer;
    public Animator FrogAnimator;

    public void FrameUpdate(PlayerFrogAction inputFrogAction, GameStateSnapshot lastFrameSnapshot, float dt, GameConfig gameConfig)
    {
        FrogData.UpdateFrogData(inputFrogAction, lastFrameSnapshot, dt, gameConfig);
        transform.position = FrogData.CurrentPosition;
        FrogAnimator.SetBool("Jumping", FrogData.State == FrogState.Jumping);
        if(FrogData.State == FrogState.Die)
        {
            if(FrogData.IsDrown())
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collide with tag: " + other.tag);
        FrogData.UpdateFrogDataTriggerEnter2D(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("exit: " + other.tag);
        FrogData.UpdateFrogDataTriggerEnter2D(other);
    }
}