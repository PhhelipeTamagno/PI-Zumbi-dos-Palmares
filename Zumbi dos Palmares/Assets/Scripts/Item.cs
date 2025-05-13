using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Knife")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public bool isUsable;
}
