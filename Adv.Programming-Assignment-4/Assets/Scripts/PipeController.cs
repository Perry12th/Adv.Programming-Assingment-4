using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PipeType { Entry, Exit, T2R, T2L, B2R, B2L, T2B, L2R, Bomb};
public class PipeController : MonoBehaviour
{
    [SerializeField]
    private Sprite unFilledPipe;
    [SerializeField]
    private Sprite filledPipe;
    [SerializeField]
    private List<Vector3> exitDirection;
    [SerializeField]
    public List<BoxCollider2D> outways;
    private BoxCollider2D entryway;
    public SpriteRenderer spriteRenderer;
    public bool isFilled;
    public PipeType type;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SearchForNextPipe()
    {
        for (int i = 0; i < outways.Count; i++)
        {
            if (outways[i] != entryway)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(outways[i].offset.x, outways[i].offset.y, transform.position.z), exitDirection[i], PuzzlePipeManager.instance.checkDistance, PuzzlePipeManager.instance.pipeLayer);
                Debug.Log("Direction: " + exitDirection[i]);
                //Debug.DrawLine(transform.position + new Vector3(outways[i].offset.x, outways[i].offset.y, transform.position.z), transform.position + new Vector3(outways[i].offset.x, outways[i].offset.y, transform.position.z) + exitDirection[i], Color.blue, 8.0f);
                if (hit.collider != null)
                {
                    Debug.Log("HitPipe");
                    PipeController otherPipe = hit.collider.GetComponent<PipeController>();
                    spriteRenderer.sprite = filledPipe;
                    if (otherPipe.type == PipeType.Exit)
                    {
                        UIManager.instance.GameOver();
                        UIManager.instance.GameOverTxt.text = "Nice you got the water to the end pipe, now what did this machine do again?";
                    }
                    else if (otherPipe.type == PipeType.Bomb)
                    {
                        UIManager.instance.GameOver();
                        UIManager.instance.GameOverTxt.text = "The Office of the Reaper wishes to inform you that death by bombs is a unique way of dying, try again";
                    }
                    else
                    {
                        PuzzlePipeManager.instance.activePipe = otherPipe;
                        otherPipe.spriteRenderer.color = Color.blue;
                        otherPipe.entryway = (BoxCollider2D)hit.collider;
                        UIManager.instance.ResetTimer();
                        SFXManager.instance.PlaySFX(Clip.Clear);
                    }
                }
                else
                {
                    Debug.Log("DidntHitPipe");
                    UIManager.instance.GameOver();
                    UIManager.instance.GameOverTxt.text = "Welp there's water spilling all over the floor now, who's cleaning this up (not me that's for sure)";
                }
            }
        }
    }
}
