using System;
using UnityEngine;

public class HightlightControl: MonoBehaviour
{

    void Start()
    {
        GameManager.Instance.globalRuleData.OnHighlightKeyItemsChanged += OnHighlightKeyItemsChanged;
        gameObject.SetActive(false);
    }

    void OnHighlightKeyItemsChanged(bool flag)
    {
        gameObject.SetActive(flag);
    }
}


