using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public ItemData[] allItems; // 在 Inspector 拖入全部 ItemData

    private Dictionary<string, ItemData> itemDict;

    public void Init()
    {
        itemDict = new Dictionary<string, ItemData>();
        foreach (var item in allItems)
        {
            if(!itemDict.ContainsKey(item.itemID))
                itemDict[item.itemID] = item;
            else
                Debug.LogWarning($"重复的 itemID: {item.itemID}");
        }
    }

    public ItemData GetItem(string id)
    {
        if (itemDict.TryGetValue(id, out var item))
            return item;
        return null;
    }
}