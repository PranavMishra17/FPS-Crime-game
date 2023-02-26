using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInventory : MonoBehaviour
{
    private Inventory inventory = new Inventory();

    public void AddItem(InventoryItem item)
    {
        inventory.AddItem(item);
    }

    public List<InventoryItem> GetItems()
    {
        return inventory.GetItems();
    }
}
