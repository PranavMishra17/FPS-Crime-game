using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

[Serializable]
public class InventorySaveData : ScriptableObject
{
    [SerializeField] private List<InventoryItemData> itemDataList = new List<InventoryItemData>();

    public List<SerializableInventoryItemsz> serializedItems;
    public List<LevelInventoryData> levelInventoryDataList = new List<LevelInventoryData>();

    [Serializable]
    public class InventoryItemData
    {
        public byte[] spriteData;
        public string itemInfo;

        public InventoryItemData(byte[] data, string info)
        {
            spriteData = data;
            itemInfo = info;
        }
    }

    [System.Serializable]
    public class LevelInventoryData
    {
        public string levelName;
        public List<SerializableInventoryItemsz> serializableItems;

        public LevelInventoryData(string levelName, List<SerializableInventoryItemsz> serializableItems)
        {
            this.levelName = levelName;
            this.serializableItems = serializableItems;
        }
    }

    /*[System.Serializable]
    public class SerializableInventoryItem
    {
        public byte[] spriteData;
        public string info;

        public SerializableInventoryItem(byte[] spriteData, string info)
        {
            this.spriteData = spriteData;
            this.info = info;
        }
    }*/

    [Serializable]
    public class SerializableInventoryItemsz
    {
        public SerializableSprite serializableSprite;
        public string itemInfo;

        public SerializableInventoryItemsz(SerializableSprite serializableSprite, string itemInfo)
        {
            this.serializableSprite = serializableSprite;
            this.itemInfo = itemInfo;
        }
    }

    
    public InventorySaveData(List<InventoryItem> items)
    {
        foreach (InventoryItem item in items)
        {
            Texture2D texture = item.sprite.texture;
            byte[] spriteData = texture.EncodeToPNG();
            InventoryItemData itemData = new InventoryItemData(spriteData, item.itemInfo);
            itemDataList.Add(itemData);
        }
    }

    public InventorySaveData()
    {
    }

    public List<InventoryItem> Load()
    {
        List<InventoryItem> items = new List<InventoryItem>();
        foreach (InventoryItemData itemData in itemDataList)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(itemData.spriteData);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            InventoryItem item = new InventoryItem(sprite, itemData.itemInfo);
            items.Add(item);
        }
        return items;
    }
}