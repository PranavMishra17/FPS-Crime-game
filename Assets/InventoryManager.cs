using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class InventoryManager : MonoBehaviour
{
    private List<LevelInventory> levelInventories = new List<LevelInventory>();
    public GameObject inventoryUI;
    public GameObject inventoryContent;
    public GameObject inventoryItemPrefab;
    public LevelInventory levelInventory;

    public GameObject objectToSave;
    public string saveFileName = "savedInv.json";
    //public InventorySaveData saveData;

    public FirstPersonController fpc;
    private string inventorySavePath;

    void Awake()
    {
        // Create the save path using the persistent data path
        inventorySavePath = Path.Combine(Application.persistentDataPath, "inventory.json");

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {

    }

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
        Debug.Log("AddingtoInv called");

        try 
        {
            foreach (InventoryItem item in levelInventory.GetItems())
            {

                // Instantiate a new item prefab and set its parent to the scroll view content
                GameObject newItem = Instantiate(inventoryItemPrefab, inventoryContent.transform);

                // Set the sprite and info of the item in the new item prefab's image and text components
                newItem.GetComponentInChildren<TextMeshProUGUI>().text = item.itemInfo;
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

        catch(NullReferenceException ex) 
        {
            Debug.Log("Inventory is empty. error:"+ ex );
        }
        // Add current inventory items
        
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

    public void SaveGameObject()
    {
        // Serialize the game object to a JSON string using JsonUtility
        string objectData = JsonUtility.ToJson(objectToSave);

        // Save the JSON string to a file
        System.IO.File.WriteAllText(Application.persistentDataPath + "/" + saveFileName, objectData);
    }

    public void LoadGameObject()
    {
        // Load the JSON string from the file
        string objectData = System.IO.File.ReadAllText(Application.persistentDataPath + "/" + saveFileName);

        // Deserialize the JSON string to a GameObject using JsonUtility
        GameObject loadedObject = JsonUtility.FromJson<GameObject>(objectData);

        // Instantiate the loaded game object
        Instantiate(loadedObject);
    }
    /*
    // Save the inventory data to a JSON file
    public void SaveInventory()
    {
        // Create a new list to hold the serialized inventory items
        List<InventorySaveData.SerializableInventoryItemsz> serializedItems = new List<InventorySaveData.SerializableInventoryItemsz>();

        // Get all inventory items in the inventory
        List<InventoryItem> allItems = GetAllItems();

        // Serialize each item and add it to the serialized items list
        foreach (InventoryItem item in allItems)
        {
            InventorySaveData.SerializableInventoryItemsz serializedItem = new InventorySaveData.SerializableInventoryItemsz(
                new SerializableSprite(item.sprite.texture, item.sprite.rect, item.sprite.pixelsPerUnit), item.itemInfo);
            serializedItems.Add(serializedItem);
        }

        // Create a new instance of the InventorySaveData class and set its serialized items list
        InventorySaveData saveData = new InventorySaveData();
        saveData.serializedItems = serializedItems;

        // Open a file stream to the inventory save path and create a new binary formatter
        FileStream file = File.Create(inventorySavePath);
        BinaryFormatter bf = new BinaryFormatter();

        // Serialize the save data and write it to the file stream
        bf.Serialize(file, saveData);

        // Close the file stream
        file.Close();
    }
    public void LoadInventory()
    {
        if (File.Exists(inventorySavePath))
        {
            // Open a file stream to the inventory save path and create a new binary formatter
            FileStream file = File.Open(inventorySavePath, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            try
            {
                // Deserialize the save data and cast it to an InventorySaveData object
                InventorySaveData saveData = bf.Deserialize(file) as InventorySaveData;

                // Clear the inventory to remove any existing items
                levelInventory.ClearItems();

                // Loop through each serialized inventory item and add it to the inventory
                foreach (InventorySaveData.SerializableInventoryItemsz serializedItem in saveData.serializedItems)
                {
                    // Convert the serializable sprite to a Unity sprite
                    Sprite sprite = serializedItem.serializableSprite.CreateSprite();

                    // Create a new inventory item and add it to the inventory
                    InventoryItem item = new InventoryItem(sprite, serializedItem.itemInfo);
                    levelInventory.AddItem(item);
                }
            }
            catch (Exception e)
            {
                Debug.Log("Failed to load inventory: " + e.Message);
            }

            // Close the file stream
            file.Close();
        }
    }
    */

    private byte[] SpriteToBytes(Sprite sprite)
    {
        Texture2D texture = sprite.texture;
        Rect textureRect = sprite.textureRect;
        SerializableSprite serializableSprite = new SerializableSprite(texture, textureRect, sprite.pixelsPerUnit);
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, serializableSprite);
        byte[] bytes = ms.ToArray();
        ms.Close();
        return bytes;
    }

    private SerializableSprite BytesToSerializableSprite(byte[] bytes)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(bytes);
        SerializableSprite serializableSprite = bf.Deserialize(ms) as SerializableSprite;
        ms.Close();
        return serializableSprite;
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveGameObject();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGameObject();
        }
    }
}
