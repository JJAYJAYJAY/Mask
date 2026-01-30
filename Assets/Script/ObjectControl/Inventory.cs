using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<ItemData> items = new();

    private void Awake()
    {
        Instance = this;
    }

    void init()
    {
        foreach (var i in items)
        {
            InventoryUI.Instance.AddItem(i);
        }    
    }

    void Start()
    {
        init();
    }
    
    public void AddItem(ItemData item)
    {
        items.Add(item);
        InventoryUI.Instance.AddItem(item);
    }

    public void RemoveItem(ItemData item)
    {
        items.Remove(item);
        InventoryUI.Instance.RemoveItem(item);
    }

    public bool HasItem(ItemData item)
    {
        return items.Contains(item);
    }
}

