using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Create New Dialogue")]
public class Dialogue : ScriptableObject
{
    public string characterName;
    [TextArea(2, 5)]
    public string[] sentences;
}
