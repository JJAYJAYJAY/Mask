using UnityEngine;
[CreateAssetMenu(
    fileName = "MaskStoryData",
    menuName = "Game/Mask Story Data"
)]
public class MskStoryData: ScriptableObject
{
    public Mask mask;
    [TextArea(5, 20)]
    public string content;
}
