using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();

    public void AddItem(InventoryItem item)
    {
        items.Add(item);
    }

    public List<InventoryItem> GetItems()
    {
        return items;
    }

    public void ClearItems()
    {
        items.Clear();
    }

    // Save the inventory to a file
   

}
