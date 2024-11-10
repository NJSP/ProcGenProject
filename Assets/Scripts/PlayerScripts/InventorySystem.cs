using System.Collections.Generic;
using UnityEngine;

namespace Assets.ProcGen.ProcGenScripts
{ 
public class InventorySystem : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public float maxCapacity;
    public float currentWeight;

    public void AddItem(InventoryItem item)
    {
        items.Add(item);
        currentWeight += item.weight;
        CheckOverflow();
    }

    private void CheckOverflow()
    {
        // Overflow logic here
        if (currentWeight > maxCapacity)
            {
                Debug.Log("Inventory is full!");
                // Handle overflow logic here
            }
    }

        public void OpenInventory()
        {
            foreach (InventoryItem item in items)
            {
                Debug.Log(item.itemName);
            }
        }
    }
    
}
