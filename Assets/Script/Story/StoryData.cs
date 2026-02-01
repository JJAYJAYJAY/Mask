using UnityEngine;
[CreateAssetMenu(
    fileName = "StoryData",
    menuName = "Game/Story Data"
)]
public class StoryData: ScriptableObject
{
    public Mask mask;
    [TextArea(5, 20)]
    public string content;
}
