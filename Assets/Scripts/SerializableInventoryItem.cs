using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class SerializableInventoryItem: MonoBehaviour
{
    public string itemInfo;
    public byte[] spriteBytes;

    public SerializableInventoryItem(string itemName, Sprite sprite)
    {
        this.itemInfo = itemName;
        this.spriteBytes = SpriteToBytes(sprite);
    }
    /*
    public InventoryItem ToInventoryItem()
    {
        Sprite sprite = BytesToSprite(spriteBytes);
        return new InventoryItem(sprite, itemInfo, spritePath);
    }*/

    private byte[] SpriteToBytes(Sprite sprite)
    {
        Texture2D tex = sprite.texture;
        Rect textureRect = sprite.textureRect;
        byte[] texData = tex.GetRawTextureData();
        SerializableSprite serializableSprite = new SerializableSprite(tex, textureRect, sprite.pixelsPerUnit);
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, serializableSprite);
        byte[] spriteBytes = ms.ToArray();
        ms.Close();
        return spriteBytes;
    }

    private Sprite BytesToSprite(byte[] bytes)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(bytes);
        SerializableSprite serializableSprite = bf.Deserialize(ms) as SerializableSprite;
        ms.Close();
        Texture2D tex = new Texture2D((int)serializableSprite.textureRect.width, (int)serializableSprite.textureRect.height);
        tex.LoadRawTextureData(serializableSprite.textureData);
        tex.Apply();
        Rect textureRect = serializableSprite.textureRect;
        float pixelsPerUnit = serializableSprite.pixelsPerUnit;
        Sprite sprite = Sprite.Create(tex, textureRect, new Vector2(0.5f, 0.5f), pixelsPerUnit);
        return sprite;
    }
}
