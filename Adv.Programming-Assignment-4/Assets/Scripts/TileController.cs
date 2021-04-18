using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    private static Color selectedColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    private static TileController lastSelectedTile = null;

    private SpriteRenderer render;
    public PipeController pipe;
    private bool isTitleSelected = false;
    void Awake()
    {
        render = GetComponent<SpriteRenderer>();

    }
    private void SelectTile()
    {
        isTitleSelected = true;
        render.color = selectedColor;
        lastSelectedTile = this;
        SFXManager.instance.PlaySFX(Clip.Select);
    }

    private void DeselectTile()
    {
        isTitleSelected = false;
        render.color = Color.white;
        lastSelectedTile = null;
    }

    private void OnMouseDown()
    {
        if (pipe == null || pipe.isFilled || UIManager.instance.gameOver || pipe == PuzzlePipeManager.instance.activePipe)
        {
            return;
        }
        if (isTitleSelected)
        {
            DeselectTile();
        }

        else
        {
            if (lastSelectedTile == null)
            {
                SelectTile();
            }
            else
            {
                // Switch Pipes
                Debug.Log("Switch pipe");
                SwitchPipes();
                lastSelectedTile.DeselectTile();
            }
        }
    }

    private void SwitchPipes()
    {
        if (pipe.type == lastSelectedTile.pipe.type)
        {
            return;
        }

        // Switch the pipes transform
        pipe.transform.parent = lastSelectedTile.transform;
        lastSelectedTile.pipe.transform.parent = transform;

        pipe = GetComponentInChildren<PipeController>();
        pipe.transform.localPosition = new Vector3(0, 0, 0);
        lastSelectedTile.pipe = lastSelectedTile.GetComponentInChildren<PipeController>();
        lastSelectedTile.pipe.transform.localPosition = new Vector3(0, 0, 0);
        SFXManager.instance.PlaySFX(Clip.Swap);
    }
}
