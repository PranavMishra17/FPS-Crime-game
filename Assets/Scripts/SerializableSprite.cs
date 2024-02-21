using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableSprite : MonoBehaviour
{
    [SerializeField] public byte[] textureData;
    [SerializeField] public Rect textureRect;
    [SerializeField] public float pixelsPerUnit;

    public SerializableSprite(Texture2D texture, Rect textureRect, float pixelsPerUnit)
    {
        if (texture == null)
        {
            throw new ArgumentNullException(nameof(texture));
        }

        this.textureRect = textureRect;
        this.pixelsPerUnit = pixelsPerUnit;

        // Encode the texture as a PNG and store its data in a byte array
        textureData = texture.EncodeToPNG();
    }

    public Texture2D CreateTexture()
    {
        // Create a new texture and load the texture data into it
        Texture2D texture = new Texture2D((int)textureRect.width, (int)textureRect.height);
        texture.LoadImage(textureData);

        return texture;
    }

    public Sprite CreateSprite()
    {
        // Create a new texture and load the texture data into it
        Texture2D texture = CreateTexture();

        // Create a new sprite using the texture, the texture rect, and the pixels per unit
        Sprite sprite = Sprite.Create(texture, textureRect, new Vector2(0.5f, 0.5f), pixelsPerUnit);

        return sprite;
    }
}
