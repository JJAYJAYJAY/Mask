using UnityEngine;

[CreateAssetMenu(
    fileName = "BuffMeta",
    menuName = "Game/Buff Metadata"
)]
public class BuffMetadata : ScriptableObject
{
    public string buffName;
    [TextArea]
    public string description;
    public Sprite icon;

    public Mask belong;
    
}