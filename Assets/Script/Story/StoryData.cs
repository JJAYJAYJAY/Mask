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

[CreateAssetMenu(
    fileName = "StoryData",
    menuName = "Game/Story Data"
)]
public class StroyData:ScriptableObject
{
    public int id;
    [TextArea(5, 20)]
    public string content;
}