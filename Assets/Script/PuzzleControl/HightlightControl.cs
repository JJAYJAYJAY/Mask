using System;
using UnityEngine;

public class HightlightControl: MonoBehaviour
{
    public GameObject Highlight;

    private void Awake()
    {
        Highlight = GameObject.Find("Highlight");
    }

    void Start()
    {
        GameManager.Instance.Data.OnHighlightKeyItemsChanged += OnHighlightKeyItemsChanged;
    }

    void OnHighlightKeyItemsChanged(bool flag)
    {
        gameObject.SetActive(flag);
    }
}


