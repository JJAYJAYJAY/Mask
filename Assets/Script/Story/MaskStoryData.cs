using UnityEngine;

[CreateAssetMenu(
    fileName = "MaskStoryData",
    menuName = "Game/Mask Story Data"
)]
public class MaskStoryData: ScriptableObject
{
    public Mask mask;
    [TextArea(5, 20)]
    public string content;
}
