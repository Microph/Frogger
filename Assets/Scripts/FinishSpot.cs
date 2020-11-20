using System;
using UnityEngine;

public class FinishSpot : MonoBehaviour
{
    public bool IsFinished = false;
    public SpriteRenderer SpriteRenderer;
    public BoxCollider2D BoxCollider2D;

    public void ResetState()
    {
        SpriteRenderer.enabled = false;
        BoxCollider2D.enabled = true;
        IsFinished = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Frog"))
        {
            SpriteRenderer.enabled = true;
            BoxCollider2D.enabled = false;
            IsFinished = true;
        }
    }
}