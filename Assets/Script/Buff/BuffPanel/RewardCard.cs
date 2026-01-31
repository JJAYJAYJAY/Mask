using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RewardCard : MonoBehaviour
{
    public TextMeshProUGUI title;
    public UnityEngine.UI.Image icon;
    public UnityEngine.UI.Button button;
    public TextMeshProUGUI description;

    public void SetData(
        string displayName,
        Sprite iconSprite,
        Action onClick,
        String desc,
        RewardOptionType type)
    {
        title.text = displayName;
        icon.sprite = iconSprite;
        if (!iconSprite)
        {
            icon.color = new Color(0, 0, 0, 0);
        }
        else
        {
            icon.color = new Color(255, 255, 255, 1);
        }
        description.text = desc;
        if (type == RewardOptionType.RandomBuff)
        {
            icon.rectTransform.sizeDelta = new Vector2(200, 200);
        }

        if (type == RewardOptionType.RandomMask)
        {
            icon.rectTransform.sizeDelta = new Vector2(340, 340);
        }
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick?.Invoke());
    }
}

