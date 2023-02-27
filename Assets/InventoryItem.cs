using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem : MonoBehaviour
{
    public Sprite sprite;
    public string spritePath;
    public string itemInfo;

    public InventoryItem(Sprite sprite, string itemInfo)
    {
        this.sprite = sprite;
        this.itemInfo = itemInfo;
    }
}
