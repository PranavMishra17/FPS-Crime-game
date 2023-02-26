using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    private List<LevelInventory> levelInventories = new List<LevelInventory>();
    public GameObject inventoryUI;
    public GameObject inventoryContent;
    public GameObject inventoryItemPrefab;
    public LevelInventory levelInventory;

    public FirstPersonController fpc;

    public void AddLevelInventory(LevelInventory levelInventory)
    {
        levelInventories.Add(levelInventory);
    }
    public void ToggleInventoryUI()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        if (inventoryUI.activeSelf)
        {
            UpdateInventoryUI();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            fpc.cameraCanMove = false;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            fpc.cameraCanMove = true;
        }
    }

    // Update the inventory UI by clearing the existing items and adding the current items in the inventory
    public void UpdateInventoryUI()
    {

        // Clear existing inventory items
        foreach (Transform child in inventoryContent.transform)
        {
            Destroy(child.gameObject);
        }

        AddingtoInv();
        
    }

    public void AddingtoInv()
    {
        // Add current inventory items
        foreach (InventoryItem item in levelInventory.GetItems())
        {
            // Instantiate a new item prefab and set its parent to the scroll view content
            GameObject newItem = Instantiate(inventoryItemPrefab, inventoryContent.transform);

            // Set the sprite and info of the item in the new item prefab's image and text components
            //newItem.GetComponentInChildren<Image>().sprite = item.sprite;
            newItem.GetComponentInChildren<TextMeshProUGUI>().text = item.info;
            Image itemImage = newItem.GetComponentInChildren<Image>();
            if (itemImage != null)
            {
                itemImage.sprite = item.sprite;
                Debug.Log("Image component found");
            }
            Debug.Log(item.sprite.name);
            Debug.Log(itemImage.sprite.name);

        }
    }

    public List<InventoryItem> GetAllItems()
    {
        List<InventoryItem> allItems = new List<InventoryItem>();
        foreach (LevelInventory levelInventory in levelInventories)
        {
            allItems.AddRange(levelInventory.GetItems());
        }
        return allItems;
    }
    public void ShowItemInfo(string info)
    {
        // Display the item info in a dialog box or other UI element
        Debug.Log("Item info: " + info);
    }

}
