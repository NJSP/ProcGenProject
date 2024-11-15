using System.Collections.Generic;
using TMPro;
using Assets.Scripts.PlayerScripts;
using UnityEngine;

namespace Assets.Scripts.PlayerScripts
{ 
public class InventorySystem : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public float maxCapacity;
    public float currentWeight;
    public int totalValue;
    private HudManager hudManager;


        // Awake is called when the script instance is being loaded
        void Awake()
        {
            hudManager = new HudManager();
        }
        public void AddItem(InventoryItem item)
    {
        items.Add(item);
        currentWeight += item.weight;
        CheckOverflow();
        totalValue += item.value;
        hudManager.UpdateHud();
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
            int itemCount = 0;

            foreach (InventoryItem item in items)
            {
                itemCount++;
                Debug.Log("Item " + item.itemName + ": " + itemCount);
            }
        }

    }
    
}
