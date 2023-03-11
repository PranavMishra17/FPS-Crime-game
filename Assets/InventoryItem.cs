using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem : MonoBehaviour
{
    public string objectName;
    public Sprite sprite;
    public string spritePath;
    public string itemInfo;
    public string[] clues = new string[] { "Sword is rather rusty", "Sword doesn't belong here i feel" };
    public bool addedtoInv = false;
    public bool ssAdded = false;

    private void Start()
    {
        // Load the hasInteracted value for this object from PlayerPrefs
        if (PlayerPrefs.HasKey(objectName))
        {
            addedtoInv = PlayerPrefs.GetInt(objectName) == 1;
        }
    }

    // This method returns a JSON string representation of the inventory item
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    // This method returns a new inventory item instance deserialized from the specified JSON string
    public static InventoryItem FromJson(string jsonString)
    {
        return JsonUtility.FromJson<InventoryItem>(jsonString);
    }

    public void Interact()
    {
        // Perform the interaction
        if (addedtoInv)
        {
            // Save the hasInteracted value for this object to PlayerPrefs
            PlayerPrefs.SetInt(objectName, addedtoInv ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    

    public InventoryItem(Sprite sprite, string itemInfo, string spritePath)
    {
        this.sprite = sprite;
        this.itemInfo = itemInfo;
        this.spritePath = spritePath;
    }
}
