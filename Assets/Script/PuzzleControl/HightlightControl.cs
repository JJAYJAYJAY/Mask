using System;
using UnityEngine;

public class HightlightControl: MonoBehaviour
{
    public GameObject Highlight;

    private void Awake()
    {
        Highlight = GameObject.Find("Highlight");
        Highlight.SetActive(false);
    }

    void Start()
    {
        GameManager.Instance.Data.OnHighlightKeyItemsChanged += OnHighlightKeyItemsChanged;
    }

    void OnHighlightKeyItemsChanged(bool flag)
    {
        Highlight.SetActive(flag);
    }
}


