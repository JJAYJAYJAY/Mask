using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectCell : MonoBehaviour
{
    public bool canPlace = true;               // 是否可以放置碎片
    [HideInInspector] public DragCell currentPiece; // 当前格子上的碎片
    public Image highlight;                     // 可放/不可放提示
    public int index;
    private void Awake()
    {
        if (highlight != null)
            highlight.enabled = false;
    }

    public void ShowHighlight(bool canPlaceHere)
    {
        if (highlight != null)
        {
            highlight.enabled = true;
            highlight.color = canPlaceHere ? new Color(0,1,0,0.3f) : new Color(1,0,0,0.3f);
        }
    }

    public void HideHighlight()
    {
        if (highlight != null)
            highlight.enabled = false;
    }
}