using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem : MonoBehaviour
{
    public Sprite sprite;
    public string spritePath;
    public string itemInfo;
    public string[] clues = new string[] { "Sword is rather rusty", "Sword doesn't belong here i feel" };
    public bool addedtoInv = false;
    public bool ssAdded = false;

    public InventoryItem(Sprite sprite, string itemInfo)
    {
        this.sprite = sprite;
        this.itemInfo = itemInfo;
    }
}
