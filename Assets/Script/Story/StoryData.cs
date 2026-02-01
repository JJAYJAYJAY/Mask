using UnityEngine;
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