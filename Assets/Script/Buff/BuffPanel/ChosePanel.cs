using System;
using UnityEngine;

public class ChosePanel:MonoBehaviour
{
    [Header("UI")]
    public RewardCard[] cards;   // 长度 = 3

    RewardSelector selector;
    
    public DetailPanelController detailPanelController;

    private void Awake()
    {
        Debug.Log("ChosePanel Awake");
    }

    private void Start()
    {
        detailPanelController.BeforeOpened += OnOpen;
        Debug.Log("ChosePanel Get Selector");
    }
    
    void OnOpen()
    {
        Debug.Log("OnOpen");
        if(selector == null) selector = GameManager.Instance.rewardSelector;
        var options = selector.Generate();
        // foreach (var option in options)
        // {
        //     Debug.Log(option.item.itemName);
        // }
        for (int i = 0; i < cards.Length; i++)
        {
            var option = options[i];
            cards[i].gameObject.SetActive(true);

            cards[i].SetData(
                GetName(option),
                GetIcon(option),
                () =>
                {
                    option.OnSelect?.Invoke();
                    detailPanelController.CloseToWorldPos(Vector3.zero);
                },
                GetDescription(option),
                option.type
                
            );
        }
    }
    

    Sprite GetIcon(RewardOption option)
    {
        // 简单区分，后面你可以用 meta
        return option.type switch
        {
            RewardOptionType.RandomMask => option.item.icon,
            RewardOptionType.RandomBuff => option.buff.icon,
            RewardOptionType.DoNothing  => null,
            _ => null
        };
    }

    string GetName(RewardOption option)
    {
        return option.type switch
        {
            RewardOptionType.RandomMask => option.item.itemName,
            RewardOptionType.RandomBuff => option.buff.buffName,
            RewardOptionType.DoNothing  => "无",
            _ => "未知"
        };
    }
    string GetDescription(RewardOption option)
    {
        return option.type switch
        {
            RewardOptionType.RandomMask => option.item.description,
            RewardOptionType.RandomBuff => option.buff.description,
            RewardOptionType.DoNothing  => "什么都不做",
            _ => "未知"
        };
    }
}
