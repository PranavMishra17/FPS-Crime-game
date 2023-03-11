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
    public string inventorySavePath;

    void Awake()
    {
        // Create the save path using the persistent data path
        //inventorySavePath = Path.Combine(Application.persistentDataPath, "inventory.json");
        inventorySavePath = Application.dataPath + "/inventorySaveData.json";

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
        saveFileName = Application.dataPath;
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
                    //Debug.Log("Image component found");
                }
                //Debug.Log(item.sprite.name);
                //Debug.Log(itemImage.sprite.name);

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

    public Sprite pngtoSprite(string pngPath)
    {
        Sprite ssSprite;
        // Load the image from file
        Texture2D texture = new Texture2D(Screen.width, Screen.height);
        byte[] fileData = System.IO.File.ReadAllBytes(pngPath);
        texture.LoadImage(fileData);

        // Create a sprite from the texture
        //ssSprite = Sprite.Create(croppedTexture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        ssSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.width), new Vector2(0.5f, 0.5f));

        return ssSprite;
    }
    // Save the inventory data to a JSON file
    public void SaveInventory()
    {
        // Create a new list to hold the serialized inventory items
        List<InventorySaveData.SerializableInventoryItemsz> serializedItems = new List<InventorySaveData.SerializableInventoryItemsz>();

        // Get all inventory items in the inventory
        List<InventoryItem> allItems = GetAllItems();
        Debug.Log(allItems);
        
        // Serialize each item and add it to the serialized items list
        foreach (InventoryItem item in levelInventory.GetItems())
        {
            pngtoSprite(item.spritePath);
            // InventorySaveData.SerializableInventoryItemsz serializedItem = new InventorySaveData.SerializableInventoryItemsz(
            //      new SerializableSprite(item.sprite.texture, item.sprite.rect, item.sprite.pixelsPerUnit), item.itemInfo);
            InventorySaveData.SerializableInventoryItemsz serializedItem = new InventorySaveData.SerializableInventoryItemsz(
                 pngtoSprite(item.spritePath), item.itemInfo, item.spritePath);

            serializedItems.Add(serializedItem);
        }

        // Create a new instance of the InventorySaveData class and set its serialized items list
        InventorySaveData saveData = new InventorySaveData();
        saveData.serializedItems = serializedItems;

        // Serialize the save data to a JSON string
        string jsonString = JsonUtility.ToJson(saveData);

        string saveDirectory = Application.persistentDataPath + "/SaveFiles";
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
        // Get a list of existing save files
        string[] saveFiles = Directory.GetFiles(saveDirectory, "*.json");

        // Generate a new file name
        int nextFileNumber = saveFiles.Length + 1;
        string newFileName = "save" + nextFileNumber.ToString() + ".json";

        // Save the data to the new file
        File.WriteAllText(saveDirectory + "/" + newFileName, jsonString);

        // Write the JSON string to a file in the Assets folder
        string savePath = Application.dataPath + "/inventorySaveData.json";
        File.WriteAllText(savePath, jsonString);


        inventorySavePath = savePath;
        /*
        // Open a file stream to the inventory save path and create a new binary formatter
        FileStream file = File.Create(inventorySavePath);
        BinaryFormatter bf = new BinaryFormatter();

        // Serialize the save data and write it to the file stream
        bf.Serialize(file, saveData);

        // Close the file stream
        file.Close();
        */
    }
    public void LoadInventory()
    {
        if (File.Exists(inventorySavePath))
        {
            // Read the entire JSON file and store it as a string
            string jsonString = File.ReadAllText(inventorySavePath);

            InventorySaveData savedata = new InventorySaveData();
            // Deserialize the JSON string into a new instance of the InventorySaveData class
            JsonUtility.FromJsonOverwrite(jsonString, savedata);

            // Clear the inventory to remove any existing items
            levelInventory.ClearItems();

            // Open a file stream to the inventory save path and create a new binary formatter
            //FileStream file = File.Open(inventorySavePath, FileMode.Open);
            //BinaryFormatter bf = new BinaryFormatter();

           // try
           // {
                // Deserialize the save data and cast it to an InventorySaveData object
                //InventorySaveData saveData = bf.Deserialize(file) as InventorySaveData;

                // Clear the inventory to remove any existing items
                //levelInventory.ClearItems();

                // Loop through each serialized inventory item and add it to the inventory
                foreach (InventorySaveData.SerializableInventoryItemsz serializedItem in savedata.serializedItems)
                {
                    Debug.Log(serializedItem);
                    // Convert the serializable sprite to a Unity sprite
                    Sprite sprite = serializedItem.serializableSprite;

                    // Create a new inventory item and add it to the inventory
                    InventoryItem item = new InventoryItem(pngtoSprite(serializedItem.spritePath), serializedItem.itemInfo, serializedItem.spritePath);
                    Debug.Log("item: " + item.itemInfo);
                    levelInventory.AddItem(item);
                    
                }

            UpdateInventoryUI();

           // }
           // catch (Exception e)
           // {
           //      Debug.Log("Failed to load inventory: " + e.Message);
           // }

            // Close the file stream
            // file.Close();
        }
    }
    

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
            SaveInventory();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadInventory();
        }
    }
}
