using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInventory : MonoBehaviour
{
    private Inventory inventory = new();
    public string lvlname;

    private void Start()
    {
        inventory = new Inventory();
    }
    public LevelInventory(string lvlname)
    {
        this.lvlname = lvlname;
    }
    public void AddItem(InventoryItem item)
    {
        Debug.Log("Add item called");
        inventory.AddItem(item);
    }

    public void ClearItems()
    {
         inventory.ClearItems();
    }

    public List<InventoryItem> GetItems()
    {
        return inventory.GetItems();
    }
}
