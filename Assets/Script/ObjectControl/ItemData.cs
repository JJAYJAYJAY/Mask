using UnityEngine;

public enum ItemType
{
    Mask,
    Misc
}

[CreateAssetMenu(menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    public string itemID;
    public string itemName;
    public Sprite icon;
    public ItemType type;
    public float weight;
    public string note;
    [Header("UI Size (px)")]
    public float width;
    public float height;
    [TextArea]
    public string description;
}