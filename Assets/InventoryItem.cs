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

    public InventoryItem(Sprite sprite, string itemInfo)
    {
        this.sprite = sprite;
        this.itemInfo = itemInfo;
    }
}
